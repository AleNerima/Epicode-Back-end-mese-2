namespace PizzeriaApp.Models
{
    public class OrderGroupByDateViewModel
    {
        public DateTime Date { get; set; }
        public IEnumerable<Order> Orders { get; set; }
        public decimal TotalRevenue { get; set; }
    }

}
