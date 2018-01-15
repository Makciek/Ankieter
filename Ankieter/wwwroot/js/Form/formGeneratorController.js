var myApp = angular.module('myApp');

(function (app) {
    "use strict";
    app.controller('formGeneratorController',
        [
            '$scope', 'formItemService', function ($scope, formItemService) {
                $scope.items = [];

                formItemService.setItemsChangedCallback(function (items) {
                    $scope.items = items;
                });

                $scope.setDropDownValue = function (item, value) {
                    item.answerName = value.content;
                    item.answer = value._id;
                }

                $scope.setRadioAnwser = function (item, value) {
                    item.answerName = value.content;
                    item.answer = value._id;
                }

                $scope.isPreview = unescape($("#previewFlag").val());

                if ($scope.isPreview == "true")
                    return;

                var jsonBackendVal = unescape($("#_hiddednJsonStructure").val());

                $scope.backendData = JSON.parse(jsonBackendVal);

                var itemsTemp = JSON.parse($scope.backendData.jsonStructure);
                for (var i = 0; i < itemsTemp.length; i++) {
                    itemsTemp[i].answerName = "Select anwser";
                    itemsTemp[i].answer = "";
                    for (var j = 0; j < itemsTemp[i].clicableOptions; j++) {
                        itemsTemp[i].clicableOptions[j].selectedValue = false;
                    }
                }

                $scope.items = itemsTemp;

                console.log($scope.items);


                $scope.submitForm = function () {
                    if ($scope.items.length < 1) {
                        return;
                    }

                    // todo: add anwsers validation

                    $("#createdForm").submit();
                }

                $("#createdForm").submit(function (eventObj) {
                    var finalAwnsers = {
                        id: $scope.backendData.id,
                        items: []
                    };

                    for (var k = 0; k < $scope.items.length; k++) {
                        var anwserTemp = {
                            id: $scope.items[k]._id,
                            answerName: $scope.items[k].answerName,
                            answer: $scope.items[k].answer
                        };

                        if ($scope.items[k].answer == "") {
                            anwserTemp.answers = [];

                            for (var l = 0; l < $scope.items[k].clicableOptions.length; l++) {
                                anwserTemp.answers.push({
                                    id: $scope.items[k].clicableOptions[l].id,
                                    value: ($scope.items[k].clicableOptions[l].hasOwnProperty("selectedValue") ?
                                        Number($scope.items[k].clicableOptions[l].selectedValue) : 0) 
                                });
                            }
                        }
                        
                        finalAwnsers.items.push(anwserTemp);
                    }

                    $('<input />').attr('type', 'hidden')
                        .attr('name', "answerStructure")
                        .attr('value', JSON.stringify(finalAwnsers))
                        .appendTo('#createdForm');
                    return true;
                });
            }
        ]);
})(myApp);