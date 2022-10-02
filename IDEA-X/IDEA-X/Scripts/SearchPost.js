
var value1 = "";

$(document).ready(() => {
    $('.search_box').keyup((e) => {
      
        let text = e.currentTarget.value;
        if (value1 !== text) {
            value1 = text;
searchPostAction(text);
        }
        

    })


    const searchPostAction = (text) => {
        $('.search-result').empty();
    
        $.ajax({
                      type: 'POST',
            url: '/User/SearchPost',
            data: { searchText: text },
            async: true,
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",

            success: (p) => {
               
                if (p.POST_LIST.length > 0) {

                    p.POST_LIST.forEach(val => {
                        
                        $('.search-result').prepend(`<li style="margin-left:3rem;margin-top:1rem;">
                            <a href="/User/SearchPostView/${val.POST_ID}?search_text='${text.replace("#","")}'">
                                <div class="container">
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <b>${val.AUTHOR}</b>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-sm-2">
                                            <b>${val.POST_TAG == null ? '' : val.POST_TAG}</b>
                                        </div>
                                    </div>
                                </div>
                            </a>
                        </li>`);
                    })
                }
                else {
                    $('.search-result').empty();
                }
               
            },
            error: (er) => {
                alert("Error occured getting data");
            }
        })
    }
});

