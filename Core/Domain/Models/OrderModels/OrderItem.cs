namespace Domain.Models.OrderModels;

public class OrderItem : BaseEntity<Guid>
{
    public OrderItem()
    {
    }

    public OrderItem(ProductInOrderItem productInOrderItem, int quantity, decimal price)
    {
        Product = productInOrderItem;
        Quantity = quantity;
        Price = price;
    }

    public ProductInOrderItem Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}