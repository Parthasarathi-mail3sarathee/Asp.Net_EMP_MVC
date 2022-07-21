
$.noConflict();
jQuery(document).ready(function ($) {

    $(".TabLoginDetail").hide();
    $(".TabRegisterDetail").show();
    $(".TabForgotPassDetail").hide();
    if ($(':hidden#SessionSet').val() == "true") {

        $("#idlogin").show();
        $("#idlogout").hide();
    }
    if ($(':hidden#NotValidUser').val() == "true") {
        $(".TabMenuLogin").addClass("TabMenuTextSelected");
        $(".TabMenuLogin").removeClass("TabMenuTextNotSelected");
        $(".TabMenuForgotPass").addClass("TabMenuTextNotSelected");
        $(".TabMenuForgotPass").removeClass("TabMenuTextSelected");
        $(".TabMenuRegister").addClass("TabMenuTextNotSelected");
        $(".TabMenuRegister").removeClass("TabMenuTextSelected");

        $(".TabLoginDetail").show();
        $(".TabRegisterDetail").hide();
        $(".TabForgotPassDetail").hide();
    }
    $(".TabMenuLogin").click(function () {
        $(".TabMenuLogin").addClass("TabMenuTextSelected");
        $(".TabMenuLogin").removeClass("TabMenuTextNotSelected");
        $(".TabMenuForgotPass").addClass("TabMenuTextNotSelected");
        $(".TabMenuForgotPass").removeClass("TabMenuTextSelected");
        $(".TabMenuRegister").addClass("TabMenuTextNotSelected");
        $(".TabMenuRegister").removeClass("TabMenuTextSelected");

        $(".TabLoginDetail").show();
        $(".TabRegisterDetail").hide();
        $(".TabForgotPassDetail").hide();

    });

    $(".TabMenuForgotPass").click(function () {
        $(".TabMenuForgotPass").addClass("TabMenuTextSelected");
        $(".TabMenuForgotPass").removeClass("TabMenuTextNotSelected");
        $(".TabMenuLogin").addClass("TabMenuTextNotSelected");
        $(".TabMenuLogin").removeClass("TabMenuTextSelected");
        $(".TabMenuRegister").addClass("TabMenuTextNotSelected");
        $(".TabMenuRegister").removeClass("TabMenuTextSelected");


        $(".TabLoginDetail").hide();
        $(".TabRegisterDetail").hide();
        $(".TabForgotPassDetail").show();

    });


    $(".TabMenuRegister").click(function () {
        $(".TabMenuRegister").addClass("TabMenuTextSelected");
        $(".TabMenuRegister").removeClass("TabMenuTextNotSelected");
        $(".TabMenuLogin").addClass("TabMenuTextNotSelected");
        $(".TabMenuLogin").removeClass("TabMenuTextSelected");
        $(".TabMenuForgotPass").addClass("TabMenuTextNotSelected");
        $(".TabMenuForgotPass").removeClass("TabMenuTextSelected");


        $(".TabLoginDetail").hide();
        $(".TabRegisterDetail").show();
        $(".TabForgotPassDetail").hide();
    });

    $(".datepicker").datepicker({
        timepicker: false,
        dateFormat: "MM-dd-yyyy",
        changemonth: true,
        changeyear: true,
    });

    
    //$(".btnEdit").click(function (e) {
    //    // avoid to execute the actual submit of the form.
    //    var id = $(this).attr('id');
    //    var myArray = id.split("_");
    //    let word = myArray[1];
    //    alert(word);
    //    $.get("../Employee/EditEmployee/" + word);
    //});
    //$(".btnDel").click(function (e) {
    //    // avoid to execute the actual submit of the form.
    //    var id = $(this).attr('id');
    //    var myArray = id.split("_");
    //    let word = myArray[1];
    //    alert(word);
    //});

    $("#btnSave").click(function (e) {
        // avoid to execute the actual submit of the form.
        var form = $("#frmAddEmp");
        var data1 = form.serialize();
        $.ajax({
            type: "POST",
            url: "../Employee/SaveEmployee",
            data: data1, // serializes the form's elements.
            success: function (data) { // show response from the php script.
                $(".msgInner").text(data);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }       
        });
    });
});
