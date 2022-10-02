$(document).ready(() => {

    $('.email_box').keyup((e) => {
        
        $.ajax({
            type: 'POST',
            url: '/Admin/EmailValidation',
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






function emailcheck() {
   // var email = document.querySelector(".email_box").value;
    
    if (document.querySelector(".email_box").value != "") {
        $.ajax({
            type: 'POST',
            url: '/Admin/EmailValidation',
            data: { mail_text: document.querySelector(".email_box").value },
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
    }
}