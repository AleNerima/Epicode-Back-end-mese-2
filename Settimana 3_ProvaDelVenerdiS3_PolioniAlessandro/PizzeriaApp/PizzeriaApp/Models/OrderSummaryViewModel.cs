namespace PizzeriaApp.Models
{
    public class OrderSummaryViewModel
    {
        public Order Order { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
