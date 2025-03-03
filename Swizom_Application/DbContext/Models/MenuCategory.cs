using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwizomDbContext.Models
{
    public class MenuCategory
    {
        [Key]
        public int CategoryID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantID { get; set; }
        public Restaurant Restaurant { get; set; }

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
    }
}
