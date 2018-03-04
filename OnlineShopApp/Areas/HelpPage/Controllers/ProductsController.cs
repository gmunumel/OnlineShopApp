using OnlineShopApp.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace OnlineShopApp.Areas.HelpPage.Controllers
{
    /// <summary>
    /// API to get all information regarding products
    /// </summary>
    public class ProductsController : ApiController
    {
        private OnlineShopEntities db = new OnlineShopEntities();

        /// <summary>
        /// Get all the products
        /// </summary>
        // GET api/products
        public IEnumerable<ProductViewModel> Get()
        {
            List<ProductViewModel> productsViewModel = new List<ProductViewModel>();
            foreach(var product in db.Product)
            {
                productsViewModel.Add(GetProductViewModel(product));
            } 
            return productsViewModel;
        }

        /// <summary>
        /// Get a product by id
        /// </summary>
        // GET api/products/5
        public IHttpActionResult Get(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null) return NotFound();
            return Ok(GetProductViewModel(product));
        }

        /// <summary>
        /// Create a new product
        /// </summary>
        // POST api/products
        public HttpResponseMessage Post([FromBody] ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (db.ProductCategory.Find(productViewModel.ProductCategoryId) == null)
                {
                    ModelState.AddModelError("ProductCategoryId", "Not found key");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                Product product = new Product
                {
                    Name = productViewModel.Name,
                    Description = productViewModel.Description,
                    Price = productViewModel.Price,
                    Quantity = productViewModel.Quantity,
                    ProductCategoryId = productViewModel.ProductCategoryId,
                    ImagePath = productViewModel.ImageName
                };
                db.Product.Add(product);
                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Update a given product
        /// </summary>
        // PUT api/products/5
        public HttpResponseMessage Put(int id, [FromBody] ProductViewModel productViewModel)
        {
            if (ModelState.IsValid)
            {
                if (db.ProductCategory.Find(productViewModel.ProductCategoryId) == null)
                {
                    ModelState.AddModelError("ProductCategoryId", "Not found key");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                Product product = db.Product.Find(id);
                if (product == null)
                {
                    ModelState.AddModelError("ProductId", "Not found key");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }
                product.Name = productViewModel.Name;
                product.Description = productViewModel.Description;
                product.Price = productViewModel.Price;
                product.Quantity = productViewModel.Quantity;
                product.ProductCategoryId = productViewModel.ProductCategoryId;
                product.ImagePath = productViewModel.ImageName;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.OK);
            }

            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Delete a given product
        /// </summary>
        // DELETE api/products/5
        public HttpResponseMessage Delete(int id)
        {
            Product product = db.Product.Find(id);
            if (product == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            db.Product.Remove(product);
            db.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }

        private ProductViewModel GetProductViewModel(Product product)
        {
            return new ProductViewModel
            {
                ProductId = product.ProductId,
                Name = product.Name,
                ProductCategoryId = product.ProductCategoryId,
                ProductCategories = null,
                Price = product.Price,
                Quantity = product.Quantity,
                Description = product.Description,
                ImagePath = null,
                ImageName = product.ImagePath,
                IsChangeImage = false
            };
        }
    }
}