using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketSystem.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        public Guid BrandCode { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
