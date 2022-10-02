

function loadCon() {
    var mnameBox = document.getElementById("mname-text");
    $.ajax({
        type: 'POST',
        url: '/User/CheckChatRequest',
        data: { mname: mnameBox.textContent },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {

            if (s.msg == "ALREADY_SEND") {
               
                document.querySelector(".alredy-send").style.display = "block";
                document.querySelector(".waiting-accept").style.display = "none";
                document.querySelector(".send-option").style.display = "none";
            }
            else if (s.msg == "WAITING_ACCEPT") {
document.querySelector(".alredy-send").style.display = "none";
                document.querySelector(".waiting-accept").style.display = "block";
 document.querySelector(".send-option").style.display = "none";
            }
            else {
document.querySelector(".alredy-send").style.display = "none";
  document.querySelector(".waiting-accept").style.display = "none";
                document.querySelector(".send-option").style.display = "block";
            }

        },
        error: (err) => {
            alert("Error to get data");
        }
    });
}




function sendreq() {
    var mnameBox = document.getElementById("mname-text");
  
      $.ajax({
        type: 'POST',
        url: '/User/SendChatRequest',
          data: { mname: mnameBox.textContent },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
          success: (s) => {               
              if (s.req != null) {
                  SendNow();
              }
              else {
                  FailedSend();
              }
           
        },
        error: (err) => {
            alert("Error to get data");
        }
    });
}

function SendNow()
{
    document.querySelector(".alredy-send").style.display = "none";
    document.querySelector(".waiting-accept").style.display = "none";
    document.querySelector(".send-option").style.display = "none";

    document.querySelector(".send-now").style.display = "block";
   
}
function FailedSend() {
    alert("Something Went Wrong! Please try again.");
    window.location.href = "/User/Chat";

}

function acceptreq() {

    var mnameBox = document.getElementById("mname-text");

    $.ajax({
        type: 'POST',
        url: '/User/AcceptChatRequest',
        data: { mname: mnameBox.textContent },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
            if (s.req != null) {
                AcceptNow();
            }
            else {
                FailedAccept();
            }

        },
        error: (err) => {
            alert("Error to get data");
        }
    });
}
function AcceptNow() {
  // alert("Something Went Wrong! Please try again.");
    //window.location.href = "/User/Chat";
    window.location.replace("/User/Chat");
    
 /*
    document.querySelector(".alredy-send").style.display = "none";
    document.querySelector(".waiting-accept").style.display = "none";
    document.querySelector(".send-option").style.display = "none";
*/


}
function FailedAccept() {
   alert("Something Went Wrong! Please try again.");
    window.location.href = "/User/Chat";

}

function declinereq() {

    var mnameBox = document.getElementById("mname-text");

    $.ajax({
        type: 'POST',
        url: '/User/DeleteChatRequest',
        data: { mname: mnameBox.textContent },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
            if (s.req != null) {
                AcceptNow();
            }
            else {
                FailedAccept();
            }

        },
        error: (err) => {
            alert("Error to get data");
        }
    });
}

function delreq() {

    var mnameBox = document.getElementById("mname-text");

    $.ajax({
        type: 'POST',
        url: '/User/CancelChatRequest',
        data: { mname: mnameBox.textContent },
        async: true,
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",
        success: (s) => {
            if (s.req != null) {
                AcceptNow();
            }
            else {
                FailedAccept();
            }

        },
        error: (err) => {
            alert("Error to get data");
        }
    });
}