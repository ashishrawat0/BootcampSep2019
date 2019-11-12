$(document).on('click', '#loginButton', function() {
        let email = $('#inputEmail').val()
        let password = $('#inputPassword').val()
        $.ajax('http://localhost:45728/login', {
            type: 'POST',
            dataType: 'JSON',
            contentType: "application/json;charset=utf-8",
            beforeSend: function() {
                $('.main').animate({ opacity: 0.4 })
                $('.mod').fadeIn()
                $('.spinner').show()
            },
            data: JSON.stringify({
                'email': email,
                'password': password
            }),
            success: function(data) {
                localStorage.setItem('token', data.token)
                    //here i have used accountType to redirect to respective page
                if (data.accountType == "Examiner")
                    $(location).attr('href', '../../exminer/views/examiner.html')
                else if (data.accountType == "Student")
                    $(location).attr('href', './accessKey.html')
                else {
                    $(location).attr('href', '../../admin/views/adminHome.html')
                }
            },
            error: function(error) {
                window.alert(error.responseJSON.message)
                location.reload();
            }

        })
    })
    // })