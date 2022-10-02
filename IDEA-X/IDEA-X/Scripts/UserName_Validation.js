$(document).ready(() => {


    $('.uname_box').keyup((e) => {
        $.ajax({
            type: 'POST',
            url: '/Home/UserNameValidation',
            data: { uname_text: e.currentTarget.value },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                $('.uname-box-validation').text(s.msg);
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

    document.querySelector(".unamesuccess").style.pointerEvents = "all";
}
function pointerOff() {

    document.querySelector(".unamesuccess").style.pointerEvents = "none";
}


function checkuname() {
    if (document.querySelector(".uname_box").value != "") {
    $.ajax({
        type: 'POST',
        url: '/Home/UserNameValidation',
        data: { uname_text: document.querySelector(".uname_box").value },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
            $('.uname-box-validation').text(s.msg);
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
    });
    }
}