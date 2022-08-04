
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

    function GetEmp(data1) {
        $.ajax({
            type: "POST",
            url: "../Employee/GetEmpList",
            data: data1, // serializes the form's elements.
            success: function (ajaxData) { // show response from the php script.
                $('#EmpListContainer').html(ajaxData);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }
        });
    }

    $(".pgrPrv").click(function () {
        //alert($(".ddlPgsize option:selected").text());
        //alert($(this).text());
        //alert($("a.active").text());
        var currentPage = parseInt($("a.active").text(), 10) - 1;
        var PageSize = parseInt($(".ddlPgsize option:selected").text(), 10);
        var pgCount = $("#pgCount").val();

        var data1 = { currentPage: currentPage, PageSize: PageSize, pageCount: pgCount }
        $.ajax({
            type: "POST",
            url: "../Employee/Get_Pager",
            data: data1, // serializes the form's elements.
            success: function (ajaxData) { // show response from the php script.
                $('#PaginationContainer').html(ajaxData);
                GetEmp(data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }
        });

    });
    $(".pgrNxt").click(function () {
        var currentPage = parseInt($("a.active").text(), 10) + 1;
        var PageSize = parseInt($(".ddlPgsize option:selected").text(), 10);
        var pgCount = $("#pgCount").val();

        var data1 = { currentPage: currentPage, PageSize: PageSize, pageCount: pgCount }
        $.ajax({
            type: "POST",
            url: "../Employee/Get_Pager",
            data: data1, // serializes the form's elements.
            success: function (ajaxData) { // show response from the php script.
                $('#PaginationContainer').html(ajaxData);
                GetEmp(data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }
        });
    });
    $(".pgr").click(function () {
        var currentPage = parseInt($(this).text(), 10);
        var PageSize = parseInt($(".ddlPgsize option:selected").text(), 10);
        var pgCount = $("#pgCount").val();

        //$('#PaginationContainer').html(ajaxData);
        var data1 = { currentPage: currentPage, PageSize: PageSize, pageCount: pgCount }
        $.ajax({
            type: "POST",
            url: "../Employee/Get_Pager",
            data: data1, // serializes the form's elements.
            success: function (ajaxData) { // show response from the php script.
                $('#PaginationContainer').html(ajaxData);
                GetEmp(data1);
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                alert("Status: " + textStatus); alert("Error: " + errorThrown);
            }
        });
    });
    //$(".btnEdit").click(function (e) {
    //    // avoid to execute the actual submit of the form.
    //    var id = $(this).attr('id');
    //    var myArray = id.split("_");
    //    let word = myArray[1];
    //    alert(word);
    //    $.get("../Employee/EditEmployee/" + word);
    //});



    function validate() {
        var isValid = false;
        alert("Validation start");
        var val = $('#txtName').value;
        alert(val);
        if (val == '') {
            alert("Validation start");
            $('#validTxtName').text("Name field is required");
            isValid = false;
        }
        return isValid;
    }

    function validMail(email) {
        return email.match(/^(([^<>()\[\]\.,;:\s@\"]+(\.[^<>()\[\]\.,;:\s@\"]+)*)|(\".+\"))@(([^<>()\.,;\s@\"]+\.{0,1})+([^<>()\.,;:\s@\"]{2,}|[\d\.]+))$/);
    }
    $("#tbnskilladd").click(function (e) {
        e.preventDefault();
        var skill = $("#txtSkill").val();
        if (skill != '') {
            $("#DivSkillset").append('<span class="skilsetSpan">' + skill + ' <button type="button" class="removeSkill">X</button></span>');
            $("#txtSkill").val('');
        }
    });

    // This is for dynamically added button at client side
    $(document).on('click', '.removeSkill', function () {
        var ele = $(this).parent();
        ele.remove();
        // Your Code
    })
    function getSkills() {
        var skilset = [];
        var spanitem = $("#DivSkillset").children("span.skilsetSpan");
        var length = spanitem.filter('span').length;
        spanitem.each(function (index) {
            var txt = $(this).text().split(" ");
            skilset.push(txt[0]);
        });
        return skilset;
    }

    $("#btnSave").click(function (e) {
        // avoid to execute the actual submit of the form.
        var isValid = true;
        var skilset = getSkills();
        if (skilset.length < 1) {
            isValid = false;
            $("#txtSkill").css({
                "border": "1px solid red",
                "background": "#FFCECE"
            });
        } else {
            $("#txtSkill").css({
                "border": "",
                "background": ""
            });
        }
        $('#txtName,#txtAdrs,#txtrole,#txtdpt,#txtEmail,#txtDOB,#txtDOJ').each(function () {

            if ($.trim($(this).val()) == '') {

                isValid = false;
                $(this).css({
                    "border": "1px solid red",
                    "background": "#FFCECE"
                });
            }
            else {
                $(this).css({
                    "border": "",
                    "background": ""
                });
            }
        });
        if (isValid == false) {
            e.preventDefault();
        }
        else {
            var form = $("#frmAddEmp");
            var name = $("#txtName").val();
            var id = $("#ID").val();
            var address = $("#txtAdrs").val();
            var role = $("#txtrole").val();
            var dept = $("#txtdpt").val();
            var emails = $("#txtEmail").val();
            var dob = $("#txtDOB").val();
            var doj = $("#txtDOJ").val();
            var skilset = getSkills();

            var data1 = form.serialize();
            var data2 = {
                ID: id,
                Name: name,
                Address: address,
                Role: role,
                Department: dept,
                Email: emails,
                DOB: dob,
                DOJ: doj,
                SkillSets: skilset

            }
            $.ajax({
                type: "POST",
                url: "../Employee/SaveEmployee",
                data: data2, // serializes the form's elements.
                success: function (data) { // show response from the php script.
                    $(".msgInner").text(data);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    alert("Status: " + textStatus); alert("Error: " + errorThrown);
                }
            });
        }

    });
});
