// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$('.navbar-toggler').click(function() {
    if (window.innerWidth >= 0 && window.innerWidth < 520) {
        $('.nav-mobile').css('left', '0');
    }
    if (window.innerWidth >= 520 && window.innerWidth < 1201) {
        let a = ((window.innerWidth - 480) / 2);
        $('.nav-mobile').css('left', a + 'px');

    }
});


$('.navbar-toggler-admin').click(function() {
    if (window.innerWidth >= 0 && window.innerWidth < 520) {
        $('.nav-menu').css('left', '0');
    }
    if (window.innerWidth >= 520 && window.innerWidth < 1201) {
        let a = ((window.innerWidth - 480) / 2);
        $('.nav-menu').css('left', a + 'px');

    }
});

$('.close-admenu').click(function() {
    $('.nav-menu').hide();
});

$('.close-nav').click(function() {
    $('.nav-mobile').css('left', '-100%');
});
// banner
$(".banner").owlCarousel({
    items: 1,
    margin: 10,
    nav: false,
    dots: true,
    lazyLoad: true,
    responsiveClass: true,
});


$('.box-logout').hide();
$('.toggle-logout').css('cursor', 'pointer');

$('.toggle-logout').click(function() {
    $('.box-logout').toggle();
});