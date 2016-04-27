(function () {


    //var ele = $("#userName");
    //ele.text("A new name");

    //var main = $("#main");
    //main.on("mouseenter", function () {
    //    main.css("background-color", "#888");
    //});

    //main.on("mouseleave", function () {
    //    main.css("background-color", ""); //differs from browser to browser,may set main.style = ...
    //});

    //var menuItems = $("ul.menu li a");

    //menuItems.on("click", function () {
    //    alert($(this).text());
    //});

    var sideBarAndWrapper = $("#sidebar, #wrapper");
    var icon = $("#sideBarToggle i.fa");

    $("#sideBarToggle").on("click", function () {
        sideBarAndWrapper.toggleClass("hide-sidebar");

        if (sideBarAndWrapper.hasClass("hide-sidebar")) {
            icon.removeClass("fa-angle-left");
            icon.addClass("fa-angle-right");
        }
        else {
            icon.removeClass("fa-angle-right");
            icon.addClass("fa-angle-left");
        }
    });
   
})();