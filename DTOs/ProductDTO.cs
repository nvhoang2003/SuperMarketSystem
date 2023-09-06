using Microsoft.EntityFrameworkCore;
using SuperMarketSystem.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketSystem.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
       // [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Name should only include letters and number.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        public int Quantity { get; set; }

        [Range(0, 1000)]
        [DataType(DataType.Currency)]
        [Required]
        [Precision(18, 2)]
        public float UnitCost { get; set; }

        public virtual Category Categories { get; set; }

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
        [DisplayName("Select Category")]
        public virtual int CategoryId { get; set; }
        [Required]
        [DisplayName("Select Brand")]
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

        public int Quantity { get; set; }
        public float UnitCost { get; set; }

        public virtual Category Categories { get; set; }

        public virtual Brand Brand { get; set; }

        public IEnumerable<string> ImageName { get; set; }

        public int NumberOfOrder { get; set; }

        public float RateStar { get; set; }

        public float NumberOfRate { get; set; }

        public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
    }
}
