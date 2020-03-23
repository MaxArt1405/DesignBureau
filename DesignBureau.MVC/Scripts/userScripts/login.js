function login() {
    debugger
    console.log("ok");
    let email = document.getElementById("emailInput").value;
    let password = document.getElementById("passwordInput").value;
    let body = {
        email: email,
        password: password
    }

    fetch('Auth/Login', {
        method: 'post',
        headers: { "Content-type": "application/json; charset=UTF-8" },
        body: JSON.stringify(body)
    }).then(response => response.json()).then(result => alert(result[0].url)).catch(err => alert(err));
}
let button = document.getElementById("loginButton");

button.addEventListener("click", login);
