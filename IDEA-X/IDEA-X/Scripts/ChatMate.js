
//document.getElementById("scroll-to-bottom").addEventListener("change", scrollDown);

$(document).ready(() => {

    getUserChatList();




    /* $(".chat-msg-input").keyup((e) => {
           e.preventDefault();
           if (e.currentTarget.value.trim() === '') {
               
               $(".send-chat-msg-btn").css("pointerEvents","none");
   
           }
           else {
               
               $(".send-chat-msg-btn").css("pointerEvents", "all");
           }
       })
       */
    /* $(".send-chat-msg-btn").click((e) => {
         e.preventDefault();
         const msg = $(".chat-msg-input").value();
         if (msg != "") {
     SendChatMessage(msg);
         }
     
 
 
     })*/
});


function SendChatMessage() {

    var val = document.querySelector(".chat-msg-input-x").value;
    if (val != "") {
        $.ajax({
            type: 'POST',
            url: "/User/SendChatMessage",
            data: { msg: val },
            dataType: 'json',
            async: true,
            contenttype: "application/json; charset=utf-8",
            success: (val) => {
               
                    $(".chat-msg-input-x").val('');


                    //////////

                    const list = val["list"];
                    const uname = document.getElementById("uname").textContent;
                    // alert(uname);
                  //  $(".chat-box").empty();
                    list.forEach(chat => {

                        if (chat["MESSAGE"]["SENDER"] === uname) {
                            $(".chat-box").append(` <div text-align="right" style="margin-top:2px; margin-bottom:2px;">
                                            <table align="right" border="0" style="height: 19%; width: 55%; background-color: transparent;">
                                                <tr>
                                                 <td width="90%">
                                                        <p style="border-radius: 20px; background-color: #eae0c8; border: 1px solid silver; margin: 5px; padding: 8px; ">
                                                            <font size="3" face="Monospace" style="color: #3b444b; text-align: justify; ">${chat["MESSAGE"]["USER_MESSAGE"]}</font>
                                                        </p>
                                                    </td>
                                                    <td width="10%" align="center" style=" padding: 10px;">
                                                        ${chat["PROFILE_IMG"] !== null ? `<img src="/User/GetImage?name=${chat["MESSAGE"]["SENDER"]}" class="img-circle" width="40px" height="40px"/>` :
                                    `<img src="/image/dashboard/Default_pfp.jpg" class="img-circle" width="40px" height="40px" />`
                                }
                                                           
                                                        

                                                    </td>   
                                                </tr>
                                            </table>
                                        </div>
                        
                    `)
                            scrollDown();
                        }
                        else {
                            $(".chat-box").append(`
                        <div align="left" style="margin-top:2px; margin-bottom:2px; ">
                                            <table border="0" style="height: 20%; width: 55%; background-color: transparent;">
                                                <tr>

                                                  <td width="10%" align="center" style=" padding: 10px;">
                                                            ${chat["PROFILE_IMG"] !== null ? `<img src="/User/GetImage?name=${chat["MESSAGE"]["SENDER"]}" class="img-circle" width="40px" height="40px"/>` :
                                    `<img src="/image/dashboard/Default_pfp.jpg" class="img-circle" width="40px" height="40px" />`
                                }

                                                    </td>

                                                    <td width="90%">
                                                        <p style="border-radius: 20px; background-color: #c4c3d0; border: 1px solid siver; margin: 5px; padding: 8px; ">
                                                            <font size="3" face="Monospace" style="color: #3b444b; text-align: justify;">${chat["MESSAGE"]["USER_MESSAGE"]}</font>
                                                        </p>
                                                    </td>  
                                                </tr>
                                            </table>
                                        </div>

                    `)
                            scrollDown();
                        }
                    })



                    ///////////



                
            },
            error: (er) => {
                alert("Error sending message");
            }
        });
    }
}



const getUserChatList = () => {



    $.ajax({
        type: 'POST',
        dataType: 'json',
        contenttype: "application/json; charset=utf-8",
        async: true,
        url: "/User/GetChatHistory",
        success: (val) => {
            

                const list = val["list"];
                const uname = document.getElementById("uname").textContent;

                $(".chat-box").empty();

                list.forEach(chat => {

                    if (chat["MESSAGE"]["SENDER"] === uname) {
                        $(".chat-box").append(` <div text-align="right" style="margin-top:2px; margin-bottom:2px;">
                                            <table align="right" border="0" style="height: 19%; width: 55%; background-color: transparent;">
                                                <tr>
                                                 <td width="90%">
                                                        <p style="border-radius: 20px; background-color: #eae0c8; border: 1px solid silver; margin: 5px; padding: 8px; ">
                                                            <font size="3" face="Monospace" style="color: #3b444b; text-align: justify; ">${chat["MESSAGE"]["USER_MESSAGE"]}</font>
                                                        </p>
                                                    </td>
                                                    <td width="10%" align="center" style=" padding: 10px;">
                                                        ${chat["PROFILE_IMG"] !== null ? `<img src="/User/GetImage?name=${chat["MESSAGE"]["SENDER"]}" class="img-circle" width="40px" height="40px"/>` :
                                `<img src="/image/dashboard/Default_pfp.jpg" class="img-circle" width="40px" height="40px" />`
                            }
                                                           
                                                        

                                                    </td>   
                                                </tr>
                                            </table>
                                        </div>
                        
                    `)
                        scrollDown();
                    }
                    else {
                        $(".chat-box").append(`
                        <div align="left" style="margin-top:2px; margin-bottom:2px; ">
                                            <table border="0" style="height: 20%; width: 55%; background-color: transparent;">
                                                <tr>

                                                  <td width="10%" align="center" style=" padding: 10px;">
                                                            ${chat["PROFILE_IMG"] !== null ? `<img src="/User/GetImage?name=${chat["MESSAGE"]["SENDER"]}" class="img-circle" width="40px" height="40px"/>` :
                                `<img src="/image/dashboard/Default_pfp.jpg" class="img-circle" width="40px" height="40px" />`
                            }

                                                    </td>

                                                    <td width="90%">
                                                        <p style="border-radius: 20px; background-color: #c4c3d0; border: 1px solid siver; margin: 5px; padding: 8px; ">
                                                            <font size="3" face="Monospace" style="color: #3b444b; text-align: justify;">${chat["MESSAGE"]["USER_MESSAGE"]}</font>
                                                        </p>
                                                    </td>  
                                                </tr>
                                            </table>
                                        </div>

                    `)
                        scrollDown();
                    }
                })
            
        },
        error: (err) => {
            console.log("Error getting data");
        }
    })
   // scrollDown();
};
/*
function SendChatMessage() {
    const val = $(".chat-msg-input").value();
    alert(val);
    if (val != "") {
        $.ajax({
            type: 'POST',
            url: "/User/SendChatMessage",
            data: { msg: val },
            dataType: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                if (s.val) {
                    $(".chat-msg-input").val('');
                    getUserChatList();
                }
            },
            error: (er) => {
                alert("Error sending message");
            }
        });
    }
}
*/
function scrollDown() {

    let scroll_to_bottom = document.getElementById('scroll-to-bottom');
    // scroll_to_bottom.scrollTop = scroll_to_bottom.scrollHeight;
   // alert(scroll_to_bottom.scrollHeight);
    scroll_to_bottom.scrollTop = scroll_to_bottom.scrollHeight;
    //  $("#scroll-to-bottom").scrollTop($("#scroll-to-bottom")[0].scrollHeight);

}


function RemoveMate() {
    if (confirm("Do you want to remove this mate?") == true) {
       var mateName= document.getElementById("user-name").textContent;

        $.ajax({
            type: 'POST',
            url: "/User/RemoveMate",
            data: { mateName: mateName },
            dataType: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                if (s.isdel) {
                    window.location.replace("/User/Chat");
                }
            },
            error: (er) => {
                alert("Error sending message");
            }
        });

    } else {
       
    }

}

function RemoveChat() {
    if (confirm("Do you want to delete all chat history?") == true) {
        var mateName = document.getElementById("user-name").textContent;

        $.ajax({
            type: 'POST',
            url: "/User/RemoveChat",
            data: { mateName: mateName },
            dataType: 'json',
            contenttype: "application/json; charset=utf-8",
            success: (s) => {
                if (s.isdel) {
                    window.location.reload();
                }
            },
            error: (er) => {
                alert("Error sending message");
            }
        });
    } else {

    }
}