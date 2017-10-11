var myApp = angular.module('myApp', []);

myApp.controller('formCreatorController', ['$scope', function ($scope) {
    $scope.lastInputId = 0;
    $scope.inputs = [];

    $scope.generateUIForInput = function (input) {
        return "<div></div>";
    }

    $scope.renderNewInputOnUI = function (input) {
        $("#inputList").innerHTML += generateUIForInput(input);
    }
    
    $scope.inputs.push(
    {
        id: $scope.lastInputId++,
        type: "not selected",
        isRequired: true
    });
    $scope.inputs.push(
    {
        id: $scope.lastInputId++,
        type: "not selected",
        isRequired: true
    });
}]);
