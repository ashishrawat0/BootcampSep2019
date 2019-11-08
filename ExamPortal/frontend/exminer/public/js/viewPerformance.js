// this page will create dom to display details of students
function drawChart(data,total){
    let ctx = document.getElementById('myChart').getContext('2d');
    let labels = ['< 40', '40-60', '60-80', '> 80'];
    let colorHex = ['#FB3640', '#EFCA08', '#43AA8B', '#253D5B'];

            let myChart = new Chart(ctx, {
                type: 'pie',
                data: {
                    datasets: [{
                    data: [30, 10, 40, 20],
                    backgroundColor: colorHex
                    }],
                    labels: labels
                },
                options: {
                    responsive: true,
                    legend: {
                    position: 'bottom'
                    },
                    plugins: {
                        datalabels: {
                            color: '#fff',
                            anchor: 'end',
                            align: 'center',
                            // soffset: 10,
                            // borderWidth: 2,
                            borderColor: '#fff',
                            borderRadius: 30,
                            backgroundColor: (context) => {
                            return context.dataset.backgroundColor;
                            },
                            font: {
                                weight: 'bold',
                                size: '10'
                                },
                            formatter: (value) => {
                                return value + ' %';
                            }
                        }
                    }
                }
            })
}

function showStudents() {
    const tok = localStorage.getItem('token');
    if (tok == null) {
        location.replace("../../index.html")
    }

    $.ajax("http://localhost:3000/performance", {
        type: 'GET',
        dataType: 'JSON',
        headers: {
            'token': localStorage.getItem('token')
        },
        success: function(data) {
            console.log(data)
            let i = 0;
            let count = 1;
            // console.log(data.b[0].name)
            // console.log(data.b.length)

            while (i < data.length) {
                //     console.log(data.b[i]._id)
                //     console.log(data.b[i].name)
                let tr = document.createElement('tr')
                    // let td = document.createElement('td')
                    // console.log(data[i].examCode)
                    //         // create attribute and set id in all fields
                tr.innerHTML = "<td class='cursor' id='" + data[i].examCode + "'onclick='studentDetails(this)'>" + data[i].examCode + "</td>" + "<td class='cursor' id='" + data[i].examCode + "'onclick='studentDetails(this)'>" + data[i].examName + "</td>";

                //tr.append(td)
                //     // let td1 = document.createElement('td')
                //     // td1.innerHTML = data.b[i].name;
                //     // tr.append(td1)
                $("#tbdy").append(tr)

                i++;
                count++;
            }
        },
        error: function(error) {}
    })
}

function logout() {
    localStorage.removeItem("token")
    location.replace("../../index.html")
}
let flag = 0;

function studentDetails(a) {

    // $('#tcan').hide()
    // window.reload()
    console.log(a.id)
    $.ajax("http://localhost:3000/performance/students", {
        type: 'GET',
        dataType: 'JSON',
        headers: {
            'examId': a.id,
            'token': localStorage.getItem('token')
        },
        success: function(data) {
            $('#tcan').empty()
            let tr = document.createElement('tr')
            tr.innerHTML = "<th>" + " Student Name " + "</th>" + "<th>" + " Exam Code " + "</th>" + "<th>" + "Total Score" + "</th>" + "<th>" + "Question Attempted" + "</th>";
            $("#tcan").append(tr)


            console.log(data)
            let i = 0;

            // console.log(data[0].candidateId)
            // if (flag == 0) {
            while (i < data.b.length) {
                let tr = document.createElement('tr')
                tr.innerHTML = "<td>" + data.a[i].name + "</td>" + "<td>" + data.b[i].testCode + "</td>" + "<td>" + data.b[i].totalScore + "</td>" + "<td>" + data.b[i].answers.length + "</td>";
                $("#tcan").fadeIn()
                $("#tcan").append(tr)
                flag = 1;
                i++;
            }
            //}

            // console.log(data[0].testCode)
            // console.log(data[0].answers.length)
            // console.log(data[0].totalScore)
        drawChart(data.b,data.c)

        },
        error: function(error) {
            console.log('error')
        }
    })
}