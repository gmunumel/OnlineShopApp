using System.Web.Mvc;

namespace OnlineShopApp.Models.Binders
{
    public class CheckoutPurchaseModelBinder : IModelBinder
    {
        public object BindModel(ControllerContext controllerContext,
            ModelBindingContext bindingContext)
        {
            CheckoutPurchase checkoutPurchase = (controllerContext.HttpContext.Session != null) ?
                (controllerContext.HttpContext.Session["key_cp"] as CheckoutPurchase) :
                    null;

            if (checkoutPurchase == null)
            {
                checkoutPurchase = new CheckoutPurchase();
                controllerContext.HttpContext.Session["key_cp"] = checkoutPurchase;
            }

            return checkoutPurchase;
        }
    }
}