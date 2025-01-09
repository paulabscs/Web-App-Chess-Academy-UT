using Microsoft.AspNetCore.Mvc;
using UT.Models;
using UT.Data;
using UT.Dtos;


namespace UT.Controllers
{
    [ApiController]
    [Route("api")] // Identifies the API name such that api/{method} is in the URL
    public class DataController : Controller
    {
        readonly IUTRepo _repo;
        public DataController(IUTRepo repo) { _repo = repo; }
        
        [HttpGet("GetVersion")]
        public ActionResult<string> GetVersion()
        {
            return Ok("1.01"); // Returning a simple string response
        }

        // utilizes _repo.AvailableName & _repo.RegisterUser
        [HttpPost("Register")]
        public ActionResult<string> Register([FromBody] UserInput userInput)
        {
            var currentUser = new User
            {
                UserName = userInput.UserName,
                Password = userInput.Password,
                Address = userInput.Address == null ? "" : "Unknown",
                Role =  "User",
            };
            bool isNameAvailable = _repo.AvailableName(currentUser.UserName);
            if (isNameAvailable)
            {
                _repo.RegisterUser(currentUser);
                Login(userInput);
                return Ok("User successfully registered.");
            }
            return BadRequest("Username not available.");
        }

        // utilizes _repo.verifyPlayer & _repo.IsAdmin
        [HttpPost("Login")]
        public ActionResult<string> Login([FromBody] UserInput userInput)
        {
             if (string.IsNullOrEmpty(userInput.UserName) || string.IsNullOrEmpty(userInput.Password))
            {
                return BadRequest("UserName and password are required.");
            }
            if (_repo.verifyPlayer(userInput.UserName, userInput.Password)){
                var user_role = _repo.IsAdmin(userInput.UserName) ? "Admin" : "User";
                HttpContext.Session.SetString("user_string", userInput.UserName);
                HttpContext.Session.SetString("user_role_string", user_role); 
                return Ok(user_role);
            }
            return Unauthorized("Invalid username or password.");
        }

        // utilizes _repo.getItemsUser & _repo.getItemsAdmin
        [HttpGet("ListItems")]
        public ActionResult<IEnumerable<Item>> ListItems() { 
            var listed_items = _repo.getItemsUser();
            var isAdmin = HttpContext.Session.GetString("user_role_string") == "Admin";
            if (isAdmin  == true)
            {
                listed_items = _repo.getItemsAdmin();
            }
            return Ok(listed_items); 
        }
    
        // utilizes _repo.validatePath
        [HttpGet("GetItemPhoto/{id}")]
        public ActionResult GetItemPhoto(long id)
        {
            string imageName = id.ToString();
            string solutionDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "ItemsImages");
            string gifExtension = Path.Combine(solutionDirectory, imageName + ".gif");
            string pngExtension = Path.Combine(solutionDirectory, imageName + ".png");
            string jpegExtension = Path.Combine(solutionDirectory, imageName + ".jpg");
            string contentIdentifier = "image/png"; // Default returns logo
            string imageToReturn = Path.Combine(solutionDirectory, "default.png");

            // In case of unexpected image types
            if (_repo.validatePath(gifExtension)) {
                contentIdentifier = "image/gif";
                imageToReturn = gifExtension; 
            } else if (_repo.validatePath(jpegExtension)) {
                contentIdentifier = "image/jpeg";
                imageToReturn = jpegExtension;
            } else if (_repo.validatePath(pngExtension)) {
                return PhysicalFile(pngExtension, "image/png");
            }

            return PhysicalFile(imageToReturn, contentIdentifier);
        }

        [HttpGet("Logout")]
        public IActionResult Logout()
        {   
            HttpContext.Session.Clear();
            return Ok("user logged out");
        }

        // utilizes _repo.AuthenticateUser
        [HttpGet("Session")]
        public IActionResult Session()
        {
            var check_user = HttpContext.Session.GetString("user_string");
            var check_user_role = HttpContext.Session.GetString("user_role_string");
            
            var (username, authResult) = _repo.AuthenticateUser(check_user, check_user_role);

            if (authResult == "admin authenticated" || authResult == "standard user authenticated")
            {
                return Ok(new { Username = username, Message = authResult });
            }
            else if (authResult == "forbid")
            {
                return Forbid("attempt to authenticate unexpectedly");
            }
            else
            {
                return Unauthorized("user not authenticated");
            }
        }

        // utilizes _repo.AuthenticateUser, _repo.AvailableName & _repo.AddComment(comment)
        [HttpPost("Comment")]
        public IActionResult Comment([FromBody] CommentInput commentInput)
        {
            if (string.IsNullOrEmpty(commentInput.Content) || string.IsNullOrEmpty(commentInput.Name))
            {
                return BadRequest("Name and content are required.");
            }
            var check_user = HttpContext.Session.GetString("user_string");
            var check_user_role = HttpContext.Session.GetString("user_role_string");
            var (username, authResult) = _repo.AuthenticateUser(check_user, check_user_role);
            if (authResult == "admin authenticated" || authResult == "standard user authenticated")
            {
                if (commentInput.Name != username)
                {
                    return BadRequest("Must use your username while logged in.");
                }
                commentInput.Content += " (" + check_user_role + ")";
            }
            else
            {
                if (!_repo.AvailableName(commentInput.Name))
                {
                    return BadRequest("Name is taken, try another temporary name.");
                }
                commentInput.Content += " (Guest)";
            }
            var comment = new Comment
            {
                Name = commentInput.Name,
                Content = commentInput.Content,
                Timestamp = DateTime.Now
            };
            var comments = _repo.AddComment(comment);
            return Ok(comments);
        }

        // utilizes _repo.GetAllComments
        [HttpGet("Comments")]
        public ActionResult<List<Comment>> Comments()
        {
            List<Comment> comments = _repo.GetAllComments();
            if (!comments.Any())
            {
                comments.Add(new Comment
                {
                    Name = "Default User",
                    Content = "This is a default comment.",
                    Timestamp = DateTime.Now
                });
            }
            return Ok(comments);
        }
    }
}
