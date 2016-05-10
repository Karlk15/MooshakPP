$(document).ready(function () {

    $('.interactivelist a').click(function (e) {

        $('selected').removeClass('selected');
        $(this).addClass('selected');
    });

});