using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using OnlineShopApp.Helpers;
using OnlineShopApp.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace OnlineShopApp.Areas.HelpPage.Controllers
{
    /// <summary>
    /// API to get all information regarding users
    /// </summary>
    public class UsersController : ApiController
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db;
        private OnlineShopEntities onlineShopDB;

        public UsersController()
        {
            db = new ApplicationDbContext();
            onlineShopDB = new OnlineShopEntities();
        }

        public UsersController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        /// <summary>
        /// Get all the users
        /// </summary>
        // GET api/users
        public IEnumerable<UserViewModel> Get()
        {
            List<ApplicationUser> appUsers = db.Users.ToList();
            List<UserViewModel> userViewModel = new List<UserViewModel>();

            foreach (ApplicationUser appUser in appUsers)
            {
                var myHelper = new ControllersHelper();
                userViewModel.Add(myHelper.GetUserViewModel(appUser, db, UserManager));
            }
            return userViewModel;
        }

        /// <summary>
        /// Get an user by id
        /// </summary>
        // GET api/users/5
        public IHttpActionResult Get(string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            ApplicationUser appUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (appUser == null) return NotFound();
            var myHelper = new ControllersHelper();
            return Ok(myHelper.GetUserViewModel(appUser, db, UserManager));
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        // POST api/users
        public async Task<HttpResponseMessage> Post([FromBody] RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = registerViewModel.UserName, Email = registerViewModel.Email };
                var result = await UserManager.CreateAsync(user, registerViewModel.Password);
                if (result.Succeeded)
                {
                    await this.UserManager.AddToRoleAsync(user.Id, registerViewModel.SelectedUserRole);
                    return new HttpResponseMessage(HttpStatusCode.OK);
                }
            }

            // If we got this far, something failed, redisplay form
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Update a given user
        /// </summary>
        // PUT api/users/5
        public async Task<HttpResponseMessage> Put(string id, [FromBody]UserViewModel userViewModel)
        {
            var myHelper = new ControllersHelper();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Not found key");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            var result = await UserManager.ChangePasswordAsync(id, userViewModel.OldPassword, userViewModel.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(id);
                if (user == null)
                {
                    ModelState.AddModelError("", "User Not found");
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
                }

                var roles = await UserManager.GetRolesAsync(id);
                IdentityResult resultRole = await UserManager.RemoveFromRolesAsync(id, roles.ToArray());
                if (resultRole.Succeeded)
                {
                    // add role
                    await this.UserManager.AddToRoleAsync(user.Id, userViewModel.SelectedRoleName);

                    user.UserName = userViewModel.UserName;
                    user.Email = userViewModel.Email;
                    IdentityResult resultUser = UserManager.Update(user);
                    if (resultUser.Succeeded)
                    {
                        return new HttpResponseMessage(HttpStatusCode.OK);
                    }
                }
            }
            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        }

        /// <summary>
        /// Delete a given user
        /// </summary>
        // DELETE api/users/5
        public HttpResponseMessage Delete(string id)
        {
            if (string.IsNullOrEmpty(id)) return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            ApplicationUser appUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (appUser == null)
            {
                ModelState.AddModelError("", "User Not found");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            var isPurchaseForClientExists = onlineShopDB.Purchase.Any(p => p.ClientId == appUser.Id);
            if (isPurchaseForClientExists)
            {
                ModelState.AddModelError("", "Error! An error has occured. May be related to a Purchase associated with this Client.");
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
            db.Users.Remove(appUser);
            db.SaveChanges();
            return new HttpResponseMessage(HttpStatusCode.OK);
        }
    }
}