// a placeholder for reusable controls
(function () {
    "use strict";

    angular.module("simpleControls", [])
        .directive("waitCursor", waitCursor);

    function waitCursor() {
        return {
            scope: { // scope is the object the we bind the cursor to
                isShown: "=displayWhen"
            },
            restrict: "E", //element only
            templateUrl: "/views/waitCursor.html"
        };
    };
})();