$(document).ready(function () {

    $('.interactiveList a').click(function (e) {

        $('selected').removeClass('selected');
        $(this).addClass('selected');
    });

    $(function () {
        $('#datepicker').datepicker();
    });
});

