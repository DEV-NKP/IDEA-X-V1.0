dycalendar.draw({
    target: "#dycalendar",
    type: "month",
    dayformat: "full",
    monthformat: "full",
    highlighttoday: true,
    prevnextbutton: "show",
});

$('.click-day').click(function (event) {
    var text = $(event.target).text();
    text += " " + document.querySelector('.dycalendar-span-month-year').textContent;
    // alert(text);
   // $(event.target).classList.add('selected-block');
   document.querySelector(".date-time-show").value = text;
    searchNote(text);
});

$(document).click(function (event) {
   
    $('.click-day').click(function (event) {
        var text = $(event.target).text();
        text += " " + document.querySelector('.dycalendar-span-month-year').textContent;
        // alert(text);
        

       document.querySelector(".date-time-show").value = text;
        searchNote(text);
    });
});


var value1 = "";
function searchNote(text) {
    if (value1 != text) {
        value1 = text;
       
       // alert("hi");
        $.ajax({
            type: 'POST',
            url: '/User/SearchNote',
            data: { date: document.querySelector(".date-time-show").value},
            datatype: 'json',
            contenttype: "application/json; charset=utf-8",

            success: (u) => { 
              //  alert(u.LIST.NOTE_TEXT);
                        document.querySelector(".note-text").value = u.LIST.NOTE_TEXT;

            },
            error: (er) => {
                alert("Error occured getting data");
            }
        });

    }

}