using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OnlineShopApp.Models;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using static OnlineShopApp.Controllers.ManageController;
using OnlineShopApp.Helpers;
using System.Threading.Tasks;

namespace OnlineShopApp.Controllers
{
    public class UsersController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private ApplicationDbContext db = new ApplicationDbContext();

        public UsersController()
        {
            db = new ApplicationDbContext();
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
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
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
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Users
        public ActionResult Index()
        {
            List<ApplicationUser> appUsers = db.Users.ToList();
            List<UserViewModel> model = new List<UserViewModel>();
        
            foreach (ApplicationUser appUser in appUsers)
            {
                model.Add(GetUserViewModel(appUser));
            }
            return View(model);
        }

        // GET: Users/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser appUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(GetUserViewModel(appUser));
        }

        // GET: Users/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Id,Email,UserName,SelectedRoleName,Password")] UserViewModel userViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.UserViewModels.Add(userViewModel);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(userViewModel);
        //}

        // GET: Users/Edit/5
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser appUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(GetUserViewModel(appUser));
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Email,UserName,SelectedRoleName,NewPassword,OldPassword,ConfirmPassword")]
            UserViewModel model)
        {
            var myHelper = new ControllersHelper();
            if (!ModelState.IsValid)
            {
                model.RoleChoices = db.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
                model.SelectedRoleName = UserManager.GetRoles(model.Id).FirstOrDefault();
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(model.Id, model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                
                var roles = await UserManager.GetRolesAsync(model.Id);
                IdentityResult resultRole = await UserManager.RemoveFromRolesAsync(model.Id, roles.ToArray());
                if (resultRole.Succeeded)
                {
                    // add role
                    await this.UserManager.AddToRoleAsync(user.Id, model.SelectedRoleName);

                    user.UserName = model.UserName;
                    user.Email = model.Email;
                    IdentityResult resultUser = UserManager.Update(user);
                    if (resultUser.Succeeded)
                    {
                        return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                }
            }
            model.RoleChoices = db.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            model.SelectedRoleName = UserManager.GetRoles(model.Id).FirstOrDefault();
            AddErrors(result);
            return View(model);

            //if (ModelState.IsValid)
            //{
            //    db.Entry(userViewModel).State = EntityState.Modified;
            //    db.SaveChanges();
            //    return RedirectToAction("Index");
            //}
            //return View(userViewModel);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser appUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            if (appUser == null)
            {
                return HttpNotFound();
            }
            return View(GetUserViewModel(appUser));
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleConstants.Admin)]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser appUser = db.Users.Where(u => u.Id == id).FirstOrDefault();
            db.Users.Remove(appUser);
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

        #region Helpers
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private UserViewModel GetUserViewModel(ApplicationUser appUser)
        {
            UserViewModel model = new UserViewModel
            {
                Id = appUser.Id,
                UserName = appUser.UserName,
                Email = appUser.Email,
                SelectedRoleName = UserManager.GetRoles(appUser.Id).FirstOrDefault(),
                RoleChoices = db.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList()
            };
            return model;
        }
        #endregion

    }
}
