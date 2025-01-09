using UT.Models;

namespace UT.Data
{
    public interface IUTRepo
    {
        bool verifyPlayer(string userString, string userKey);
        bool AvailableName(string userName);
        void RegisterUser(User user);
        IEnumerable<Item> getItemsUser();
        IEnumerable<Item> getItemsAdmin();
        bool validatePath(string path);
        List<Comment> AddComment(Comment comment); 
        List<Comment> GetAllComments();
        bool IsAdmin(string userString);
        Tuple<string, string> AuthenticateUser(string check_user, string check_user_role);
    }
}