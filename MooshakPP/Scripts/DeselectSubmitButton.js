$(document).ready(function () {
    $('input#upld').change(function () {
        if ($(this).val()) {
            $('input.test').attr('disabled', false); 
        }
    });
});