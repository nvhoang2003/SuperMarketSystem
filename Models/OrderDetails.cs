using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailId { get; set; }

        public int Amount { get; set; }

        public decimal Price { get; set; }

        public int ProductId { get; set; }
        public virtual Product Product { get; set; }

        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
