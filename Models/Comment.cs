using System.ComponentModel.DataAnnotations;

namespace UT.Models
{
    /* all fields are required here as the client doesn't 
     have access to modifier api (readonly) */
    public class Comment
    {
        public string Name { get; set; } 
        public string Content { get; set; } 

        [Key]
        public DateTime Timestamp { get; set; } 
    }
}