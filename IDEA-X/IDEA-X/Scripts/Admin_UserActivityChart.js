
$(document).ready(function () {
    var uname = document.getElementById("uname").textContent;
    $.ajax({
        type: 'GET',
        url: '/Admin/SearchUserActivityProgress',
        data: {name:uname},
        datatype: 'json',
        contenttype: "application/json; charset=utf-8",

        success: (u) => {
           
            showChart(u);
            showPie(u);

            /* drawCircle();*/
        },
        error: (er) => {
            alert("Error occured getting data");
        }
    })
});


function showChart(u) {

    var xValues = [];
    var y1Values = [];
    var y2Values = [];

    let x = 0;

    u.POSTLIST.forEach(item => {
        if (x < 100) {
            xValues.push(item.POST_ID);
            y1Values.push(item.LIKE);
            y2Values.push(item.DISLIKE);

        }
        x++;
    })

    var maxValue = Math.max(Math.max(...y1Values), Math.max(...y2Values));
    var minValue = Math.min(Math.min(...y1Values), Math.min(...y2Values));
    // alert(maxValue + "   " + minValue);
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


}



function showPie(u) {
    var yValuesPupvote = 0;
    var yValuesPdownvote = 0;

    u.POSTLIST.forEach(item => {

        yValuesPupvote += Number(item.LIKE);
        yValuesPdownvote += Number(item.DISLIKE);


    })
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


}


