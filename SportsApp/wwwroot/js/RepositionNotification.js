$(document).ready(function () {
    if (window.innerWidth < 992) {
        $("#navbarSupportedContent-5").appendTo("#repositionedNotification");
    } else {
        $("#navbarSupportedContent-5").appendTo("#normalNotification");
    }
});
$(window).resize(function () {
    if (window.innerWidth < 992) {
        $("#navbarSupportedContent-5").appendTo("#repositionedNotification");
    }
    else{
        $("#navbarSupportedContent-5").appendTo("#normalNotification");
    }
});