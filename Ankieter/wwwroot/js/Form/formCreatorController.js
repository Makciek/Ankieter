var myApp = angular.module('myApp', []);

myApp.controller('formCreatorController', ['$scope', function ($scope) {
    $scope.lastInputId = 0;
    $scope.lastOptionId = 0;
    $scope.newInput = [];
    $scope.newClicableOptions = [];
    $scope.newClicableOption = { id: $scope.lastOptionId++, content: "" };
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

    $scope.addOrUpdateNewInputToInputList = function () {
        if ($scope.newInput.name.length < 1 ||
            $scope.newInput.type.id < 1) {
            $scope.errorMsg = "You must fill all required fields!";
            $("#errorDiv").show();
            return;
        }

        index = $scope.inputs.map(function (e) { return e.id; }).indexOf($scope.newInput.id);
        if (index > -1) {
            $scope.inputs[index] = $scope.newInput;
            $scope.inputs[index].clicableOptions = $scope.newClicableOptions;
        } else {
            $scope.newInput.clicableOptions = $scope.newClicableOptions;
            $scope.inputs.push($scope.newInput);
        }

        $scope.cleanNewInput();
        $scope.cleanTypesMetadata();
        $scope.newClicableOptions = [];
    }

    $scope.addOrUpdateNewClicableOption = function () {
        if ($scope.newClicableOption.content.length < 1) {
            return;
        }

        index = $scope.newClicableOptions.map(function (e) { return e.id; }).indexOf($scope.newClicableOption.id);
        if (index > -1) {
            $scope.newClicableOptions[index] = $scope.newClicableOption;
        } else {
            $scope.newClicableOptions.push($scope.newClicableOption);
        }
        $scope.newClicableOption = { id: $scope.lastOptionId++, content: "" };
    }

    $scope.hideErrorBox = function () {
        $("#errorDiv").hide();
    }

    $scope.editInput = function (input) {
        if (!$scope.newInput.name.length < 1 &&
            !$scope.newInput.type.id < 1) {
            $scope.addOrUpdateNewClicableOption();
            $scope.addOrUpdateNewInputToInputList();
        }
        
        index = $scope.inputs.map(function (e) { return e.id; }).indexOf(input.id);
        if (index > -1) {
            $scope.newInput = $scope.inputs[index];
            $scope.newClicableOptions = $scope.inputs[index].clicableOptions;
        } else {
            alert("Something went realy wrong!");
        }
    }

    $scope.editOption = function (input) {
        if ($scope.newClicableOption.content !== "") {
            $scope.addOrUpdateNewClicableOption();
        }
        $scope.newClicableOption = input;
    }

    $scope.removeInputStep1 = function (input) {
        $scope.inputToRemove = input;
    }

    $scope.removeInputStep2 = function () {
        $scope.inputs = $scope.inputs.filter(item => item.id !== $scope.inputToRemove.id);
        $scope.inputToRemove = null;
    }

    $scope.removeOptionStep1 = function (input) {
        $scope.optionToRemove = input;
    }

    $scope.removeOptionStep2 = function () {
        $scope.newClicableOptions = $scope.newClicableOptions.filter(item => item.id !== $scope.optionToRemove.id);
        $scope.inputToRemove = null;
    }

    $scope.submitForm = function() {
        if (!$scope.newInput.name.length < 1 &&
            !$scope.newInput.type.id < 1) {
            $scope.addOrUpdateNewClicableOption();
            $scope.addOrUpdateNewInputToInputList();
        }

        $("#createdForm").submit();
    }

    $("#createdForm").submit(function (eventObj) {
        $('<input />').attr('type', 'hidden')
            .attr('name', "FormStructure")
            .attr('value', JSON.stringify($scope.inputs))
            .appendTo('#createdForm');
        return true;
    });

    $scope.cleanNewInput();
    $scope.hideErrorBox();
}]);
