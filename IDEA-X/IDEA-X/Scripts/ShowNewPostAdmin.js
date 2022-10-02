// TODO : added this script
let prevCount = 0;
let currPostCount = 0;
$(document).ready(() => {
    $.ajax({
        type: 'POST',
        async: true,
        url: '/Admin/GetPostCount',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: (s) => {
            currPostCount = s.p_count;
            prevCount = s.p_count;
        },
        error: (e) => {
            console.log('Error getting data')
        }
    })
    setInterval(() => {
        getPostCount();
        showNewPost();
    }, 5000)
})

const getPostCount = () => {
    $.ajax({
        type: 'POST',
        async: true,
        url: '/Admin/GetPostCount',
        contentType: "application/json; charset=utf-8",
        dataType: 'json',
        success: (s) => {
            currPostCount = s.p_count;
            if (currPostCount < prevCount) {
                prevCount = currPostCount;
            }
        },
        error: (e) => {
            console.log('Error getting data')
        }
    })
}

const showNewPost = () => {
    if (currPostCount > prevCount) {
        let newPostCount = currPostCount - prevCount;

        //if (newPostCount > 0) {
        const htmlElem = `<div class="container-fluid new-post-count" style="border-bottom: 2px solid #68849b;padding-bottom:1.5%">
            <div class="row">
                <div class="col-sm-12" style="text-align:center">
                    <a href="/Admin/Timeline" style="color:#68849b;text-decoration:none;font-size:medium;font-weight:bold">Show ${newPostCount} new post</a>
                </div>
            </div>
        
        </div>`;
        if ($('.admin-content').has('.new-post-count').length > 0) {
            $('.new-post-count').remove();
        }
        //print(newPostCount);
        $('.admin-content').prepend(htmlElem);
        EnableScrollToTop();

        //}

    }

}

$('.new-post-count').click(() => {
    prevCount = currPostCount;
})
$('.new-post-up-btn').click(() => {
    $('.admin-content').animate({ scrollTop: 0 }, '50');
})
const EnableScrollToTop = () => {
    $('.admin-content').scroll((e) => {
        if (e.currentTarget.scrollTop > 100) {
            $(".post-up-btn").css('display', 'table');
        }
        else {
            $(".post-up-btn").css('display', 'none');
        }
    })
}

