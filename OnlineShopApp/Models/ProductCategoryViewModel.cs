using OnlineShopApp.Validators;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace OnlineShopApp.Models
{
    public partial class ProductCategoryViewModel
    {
        public int ProductCategoryId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Image")]
        [ValidationFile]
        public HttpPostedFileBase ImagePath { get; set; }

        public string ImageName { get; set; }

        public bool IsChangeImage { get; set; }
    }
}
