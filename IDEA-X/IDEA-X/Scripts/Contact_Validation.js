var fname_flag = false;
var lname_flag = false;
var email_flag = false;
var msg_flag = false;
var uname_flag = false;

$(document).ready(() => {
    var fname = document.querySelector(".fname-nreg").value;
    var lname = document.querySelector(".lname-nreg").value;
    var email = document.querySelector(".email-nreg").value;
    var msg = document.querySelector(".msg-nreg").value;
    var uname = document.querySelector(".uname-reg").value;
    var msg_reg = document.querySelector(".msg-reg").value;
    var email_reg = document.querySelector(".email-reg").value;

    
   
    
   
    if (uname == "") {

    }
    if (email_reg == "") {

    }
    if (msg_reg == "") {

    }


    $('.email-nreg').keyup((e) => {
        document.getElementById("show-email").innerHTML = "";

        $.ajax({
            type: 'POST',
            url: '/Home/EmailValidation',
            data: { mail_text: e.currentTarget.value },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                $('.email-box-validation-nreg').text(s.msg);

                if (s.msg == "") {
                    email_flag = true;
                    NonRegOnClick();

                }
                else {
                    
                    email_flag = false;
                    NonRegOnClick();


                }
            },
            error: (err) => {
                console.log(err.msg);
            }
        })
    })


    $('.uname-reg').keyup((e) => {
        document.getElementById("show-uname-reg").innerHTML = "";

        $.ajax({
            type: 'POST',
            url: '/Home/UserNameValidationReg',
            data: { uname_text: e.currentTarget.value },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                $('.uname-box-validation').text(s.msg);
                if (s.msg == "") {
                    uname_flag = true;
                    RegOnClick();
                }
                else {

                    uname_flag = false;
                    RegOnClick();

                }
            },
            error: (err) => {
                console.log(err.msg);
            }
        })
    })


    $('.email-reg').keyup((e) => {
        document.getElementById("show-email-reg").innerHTML = "";

        $.ajax({
            type: 'POST',
            url: '/Home/EmailValidationReg',
            data: { mail_text: e.currentTarget.value },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                $('.email-box-validation-reg').text(s.msg);

                if (s.msg == "") {
                    email_flag = true;
                    RegOnClick();

                }
                else {
                    email_flag = false;
                    RegOnClick();


                }
            },
            error: (err) => {
                console.log(err.msg);
            }
        })
    })

});






function fnamecheck() {

    if (document.querySelector(".fname-nreg").value != "") {
        fname_flag = true;
        document.getElementById("show-fname").innerHTML = "";
        NonRegOnClick();
    }
    else {
        
            document.getElementById("show-fname").innerHTML = "This field is required.";
        fname_flag = false;
        NonRegOnClick();


    }
}


function lnamecheck() {
    if (document.querySelector(".lname-nreg").value != "") {
        lname_flag = true;
        document.getElementById("show-lname").innerHTML = "";

        NonRegOnClick();
    }
    else {
    
            document.getElementById("show-lname").innerHTML = "This field is required.";

        lname_flag = false;
        NonRegOnClick();

    }
}


function msgcheck() {
    if (document.querySelector(".msg-nreg").value != "") {
        msg_flag = true;
        document.getElementById("show-msg").innerHTML = "";
        NonRegOnClick();

    }
    else {
        document.getElementById("show-msg").innerHTML = "This field is required.";

        msg_flag = false;
        NonRegOnClick();


    }
}


function msgcheckreg() {

    if (document.querySelector(".msg-reg").value != "") {
        msg_flag = true;
        document.getElementById("show-msg-reg").innerHTML = "";
        RegOnClick();

    }
    else {
        document.getElementById("show-msg-reg").innerHTML = "This field is required.";
        msg_flag = false;
        RegOnClick();

    }
}

function emailcheck() {
    // var email = document.querySelector(".email_box").value;

    if (document.querySelector(".email-nreg").value != "") {
        document.getElementById("show-email").innerHTML = "";
        $.ajax({
            type: 'POST',
            url: '/Home/EmailValidation',
            data: { mail_text: document.querySelector(".email-nreg").value },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                $('.email-box-validation-nreg').text(s.msg);

                if (s.msg == "") {
                    email_flag = true;
                    NonRegOnClick();

                }
                else {
                    email_flag = false;
                    NonRegOnClick();

                }
            },
            error: (err) => {
                console.log(err.msg);
            }
        })
    }
}




const nonregdiv = document.getElementById("non-register-div");
const regdiv = document.getElementById("register-div");
const success = document.getElementById("send-success");
nonregdiv.hidden = false;
success.hidden = true;
regdiv.hidden = true;
const sel1 = document.getElementById("non-register-div-sel");
const sel2 = document.getElementById("register-div-sel");
function shownonReg() {
    nonregdiv.hidden = false;
    regdiv.hidden = true;
}
function showReg() {
    nonregdiv.hidden = true;
    regdiv.hidden = false;
}


function emailcheckreg() {
    // var email = document.querySelector(".email_box").value;

    if (document.querySelector(".email-nreg").value != "") {
        document.getElementById("show-email-reg").innerHTML = "";
        $.ajax({
            type: 'POST',
            url: '/Home/EmailValidationReg',
            data: { mail_text: document.querySelector(".email-nreg").value },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                $('.email-box-validation-nreg').text(s.msg);

                if (s.msg == "") {
                    email_flag = true;
                    NonRegOnClick();

                }
                else {
                    email_flag = false;
                    NonRegOnClick();

                }
            },
            error: (err) => {
                console.log(err.msg);
            }
        })
    }
}

function checkunamereg() {
    if (document.querySelector(".uname-reg").value != "") {
        document.getElementById("show-uname-reg").innerHTML = "";

        $.ajax({
            type: 'POST',
            url: '/Home/UserNameValidationReg',
            data: { uname_text: document.querySelector(".uname-reg").value },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                $('.uname-box-validation').text(s.msg);
                if (s.msg == "") {
                    uname_flag = true;
                    RegOnClick();

                }
                else {
                    uname_flag = false;
                    RegOnClick();


                }
            },
            error: (err) => {
                console.log(err.msg);
            }
        });
    }
}


function RegOnClick() {
    if (uname_flag == true && email_flag == true && msg_flag == true) {
        document.querySelector(".emailsuccess").style.pointerEvents = "all";

    }
    else {
        document.querySelector(".emailsuccess").style.pointerEvents = "none";

    }
}

function sendReg() {
    var uname = document.querySelector(".uname-reg").value;
    var email = document.querySelector(".email-reg").value;
    var msg = document.querySelector(".msg-reg").value;

    $.ajax({
        type: 'POST',
        url: '/Home/ContactReg',
        data: { USERNAME: uname, EMAIL:email, MESSAGE:msg},
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
            if (s.msg == "DoNotSave") {
                document.getElementById("show-result-reg").innerHTML = "Message is sent 3 times using this email. Please be patience and wait for response.";

            }
            else if (s.msg == "UserNotMatch") {
                document.getElementById("show-result-reg").innerHTML = "The UserName or Email did not match. Please try again.";

            }
            else {
                nonregdiv.hidden = true;
                success.hidden = false;
                regdiv.hidden = true;

            }

        },
        error: (err) => {
            alert(err.msg);
        }
    });

}

function NonRegOnClick() {
    if (fname_flag == true && lname_flag == true && email_flag == true && msg_flag == true) {
        document.querySelector(".nreg-emailsuccess").style.pointerEvents = "all";

    }
    else {
        document.querySelector(".nreg-emailsuccess").style.pointerEvents = "none";

    }
}
function sendNonReg() {
    var fname = document.querySelector(".fname-nreg").value;
    var lname = document.querySelector(".lname-nreg").value;
    var email = document.querySelector(".email-nreg").value;
    var msg = document.querySelector(".msg-nreg").value;
   

    
       

        $.ajax({
            type: 'POST',
            url: '/Home/ContactNonReg',
            data: { FIRST_NAME: fname, LAST_NAME: lname, EMAIL: email, MESSAGE: msg },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                if (s.msg == "DoNotSave") {
                    document.getElementById("show-result").innerHTML = "Message is sent 3 times using this email. Please be patience and wait for response.";

                }
                else if (s.msg == "DoNotMatch") {
                    document.getElementById("show-result").innerHTML = "The UserName or Email did not match. Please try again.";

                }
                else {
                    nonregdiv.hidden = true;
                    success.hidden = false;
                    regdiv.hidden = true;

                }

            },
            error: (err) => {
                alert(err.msg);
            }
        });
    

}