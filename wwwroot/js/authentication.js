// Development port
const port_string_identifier = 5283; 

// Helper function for URL building
function get_url(modifier) {
    return "http://localhost:" + port_string_identifier + "/api/" + modifier;
}

// Authentication Part

// User Login Helper Function 2 - Complete
function isAuth(status) {
    return status == 200;
}


// User Login Helper Function 1 - Complete
function handleAuth(status_string_identifier) {
    const status_code = status_string_identifier;
    const response_message = "Status Code: " + status_code + ", Login successful: " + isAuth(status_code);
    return response_message;
}

// Utilized by user_registration_continue and user_login_continue
function login_layer(api_name, username, password, address = null) {
    const url = get_url(api_name);
    const requestBody = { UserName: username, Password: password, Address: address };
    const api_identifier = "U" + api_name.charAt(0).toUpperCase();
    fetch(url, {
        method: "POST",
        headers: { "Accept": "text/plain", "Content-Type": "application/json" },
        credentials: "include",
        body: JSON.stringify(requestBody)
    }).then(response => {
        const status = response.status;
        response.text().then(responseText => {
            if (status === 200) {
                window.location.reload();
            } 
            document.getElementById(api_identifier+"_return").innerText = handleAuth(status) + " " + responseText;
        });
    }).catch(error => {
        console.error('Error:', error);
        document.getElementById(api_identifier + "_info").innerText = `Login failed: ${error}`;
    });
}


// User Login Clicked Continue Function 
function user_login_continue() {
    const username_string_identifier = document.getElementById("UL_username").value;
    const password_string_identifier = document.getElementById("UL_password").value;
    login_layer("Login", username_string_identifier, password_string_identifier); // Call login_layer function
}


// Navigation Bar State - Logout
function resetLoginFields() {
    const loginEmoji = "\u{1F469}";
    document.getElementById("User Login Button").innerText = loginEmoji + " User Login"
    document.getElementById("UL_info").style.display = "block";
    document.getElementById("UL_form").style.display = "block";
    document.getElementById("UL_enter").style.display = "inline";
    document.getElementById("UL_line").style.display = "inline";
    document.getElementById("UL_info").innerText = "You are logged out.";
    document.getElementById("UL_username").value = "";
    document.getElementById("UL_password").value = "";
    document.getElementById("UL_return").innerText = "";
}


// Check Session Status Function - New
function check_session_status() {
    const url = get_url("Session");
    fetch(url, {
        method: "GET",
        credentials: "include" // Ensure cookies are included in the request
    }).then(response => {
        if (response.ok) {
            document.getElementById("User Login Button").style.display = "none";
            document.getElementById("User Registration Button").style.display = "none";
            document.getElementById("Logout").style.display = "inline";
            document.getElementById("UL_form").style.display = "none";
            document.getElementById("UL_line").style.display = "none";
            return response.text().then(text => {
                document.getElementById("UL_info").innerText = text;
                return text;
            });
        } else if (response.status === 401) {
            document.getElementById("Logout").style.display = "none";
            document.getElementById("User Login Button").style.display = "inline";
            document.getElementById("Login Status").innerText = "You are logged out.";
            resetLoginFields();
            return Promise.reject("Unauthorized");
        } else {
            console.error(`Failed to fetch session status: ${response.status} ${response.statusText}`);
            throw new Error(`Failed to fetch session status: ${response.status} ${response.statusText}`);
        }
    }).then(session_response_text => { // 'session_response_text' is the result of response.text()
        if (session_response_text.includes("authenticated")) {
            if (session_response_text.includes("admin")) {
                document.getElementById("Login Status").innerText = "Signed in - Admin";
            } else {
                document.getElementById("Login Status").innerText = "Signed in - User";
            }
        } else {
            console.error("Unexpected response format:", session_response_text);
        }
    }).catch(error => {
        if (error !== "Unauthorized") {
            console.error("Error checking session status:", error);
        }
    });
}


// User Registration Clicked Continue Function - Complete
function user_registration_continue() {
    const username_string_identifier = document.getElementById("UR_username").value;
    const password_string_identifier = document.getElementById("UR_password").value;
    const address_string_identifier = document.getElementById("UR_address").value;
    login_layer("Register", username_string_identifier, password_string_identifier, address_string_identifier);
}



// Reflects logout state in the client-side after server confirmation
function user_logout() {
    const url = get_url("Logout");
    fetch(url, {
        method: "GET",
        credentials: "include" 
    }).then(response => {
        if (response.ok) {
            return response.text();
        } else {
            console.error(`Failed to logout: ${response.status} ${response.statusText}`);
            throw new Error(`Failed to logout: ${response.status} ${response.statusText}`);
        }
    }).then(responseText => {
        console.log("Logout successful:", responseText);
        document.getElementById("Login Status").innerText = "Not logged in";
        window.location.reload();
    }).catch(error => {
        console.error("Error logging out:", error);
    });
}



