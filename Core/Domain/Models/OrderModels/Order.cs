using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models.OrderModels;
public class Order : BaseEntity<Guid>
{
    public Order()
    {
        
    }
    public Order(string userEmail, Address shippingAdddress, ICollection<OrderItem> orderItems, DeliveryMethod deliveryMethod, decimal subTotal, string paymentIntentId)
    {
        UserEmail = userEmail;
        ShippingAdddress = shippingAdddress;
        OrderItems = orderItems;
        DeliveryMethod = deliveryMethod;
        SubTotal = subTotal;
        PaymentIntentId = paymentIntentId;
    }

    public string UserEmail { get; set; }
    public Address ShippingAdddress { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
    public DeliveryMethod DeliveryMethod { get; set; }
    public int? DeliveryMethodId { get; set; }
    public OrderPaymentStatus PaymentStatus { get; set; } = OrderPaymentStatus.Pending;
    public decimal SubTotal { get; set; }
    public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.Now;
    public string PaymentIntentId { get; set; }

}
