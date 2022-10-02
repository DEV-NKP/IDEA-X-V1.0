$(document).ready(() => {
        $.ajax({
            type: 'POST',
            url: '/User/EditPostRequest',
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
   
});