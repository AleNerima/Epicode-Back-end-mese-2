using System.ComponentModel.DataAnnotations;

namespace PizzeriaApp.Models
{
    public class CreateOrderViewModel
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public int[] ProductIds { get; set; }
        public int[] Quantities { get; set; }
        public string IndirizzoSpedizione { get; set; }
        public string Note { get; set; }
    }

}
