var value1 = "";

$(document).ready(() => {

    $('.search_box').keyup((e) => {
        let text = e.currentTarget.value;
        if (value1 !== text) {
            value1 = text;
            $.ajax({
                type: 'POST',
                url: '/Admin/SearchUserList',
                data: { text: e.currentTarget.value },
                async: true,
                datatype: 'json',
                contenttype: "application/json; charset=utf-8",
                success: (s) => {
                    $('.table-body').empty();
                    var count = s.LIST.length+1;
                    if (s.LIST.length > 0) {
                        s.LIST.forEach(val => {
                            count=count-1;
                            $('.table-body').prepend(`
   <tr onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=UserList')" style="cursor: pointer; ">
                            <td>${count}</td>
                            <td>${val.USERNAME}</td>
                            <td>${val.EMAIL}</td>
                            <td>${val.LEVEL}</td>
           
                    
                        </tr>
`);
                        })
                    }
                    else {
                        $('.table-body').empty();
                    }
                    document.getElementById("show-result").innerHTML = "Total " + s.LIST.length + " results found";
                },
                error: (err) => {
                    alert(err.msg);
                }
            })
        }



    })
});
