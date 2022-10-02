
function CheckBan() {
    var uname = document.getElementById("uname").textContent;

        $.ajax({
            type: 'POST',
            async: true,
            url: '/Admin/CheckBan',
            dataType: 'json',
            data: { uname: uname },
            success: (s) => {
                if (s.isban==true) {
                    document.querySelector(".ban-user").style.display = "none";
  document.querySelector(".unban-user").style.display = "normal";
                }
                else {
           document.querySelector(".ban-user").style.display = "normal";
  document.querySelector(".unban-user").style.display = "none";
                }
              
            },
            error: (e) => {
                alert("Error occured updating");
            }
        });

}





function BanUser() {
    var uname = document.getElementById("uname").textContent;
    if (confirm("Are you sure! You want to ban this user?") == true) {
        $.ajax({
            type: 'POST',
            async: true,
            url: '/Admin/BanUser',
            dataType: 'json',
            data: { uname: uname },
            success: (s) => {

                window.location.reload();
            },
            error: (e) => {
                alert("Error occured updating");
            }
        });

    }
    else {

    }

}


function UnBanUser() {
    var uname = document.getElementById("uname").textContent;
    if (confirm("Are you sure! You want to Unban this user?") == true) {
        $.ajax({
            type: 'POST',
            async: true,
            url: '/Admin/UnBanUser',
            dataType: 'json',
            data: { uname: uname },
            success: (s) => {

                window.location.reload();
            },
            error: (e) => {
                alert("Error occured updating");
            }
        });

    }
    else {

    }

}