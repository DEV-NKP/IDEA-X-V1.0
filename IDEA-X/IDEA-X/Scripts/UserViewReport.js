function revokeReport(id)
{
 
    $.ajax({
        type: 'POST',
        url: '/User/RevokeReport',
        data: { rid: id},
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