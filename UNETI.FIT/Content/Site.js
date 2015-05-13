$(document).ready(function () {
    $('#navScroll').on('click', 'li a', function () {
        var el = $(this);
        var name = el.attr('href');
        $('html, body').scrollTop($(name).offset().top);
    })
})