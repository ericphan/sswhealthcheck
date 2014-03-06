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
                for (var k in $scope.tests) {
                    var t = tests[k];
                    testsByKey[t.Key] = t;
                }
                
                this.$http = $http;
                this.Check = (model: ITestMonitor) => {
                    $http.get(($("#ng-app").data("root-path") || "/") + "HealthCheck/Check?Key=" + model.Key)
                        .success((data: any, status: any, headers: any, config: any) => {
                            // model.Result = data;
                            console.log(data);
                        })
                        .error((data: any, status: any, headers: any, config: any) => {
                            console.log(data);
                        });
                };
                this.CheckAll = () => {
                    for (var k in $scope.tests) {
                        var test = $scope.tests[k];
                        this.Check(test);
                    }
                };
                this.CheckAllDefault = () => {
                    for (var k in $scope.tests) {
                        var test = $scope.tests[k];
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
    hcheck.value('tests', <string[]>[]);
    hcheck.controller('HealthCheck', ['$scope', '$http', 'tests', ssw.healthcheck.HealthCheckController]);
}