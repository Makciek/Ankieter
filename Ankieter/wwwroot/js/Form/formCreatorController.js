﻿var myApp = angular.module('myApp');

(function (app) {
    "use strict";
    app.controller('formCreatorController',
        [
            '$scope', 'formItemService', function ($scope, formItemService) {
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

                    $scope.addOrUpdateNewClicableOption();

                    var index = $scope.inputs.map(function (e) { return e.id; }).indexOf($scope.newInput.id);
                    if (index > -1) {
                        $scope.inputs[index] = angular.copy($scope.newInput);

                        $scope.inputs[index].clicableOptions = $scope.newClicableOptions.length > 0
                            ? angular.copy($scope.newClicableOptions)
                            : [];

                    } else {
                        $scope.newInput.clicableOptions = $scope.newClicableOptions.length > 0
                            ? angular.copy($scope.newClicableOptions)
                            : [];
                        $scope.inputs.push(angular.copy($scope.newInput));
                    }

                    formItemService.setItems($scope.inputs);

                    $scope.cleanNewInput();
                    $scope.cleanTypesMetadata();
                    $scope.newClicableOption = { id: 0, content: "" };
                    $scope.newClicableOptions = [];
                    $scope.lastOptionId = 1;
                }

                $scope.addOrUpdateNewClicableOption = function () {
                    if ($scope.newClicableOption.content.length < 1) {
                        return;
                    }

                    var index = $scope.newClicableOptions.map(function (e) { return e.id; })
                        .indexOf($scope.newClicableOption.id);
                    if (index > -1) {
                        $scope.newClicableOptions[index] = angular.copy($scope.newClicableOption);
                    } else {
                        $scope.newClicableOptions.push(angular.copy($scope.newClicableOption));
                    }
                    $scope.newClicableOption = { id: $scope.lastOptionId++, content: "" };

                    formItemService.setItems($scope.inputs);
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

                    var index = $scope.inputs.map(function (e) { return e.id; }).indexOf(input.id);
                    if (index > -1) {
                        $scope.newInput = angular.copy($scope.inputs[index]);
                        $scope.newClicableOptions = angular.copy($scope.inputs[index].clicableOptions);
                    } else {
                        alert("Something went realy wrong!");
                    }

                    formItemService.setItems($scope.inputs);
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
                    formItemService.setItems($scope.inputs);
                }

                $scope.removeOptionStep1 = function (input) {
                    $scope.optionToRemove = input;
                }

                $scope.removeOptionStep2 = function () {
                    $scope.newClicableOptions =
                        $scope.newClicableOptions.filter(item => item.id !== $scope.optionToRemove.id);
                    $scope.inputToRemove = null;
                    formItemService.setItems($scope.inputs);
                }

                $scope.submitForm = function () {
                    if (!$scope.newInput.name.length < 1 &&
                        !$scope.newInput.type.id < 1) {
                        $scope.addOrUpdateNewClicableOption();
                        $scope.addOrUpdateNewInputToInputList();
                    }

                    $scope.inputs = angular.copy($scope.inputs);
                    
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
            }
        ]);
})(myApp);
