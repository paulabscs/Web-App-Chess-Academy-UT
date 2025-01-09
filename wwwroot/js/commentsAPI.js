  // Builds the layout of comments originating from guest_book_continue
function process_comments(comments_json) {
  let super_string = "";
  comments_json.forEach(comment => {
      super_string += `<p>[${comment.timestamp}] ${comment.name}: ${comment.content}</p>`;
  });
  ensure_comments_loaded(super_string);
}

//Guest Book Function - Complete
function guest_book_continue() {
  const returned_message = document.getElementById("GB_return");
  const username_string_identifier = document.getElementById("GB_username").value;
  const comment_string_identifier = document.getElementById("GB_comment").value;
  const body_message = {
      Content: comment_string_identifier,
      Name: username_string_identifier
  };
  const body_message_identifier = JSON.stringify(body_message);
  const headers_message = {
      "accept": "application/json", // Ensure expected application/json
      "Content-Type": "application/json"
  }
  fetch(get_url("Comment"), {
      method: "POST",
      headers: headers_message,
      body: body_message_identifier
  })
  .then(response => {
      if (!response.ok) {
          throw new Error('Failed to post comment');
      }
      return response.json();
  })
  .then(data => {
      // Handle the response and call the new process_comments function
      console.log('Response from server:', data);
      process_comments(data);
  })
  .catch(error => {
      console.error('Error:', error);
      returned_message.innerText = error;
  });
}

// Reflects the parsed comment content in the client
function ensure_comments_loaded(comments_string){
    var comments_obj = document.getElementById("GB_comments");
    comments_obj.innerHTML = "<em> Timestamp - Commenter's Name - Comment — Access Level </em>" + " <p>" + comments_string + "<p>";
  }


//Guest Book Helper Function - Complete
function load_comments() {
  const headers_message = {
      "accept": "application/json", // Ensure we expect JSON
  };
  fetch(get_url("Comments"), {
      headers: headers_message,
  }).then(response => response.json()) // Parse JSON response to JavaScript objects
  .then(comments_json => {
      // Call the new process_comments function
      process_comments(comments_json);
  }).catch(error => {
      console.error('Error fetching comments:', error);
  });
}

load_comments()