using DataAccessLayer.DataObject;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.DTOs
{
    public class ProductDTO
    {
        public int Id { get; set; }

        [StringLength(100, MinimumLength = 2)]
        [RegularExpression("([a-zA-Z0-9 .&'-]+)", ErrorMessage = "The field Name should only include letters and number.")]
        [DataType(DataType.Text)]
        [Required]
        public string Name { get; set; }

        [StringLength(255, MinimumLength = 2)]
        [DataType(DataType.MultilineText)]
        [Required]
        public string Description { get; set; }

        [DisplayName("Select Category")]
        public int CategoryId { get; set; }

        [DataType(DataType.ImageUrl)]
        //public string ImageUrl { get; set; }
        public bool IsProductOfTheWeek { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Range(0, 1000)]
        [DataType(DataType.Currency)]
        [Required]
        [Precision(18, 2)]
        public float UnitCost { get; set; }
        [Required]
        public float TotalAmount { get; set; }

        public virtual Category Category { get; set; }

        public virtual ICollection<Rate> Rates { get; set; } = new List<Rate>();
    }
}
