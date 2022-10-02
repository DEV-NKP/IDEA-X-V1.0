var value1 = "";

$(document).ready(() => {

    $('.search_box').keyup((e) => {
        let text = e.currentTarget.value;
        if (value1 !== text) {
            value1 = text;
            $.ajax({
                type: 'POST',
                url: '/Admin/SearchPostArchives',
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
                            <td onclick="window.location.replace('/Admin/SearchPostView/${val.POST_ID}?page=PostArchives')" style="cursor: pointer; ">${val.POST_ID}</td>
                            <td onclick="window.location.replace('/Admin/UserProfile/${val.AUTHOR}?page=PostArchives')" style="cursor: pointer; ">${val.AUTHOR}</td>
                            <td onclick="window.location.replace('/Admin/SearchPostView/${val.POST_ID}?page=PostArchives')" style="cursor: pointer; ">${val.TIMELINE_TEXT}</td>
                             <td onclick="window.location.replace('/Admin/SearchPostView/${val.POST_ID}?page=PostArchives')" style="cursor: pointer; "><img id="${val.POST_ID}" style="max-height:70px; max-width:70px"></td>

                            <td onclick="window.location.replace('/Admin/SearchPostView/${val.POST_ID}?page=PostArchives')" style="cursor: pointer; ">${val.POSTING_TIME}</td>
                            <td onclick="window.location.replace('/Admin/SearchPostView/${val.POST_ID}?page=PostArchives')" style="cursor: pointer; ">${val.POSTING_STATUS}</td>
                            <td onclick="window.location.replace('/Admin/SearchPostView/${val.POST_ID}?page=PostArchives')" style="cursor: pointer; ">${val.POST_LIKE}</td>
                            <td onclick="window.location.replace('/Admin/SearchPostView/${val.POST_ID}?page=PostArchives')" style="cursor: pointer; ">${val.POST_DISLIKE}</td>
                            <td onclick="window.location.replace('/Admin/SearchPostView/${val.POST_ID}?page=PostArchives')" style="cursor: pointer; ">${val.POST_IP}</td>
                            <td onclick="window.location.replace('/Admin/SearchPostView/${val.POST_ID}?page=PostArchives')" style="cursor: pointer; ">${val.POST_TAG}</td>
                            
                        </tr>
`);
                            if (val.TIMELINE_IMAGE != null) {
                               
                                document.getElementById(val.POST_ID).src = `/Admin/GetPostImageAdmin/?id=${val.POST_ID}`;
                            }
                            else {
                                document.getElementById(val.POST_ID).src = "/image/dashboard/Default_pfp.jpg";
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
