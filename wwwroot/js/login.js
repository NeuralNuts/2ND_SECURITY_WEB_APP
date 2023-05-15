$("#login-btn").click(function (event) {
    $.ajax({
        type: "GET",
        url: 'https://localhost:7080/api/User/GetUsersHash?' + `email=${$("#email-input").val()}`,
        dataType: "JSON",
        success: function (response) {
            console.log(response);

            for (var i = 0; i < response.length; i++) {
                $.ajax({
                    type: "GET",
                    url: 'https://localhost:7080/api/User/AuthenticateLogin?' + `email=${$("#email-input").val()}&password=${$("#password-input").val()}&hashPassword=${response[i].hashPassword}`,
                    dataType: "JSON",
                    success: function (data) {
                        console.log(data);
                    }
                })
            }
        }
    })
})

