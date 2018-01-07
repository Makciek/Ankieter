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

                var jsonBackendVal = unescape($("#_hiddednJsonStructure").val());

                $scope.backendData = JSON.parse(jsonBackendVal);
                $scope.items = JSON.parse($scope.backendData.jsonStructure);

                console.log($scope.items);
            }
        ]);
})(myApp);