﻿var value1 = "";

$(document).ready(() => {

    $('.search_box').keyup((e) => {
        let text = e.currentTarget.value;
        if (value1 !== text) {
            value1 = text;
            $.ajax({
                type: 'POST',
                url: '/Admin/SearchMessageRequests',
                data: { text: e.currentTarget.value },
                async: true,
                datatype: 'json',
                contenttype: "application/json; charset=utf-8",
                success: (s) => {
                    $('.table-body').empty();
                    var count = 0;
                    if (s.LIST.length > 0) {
                        s.LIST.forEach(val => {

                            $('.table-body').prepend(`
   <tr>${count++}
                            <td>${val.REQUEST_ID}</td>
                            <td  onclick="window.location.replace('/Admin/UserProfile/${val.SENDER}?page=MessageRequests')" style="cursor: pointer; ">${val.SENDER}</td>
                            <td  onclick="window.location.replace('/Admin/UserProfile/${val.RECEIVER}?page=MessageRequests')" style="cursor: pointer; ">${val.RECEIVER}</td>
                            <td>${val.MESSAGE_TIME}</td>
                           
                        </tr>
`);
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
