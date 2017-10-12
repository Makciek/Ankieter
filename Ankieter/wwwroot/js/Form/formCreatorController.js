var myApp = angular.module('myApp', []);

myApp.controller('formCreatorController', ['$scope', function ($scope) {
    $scope.lastInputId = 0;
    $scope.newInput = {};
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
            typeMetadata: null
        };
    }

    $scope.typeSelected = function (type) {
        $scope.newInput.type = type;

        switch (type.id) {
        case 0:
            break;
        default:
        }

    }

    $scope.addNewInputToInputList = function() {
        if ($scope.newInput.name.length < 1 ||
            $scope.newInput.type.id < 1) {
            $scope.errorMsg = "You must fill all required fields!";
            $("#errorDiv").show();
            return;
        }

        $scope.inputs.push($scope.newInput);
        $scope.cleanNewInput();
    }

    $scope.hideErrorBox = function () {
        $("#errorDiv").hide();
    }

    $scope.editInput = function(input) {
        
    }

    $scope.removeInputStep1 = function(input) {
        $scope.inputToRemove = input;
    }

    $scope.removeInputStep2 = function() {
        $scope.inputs = $scope.inputs.filter(item => item.id !== $scope.inputToRemove.id);
        $scope.inputToRemove = null;
    }

    $scope.cleanNewInput();
    $scope.hideErrorBox();
}]);
