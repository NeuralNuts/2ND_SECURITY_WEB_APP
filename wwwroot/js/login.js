function showEmailInvalidToast() {
    var x = document.getElementById("snackbar");
    x.className = "show";

    setTimeout(function () { x.className = x.className.replace("show", ""); }, 3000);
}

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

var role = "guest";
var emailError = "Enter an email";

$("#sign-up-btn").click(function (event) {
    $.ajax({
        type: "POST",
        url: 'https://localhost:7080/api/User/PostUser?' + `email=${$("#email-input").val()}&password=${$("#password-input").val()}&role=${role}`,
        dataType: "JSON",
        data: Request,
        success: function (response) {

            if (response === emailError) {
                showEmailInvalidToast();
                console.log("bad")
            }
        }
    })
})

