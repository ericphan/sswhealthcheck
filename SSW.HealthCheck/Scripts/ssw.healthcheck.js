var ssw;
(function (ssw) {
    (function (healthcheck) {
        var ZsValidateController = (function () {
            function ZsValidateController($scope, $http, tests) {
                var _this = this;
                $scope.tests = tests;

                // convert tests into mapping
                var testsByKey = {};
                for (var k in $scope.tests) {
                    var t = tests[k];
                    testsByKey[t.Key] = t;
                }

                this.$http = $http;
                this.Check = function (model) {
                    $http.get("/zsValidate/Check?Key=" + model.Key).success(function (data, status, headers, config) {
                        // model.Result = data;
                        console.log(data);
                    }).error(function (data, status, headers, config) {
                        console.log(data);
                    });
                };
                this.CheckAll = function () {
                    for (var k in $scope.tests) {
                        var test = $scope.tests[k];
                        _this.Check(test);
                    }
                };
                this.OnTestStarted = function (x) {
                    var test = testsByKey[x.Key];
                    if (test) {
                        test.IsRunning = x.IsRunning;
                        test.Result = x.Result;
                        test.Events = [];
                        $scope.$apply();
                    }
                };
                this.OnTestCompleted = function (x) {
                    var test = testsByKey[x.Key];
                    if (test) {
                        test.IsRunning = x.IsRunning;
                        test.Result = x.Result;
                        $scope.$apply();
                    }
                };
                this.OnTestEvent = function (x) {
                    console.log(x);
                    var test = testsByKey[x.Key];
                    if (test) {
                        test.Events.push(x.Event);
                        $scope.$apply();
                    }
                };

                this.OnTestProgress = function (x) {
                    console.log(x);
                    var test = testsByKey[x.Key];
                    if (test) {
                        test.Progress = x.Progress;
                        $scope.$apply();
                    }
                };

                // signalr
                $.connection.hub.logging = true;
                var healthCheckHub = ($.connection).healthCheckHub;
                var healthCheckServer = healthCheckHub.server;
                var healthCheckClient = healthCheckHub.client;
                healthCheckClient.testCompleted = this.OnTestCompleted;
                healthCheckClient.testStarted = this.OnTestStarted;
                healthCheckClient.testEvent = this.OnTestEvent;
                healthCheckClient.testProgress = this.OnTestProgress;
                $.connection.hub.start();
            }
            return ZsValidateController;
        })();
        healthcheck.ZsValidateController = ZsValidateController;
    })(ssw.healthcheck || (ssw.healthcheck = {}));
    var healthcheck = ssw.healthcheck;

    var hcheck = angular.module('ssw.healthcheck', []);
    hcheck.value('tests', []);
    hcheck.controller('ZsValidate', ['$scope', '$http', 'tests', ssw.healthcheck.ZsValidateController]);
})(ssw || (ssw = {}));
//# sourceMappingURL=ssw.healthcheck.js.map
