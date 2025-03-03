using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwizomDbContext.Models
{
    public class Order
    {
        [Key]
        public int OrderID { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.Now;

        [Required]
        public string CustomerName { get; set; }

        [Required]
        public string CustomerPhone { get; set; }

        [Required]
        public string DeliveryAddress { get; set; }

        public decimal TotalAmount { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        // Relationships
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }

    public class OrderItem
    {
        [Key]
        public int OrderItemID { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderID { get; set; }
        public Order Order { get; set; }

        [Required]
        [ForeignKey("MenuItem")]
        public int ItemID { get; set; }
        public MenuItem MenuItem { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal Total => Quantity * Price;
    }

    public enum OrderStatus
    {
        Pending,
        Processing,
        Completed,
        Canceled
    }
}
