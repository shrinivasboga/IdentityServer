function log() {
    document.getElementById('results').innerText = '';

    Array.prototype.forEach.call(arguments, function (msg) {
        if (msg instanceof Error) {
            msg = "Error: " + msg.message;
        }
        else if (typeof msg !== 'string') {
            msg = JSON.stringify(msg, null, 2);
        }
        document.getElementById('results').innerHTML += msg + '\r\n';
    });
}

document.getElementById("login").addEventListener("click", login, false);
document.getElementById("fetchdataapi").addEventListener("click", fetchdata_api, false);
document.getElementById("accesstokenapi").addEventListener("click", accesstoken_api, false);
document.getElementById("logout").addEventListener("click", logout, false);


var config = {
    authority: "https://localhost:44311",
    client_id: "CodeGenerator.Sample.Client.JsWebApp",
    scope: "openid profile CodeGenerator.Sample.Api",
    redirect_uri: "https://localhost:44344/callback.html",
    post_logout_redirect_uri: "https://localhost:44344/index.html",
    response_type: "code",
};
var mgr = new Oidc.UserManager(config);

mgr.getUser().then(function (user) {
    if (user) {
        log("User logged in", user.profile);
    }
    else {
        log("User not logged in");
    }
});

function login() {
    mgr.signinRedirect();
}

function fetchdata_api() {
    mgr.getUser().then(function (user) {
        try {
            var url = "https://localhost:44322/WeatherForecast";

            var xhr = new XMLHttpRequest();
            xhr.open("GET", url);
            xhr.onload = function () {
                if (xhr.status != 200) {
                    // analyze HTTP status of the response
                    document.getElementById('results').innerHTML = `Error ${xhr.status}: ${xhr.statusText}` + '\r\n';
                } else {
                    // show the result
                    log(xhr.status, JSON.parse(xhr.responseText));
                }
            }
            xhr.onerror = function () {
                document.getElementById('results').innerHTML = "Request failed" + '\r\n';
            }
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            xhr.send();
        }
        catch (err) {
            document.getElementById('results').innerHTML = err + '\r\n';
            document.getElementById('results').innerHTML += "You need to login first!!" + '\r\n';
        }
    });
}

function accesstoken_api() {
    mgr.getUser().then(function (user) {
        try {
            var url = "https://localhost:44322/identity";

            var xhr = new XMLHttpRequest();
            xhr.open("GET", url);
            xhr.onload = function () {
                if (xhr.status != 200) {
                    // analyze HTTP status of the response
                    document.getElementById('results').innerHTML = `Error ${xhr.status}: ${xhr.statusText}` + '\r\n';
                } else {
                    // show the result
                    log(xhr.status, JSON.parse(xhr.responseText));
                }
            }
            xhr.onerror = function () {
                document.getElementById('results').innerHTML = "Request failed" + '\r\n';
            }
            xhr.setRequestHeader("Authorization", "Bearer " + user.access_token);
            xhr.send();
        }
        catch (err) {
            document.getElementById('results').innerHTML = err + '\r\n';
            document.getElementById('results').innerHTML += "You need to login first!!" + '\r\n';
        }
    });
}

function logout() {
    mgr.signoutRedirect();
}
