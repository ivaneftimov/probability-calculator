describe('probabilityCalculatorApp', function () {
    var $rootScope,
        $scope,
        $httpBackend,
        $controller,
        calculator;

    beforeEach(angular.mock.module('probabilityCalculatorApp'));

    beforeEach(inject(function ($injector) {
        $rootScope = $injector.get('$rootScope');
        $scope = $rootScope.$new();
        $httpBackend = $injector.get('$httpBackend');
        $controller = $injector.get('$controller');
        calculator = $injector.get('calculator');
    }));

    describe('probabilityCalculatorCtrl', function () {

        beforeEach(function () {
            controller = function () {
                $controller('probabilityCalculatorCtrl', {
                    '$rootScope': $rootScope,
                    '$scope': $scope
                });
            };
        });

        it('properly initializes the view model', function () {
            controller();

            expect($scope.viewModel).toBeDefined();
            expect($scope.viewModel.calcType).toBe('CombinedWith');
        });

        describe('#calculate', function () {

            it('calculates the probability and saves the result', function () {
                controller();
                $scope.viewModel.probOne = 0.5;
                $scope.viewModel.probTwo = 0.5;
                var resultCombineWith = $scope.viewModel.probOne * $scope.viewModel.probTwo;

                var postData = {
                    probabilityOne: $scope.viewModel.probOne,
                    probabilityTwo: $scope.viewModel.probTwo,
                    calculationType: $scope.viewModel.calcType,
                    calculationResult: resultCombineWith
                };
                var getDummyData = [{dummy1: 'dummy calc 1'}];

                // initial get for calculations
                $httpBackend.expectGET('/api/calculations').respond([]);
                $httpBackend.flush();

                // create new calculation and get again to refresh the list
                $httpBackend.expectPOST('/api/calculations', postData).respond(201);
                $httpBackend.expectGET('/api/calculations').respond(getDummyData);

                $scope.calculate();

                $httpBackend.flush();

                expect($scope.viewModel.calcResult).toBeDefined();
                expect($scope.viewModel.calcResult).toBe(resultCombineWith);
                expect($scope.viewModel.calculations).toEqual(getDummyData);
            });
        });

    });

    describe('calculator', function () {
        describe('#calculatePobability', function () {
            it('calculates correctly when calculation type is CombinedWith', function () {
                var prob1 = 0.5,
                    prob2 = 0.5,
                    calcType = 'CombinedWith';
                var expectedResult = prob1 * prob2;

                var result = calculator.calculatePobability(prob1, prob2, calcType);

                expect(result).toBe(expectedResult);
            });

            it('calculates correctly when calculation type is Either', function () {
                var prob1 = 0.5,
                    prob2 = 0.5,
                    calcType = 'Either';
                var expectedResult = prob1 + prob2 - prob1 * prob2;

                var result = calculator.calculatePobability(prob1, prob2, calcType);

                expect(result).toBe(expectedResult);
            });

            it('throws an error when calculation type is incorrect', function () {
                var prob1 = 0.5,
                    prob2 = 0.5,
                    calcType = 'Incorrect';                

                expect(function () { calculator.calculatePobability(prob1, prob2, calcType); }).toThrow();                
            });

        });
    });
});