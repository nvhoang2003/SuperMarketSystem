using System.ComponentModel.DataAnnotations.Schema;

namespace SuperMarketSystem.Models
{
    public class Image
    {
        public int ImageId { get; set; }

        public int ProductId { get; set; }

        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }
    }
}
