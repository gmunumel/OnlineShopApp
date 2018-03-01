using OnlineShopApp.Models;
using System.Net;
using System.Web.Mvc;

namespace OnlineShopApp.Controllers
{
    [Authorize(Roles = RoleConstants.Client)]
    public class CheckoutController : Controller
    {
        private OnlineShopEntities onlineShopContext = new OnlineShopEntities();

        // GET: Checkout
        public ActionResult Index(CheckoutPurchase checkoutPurchase)
        {
            return View(checkoutPurchase);
        }

        public ActionResult AddProduct(CheckoutPurchase checkoutPurchase, int productId, string clientName, int quantity = 0)
        {
            Product product = onlineShopContext.Product.Find(productId);
            CheckoutProduct myCheckoutPurchase = new CheckoutProduct
            {
                Index = checkoutPurchase.Count,
                ProductId = product.ProductId,
                ProductName = product.Name,
                ProductImage = product.ImagePath,
                ClientName = clientName,
                ProductPrice = product.Price,
                Quantity = quantity
            };
            checkoutPurchase.Add(myCheckoutPurchase);

            return RedirectToAction("Index", "Products");
        }

        public ActionResult QuantityProducts(CheckoutPurchase checkoutPurchase)
        {
            return PartialView(checkoutPurchase);
        }

        // GET: Checkout/Edit/5
        public ActionResult Edit(CheckoutPurchase checkoutPurchase, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(checkoutPurchase[id.Value] as CheckoutProduct);
        }

        // POST: Checkout/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Index,ProductId,ProductName,ProductImage,ClientName,ProductPrice,Quantity")] CheckoutProduct checkoutProductViewModel,
            CheckoutPurchase checkoutPurchase)
        {
            if (ModelState.IsValid)
            {
                Product product = onlineShopContext.Product.Find(checkoutProductViewModel.ProductId);
                if (product.Quantity >= checkoutProductViewModel.Quantity)
                {
                    checkoutPurchase[checkoutProductViewModel.Index].Quantity = checkoutProductViewModel.Quantity;
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("Quantity", "Error! Quantity cannot be higher than stock available");
                }
            }
            return View(checkoutProductViewModel);
        }

        // GET: Checkout/Delete/5
        public ActionResult Delete(CheckoutPurchase checkoutPurchase, int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return View(checkoutPurchase[id.Value] as CheckoutProduct);
        }

        // POST: Checkout/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(CheckoutPurchase checkoutPurchase, int id)
        {
            checkoutPurchase.RemoveAt(id);
            return RedirectToAction("Index");
        }
    }
}