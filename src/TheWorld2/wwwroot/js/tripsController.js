(function () {
    "use strict";

    // get reference to the existing module
    angular.module("app-trips")
        .controller("tripsController", tripsController);

    // declare $http dependency
    function tripsController($http) {
        var vm = this;

        vm.trips = [];

        vm.newTrip = {};
        vm.errorMessage = "";
        vm.isBusy = true;

        $http.get("/api/trips")
        .then(function (response) { //callback for success
            angular.copy(response.data, vm.trips);
        }, function (error) { //callback for failure
            vm.errorMessage = "Failed to load the data: " + error;
        })
        .finally(function () {
            vm.isBusy = false;
        });

        vm.addTrip = function () {
            vm.isBusy = true;
            vm.errorMessage = "";

            $http.post("/api/trips", vm.newTrip)
            .then(function (response) {
                vm.trips.push(response.data);
                vm.newTrip = {};
            }, function (error) {
                vm.errorMessage = "Failed to add new trip";
            })
            .finally(function () {
                vm.isBusy = false;
            });
        };
    }
})();