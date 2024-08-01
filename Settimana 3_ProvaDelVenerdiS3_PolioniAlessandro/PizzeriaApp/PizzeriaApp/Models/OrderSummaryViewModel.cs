namespace PizzeriaApp.Models
{
    public class OrderSummaryViewModel
    {
        public Order Order { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } // Modifica questa proprietà
    }
}
