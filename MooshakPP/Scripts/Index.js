$(document).ready(function(){
    $("#tabs").tabs();
   
    $(".btnNext").click(function () {
       
        $("#tabs").tabs("option", "active", $("#tabs").tabs('option', 'active') + 2);
    });
});