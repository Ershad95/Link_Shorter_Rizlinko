// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$("#usePassword").change(function () { $("#Password").slideToggle(); });
$("#usecustomShortLink").change(function () { $("#ShortUrl").slideToggle(); });


var typed = new Typed('.typed', {
    stringsElement: '#typed-strings',
    typeSpeed: 70,
    startDelay: 0,
    backSpeed: 35,
    shuffle: true,
    loop: true,
    loopCount: 1,
});

function Submit() {
    $("#waiting_bar").fadeIn();
}