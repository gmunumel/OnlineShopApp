using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using OnlineShopApp.Models;

using OnlineShopApp.Helpers;

namespace OnlineShopApp.Controllers
{
    public class ProductCategoriesController : Controller
    {

        private OnlineShopEntities db = new OnlineShopEntities();

        // GET: ProductCategories
        public ActionResult Index()
        {
            return View(db.ProductCategory.ToList());
        }

        // GET: ProductCategories/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // GET: ProductCategories/Create
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductCategories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Create([Bind(Include = "ProductCategoryId,Name,Description,ImagePath")] ProductCategoryViewModel productCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var myHelper = new ControllersHelper();
                var imagePath = myHelper.SaveFile(productCategoryViewModel.ImagePath, ModelState, Server);

                ProductCategory productCategory = new ProductCategory
                {
                    Name = productCategoryViewModel.Name,
                    Description = productCategoryViewModel.Description,
                    ImagePath = imagePath
                };
                db.ProductCategory.Add(productCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productCategoryViewModel);
        }

        // GET: ProductCategories/Edit/5
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            ProductCategoryViewModel productCategoryViewModel = new ProductCategoryViewModel
            {
                ProductCategoryId = id.Value,
                Name = productCategory.Name,
                Description = productCategory.Description,
                ImageName = productCategory.ImagePath
            };
            return View(productCategoryViewModel);
        }

        // POST: ProductCategories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Edit([Bind(Include = "ProductCategoryId,Name,Description,ImagePath,IsChangeImage")] ProductCategoryViewModel productCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                var imagePath = string.Empty;
                if (productCategoryViewModel.IsChangeImage)
                {
                    var myHelper = new ControllersHelper();
                    imagePath = myHelper.SaveFile(productCategoryViewModel.ImagePath, ModelState, Server);
                }
                
                ProductCategory productCategory = new ProductCategory
                {
                    ProductCategoryId = productCategoryViewModel.ProductCategoryId,
                    Name = productCategoryViewModel.Name,
                    Description = productCategoryViewModel.Description,
                    ImagePath = imagePath
                };
                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productCategoryViewModel);
        }

        // GET: ProductCategories/Delete/5
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductCategory productCategory = db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return HttpNotFound();
            }
            return View(productCategory);
        }

        // POST: ProductCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductCategory productCategory = db.ProductCategory.Find(id);
            db.ProductCategory.Remove(productCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
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
