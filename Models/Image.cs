using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketSystem.Models
{
    public class Image
    {
        [Key]
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
