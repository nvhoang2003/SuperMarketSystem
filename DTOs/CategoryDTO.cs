using DataAccessLayer.DataObject;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.DTOs
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }

        public virtual ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
