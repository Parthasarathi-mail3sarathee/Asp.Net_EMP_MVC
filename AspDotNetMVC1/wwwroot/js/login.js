
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
        dateFormat: "MMM dd, yyyy",
        changemonth: true,
        changeyear: true
    });

});
