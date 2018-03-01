using System;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopApp.Models
{
    [Serializable]
    public class CheckoutProduct
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public int Index { get; set; }

        public int ProductId { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Product Image")]
        public string ProductImage { get; set; }

        [Display(Name = "Product Price")]
        public int ProductPrice { get; set; }

        [Display(Name = "Client Name")]
        public string ClientName { get; set; } 

        public int Quantity { get; set; }
    }
}