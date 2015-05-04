; (function () {
    var probabilityCalculatorApp = angular.module('probabilityCalculatorApp', ['ngRoute']);

    probabilityCalculatorApp.config(['$routeProvider', function ($routeProvider) {
        $routeProvider.
            when('/home', {
                templateUrl: 'Client/views/home.html',
                controller: 'probabilityCalculatorCtrl'
            }).
            when('/about', {
                templateUrl: 'Client/views/about.html'
            }).
            otherwise({
                redirectTo: '/home'
            });
        }]);

    probabilityCalculatorApp.controller('probabilityCalculatorCtrl', ['$scope', 'calculator', 'calculationsRepository', function ($scope, calculator, calculationsRepository) {
        $scope.viewModel = {};
        $scope.viewModel.calcType = 'CombinedWith';

        var refreshLog = function () {
            calculationsRepository.getAllCalcRecords().then(function (data) {
                $scope.viewModel.calculations = data;
            });
        };
        refreshLog(); // initial get of calculations

        $scope.calculate = function () {
            $scope.viewModel.calcResult = calculator.calculatePobability($scope.viewModel.probOne,
                                                                $scope.viewModel.probTwo,
                                                                $scope.viewModel.calcType);
            calculationsRepository.addCalcRecord($scope.viewModel.probOne,
                                                    $scope.viewModel.probTwo,
                                                    $scope.viewModel.calcType,
                                                    $scope.viewModel.calcResult)
                .then(function (status) {
                    refreshLog();
                });
        };        

    }]);

    probabilityCalculatorApp.factory('calculator', [function () {
        return {
            calculatePobability: function (probOne, probTwo, calcType) {
                probOne = parseFloat(probOne);
                probTwo = parseFloat(probTwo);

                if (calcType === 'CombinedWith') {
                    return probOne * probTwo;
                } else if (calcType === 'Either') {
                    return probOne + probTwo - probOne * probTwo;
                } else {
                    throw 'Type is not correct';
                }
            }
        };
    }]);

    probabilityCalculatorApp.factory('calculationsRepository', ['$http', '$q', function ($http, $q) {
        return {
            addCalcRecord: function (probOne, probTwo, calcType, calcResult) {
                var calcRecordDto = {
                    probabilityOne: probOne,
                    probabilityTwo: probTwo,
                    calculationType: calcType,
                    calculationResult: calcResult
                };
                var defered = $q.defer();
                $http.post('/api/calculations', calcRecordDto)
                    .success(function (data) {
                        defered.resolve(data);
                    })
                    .error(function (data, status) {
                        defered.reject(status);
                    });

                return defered.promise;

            },
            getAllCalcRecords: function () {
                var defered = $q.defer();
                $http.get('/api/calculations')
                    .success(function (data) {
                        defered.resolve(data);
                    })
                    .error(function (data, status) {
                        defered.reject(status);
                    });

                return defered.promise;
            }
        };
    }]);

})();