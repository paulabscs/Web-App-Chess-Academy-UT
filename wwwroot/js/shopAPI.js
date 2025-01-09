// Helper function for URL building
function get_url(modifier) {
    return "http://localhost:" + port_string_identifier + "/api/" + modifier;
}

// per-image processing
function getImageFromAPI(itemId, imgElement) {
    fetch(get_url(`GetItemPhoto/${itemId}`)) 
        .then(response => {
            if (response.ok) {
                return response.blob(); // parse image directly via binary data
            } else {
                console.error(`Failed to retrieve image for item ID: ${itemId}`);
                throw new Error(`Failed to retrieve image for item ID: ${itemId}`);
            }
        })
        .then(imageBlob => {
            const imageUrl = URL.createObjectURL(imageBlob);
            imgElement.src = imageUrl;
        })
        .catch(error => {
            console.error(`Error fetching image for item ID: ${itemId}:`, error);
        });
}

// Image processing controller
function dynamic_images(){
    var images = document.querySelectorAll('.item');
    images.forEach(img => {
        var itemId = img.id.replace('img_', '');
        getImageFromAPI(itemId, img);
    });
}

//Loads item information in the client
function ensure_items_loaded(items_string1) {
    console.log("Ensuring items loaded...");
    var items_obj = document.getElementById("shop_data");
    if (!items_obj) {
        console.error("Shop data not found! Ensure the 'shop_data' div is present in the HTML.");
        return;
    }
    var items_string = items_string1.substring(items_string1.indexOf("[{") + 1, items_string1.lastIndexOf("}]"));
    var my_list = items_string.split("{");
    var item_html = "";
    my_list.forEach(function (itemsSet) {
        var obj_list = itemsSet.split("\",\"");
        var items_set_identifier = "";
        var id_identifier = "";
        var current_y1 = "";
        obj_list.forEach(function (items_set_var) {
            var [y1, y2] = items_set_var.split("\":\"");
            current_y1 = y1;
            var additive = "";
            var defaultAdditive = "<em class=\"em_block\">[" + y1 + " ] [" + y2 + " ]</em>";
            if (y1.indexOf("id") == 1) {
                id_identifier = y1.split(':')[1];
                id_identifier = id_identifier.split(',\"')[0];
                additive = "<em class=\"em_block\">#" + id_identifier + "</em>";
            }
            if (y1.indexOf("name") == 0) {
                additive = "<p>" + y2 + "</p>";
            }
            if (y1.indexOf("description") == 0) {
                additive = "<em class=\"em_block\">" + y2 + " </em>";
            }
            if (y1.indexOf("price") == 0) {
                y2 = y2.replace("\"},", "");
                y2 = y2.replace("\"", "");
                additive = "<br><button id=\"" + id_identifier + "\" onclick=\"purchase_continue(" + id_identifier + ")\" class=\"postpage\">Buy for " + y2 + "</button><br><br>";
            }
            if (additive == defaultAdditive) {
            } else {
                items_set_identifier += additive;
            }
        });
        if (current_y1.length > 1) {
            var img_string = `<img width="150" height="150" id="img_${id_identifier}" class="item" style="width:150px; height:150px; float:inline-start;" alt="">`;
            item_html += `<div class="en">${img_string} ${items_set_identifier}</div><br><br>`;
        }
    });
    items_obj.innerHTML = `<div class="en_container"> ${item_html} </div>`;
    setTimeout(dynamic_images, 2000);
}

//Shop Loading Function - Complete
function load_shop() {
    console.log("Loading shop items...");
    const headers_message = {
        "Accept": "application/json", 
    };
    const url_string_fetcher = fetch(get_url("ListItems"), {
        headers: headers_message,
    });
    const fetch_string_obj = url_string_fetcher.then(fetch_string_var => fetch_string_var.text());
    fetch_string_obj.then(stream_string_var => {
        console.log("stream_string_var:", stream_string_var);
        if (stream_string_var.trim().length === 0 || stream_string_var === "[]") {
            console.log("stream_string_var is empty.");
        } else {
            console.log("stream_string_var is not empty.");
        }
        ensure_items_loaded(stream_string_var);
    });
}

load_shop();

