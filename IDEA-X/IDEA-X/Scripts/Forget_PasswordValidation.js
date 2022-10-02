$(document).ready(() => {

    $('.email_box').keyup((e) => {
        $.ajax({
            type: 'POST',
            url: '/Home/EmailValidationForget',
            data: { mail_text: e.currentTarget.value },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                $('.email-box-validation').text(s.msg);

                if (s.msg == "") {
                    pointerOn();
                }
                else {
                    pointerOff();
                }
            },
            error: (err) => {
                console.log(err.msg);
            }
        })
    })
});

function pointerOn() {

    document.querySelector(".emailsuccess").style.pointerEvents = "all";
}
function pointerOff() {

    document.querySelector(".emailsuccess").style.pointerEvents = "none";
}

var code = "";

function sendcode() {
    var email = document.querySelector(".email_box");
    document.querySelector(".emailsuccess").style.pointerEvents = "none";
    document.querySelector(".emailsuccess").style.display = "none";

    $.ajax({
        type: 'POST',
        url: '/Home/SendCode',
        data: { remail: email.value },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
            code = s.msg;
            alert("Mail Send to your provided Email Address.")

        },
        error: (err) => {
            console.log(err.msg);
        }
    });

}



function clickEvent(first, last) {

    if (first.value.length) {   
        
        document.getElementById(last).focus();
        checkCode();
    }
}

function redirecPage() {
    checkCode();
}


function checkCode() {
   
    var box1 = document.getElementById("ist").value.toLocaleUpperCase();
    var box2 = document.getElementById("sec").value.toLocaleUpperCase();
    var box3 = document.getElementById("third").value.toLocaleUpperCase();
    var box4 = document.getElementById("fourth").value.toLocaleUpperCase();
    var box5 = document.getElementById("fifth").value.toLocaleUpperCase();
    var allboxs = box1 + box2 + box3 + box4 + box5;
  
    if (allboxs == code) {
 
        document.querySelector(".verify-valid").innerHTML = "Verified Email";
        document.getElementById("ist").readOnly = true;
        document.getElementById("sec").readOnly = true;
        document.getElementById("third").readOnly = true;
        document.getElementById("fourth").readOnly = true;
        document.getElementById("fifth").readOnly = true;

        window.location.href = ("/Home/ForgotChangePassword?id=" + document.querySelector(".email_box").value);

    }
    else {

    
      
        document.querySelector(".verify-valid").innerHTML = "Verification failed";
        document.getElementById("ist").readOnly = false;
        document.getElementById("sec").readOnly = false;
        document.getElementById("third").readOnly = false;
        document.getElementById("fourth").readOnly = false;
        document.getElementById("fifth").readOnly = false;
    }

}

