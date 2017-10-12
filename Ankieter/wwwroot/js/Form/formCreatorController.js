var myApp = angular.module('myApp', []);

myApp.controller('formCreatorController', ['$scope', function ($scope) {
    $scope.lastInputId = 0;
    $scope.newInput = [];
    $scope.newClicableOptions = [];
    $scope.newClicableOption = "";
    $scope.inputs = [];
    $scope.errorMsg = "";

    $scope.possibleInputTypes = [
        { id: 0, name: "Not Selected" },
        { id: 1, name: "Text" },
        { id: 2, name: "Textarea" },
        { id: 3, name: "Radio" },
        { id: 4, name: "Checkbox" },
        { id: 5, name: "Dropdown" },
    ];

    $scope.cleanNewInput = function (input) {
        $scope.newInput = {
            id: $scope.lastInputId++,
            type: $scope.possibleInputTypes[0],
            name: "",
            description: "",
            isRequired: true,

            textMinLength: 0,
            textMaxLength: -1,

            clicableOptions: []
        };
    }
    
    $scope.cleanTypesMetadata = function () {
        $scope.newInput.textMinLength = 0;
        $scope.newInput.textMaxLength = -1;
        $scope.newInput.clicableOptions = [];
    }

    $scope.typeSelected = function (type) {
        if ($scope.newInput.type.id === type.id)
            return;

        $scope.cleanTypesMetadata();
        $scope.newInput.type = type;
    }

    $scope.addNewInputToInputList = function () {
        if ($scope.newInput.name.length < 1 ||
            $scope.newInput.type.id < 1) {
            $scope.errorMsg = "You must fill all required fields!";
            $("#errorDiv").show();
            return;
        }

        $scope.inputs.clicableOptions = $scope.newClicableOptions;

        $scope.inputs.push($scope.newInput);
        $scope.cleanNewInput();
    }

    $scope.addNewClicableOption = function () {
        $scope.newClicableOptions.push($scope.newClicableOption);
        $scope.newClicableOption = "";
    }

    $scope.hideErrorBox = function () {
        $("#errorDiv").hide();
    }

    $scope.editInput = function (input) {

    }

    $scope.removeInputStep1 = function (input) {
        $scope.inputToRemove = input;
    }

    $scope.removeInputStep2 = function () {
        $scope.inputs = $scope.inputs.filter(item => item.id !== $scope.inputToRemove.id);
        $scope.inputToRemove = null;
    }

    $scope.cleanNewInput();
    $scope.hideErrorBox();
}]);
