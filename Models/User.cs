using System.ComponentModel.DataAnnotations;

namespace UT.Models
{
    /* all fields are required here as the client doesn't 
     have access to modifier api (readonly) */
    public class User
    {
        [Key]
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Address{ get; set; } 
        public string Role { get; set; }
    }
}
