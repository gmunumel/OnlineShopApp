using Microsoft.AspNet.Identity;
using OnlineShopApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OnlineShopApp.Helpers
{
    public class ControllersHelper
    {
        public string SaveFile(HttpPostedFileBase file, ModelStateDictionary modelState,
            HttpServerUtilityBase server)
        {
            var ret = string.Empty;
            if (file != null && file.ContentLength > 0)
            {
                var fileName = GetRandomImageName();
                var extension = Path.GetExtension(file.FileName);
                var path = Path.Combine(server.MapPath("~/Content/Images/Upload"), fileName + extension);
                file.SaveAs(path);
                ret = fileName + extension;
            }
            return ret;
        }

        public List<SelectListItem> GetRolesForUser(ApplicationDbContext context, 
            ApplicationUserManager userManager, System.Security.Principal.IPrincipal user = null)
        {
            var allUserRoles = context.Roles.Select(r => new SelectListItem { Value = r.Name, Text = r.Name }).ToList();
            var userRoles = new List<SelectListItem>();

            var userRole = user == null ? RoleConstants.Client : userManager.GetRoles(user.Identity.GetUserId()).FirstOrDefault();
            foreach (var role in allUserRoles)
            {
                userRoles.Add(new SelectListItem()
                {
                    Text = role.Text,
                    Value = role.Value.ToString(),
                    // Put all sorts of business logic in here
                    Selected = userRole == role.Value ? true : false
                });
            }
            return userRoles;
        }

        private string GetRandomImageName()
        {
            return RandomDigits(10);
        }

        private string RandomDigits(int length)
        {
            var random = new Random();
            string s = string.Empty;
            for (int i = 0; i < length; i++)
                s = string.Concat(s, random.Next(10).ToString());
            return s;
        }
    }
}