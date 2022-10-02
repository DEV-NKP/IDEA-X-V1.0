   var value1 = "";

$(document).ready(() => {



    $('.search_box_user').keyup((e) => {

        let text = e.currentTarget.value;
        if (value1 !== text) {
            value1 = text;
            SearchUserAction(text);
        }
        


    })

});



function SearchUserAction(text) {
    $('.search-result-user').empty();
    $.ajax({
        type: 'POST',
        url: '/Admin/SearchProfile',
        data: { searchText: text },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",

        success: (u) => {

            if (u.USER_LIST.length > 0) {
                u.USER_LIST.forEach(val => {

                    $('.search-result-user').prepend(`<li style="margin-left:1rem;margin-top:1rem;">
 <a href="/Admin/UserProfile/${val.USERNAME}" style=" width: 17vw; ">
    <div align="center" style=" margin-top: 5px; margin-left:0px; border: 1px solid #76b8ac; border-radius: 5px; height: 10%; width: 15vw; " id="div-grad">
                   <table style="border-radius: 15px; background-color: transparent; height: 70px; width: 100%; ">
                       <tr style="  margin-left: 10px; margin-bottom: -5px; ">
                           <td style=" padding: 10px; padding-right: 10px; width: 20%; ">
                               <img src="" id="${val.USERNAME}" class="img-circle" width="40px" height="40px" />
                           </td>
                           <td>
                               <table align="left" style=" margin-left: 0px; background-color: transparent; width:80% ">
                                   <tr>
                                       <td style="background-color:transparent; width:100%">
                                           <font size="3" face="Monospace" style="font-weight: bold; color: azure; white-space: nowrap; overflow: hidden; text-overflow: ellipsis;word-wrap: break-word; ">${val.FIRST_NAME}&nbsp;${val.LAST_NAME}</font>
                                       </td>
                                   </tr>
                                   <tr>
                                       <td style="background-color: transparent;">
                                           <font size="3" face="Monospace" style="color: #696969; white-space: nowrap; overflow: hidden; text-overflow: ellipsis; word-wrap: break-word;">${val.USERNAME}</font>
                                       </td>
                                   </tr>
                               </table>
                           </td>

                       </tr>
                   </table>
               </div>
 </a>
 </li>`);

                    document.getElementById(val.USERNAME).src = `/Admin/GetImage/?name=${val.USERNAME}`;

                })
            }
            else {
                $('.search-result-user').empty();
            }

        },
        error: (er) => {
            alert("Error occured getting data");
        }
    })
}