using DataAccessLayer.DataObject;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SuperMarketSystem.ViewModels
{
    public class SearchProductViewModel
    {
        [Required]
        [DisplayName("Serach")]
        public string SearchText { get; set; }

        //public IEnumerable<string> SearchListExamples { get; set; }

        public IEnumerable<Product> ProductList { get; set; }

    }
}
