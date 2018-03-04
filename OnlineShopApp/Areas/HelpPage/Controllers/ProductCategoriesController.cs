using OnlineShopApp.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineShopApp.Areas.HelpPage.Controllers
{
    /// <summary>
    /// API to get all information regarding product categories
    /// </summary>
    public class ProductCategoriesController : ApiController
    {
        private OnlineShopEntities db = new OnlineShopEntities();

        /// <summary>
        /// Get all the product categories
        /// </summary>
        // GET api/productcategories
        public IEnumerable<ProductCategoryViewModel> Get()
        {
            List<ProductCategoryViewModel> productCategoriesViewModel = new List<ProductCategoryViewModel>();
            foreach (var productCategory in db.ProductCategory)
            {
                productCategoriesViewModel.Add(GetProductCategoryViewModel(productCategory));
            }
            return productCategoriesViewModel;
        }

        /// <summary>
        /// Get a product category by id
        /// </summary>
        // GET api/productcategories/5
        public IHttpActionResult Get(int id)
        {
            ProductCategory productCategory = db.ProductCategory.Find(id);
            if (productCategory == null) return NotFound();
            return Ok(GetProductCategoryViewModel(productCategory));
        }

        /// <summary>
        /// Create a new product category
        /// </summary>
        // POST api/productcategories
        public HttpResponseMessage Post([FromBody] ProductCategoryViewModel productCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductCategory productCategory = new ProductCategory
                {
                    Name = productCategoryViewModel.Name,
                    Description = productCategoryViewModel.Description,
                    ProductCategoryId = productCategoryViewModel.ProductCategoryId,
                    ImagePath = productCategoryViewModel.ImageName
                };
                db.ProductCategory.Add(productCategory);
                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Update a given product category
        /// </summary>
        // PUT api/productcategories/5
        public HttpResponseMessage Put(int id, [FromBody] ProductCategoryViewModel productCategoryViewModel)
        {
            if (ModelState.IsValid)
            {
                ProductCategory productCategory = db.ProductCategory.Find(id);
                if (productCategory == null)
                {
                    ModelState.AddModelError("ProductCategoryId", "Not found key");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                productCategory.Name = productCategoryViewModel.Name;
                productCategory.Description = productCategoryViewModel.Description;
                productCategory.ProductCategoryId = productCategoryViewModel.ProductCategoryId;
                productCategory.ImagePath = productCategoryViewModel.ImageName;
                db.Entry(productCategory).State = EntityState.Modified;
                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Delete a given product category
        /// </summary>
        // DELETE api/productcategories/5
        public HttpResponseMessage Delete(int id)
        {
            ProductCategory productCategory = db.ProductCategory.Find(id);
            if (productCategory == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            db.ProductCategory.Remove(productCategory);
            db.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private ProductCategoryViewModel GetProductCategoryViewModel(ProductCategory productCategory)
        {
            return new ProductCategoryViewModel
            {
                ProductCategoryId = productCategory.ProductCategoryId,
                Name = productCategory.Name,
                Description = productCategory.Description,
                ImagePath = null,
                ImageName = productCategory.ImagePath,
                IsChangeImage = false
            };
        }
    }
}