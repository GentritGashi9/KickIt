/*  ---------------------------------------------------
    Template Name: Local Direction
    Description: Local Direction HTML Template
    Author: Colorlib
    Author URI: https://www.colorlib.com
    Version: 1.0
    Created: Colorlib
---------------------------------------------------------  */

'use strict';

(function ($) {

    /*------------------
        Preloader
    --------------------*/
    $(window).on('load', function () {
        $(".loader").fadeOut();
        $("#preloder").delay(100).fadeOut("slow");
    });

    /*------------------
        Background Set
    --------------------*/
    $('.set-bg').each(function () {
        //var base_url = window.location.origin;
        //var getUrl = window.location;

       // var baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];
        
        var bg = $(this).data('setbg');
        var url = window.location.origin + "/"+bg;
        //console.log(url);
        $(this).css('background-image', 'url(' + url + ')');
    });
    $('.set-bg2').each(function () {
        //var base_url = window.location.origin;
        //var getUrl = window.location;

        // var baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];

        var bg = $(this).data('setbg2');
        var url = window.location.origin + "/" + bg;
        //console.log(url);
        $(this).css('background-image', 'url(' + url + ')');
    });
    $('.set-bg3').each(function () {
        //var base_url = window.location.origin;
        //var getUrl = window.location;

        // var baseUrl = getUrl.protocol + "//" + getUrl.host + "/" + getUrl.pathname.split('/')[1];

        var bg = $(this).data('setbg3');
        var url = window.location.origin + "/" + bg;
        //console.log(url);
        $(this).css('background-image', 'url(' + url + ')');
    });
    /*------------------
		Navigation
	--------------------*/
    $(".mobile-menu").slicknav({
        prependTo: '#mobile-menu-wrap',
        allowParentLinks: true
    });

    $('.slicknav_nav ul ').prepend('<li class="header-right-warp"></li>');
    $('.header-right').clone().prependTo('.slicknav_nav > ul > .header-right-warp');

    /*----------------------
        Testimonial Slider
    -----------------------*/
    $(".testimonial-item").owlCarousel({
        loop: true,
        margin: 0,
        nav: true,
        items: 1,
        dots: false,
        navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
        smartSpeed: 1200,
        autoplay: false,
    });

    /*------------------
        Magnific Popup
    --------------------*/
    $('.pop-up').magnificPopup({
        type: 'image'
    });

    /*-------------------
		Category Select
	--------------------- */
    $('.ca-search').niceSelect();

    /*-------------------
		Local Select
	--------------------- */
    $('.lo-search').niceSelect();

    /*-------------------
		Arrange Select
	--------------------- */
    $('.arrange-select select').niceSelect();

    /*-------------------
		Radio Btn
	--------------------- */
    $(".filter-left .category-filter .category-option .co-item label").on('click', function () {
        $(".filter-left .category-filter .category-option .co-item label").removeClass('active');
        $(this).addClass('active');
    });

    $(".filter-left .rating-filter .rating-option .ro-item label").on('click', function () {
        $(".filter-left .rating-filter .rating-option .ro-item label").removeClass('active');
        $(this).addClass('active');
    });

    $(".filter-left .distance-filter .distance-option .do-item label").on('click', function () {
        $(".filter-left .distance-filter .distance-option .do-item label").removeClass('active');
        $(this).addClass('active');
    });

})(jQuery);