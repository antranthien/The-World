(function () {
    "use strict";
    //define a module
    angular.module("app-trips", ["simpleControls", "ngRoute"])
        .config(function ($routeProvider) { // config the module object that's returned
            $routeProvider.when("/", {
                controller: "tripsController",
                controllerAs: "vm", //alias
                templateUrl: "/views/tripsView.html"
            });

            $routeProvider.when("/editor/:tripName", {
                controller: "tripEditorController",
                controllerAs: "vm", //alias
                templateUrl: "/views/tripEditorView.html"
            });

            $routeProvider.otherwise({ redirectTo: "/" });
        });
})();