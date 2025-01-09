//Emoji Loader
function load_emojis() {
    console.log("Loading emojis...");
    const homeEmoji = "\u{1F3E0}";
    const shopEmoji = "\u{1F6D2}";
    const userEmoji = "\u{270D}";
    const loginEmoji = "\u{1F469}";
    const chessEmoji = "\u{265A}";
    const bookEmoji = "\u{1F4D6}";
    const submitEmoji = "\u{279C}";
    document.getElementById("Home Button").innerText = homeEmoji + " Home";
    document.getElementById("Academy Shop Button").innerText = shopEmoji + " Academy Shop";
    document.getElementById("User Registration Button").innerText = userEmoji + " User Registration";
    document.getElementById("User Login Button").innerText = loginEmoji + " User Login";
    document.getElementById("Game of Chess Button").innerText = chessEmoji + " Game of Chess";
    document.getElementById("Guest Book Button").innerText = bookEmoji + " Guest Book";
    document.getElementById("GB_enter").innerText = submitEmoji
    document.getElementById("UL_enter").innerText = submitEmoji
    document.getElementById("UR_enter").innerText = submitEmoji
}
