// Navigation Bar State - Responsive deselection
function hideAllSections() {
    const sections = ["Home", "Academy Shop", "User Registration", "User Login", "Game of Chess", "Guest Book"];
    sections.forEach(section => {
        document.getElementById(section).style.display = "none";
    });
}

// Navigation Bar State - Selection Made
function updateButtonStyles(button) {
    const buttons = ["Home Button", "Academy Shop Button", "User Registration Button", "User Login Button", "Game of Chess Button", "Guest Book Button"];
    buttons.forEach(buttonId => {
        document.getElementById(buttonId).style.backgroundColor = "black";
        document.getElementById(buttonId).style.color = "whitesmoke";
    });
    button.style.backgroundColor = "whitesmoke";
    button.style.color = "black";
}

//Navigation Function - Complete
function section(button, button_text) {
        hideAllSections();
        document.getElementById(button_text).style.display = "block";
        updateButtonStyles(button);
}

section(document.getElementById("Home Button"), "Home")
