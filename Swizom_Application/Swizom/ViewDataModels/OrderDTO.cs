using SwizomDbContext.Models;

namespace Swizom.ViewDataModels
{
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string DeliveryAddress { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public List<OrderItemDTO> OrderItems { get; set; }
    }

    public class OrderItemDTO
    {
        public int OrderItemID { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string MenuItemName { get; set; }
        public decimal MenuItemPrice { get; set; }
    }
}
