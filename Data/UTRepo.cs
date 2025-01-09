using UT.Models;
using UT.Dtos;

namespace UT.Data
{
    public class UTRepo : IUTRepo // intitializes the functions defined in the interface
    {
        readonly UTDBContext _repoContext; // defines the database context / connection in a more global way
        public UTRepo(UTDBContext context)
        {
            _repoContext = context;}

        /*used by Handler instead of controller*/
        public bool verifyPlayer(string userString, string userKey)
        {
            var current = _repoContext.Users.FirstOrDefault(currentUser => currentUser.UserName == userString && currentUser.Password == userKey);
            return current != null;
        }

        //admins cant be registered via the api
        public void RegisterUser(User user)
        {
            _repoContext.Users.Add(user); 
            _repoContext.SaveChanges();
        }

        //orders the item list correctly, only active auctions are visible to the user
        public IEnumerable<Item> getItemsUser()
        {
            IEnumerable<Item> ValidItems = _repoContext.Items.Where(currentItem =>
            currentItem.State == "active");
            ValidItems = ValidItems.OrderBy(attItem => attItem.StartBid).ThenBy(attItem => attItem.Id);
            return ValidItems;
        }

        //provides administrator visibility of inactive items
        public IEnumerable<Item> getItemsAdmin()
        {
            IEnumerable<Item> ValidItems = _repoContext.Items;
            ValidItems = ValidItems.OrderBy(attItem => attItem.State).ThenBy(attItem => attItem.StartBid).ThenBy(attItem => attItem.Id);
            return ValidItems;
        }

        //checks if there is such a file with such a title and url
        public bool validatePath(string pathedFile)
        {
            bool pathValidated = false;
            if (System.IO.File.Exists(pathedFile))
            {
                pathValidated = true;
            }
            return pathValidated;
        }

        //check if the username extracted from the client is already present in database
        public bool AvailableName(string userName)
        {
            User requestedUser = _repoContext.Users.FirstOrDefault(attUser => attUser.UserName == userName);
            return ((requestedUser == null));
        }

        //Adds a comment, only allows most recent comments
        public List<Comment> AddComment(Comment comment)
        {
            _repoContext.Comments.Add(comment);
            _repoContext.SaveChanges();
            Console.WriteLine("Comment added repo: " + comment.ToString());
            var comments = _repoContext.Comments.OrderBy(c => c.Timestamp).ToList();
            if (comments.Count > 5) {
                var commentsToRemove = comments.Take(comments.Count - 5).ToList();
                _repoContext.Comments.RemoveRange(commentsToRemove);
                _repoContext.SaveChanges();
                comments = _repoContext.Comments.OrderBy(c => c.Timestamp).ToList();
            }
            return comments;
        }

        //Retrieves comments from the database.
        public List<Comment> GetAllComments()
        {
            List<Comment> commentList = _repoContext.Comments.ToList();
            return commentList;
        }

        //provides a method for a Simple but high efficacy binary determination regarding administrator privaleges
        public bool IsAdmin(string userString)
        {
            User current_user = _repoContext.Users.FirstOrDefault(currentUser => currentUser.UserName == userString);
            if (current_user != null && current_user.Role == "Admin")
            {
                return true;
            }
            return false;
        }

        // Used for tracking user's role in more comprehensive fashion
        public Tuple<string, string> AuthenticateUser(string check_user, string check_user_role)
        {
            var isUser = check_user_role == "Admin" || check_user_role == "User";
            var isAdmin = check_user_role == "Admin";

            if (!string.IsNullOrEmpty(check_user) || !string.IsNullOrEmpty(check_user_role))
            {
                if (isUser)
                {
                    if (isAdmin)
                    {
                        return new Tuple<string, string>(check_user, "admin authenticated");
                    }
                    return new Tuple<string, string>(check_user, "standard user authenticated");
                }
                return new Tuple<string, string>("null", "forbid");
            }
            return new Tuple<string, string>("null", "unauthorized");
        }
    }
}
