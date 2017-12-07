﻿var myApp = angular.module('myApp');

(function (app) {
    "use strict";
    app.controller('formGeneratorController',
        [
            '$scope', 'formItemService', function ($scope, formItemService) {
                $scope.items = [];

                formItemService.setItemsChangedCallback(function (items) {
                    $scope.items = items;
                });

            }
        ]);
})(myApp);