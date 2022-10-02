function upvoteSelect(PostId) {


    voteManage(PostId, "UPVOTE-DECLINE");
    $(`#${PostId}.upvote-select`).hide();
    $(`#${PostId}.upvote-normal`).show();


}


function upvoteNormal(PostId) {

    voteManage(PostId, "UPVOTE-ACCEPT");

    $(`#${PostId}.upvote-select`).show();
    $(`#${PostId}.upvote-normal`).hide();

    if ($(`#${PostId}.downvote`).hasClass('downvote-active-style')) {
        $(`#${PostId}.downvote-select`).hide();
        $(`#${PostId}.downvote-normal`).show();
    }

}
function downvoteSelect(PostId) {
    voteManage(PostId, "DOWNVOTE-DECLINE");

    $(`#${PostId}.downvote-select`).hide();
    $(`#${PostId}.downvote-normal`).show();

}


function downvoteNormal(PostId) {
    voteManage(PostId, "DOWNVOTE-ACCEPT");

    $(`#${PostId}.downvote-select`).show();
    $(`#${PostId}.downvote-normal`).hide();

    if ($(`#${PostId}.upvote`).hasClass('upvote-active-style')) {
        $(`#${PostId}.upvote-select`).hide();
        $(`#${PostId}.upvote-normal`).show();
    }

}


function voteManage(PostId, VoteCondition) {

    $.ajax({
        type: 'POST',
        async: true,
        url: '/Admin/PostAction',
        dataType: 'json',
        data: { PostId: PostId, VoteCondition: VoteCondition },
        success: (s) => {
            $(`#${PostId}.upvote_count`).text(`${s.upvoteCount}`);
            $(`#${PostId}.downvote_count`).text(`${s.downvoteCount}`);
        },
        error: (e) => {
            alert("Error occured updating");
        }
    });

}




function BanPost(PostId) {
    if (confirm("Are you sure! You want to ban this post?") == true) {
        $.ajax({
            type: 'POST',
            async: true,
            url: '/Admin/BanPost',
            dataType: 'json',
            data: { id: PostId },
            success: (s) => {

                var url = window.location.href;
                if (url.includes("page=") == true) {
                   
                    window.location.reload();

                }
                else {
                    window.location.replace("/Admin/Timeline");

                }

               
                
            },
            error: (e) => {
                alert("Error occured updating");
            }
        });

    }
    else {

    }

}


function UnBanPost(PostId) {
    if (confirm("Are you sure! You want to Unban this post?") == true) {
        $.ajax({
            type: 'POST',
            async: true,
            url: '/Admin/UnBanPost',
            dataType: 'json',
            data: { id: PostId },
            success: (s) => {
                var url = window.location.href;
                if (url.includes("page=") == true) {

                    window.location.reload();

                }
                else {
                    window.location.replace("/Admin/Timeline");

                }
                
            },
            error: (e) => {
                alert("Error occured updating");
            }
        });

    }
    else {

    }

}


