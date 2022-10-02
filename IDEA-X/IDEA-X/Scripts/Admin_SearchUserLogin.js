var value1 = "";

$(document).ready(() => {

    $('.search_box').keyup((e) => {
        let text = e.currentTarget.value;
        if (value1 !== text) {
            value1 = text;
            $.ajax({
            type: 'POST',
            url: '/Admin/SearchUserLogin',
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
   <tr onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=UserLogIn')" style="cursor: pointer; ">
${count++}
                            <td>${val.LOGIN_ID}</td>
                            <td>${val.USERNAME}</td>
                            <td>${val.EMAIL}</td>
                            <td>${val.LOGIN_TIME}</td>
                            <td>${val.LOGIN_IP}</td>
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
