using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwizomDbContext.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrderDetailID { get; set; }

        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public Order Order { get; set; }

        [ForeignKey("MenuItem")]
        public int ItemID { get; set; }
        public MenuItem Item { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
