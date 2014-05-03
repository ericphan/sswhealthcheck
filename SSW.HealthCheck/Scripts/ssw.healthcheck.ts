module ssw {
    export module healthcheck {
        export interface ITestMonitor {
            Key: string;
            Name: string;
            Description: string;
            IsRunning: boolean;
            Result: {
                Success: boolean;
                Message: string;
            }
            Events: ITestEvent[];
            Progress: IProgress;
        }
        export interface ITestChanged {
            Key: string;
            IsRunning: boolean;
            Result: {
                Success: boolean;
                Message: string;
            }
        }
        export interface ITestEvent {
            DateTime: Date;
            Message: string;
        }
        export interface IProgress {
            Min: number;
            Max: number;
            Val: number
        }

        export class HealthCheckController {
            $http: any;
            UpdateStats: () => void;
            Check: (model: ITestMonitor) => void;
            CheckAll: () => void;
            CheckAllDefault: () => void;
            OnTestStarted: (x: ITestChanged) => void;
            OnTestCompleted: (x: ITestChanged) => void;
            OnTestEvent: (x: { Key: string; Event: ITestEvent })  => void;
            OnTestProgress: (x: { Key: string; Progress: IProgress }) => void;
            
            constructor($scope: any, $http: any, tests: ITestMonitor[]) {
                $scope.tests = tests;

                // convert tests into mapping
                var testsByKey = {};
                var allTests = [];
                for (var i = 0; i < $scope.tests.length; i++) {
                    for (var j = 0; j < $scope.tests[i].TestMonitors.length; j++) {
                        allTests.push($scope.tests[i].TestMonitors[j]);
                    }
                }

                for (var k in allTests) {
                    var t = allTests[k];
                    testsByKey[t.Key] = t;
                }

                this.$http = $http;
                this.UpdateStats = () => {
                    var $allTests = $(".panel");

                    var all = $allTests.length;
                    var passed = $allTests.find(".panel-title .pass-text").length;
                    var warning = $allTests.find(".panel-title .pass-warning-text").length;
                    var failed = $allTests.find(".panel-title .fail-text").length;

                    $("#all-stat").text(all);
                    $("#passed-stat").text(passed);
                    $("#warning-stat").text(warning);
                    $("#failed-stat").text(failed);
                    $("#pending-stat").text((all - passed - warning - failed));
                };
                this.Check = (model: ITestMonitor) => {
                    var that = this;
                    $http.get(($("#ng-app").data("root-path") || "/") + "HealthCheck/Check?Key=" + model.Key)
                        .success((data: any, status: any, headers: any, config: any) => {
                            that.UpdateStats();
                            console.log(data);
                        })
                        .error((data: any, status: any, headers: any, config: any) => {
                            console.log(data);
                        });
                };
                this.CheckAll = () => {
                    for (var k in allTests) {
                        var test = allTests[k];
                        this.Check(test);
                    }
                };
                this.CheckAllDefault = () => {
                    for (var k in allTests) {
                        var test = allTests[k];
                        if (!test.IsRunning && test.IsDefault) {
                            this.Check(test);
                        }
                    }
                };
                this.OnTestStarted = (x: ITestChanged) => {
                    var test: ITestMonitor = testsByKey[x.Key];
                    if (test) {
                        test.IsRunning = x.IsRunning;
                        test.Result = x.Result;
                        test.Events = [];
                        $scope.$apply();
                    }
                };
                this.OnTestCompleted = (x: ITestChanged) => {
                    var test: ITestMonitor = testsByKey[x.Key];
                    if (test) {
                        test.IsRunning = x.IsRunning;
                        test.Result = x.Result;
                        $scope.$apply();
                    }
                };
                this.OnTestEvent = (x: { Key: string; Event: ITestEvent }) => {
                    console.log(x);
                    var test: ITestMonitor = testsByKey[x.Key];
                    if (test) {
                        test.Events.push(x.Event);
                        $scope.$apply();
                    }
                };

                this.OnTestProgress = (x: { Key: string; Progress: IProgress }) => {
                    console.log(x);
                    var test: ITestMonitor = testsByKey[x.Key];
                    if (test) {
                        test.Progress = x.Progress;
                        $scope.$apply();
                    }
                };

                // signalr
                $.connection.hub.logging = true;
                var healthCheckHub = (<any>$.connection).healthCheckHub;
                var healthCheckServer = healthCheckHub.server;
                var healthCheckClient = healthCheckHub.client;
                healthCheckClient.testCompleted = this.OnTestCompleted;
                healthCheckClient.testStarted = this.OnTestStarted;
                healthCheckClient.testEvent = this.OnTestEvent;
                healthCheckClient.testProgress = this.OnTestProgress;
                $.connection.hub.start().done(()=> {
                    this.CheckAllDefault();
                });
            }
        }
    }

    var hcheck = angular.module('ssw.healthcheck', <string[]>[]);
    hcheck.filter('toTrusted', function ($sce) {
        return function (val) {
            return $sce.trustAsHtml(val);
        };
    });
    hcheck.value('tests', <string[]>[]);
    hcheck.controller('HealthCheck', ['$scope', '$http', 'tests', ssw.healthcheck.HealthCheckController]);
}