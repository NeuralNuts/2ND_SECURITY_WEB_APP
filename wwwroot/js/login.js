function showErrorToast(error) {
    var emailError = "Enter an email";
    var emailTakenError = "Email allready in use";
    var accountCreated = "Account created";
    var loggedIn = "LOGIN VALID";

    var y = document.getElementById("snackbar-email-taken");
    var x = document.getElementById("snackbar-email");
    var z = document.getElementById("snackbar-created");
    var n = document.getElementById("snackbar-null");
    var l = document.getElementById("snackbar-logged-in");

    if (error === emailError) {
        x.className = "show";
        y.className = "hide";
        z.className = "hide";
        n.className = "hide";
    }
    else if (error === emailTakenError) {
        y.className = "show";
        x.className = "hide";
        z.className = "hide";
        n.className = "hide";
    }
    else if (error === accountCreated) {
        z.className = "show";
        y.className = "hide";
        x.className = "hide";
        n.className = "hide";
    }
    else if (error == loggedIn) {
        l.className = "show";
        x.className = "hide";
        y.className = "hide";
        z.className = "hide";
        n.className = "hide";
    }

    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}

$("#login-btn").click(function (event) {
    $.ajax({
        type: "GET",
        url: 'https://localhost:7080/api/User/GetUsersHash?' + `email=${$("#email-input").val()}`,
        dataType: "JSON",
        success: function (response) {

            for (var i = 0; i < response.length; i++) {
                $.ajax({
                    type: "GET",
                    url: 'https://localhost:7080/api/User/AuthenticateLogin?' + `email=${$("#email-input").val()}&password=${$("#password-input").val()}&hashPassword=${response[i].hashPassword}`,
                    dataType: "JSON",
                    data: Request,
                    success: function (data) {
                        showErrorToast(data);
                    }
                })
            }
        }
    })
})

var role = "guest";

$("#sign-up-btn").click(function (event) {
    $.ajax({
        type: "POST",
        url: 'https://localhost:7080/api/User/PostUser?' + `email=${$("#email-input").val()}&password=${$("#password-input").val()}&role=${role}`,
        dataType: "JSON",
        data: Request,
        success: function (response) {
            showErrorToast(response);
        },
        statusCode: {
            500: function (xhr) {
                var n = document.getElementById("snackbar-null");
                n.className = "show";
            }
        }
    })
})

