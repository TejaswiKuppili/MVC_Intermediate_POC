using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwizomDbContext.Models
{
    public class MenuItem
    {
        [Key]
        public int ItemID { get; set; }

        [Required, MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsAvailable { get; set; } = true;

        [ForeignKey("MenuCategory")]
        public int CategoryID { get; set; }
        public MenuCategory Category { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantID { get; set; }
        public Restaurant Restaurant { get; set; }
    }
}
