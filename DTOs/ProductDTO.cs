using SuperMarketSystem.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketSystem.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public float UnitCost { get; set; }

        public virtual Category Category { get; set; }

        public virtual Brand Brand { get; set; }

        public float TotalAmount => Quantity * UnitCost;

        public IEnumerable<string> ImageName { get; set; }
    }

    public class CreateProductDTO
    {
        [Required]
        [StringLength(45)]
        public string Name { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public float UnitCost { get; set; }
        [Required]
        public virtual int CategoryId { get; set; }
        [Required]
        public virtual int BrandId { get; set; }

        [NotMapped]
        public IEnumerable<IFormFile>? ImageFile { get; set; }
    }

    public class CreateImage
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }

    public class CustomerProductDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }


        public float UnitCost { get; set; }

        public virtual Category Category { get; set; }

        public virtual Brand Brand { get; set; }

        public IEnumerable<string> ImageName { get; set; }

        public int NumberOfOrder { get; set; }

        public float RateStar { get; set; }

        public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
    }
}
