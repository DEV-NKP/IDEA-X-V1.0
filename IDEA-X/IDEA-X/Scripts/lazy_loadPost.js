// TODO : added this script --adb
$(document).ready(() => {
    //    alert("called");
    //console.log(document.querySelector(".scrolling-content"));
    //console.log(document.querySelector(".main-content-scoll").lastElementChild.querySelector("a").id)
    let observer = new IntersectionObserver((entries) => {

        const lstPost = entries[0];

        if (lstPost.isIntersecting === true) {
            loadNewPost(observer);
            observer.unobserve(lstPost.target)

        }
        else {
            return;
        }
    }, { root: document.querySelector(".main-content-scoll"), threshold: 1, rootMargin: "500px" });

    observer.observe(document.querySelector(".main-content-scoll").lastElementChild);

    const loadNewPost = (obv) => {
        let id = document.querySelector(".main-content-scoll").lastElementChild.querySelector("a.upvote").id;

        $.ajax({
            async: true,
            url: '/User/GetNewPost',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ pr_id: id }),
            success: (s) => {

                let post = `<br />
        <div class="container-fluid post">
            <div class="row">
                <div class="col-sm-12 ">
                    <div class="row post-heading">
                        <div class="col-sm-2   flex-row-style">

                            ${
                    s.obj.PROFILE_IMG !== null ? `<img src="/User/GetImage?name=${s.obj.AUTHOR}" class="img-circle" width="40px" height="40px" style="margin-right:10%;" />` :
                        `<img  src="/image/dashboard/Default_pfp.jpg" class="img-circle" width="40px" height="40px" style="margin-right:10%;" />`
                    }

                                <p class="text-color text-left text-nowrap" style="vertical-align:central;margin-top:10%;">${s.obj.AUTHOR}</p>
                        </div>

                    </div>
                    <div class="row post-body">
                        <div class="col-sm-12 ">
                            <p class="text-color">
                                ${s.obj.TIMELINE_TEXT}
                            </p>
                        </div>
                    </div>


                                ${s.obj.TIMELINE_IMAGE !== null ? `<div class="row post-body">
                        <div class="col-sm-12">
                            <img  src="/User/GetPostImage?id=${s.obj.POST_ID}" alt="Post image" class="img-responsive" />
                        </div>
                    </div>` : ''}
                     ${s.obj.POST_TAG !== null ? `<div class="row post-body flex-row-style">
                            <div class="col-sm-2">
                                <p class="tag-style">${s.obj.POST_TAG}</p>
                            </div>

                        </div>`: ''}

                    <div class="row post-action">
                        <div class="col-sm-2  flex-row-style">
                            <!--Place id here-->
                            ${s.obj.POST_STATUS !== null && s.obj.POST_STATUS === 'UPVOTE' ? `<a href="javascript:upvoteSelect(${s.obj.POST_ID})" id="${s.obj.POST_ID}" class="post-action-style upvote-select upvote upvote-active-style" style="margin-right: 0.5rem; display: normal; cursor: pointer;">
                                    <img src="/image/timeline/like-s.svg" class="img-responsive" width="25" height="25" />
                                </a> <a href="javascript:upvoteNormal(${s.obj.POST_ID})" id="${s.obj.POST_ID}" class="post-action-style upvote upvote-normal" style="margin-right: 0.5rem; display: none; cursor: pointer;">
                                    <img src="/image/timeline/like-n.svg" class="img-responsive" width="25" height="25" />
                                </a>`: `<a href="javascript:upvoteSelect(${s.obj.POST_ID})" id="${s.obj.POST_ID}" class="post-action-style upvote-select upvote upvote-active-style" style="margin-right: 0.5rem; display: none; cursor: pointer;">
                                    <img src="/image/timeline/like-s.svg" class="img-responsive" width="25" height="25" />
                                </a> <a href="javascript:upvoteNormal(${s.obj.POST_ID})" id="${s.obj.POST_ID}" class="post-action-style upvote upvote-normal" style="margin-right: 0.5rem; display: normal; cursor: pointer;">
                                    <img src="/image/timeline/like-n.svg" class="img-responsive" width="25" height="25" />
                                </a>`}

                            <b id="${s.obj.POST_ID}" class="upvote_count">${s.obj.POST_LIKE}</b>
                        </div>

                        <div class="col-sm-2  flex-row-style">
                            <!--Place id here-->
                            ${s.obj.POST_STATUS !== null && s.obj.POST_STATUS === 'DOWNVOTE' ? `<a href="javascript:downvoteSelect(${s.obj.POST_ID})" id="${s.obj.POST_ID}" class="post-action-style downvote-select downvote downvote-active-style" style="margin-right: 0.5rem; display: normal; cursor: pointer; ">
                                    <img src="/image/timeline/dislike-s.svg" class="img-responsive" width="25" height="25" />
                                </a> <a href="javascript:downvoteNormal(${s.obj.POST_ID})" id="${s.obj.POST_ID}" class="post-action-style downvote downvote-normal" style="margin-right: 0.5rem; display: none; cursor: pointer; ">
                                    <img src="/image/timeline/dislike-n.svg" class="img-responsive" width="25" height="25" />
                                </a>`: `<a href="javascript:downvoteSelect(${s.obj.POST_ID})" id="${s.obj.POST_ID}" class="post-action-style downvote-select downvote downvote-active-style" style="margin-right: 0.5rem; display: none; cursor: pointer; ">
                                    <img src="/image/timeline/dislike-s.svg" class="img-responsive" width="25" height="25" />
                                </a> <a href="javascript:downvoteNormal(${s.obj.POST_ID})" id="${s.obj.POST_ID}" class="post-action-style downvote downvote-normal" style="margin-right: 0.5rem; display: normal; cursor: pointer; ">
                                    <img src="/image/timeline/dislike-n.svg" class="img-responsive" width="25" height="25" />
                                </a>`}
                            <b id="${s.obj.POST_ID}" class="downvote_count">${s.obj.POST_DISLIKE}</b>
                        </div>

                        <div class="col-sm-1  col-sm-offset-7">
                             <a href="/User/ReportPost/${s.obj.POST_ID}">
                                    ${s.obj.REPORTED_POST === 'REPORTED' ? `<i class="fa-solid fa-flag report-btn-style" style="background-color:red; color: aliceblue;"></i>` : `<i class="fa-solid fa-flag report-btn-style"></i>`}

                                        

                                
                                </a>
                        </div>
                    </div>
                </div>
            </div>
        </div>
       `;
                let elem = document.querySelector(".main-content-scoll").lastElementChild;

                obv.observe(elem);
                $('.main-content-scoll').append(post);
            },
            error: (e) => {

            },
        });

    };

});
