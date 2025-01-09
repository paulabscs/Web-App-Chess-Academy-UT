using System.ComponentModel.DataAnnotations;

namespace UT.Models
{
    /* all fields are required here as the client doesn't 
     have access to modifier api (readonly) */
    public class Item
    {
        [Key]
        public long Id { get; set; }
        public string Owner { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public float? StartBid { get; set; }
        public float CurrentBid { get; set; }
        public string State { get; set; }
    }
}
