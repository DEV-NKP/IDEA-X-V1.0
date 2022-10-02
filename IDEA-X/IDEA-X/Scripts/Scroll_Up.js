
$(document).ready(function () {

   
    $('.show-result-btn').addClass("show");
   
    
    $('.main-content-scoll').scroll(function () {
        var scrollTop = $('.main-content-scoll').scrollTop();
        // scroll-up button show/hide script
        //alert(scrollTop);
        if (scrollTop > 500) {
            $('.scroll-up-btn').addClass("show");
        } else {
            $('.scroll-up-btn').removeClass("show");
        }
    });
    $('.scroll-up-btn').click(function () {
        $('.main-content-scoll').animate({ scrollTop: 0 });
        // removing smooth scroll on slide-up button click
        $('.main-content-scoll').css("scrollBehavior", "auto");
    });



    $('.main_content_result').scroll(function () {
        var scrollTop = $('.main_content_result').scrollTop();
        // scroll-up button show/hide script
        //alert(scrollTop);
        if (scrollTop < 300) {
            $('.show-result-btn').addClass("show");
        } else {
            $('.show-result-btn').removeClass("show");
        }
    });
});