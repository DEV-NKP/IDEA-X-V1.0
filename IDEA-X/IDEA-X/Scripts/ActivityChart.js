
//alert("fddsfd");
var yValuesPupvote = document.getElementById("T-like").value;
var yValuesPdownvote = document.getElementById("T-dislike").value;



var like1 = document.getElementById("TL-1").value;
var like2 = document.getElementById("TL-2").value;
var like3 = document.getElementById("TL-3").value;
var like4 = document.getElementById("TL-4").value;
var like5 = document.getElementById("TL-5").value;
var like6 = document.getElementById("TL-6").value;
var like7 = document.getElementById("TL-7").value;
var like8 = document.getElementById("TL-8").value;
var like9 = document.getElementById("TL-9").value;
var like10 = document.getElementById("TL-10").value;


var dislike1 = document.getElementById("TD-1").value;
var dislike2 = document.getElementById("TD-2").value;
var dislike3 = document.getElementById("TD-3").value;
var dislike4 = document.getElementById("TD-4").value;
var dislike5 = document.getElementById("TD-5").value;
var dislike6 = document.getElementById("TD-6").value;
var dislike7 = document.getElementById("TD-7").value;
var dislike8 = document.getElementById("TD-8").value;
var dislike9 = document.getElementById("TD-9").value;
var dislike10 = document.getElementById("TD-10").value;



/*
$.ajax({
    type: 'POST',
    url: '/User/UserActivity',
    data: {},
    datatype: 'json',
    contenttype: "application/json; charset=utf-8",

    success: (u) => {
        yValuesPupvote = u.like;
        yValuesPdownvote = u.dislike;
        drawPie();
        drawCircle();
    },
    error: (er) => {
        alert("Error occured getting data");
    }
})*/


    var xValuesP = ["UpVote", "DownVote"];
    var yValuesP = [yValuesPupvote, yValuesPdownvote];
    var barColors = [
        "#0abab5",
        "#f08080"
    ];

    new Chart("myPie", {
        type: "pie",
        data: {
            labels: xValuesP,
            datasets: [{
                backgroundColor: barColors,
                borderColor: barColors,
                data: yValuesP
            }]
        },
        options: {
            legend: {
                position: 'bottom',
                labels: {
                    boxWidth: 10,
                }

            },
            title: {
                display: true,
                //text: ""
            }
        }
    });





var calcTotal = Number(yValuesPupvote) + Number(yValuesPdownvote);

   var percUpvote=0;
var percDownvote=0;

if (Number(calcTotal) != 0) {
    percUpvote = (Number(yValuesPupvote) / Number(calcTotal));
    percDownvote = (Number(yValuesPdownvote) / Number(calcTotal));
}




    let optionL = {
        startAngle: -1.55,
        size: 150,
        value: 1.00,
        fill: {
            gradient: ['#f08080', '#0bdad3'] }
}
    let optionD = {
        startAngle: -1.55,
        size: 150,
        value: 1.00,
        fill: {
            gradient: ['#66ffff','#ff6666' ] }
}

    $(".circleL .barL").circleProgress(optionL).on('circle-animation-progress',
        function (event, progress, stepValue) {
            $(this).parent().find("span").text(String((stepValue.toFixed(2).substr(0) * 100).toFixed(0).substr(0)) + "%");
        });

    $(".circleD .barD").circleProgress(optionD).on('circle-animation-progress',
        function (event, progress, stepValue) {
            $(this).parent().find("span").text(String((stepValue.toFixed(2).substr(0) * 100).toFixed(0).substr(0)) + "%");
        });


    $(".upL .barL").circleProgress({
        value: percUpvote
    });
    $(".downD .barD").circleProgress({
        value: percDownvote
    });










////////////

var xValues = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];
var y1Values = [Number(like1), Number(like2), Number(like3), Number(like4), Number(like5), Number(like6), Number(like7), Number(like8), Number(like9), Number(like10)];
var y2Values = [Number(dislike1), Number(dislike2), Number(dislike3), Number(dislike4), Number(dislike5), Number(dislike6), Number(dislike7), Number(dislike8), Number(dislike9), Number(dislike10)];
var maxValue = Math.max(Number(like1), Number(like2), Number(like3), Number(like4), Number(like5), Number(like6), Number(like7), Number(like8), Number(like9), Number(like10), Number(dislike1), Number(dislike2), Number(dislike3), Number(dislike4), Number(dislike5), Number(dislike6), Number(dislike7), Number(dislike8), Number(dislike9), Number(dislike10));
var minValue = Math.min(Number(like1), Number(like2), Number(like3), Number(like4), Number(like5), Number(like6), Number(like7), Number(like8), Number(like9), Number(like10), Number(dislike1), Number(dislike2), Number(dislike3), Number(dislike4), Number(dislike5), Number(dislike6), Number(dislike7), Number(dislike8), Number(dislike9), Number(dislike10));

new Chart("myChart",
    {
        type: "line",
        data: {
            labels: xValues,
            datasets: [{
                label: 'Upvote',
                fill: false,
                lineTension: 0,
                backgroundColor: "#00ced1",
                borderColor: "#0abab5",
                data: y1Values

            }, {
                label: 'Downvote',
                fill: false,
                lineTension: 0,
                backgroundColor: "#fd5e53",
                borderColor: "#f08080",
                data: y2Values
            }]
        },
        options: {
            legend: {
                position: 'bottom',
                labels: {
                    boxWidth: 10,
                }

            },
            scales: {
                yAxes: [{ ticks: { min: Number(minValue), max: Number(maxValue) } }],
            }
        }
    });


