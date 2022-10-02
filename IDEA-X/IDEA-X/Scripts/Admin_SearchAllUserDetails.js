var value1 = "";

$(document).ready(() => {

    $('.search_box').keyup((e) => {
        let text = e.currentTarget.value;
        if (value1 !== text) {
            value1 = text;
            $.ajax({
                type: 'POST',
                url: '/Admin/SearchAllUserDetails',
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
                         <tr onclick="window.location.replace('/Admin/UserProfile/${val.USERNAME}?page=AllUserDetails')" style="cursor: pointer; ">
${count++}
                            <td>${val.USERNAME}</td>
                            <td>${val.FIRST_NAME}</td>
                            <td>${val.LAST_NAME}</td>
                            <td>${val.HEADLINE}</td>
                            <td>${val.DATE_OF_BIRTH}</td>
                            <td>${val.GENDER}</td>
                            <td>${val.MOBILE}</td>
                            <td>${val.USER_ADDRESS}</td>
                            <td>${val.USER_STATE}</td>
                            <td>${val.ZIP_CODE}</td>
                            <td>${val.COUNTRY}</td>
                            <td>${val.INDUSTRY}</td>
                            <td>${val.EDUCATIONAL_INSTITUTION}</td>
                            <td>${val.DEPARTMENT}</td>
                            <td>${val.CONTACT_URL}</td>
                          
                                <td><img id="${val.USERNAME}" style="max-height:70px; max-width:70px"></td>

                            
                        
                            <td>${val.SIGNUP_TIME}</td>
                            <td>${val.USER_STATUS}</td>
                            <td>${val.SIGNUP_IP}</td>


                        </tr>
`);
                            if (val.PROFILE_PICTURE != null) {
                                document.getElementById(val.USERNAME).src = `/Admin/GetUserImageAdmin/?user=${val.USERNAME}`;
                            }
                            else {
                                document.getElementById(val.USERNAME).src = "/image/dashboard/Default_pfp.jpg";
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
