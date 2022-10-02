
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
    }, { root: document.querySelector(".admin-content"), threshold: 1, rootMargin: "500px" });

    observer.observe(document.querySelector(".admin-content").lastElementChild);

    const loadNewPost = (obv) => {
        let id = document.querySelector(".admin-content").lastElementChild.querySelector("a.upvote").id;

        $.ajax({
            async: true,
            url: '/UAC/GetNewPost',
            type: 'POST',
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: JSON.stringify({ pr_id: id }),
            success: (s) => {
                console.log(s.obj);
                let post = '';
                if (s.obj.POSTING_STATUS === 'BANNED') {
                    console.log(s.obj.POSTING_STATUS);
                    post = `<br />
    <div class="container-fluid post">
        <div class="row">
            <div class="col-sm-12" style="border:1px solid red;border-radius:10px">
                <div class="row post-heading">
                    <div class="col-sm-2   flex-row-style">

                        ${
                        s.obj.PROFILE_IMG !== null ? `<img src="/UAC/GetImage?name=${s.obj.AUTHOR}" class="img-circle" width="40px" height="40px" style="margin-right:10%;" />` :
                            `<img  src="/image/dashboard/Default_pfp.jpg" class="img-circle" width="40px" height="40px" style="margin-right:10%;" />`
                        }

                        <p class="text-color text-left text-nowrap" style="vertical-align:central;margin-top:10%;">${s.obj.AUTHOR}</p>
                    </div>
                    <div class="col-sm-1 col-sm-offset-9">
                        <a href="" class="menu-link-style dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                            <i class="fa-stack-1x fa-solid fa-ellipsis-vertical" style="padding:1rem 0rem;"></i>
                        </a>
                        <!--Drop down menu-->
                                    <ul class="dropdown-menu dropdown-menu-right">
                            <li>
                                <a onclick="javascript:UnBanPost(${s.obj.POST_ID})" style="cursor:pointer;">
                                    <span class="row-no-gutters" style="padding:1rem;">
                                        <span class="col-sm-3">
                                            <i class="fa fa-universal-access"></i>
                                        </span>
                                        <span class="col-sm-5">
                                            Revoke Ban
                                                    </span>
                                    </span>
                                </a>
                            </li>

                        </ul>
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
                            <img  src="/UAC/GetPostImage?id=${s.obj.POST_ID}" alt="Post image" class="img-responsive" />
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
                    <div class="col-sm-offset-10  " style="background-color: rgba(228, 113, 122,0.7); padding-top: 5px; color: gainsboro; padding-bottom: 5px; padding-left: 13px; max-width: 90px; border-radius: 15px; border: 1px solid red; ">
                        <i class="fa fa-skull-crossbones"></i>

                        Banned

                                </div>

                    <div class="col-sm-1  col-sm-offset-7">

                    </div>
                </div>
            </div>
        </div>
    </div>
    `
                }
                else {
                    post = `<br />
        <div class="container-fluid post">
            <div class="row">
                <div class="col-sm-12">
                    <div class="row post-heading">
                        <div class="col-sm-2   flex-row-style">

                            ${
                        s.obj.PROFILE_IMG !== null ? `<img src="/UAC/GetImage?name=${s.obj.AUTHOR}" class="img-circle" width="40px" height="40px" style="margin-right:10%;" />` :
                            `<img  src="/image/dashboard/Default_pfp.jpg" class="img-circle" width="40px" height="40px" style="margin-right:10%;" />`
                        }

                                <p class="text-color text-left text-nowrap" style="vertical-align:central;margin-top:10%;">${s.obj.AUTHOR}</p>
                        </div>
                        <div class="col-sm-1 col-sm-offset-9">
                                    <a href="" class="menu-link-style dropdown-toggle" data-toggle="dropdown" aria-expanded="false">
                                        <i class="fa-stack-1x fa-solid fa-ellipsis-vertical" style="padding:1rem 0rem;"></i>
                                    </a>
                                    <!--Drop down menu-->
                                    <ul class="dropdown-menu dropdown-menu-right">
                                        <li>
                                            <a onclick="javascript:BanPost(${s.obj.POST_ID})" style="cursor:pointer;">
                                                <span class="row-no-gutters" style="padding:1rem;">
                                                    <span class="col-sm-3">
                                                        <i class="fa fa-low-vision"></i>
                                                    </span>
                                                    <span class="col-sm-5">
                                                        Ban
                                                    </span>
                                                </span>
                                            </a>
                                        </li>


                                    </ul>
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
                            <img  src="/UAC/GetPostImage?id=${s.obj.POST_ID}" alt="Post image" class="img-responsive" />
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
                            
                        </div>
                    </div>
                </div>
            </div>
        </div>
       `;
                }



                let elem = document.querySelector(".admin-content").lastElementChild;

                obv.observe(elem);
                $(".admin-content").append(post);
            },
            error: (e) => {

            },
        });

    };

});


