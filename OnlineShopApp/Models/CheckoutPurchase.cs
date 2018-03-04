using System;
using System.Collections.Generic;
using System.Web.Http.ModelBinding;

namespace OnlineShopApp.Models
{
    [Serializable]
    [ModelBinder(typeof(CheckoutPurchase))]
    public class CheckoutPurchase : List<CheckoutProduct> { }
}