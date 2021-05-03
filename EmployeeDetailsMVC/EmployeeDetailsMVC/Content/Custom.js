/// <reference path="../scripts/jquery-3.3.1.intellisense.js" />
/// <reference path="../scripts/jquery-3.3.1.js" />


    jQuery(document).ready(function ($){
        $("#btnSubmit").mouseover(function () {
            $("#btnSubmit").css("backgroundColor", "blue");
        });

        $("#btnSubmit").mouseout(function () {
            $("#btnSubmit").css("backgroundColor", "grey");
        });
    });

 
   
