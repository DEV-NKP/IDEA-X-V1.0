

var value1 = "";

$(document).ready(() => {

    $('.search_box').keyup((e) => {
        let text = e.currentTarget.value;

        if (value1 !== text) {

            value1 = text;
            $.ajax({
                type: 'POST',
                url: '/Admin/SearchSolvedMessage',
                data: { text: e.currentTarget.value },
                async: true,
                datatype: 'json',
                contenttype: "application/json; charset=utf-8",
                success: (s) => {
                    $('.table-body').empty();
                    var count = 0;
                    if (s.LIST.length > 0) {
                        s.LIST.forEach(val => {
                      


                            count++;
                            if ( val.LEVEL == "USER"   ||  val.LEVEL == "BANNED" )
                        {
                            $('.table-body').prepend(`
                           <tr>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.CONTACT_ID}</td>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.FIRST_NAME}</td>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.LAST_NAME}</td>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.USERNAME}</td>
                          <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.EMAIL}</td>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.MESSAGE}</td>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.LEVEL}</td>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.STATUS}</td>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.LOGIN_TIME}</td>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=PendingMessage')" style="cursor: pointer; ">${val.LOGIN_IP}</td>
    <td align="center">
                            <button onclick="MessageSolve(${val.CONTACT_ID})" id="call" style="margin-right: 20px; padding-left: 20px; padding-right: 20px; padding-top: 10px; padding-bottom: 10px; text-decoration: none; background: linear-gradient(135deg, rgba(255,255,255,0.3), rgba(255,255,255,0.1)); border-radius: 10px; font-size: 18px; color: #ee7e7e; cursor:pointer;" >Delete Message</button>
                        </td>
                    </tr>
`);

                        }
                        if ( val.LEVEL == "NONREG"   ||  val.LEVEL == "UAC" )

            {

                $('.table-body').prepend(`
                       <tr>
                            <td >${val.CONTACT_ID}</td>
                            <td >${val.FIRST_NAME}</td>
                            <td >${val.LAST_NAME}</td>
                            <td >${val.USERNAME}</td>
                          <td >${val.EMAIL}</td>
                            <td >${val.MESSAGE}</td>
                            <td >${val.LEVEL}</td>
                            <td >${val.STATUS}</td>
                            <td >${val.LOGIN_TIME}</td>
                            <td >${val.LOGIN_IP}</td>
    <td align="center">
                            <button onclick="MessageSolve(${val.CONTACT_ID})" id="call" style="margin-right: 20px; padding-left: 20px; padding-right: 20px; padding-top: 10px; padding-bottom: 10px; text-decoration: none; background: linear-gradient(135deg, rgba(255,255,255,0.3), rgba(255,255,255,0.1)); border-radius: 10px; font-size: 18px; color: #ee7e7e; cursor:pointer;" >Delete Message</button>
                        </td>
                         </tr>
`);
            }




        })
}
                    else {
        $('.table-body').empty();
                    }
                    document.getElementById("show-result").innerHTML = "Total " + count + " results found";
                },
        error: (err) => {
        alert(err.msg);
    }
            })
        }



    })
});










function MessageDelete(id) {

    $.ajax({
        type: 'POST',
        url: '/Admin/MessageDelete',
        data: { rid: id },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
            window.location.reload();
        },
        error: (err) => {
            alert(err.msg);
        }
    });

}