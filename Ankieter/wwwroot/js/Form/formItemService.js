myApp = angular.module('myApp', []);

(function (app) {
    "use strict";
    app.service('formItemService',
        function () {
            this.items = [];
            this.itemsChangedCallbacks = [];
            
            this.setItems = function (newItems) {
                this.items = newItems;

                for (var i = 0; i < this.itemsChangedCallbacks.length; i++) {
                    this.itemsChangedCallbacks[i](this.items);
                }
            };

            this.getItems = function () {
                return this.items;
            };

            this.setItemsChangedCallback = function (callback) {
                this.itemsChangedCallbacks.push(callback);
            };
        });
})(myApp);