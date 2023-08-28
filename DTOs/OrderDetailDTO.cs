using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Models;

namespace SuperMarketSystem.DTOs
{
    public class OrderDetailDTO
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Amount { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
}
