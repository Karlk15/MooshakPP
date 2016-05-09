$(document).ready(function () {

    $('.interactiveList a').click(function (e) {

        $('selected').removeClass('selected');
        $(this).addClass('selected');
    });

    $('.datepicker').datepicker();
});

