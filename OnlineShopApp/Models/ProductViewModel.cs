using OnlineShopApp.Validators;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace OnlineShopApp.Models
{
    public partial class ProductViewModel
    {
        public int ProductId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Category is required")]
        public int ProductCategoryId { get; set; }

        [Required(ErrorMessage = "Price is required")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Image")]
        [ValidationFile]
        public HttpPostedFileBase ImagePath { get; set; }

        public string ImageName { get; set; }

        public bool IsChangeImage { get; set; }
    }
}
