namespace PizzeriaApp.Models
{
    public class UserOrderViewModel
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string IndirizzoSpedizione { get; set; }
        public string Note { get; set; }
        public string Status { get; set; }
        public List<OrderItemSummaryViewModel> Items { get; set; }
    }
}
