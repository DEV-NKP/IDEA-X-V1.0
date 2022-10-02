function changeClick() {
   
   var pass= document.querySelector(".password");
  var newpass=document.querySelector(".newpassword");
    
    $.ajax({
        type: 'POST',
        url: '/User/RequestPassChange',
        data: { password: pass.value, newpassword: newpass.value },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
         
            document.querySelector(".pass-box-validation").innerHTML = s.msg;
            if (s.msg == "") {
                alert("Password updated successfuly");
                window.location.replace("/User/Profile");

            }
            else {
            
            }
        },
        error: (err) => {
            
            console.log(err.msg);
        }
    });

}




function AdminchangeClick() {
    var pass = document.querySelector(".password");
    var newpass = document.querySelector(".newpassword");

    $.ajax({
        type: 'POST',
        url: '/Admin/RequestPassChange',
        data: { password: pass.value, newpassword: newpass.value },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {

            document.querySelector(".pass-box-validation").innerHTML = s.msg;
            if (s.msg == "") {
                alert("Password updated successfuly");
                window.location.replace("/Admin/Profile");

            }
            else {

            }
        },
        error: (err) => {

            console.log(err.msg);
        }
    });

}


function UACchangeClick() {
    var pass = document.querySelector(".password");
    var newpass = document.querySelector(".newpassword");

    $.ajax({
        type: 'POST',
        url: '/UAC/RequestPassChange',
        data: { password: pass.value, newpassword: newpass.value },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {

            document.querySelector(".pass-box-validation").innerHTML = s.msg;
            if (s.msg == "") {
                alert("Password updated successfuly");
                window.location.replace("/UAC/Profile");

            }
            else {

            }
        },
        error: (err) => {

            console.log(err.msg);
        }
    });

}


function verifyClick() {
    var pass = document.querySelector(".password");

    $.ajax({
        type: 'POST',
        url: '/User/checkPass',
        data: { password: pass.value},
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {

            document.querySelector(".pass-box-validation").innerHTML = s.msg;
            if (s.msg == "") {

                //alert("Password is matched.");
                window.location.replace("/User/logInfoView");
            }
            else {

            }
        },
        error: (err) => {

            console.log(err.msg);
        }
    });

}


function verifyClickPost() {
    var pass = document.querySelector(".passwordmsg");
    $.ajax({
        type: 'POST',
        url: '/User/checkPass',
        data: { password: pass.value},
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {

            document.querySelector(".verifymsg").innerHTML = s.msg;
            if (s.msg == "") {

                //alert("Password is matched.");
                window.location.replace("/User/postInfoView");
            }
            else {

            }
        },
        error: (err) => {
            //alert("ERROR");
            console.log(err.msg);
        }
    });

}


function verifyClickRep() {
    var pass = document.querySelector(".reppass");
    $.ajax({
        type: 'POST',
        url: '/User/checkPass',
        data: { password: pass.value },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {

            document.querySelector(".passbox").innerHTML = s.msg;
            if (s.msg == "") {

                //alert("Password is matched.");
                window.location.replace("/User/repInfoView");
            }
            else {

            }
        },
        error: (err) => {
            //alert("ERROR");
            console.log(err.msg);
        }
    });

}
    function verifyClickUser() {
        var pass = document.querySelector(".repuser");
        $.ajax({
            type: 'POST',
            url: '/Admin/checkUser',
            data: { password: pass.value },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {

                document.querySelector(".userbox").innerHTML = s.msg;
                if (s.msg == "") {

                    //alert("Password is matched.");
                    window.location.replace("/User/repInfoView");
                }
                else {

                }
            },
            error: (err) => {
                //alert("ERROR");
                console.log(err.msg);
            }
        });

    }
