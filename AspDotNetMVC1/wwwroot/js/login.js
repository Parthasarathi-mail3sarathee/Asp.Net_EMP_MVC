
$.noConflict();
jQuery(document).ready(function ($) {

    var pager;
    $('#validTxtSkill').hide();
    $('#validTxtemail').hide();
    $('#validTxtRole').hide();
    if ($(':hidden#SessionSet').val() == "true") {

        $("#idlogin").show();
        $("#idlogout").hide();
    }

    $(".TabLoginDetail").hide();
    $(".TabRegisterDetail").show();
    $(".TabForgotPassDetail").hide();
    if ($(':hidden#NotValidUser').val() == "true") {
        $(".tabmenulogin").addclass("tabmenutextselected");
        $(".tabmenulogin").removeclass("tabmenutextnotselected");
        $(".tabmenuforgotpass").addclass("tabmenutextnotselected");
        $(".tabmenuforgotpass").removeclass("tabmenutextselected");
        $(".tabmenuregister").addclass("tabmenutextnotselected");
        $(".tabmenuregister").removeclass("tabmenutextselected");

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
                pager = data1;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("Status: " + textStatus);
                console.log("Error: " + errorThrown);
            }
        });
    }

    $(document).on('click', '.pgrPrv', function () {

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
                console.log("Status: " + textStatus);
                console.log("Error: " + errorThrown);
            }
        });

    });

    $(document).on('click', '.pgrNxt', function () {

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
                console.log("Status: " + textStatus);
                console.log("Error: " + errorThrown);
            }
        });
    });

    function getParameterByName(name, url) {
        name = name.replace(/[\[\]]/g, '\\$&');
        var regex = new RegExp('[?&]' + name + '(=([^&#]*)|&|#|$)'),
            results = regex.exec(url);
        if (!results) return null;
        if (!results[2]) return '';
        return decodeURIComponent(results[2].replace(/\+/g, ' '));
    }

    $(document).on('click', 'a.downloadfile', function (e) {
        e.preventDefault();

        var href = $(this).attr('href');

        var filename = getParameterByName('fileName', href);
        var studid = getParameterByName('studid', href);

        //var xhr = new XMLHttpRequest();
        //xhr.onreadystatechange = function () {
        //    if (this.readyState == 4 && this.status == 200) {
        //        //this.response is what you're looking for

        //        var blob = new Blob([this.response], { type: "application/octet-stream" });

        //            //Check the Browser type and download the File.
        //            var isIE = false || !!document.documentMode;
        //            if (isIE) {
        //                window.navigator.msSaveBlob(blob, fileName);
        //            } else {
        //                var url = window.URL || window.webkitURL;
        //                link = url.createObjectURL(blob);
        //                var a = $("<a />");
        //                a.attr("download", filename);
        //                a.attr("href", link);
        //                $("body").append(a);
        //                a[0].click();
        //                $("body").remove(a);
        //            }
        //    }
        //}
        //xhr.open('GET', href);
        //xhr.responseType = 'blob';
        //xhr.send();   


        var a = document.createElement('a');
        if (window.URL && window.Blob && ('download' in a) && window.atob) {
            // Do it the HTML5 compliant way
            $.ajax({
                type: "GET",
                url: href,
                cache: false,
                xhr: function () {// Seems like the only way to get access to the xhr object
                    var xhr = new XMLHttpRequest();
                    xhr.responseType = 'blob'
                    return xhr;
                },
                success: function (result) { // show response from the php script.

                    var blob = new Blob([result], { type: "application/octet-stream" });

                    //Check the Browser type and download the File.
                    var isIE = false || !!document.documentMode;
                    if (isIE) {
                        window.navigator.msSaveBlob(blob, fileName);
                    } else {
                        var url = window.URL || window.webkitURL;
                        link = url.createObjectURL(blob);
                        var a = $("<a />");
                        a.attr("download", filename);
                        a.attr("href", link);
                        $("body").append(a);
                        a[0].click();
                        $("body").remove(a);
                    }
                    console.log(result);
                    //let blob = new Blob([result], { type: "application/octet-stream" });

                    //let a = document.createElement('a');
                    //a.href = window.URL.createObjectURL(blob);
                    //a.download = filename;
                    //document.body.appendChild(a);
                    //a.click();
                    //document.body.removeChild(a);
                    //window.URL.revokeObjectURL(a.href);
                    filname = "";
                    //var blob = base64ToBlob(result.download.data, result.download.mimetype);
                    //var url = window.URL.createObjectURL(blob);
                    //a.href = url;
                    //a.download = result.download.filename;
                    //a.click();
                    //window.URL.revokeObjectURL(url);
                },
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    console.log("Status: " + textStatus);
                    console.log("Error: " + errorThrown);
                }
            });
        }
    });

    $(document).on('click', '.pgr', function () {
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
                console.log("Status: " + textStatus);
                console.log("Error: " + errorThrown);
            }
        });
    });

    $(document).on('click', '.popup', function () {
        var val = $(this).attr('id').split("_");

        //$("#Divfileset").append('<span class="skilsetSpan">' + skill + ' <button type="button" class="removeSkill">X</button></span>');
        var data1 = { studid: val[1] }
        $.ajax({
            type: "POST",
            url: "../Employee/GetStudentFileListByID/" + val[1],
            data: data1, // serializes the form's elements.
            success: function (ajaxData) { // show response from the php script.
                //$('#PaginationContainer').html(ajaxData);
                //GetEmp(data1);   
                $("#Divfileset").append("<span>StudentID: " + val[1] + "</span><ul></ul>");
                $.each(ajaxData, function (index) {
                    var anchorlink = "GetthisStudentFile/?studid=" + val[1] + "&fileName=" + ajaxData[index];
                    $("#Divfileset>ul").append('<li><span class="skilsetSpan"><a class="downloadfile" studid="' + val[1] + '" fileName="' + ajaxData[index] + '" href="' + anchorlink + '" download>' + ajaxData[index] + ' </a><button type="button" class="removeSkill">X</button></span></li>');

                });
                var modal = $("#myModal");
                modal.css("display", "block");
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
                console.log("Status: " + textStatus);
                console.log("Error: " + errorThrown);
            }
        });
    });

    $(document).on('click', '.close', function () {
        var modal = $("#myModal");
        modal.css("display", "none");
        $("#Divfileset").empty();
    });


    // When the user clicks anywhere outside of the modal, close it

    //$(".btnEdit").click(function (e) {
    //    // avoid to execute the actual submit of the form.
    //    var id = $(this).attr('id');
    //    var myArray = id.split("_");
    //    let word = myArray[1];
    //    alert(word);
    //    $.get("../Employee/EditEmployee/" + word);
    //});




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

    $(document).on('click', '#btnSave', function (e) {
        // avoid to execute the actual submit of the form.
        var isValid = true;
        var skilset = getSkills();
        if (skilset.length < 1) {
            isValid = false;
            $("#txtSkill").css({
                "border": "1px solid red",
                "background": "#FFCECE"
            });
            $('#validTxtSkill').show();
        } else {
            $("#txtSkill").css({
                "border": "",
                "background": ""
            });
            $('#validTxtSkill').hide();
        }
        var email = /^[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}$/;
        if ($("#txtEmail").val() == '' || !email.test($("#txtEmail").val())) {
            isValid = false;
            $("#txtEmail").css({
                "border": "1px solid red",
                "background": "#FFCECE"
            });
            $('#validTxtemail').show();
        }
        else {
            $("#txtEmail").css({
                "border": "",
                "background": ""
            });
            $('#validTxtemail').hide();
        }

        if ($("#txtrole").val() == '' || $("#txtrole").val().length > 6) {
            isValid = false;
            $("#txtrole").css({
                "border": "1px solid red",
                "background": "#FFCECE"
            });
            $('#validTxtRole').show();
        }
        else {
            $("#txtrole").css({
                "border": "",
                "background": ""
            });
            $('#validTxtRole').hide();
        }

        $('#txtName,#txtAdrs,#txtrole,#txtdpt,#txtDOB,#txtDOJ').each(function () {

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

            var formData = new FormData();// for file upload multipart form data
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
            for (var i = 0; i < $("#profileFile").get(0).files.length; ++i) {
                formData.append('profileFile', $("#profileFile").get(0).files[i]);
            }

            // formData.append('SkillSets', skilset); // will be worked as comma seperated ".Net,java,spring"
            for (var i = 0; i < skilset.length; ++i) {
                formData.append('SkillSets', skilset[i]);// will be take it as a list object in server side
            }

            formData.append('ID', id);
            formData.append('Name', name);
            formData.append('Address', address);
            formData.append('Role', role);
            formData.append('Department', dept);
            formData.append('Email', emails);
            formData.append('DOB', dob);
            formData.append('DOJ', doj);

            $.ajax({
                type: "POST",
                cache: false,
                contentType: false,
                processData: false,
                url: "../Employee/SaveEmployee",
                data: formData, // serializes the form's elements.
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
