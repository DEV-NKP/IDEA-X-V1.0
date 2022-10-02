$(document).ready(() => {
    $('.deletePostBtn').on("click", (e) => {
        let id = e.currentTarget.dataset["id"];
        $(".delete-post-true").attr('href', `/User/DeletePost/${id}`);
    })
})