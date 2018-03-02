using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OnlineShopApp.Models;
using OnlineShopApp.Helpers;
using System.Collections.Generic;
using System;

namespace OnlineShopApp.Controllers
{
    public class ProductsController : Controller
    {
        private OnlineShopEntities db = new OnlineShopEntities();

        // GET: Products
        public ActionResult Index()
        {
            var product = db.Product.Include(p => p.ProductCategory);
            return View(product.ToList());
        }

        // GET: Products/List
        public ActionResult List()
        {
            return PartialView(db.Product.ToList());
        }

        // GET: Products/ListByCategoryId/5
        public ActionResult ListByCategoryId(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ViewBag.CategoryName = db.ProductCategory.Find(id).Name;
            return View(db.Product.Where(p => p.ProductCategoryId == id).ToList());
        }

        // GET: Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Products/Create
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Create()
        {
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategory, "ProductCategoryId", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Create([Bind(Include = "ProductId,Name,ProductCategoryId,Price,Quantity,Description,ImagePath")] ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var myHelper = new ControllersHelper();
                var imagePath = myHelper.SaveFile(productViewModel.ImagePath, ModelState, Server);

                Product product = new Product
                {
                    Name = productViewModel.Name,
                    Description = productViewModel.Description,
                    Price = productViewModel.Price,
                    Quantity = productViewModel.Quantity,
                    ProductCategoryId = productViewModel.ProductCategoryId,
                    ImagePath = imagePath
                };
                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ProductCategoryId = new SelectList(db.ProductCategory, "ProductCategoryId", "Name", productViewModel.ProductCategoryId);
            return View(productViewModel);
        }

        // GET: Products/Edit/5
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var productCategories = new List<SelectListItem>();
            foreach (var productCategory in db.ProductCategory)
            {
                productCategories.Add(new SelectListItem()
                {
                    Text = productCategory.Name,
                    Value = productCategory.ProductCategoryId.ToString(),
                    // Put all sorts of business logic in here
                    Selected = product.ProductCategoryId == productCategory.ProductCategoryId ? true : false
                });
            }
            ProductViewModel productViewModel = new ProductViewModel
            {
                ProductId = id.Value,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity,
                ImageName = product.ImagePath,
                ProductCategories = productCategories,
                ProductCategoryId = product.ProductCategoryId
            };
            return View(productViewModel);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Edit([Bind(Include = "ProductId,Name,ProductCategoryId,Price,Quantity,Description,ImagePath,ImageName,IsChangeImage")] ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                var imagePath = productViewModel.ImageName;
                if (productViewModel.IsChangeImage)
                {
                    var myHelper = new ControllersHelper();
                    imagePath = myHelper.SaveFile(productViewModel.ImagePath, ModelState, Server);
                }

                Product product = new Product
                {
                    ProductId = productViewModel.ProductId,
                    Name = productViewModel.Name,
                    Description = productViewModel.Description,
                    Price = productViewModel.Price,
                    Quantity = productViewModel.Quantity,
                    ProductCategoryId = productViewModel.ProductCategoryId,
                    ImagePath = imagePath
                };
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductCategoryId = new SelectList(db.ProductCategory, "ProductCategoryId", "Name", productViewModel.ProductCategoryId);
            return View(productViewModel);
        }

        // GET: Products/Delete/5
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            try
            {
                db.Product.Remove(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Error! An error has occured. May be related to a Purchase associated with this Product.");
            }
            
            return View(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
