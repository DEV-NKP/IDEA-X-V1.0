$(document).ready(() => {

    $('.email_box').keyup((e) => {
        $.ajax({
            type:'POST',
            url: '/Home/EmailValidation',
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
    document.querySelector(".emailsuccess").animate(
        {
            filter: ['hue-rotate(50deg)', 'hue-rotate(50deg)']
        },
        {
            duration: 2500,
            iterations: Infinity
        }
    );
    document.querySelector(".email_box").readOnly = true;
    toastr.info("Sending verfication mail.Please wait...","Sending Mail",{
        positionClass: 'toast-bottom-left',
        showMethod: 'slideDown', closeMethod: 'slideUp', hideMethod: 'slideUp',
    })

    $.ajax({
        type: 'POST',
        url: '/Home/SendCode',
        data: { remail: email.value },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
            //code = s.msg;
            //alert("Mail Send to your provided Email Address.")
            if (s.ret) {
                toastr.remove();
                code = s.msg;
                toastr.success("Email Send Sucessfully", "Success", {
                    positionClass: 'toast-bottom-left',
                    timeOut: 5000, closeButton: true,
                    showMethod: 'slideDown', closeMethod: 'slideUp', hideMethod: 'slideUp',
                })
            }
          
        },
        error: (err) => {
            console.log(err.msg);
        }
    });
 
}


var loadFile = function (event) {
    var image = document.getElementById('output');
    image.src = URL.createObjectURL(event.target.files[0]);
};

function clickEvent(first, last) {
    if (first.value.length) {
        checkCode();
        document.getElementById(last).focus();
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
        document.querySelector(".codesuccess").style.pointerEvents = "all";
        document.querySelector(".verify-valid").innerHTML = "Verified Email";
        document.querySelector(".email_box").readOnly = true;
      
        document.getElementById("ist").readOnly = true;
        document.getElementById("sec").readOnly = true;
        document.getElementById("third").readOnly = true;
        document.getElementById("fourth").readOnly = true;
        document.getElementById("fifth").readOnly = true;
    }
    else {
        document.querySelector(".codesuccess").style.pointerEvents = "none";
        document.querySelector(".verify-valid").innerHTML = "Verification failed";
        document.getElementById("ist").readOnly = false;
        document.getElementById("sec").readOnly = false;
        document.getElementById("third").readOnly = false;
        document.getElementById("fourth").readOnly = false;
        document.getElementById("fifth").readOnly = false;
    }

}


function emailcheck() {

    if (document.querySelector(".email_box").value != "") {
    $.ajax({
        type: 'POST',
        url: '/Home/EmailValidation',
        data: { mail_text: document.querySelector(".email_box").value },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
            $('.email-box-validation').text(s.msg);

            if (s.msg == "") {
                pointerOn();
                checkCode();
                document.querySelector(".emailsuccess").style.display = "none";
             
            }
            else {
                pointerOff();
                document.querySelector(".emailsuccess").style.display = "block";
            }
        },
        error: (err) => {
            console.log(err.msg);
        }
    })
}
}