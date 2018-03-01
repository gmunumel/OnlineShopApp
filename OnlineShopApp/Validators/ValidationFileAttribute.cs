using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace OnlineShopApp.Validators
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class ValidationFileAttribute : ValidationAttribute
    {
        const string PERMITTED_TYPE_FILE = ".jpeg,.gif,.png,.jpg";

        protected override ValidationResult IsValid(object value,
                 ValidationContext validationContext)
        {
            HttpPostedFileBase file = value as HttpPostedFileBase;

            if (file == null)
            {
                return ValidationResult.Success;
            }

            // The meximum allowed file size is 10MB.
            if (file.ContentLength > 10 * 1024 * 1024)
            {
                return new ValidationResult("This file is too big!");
            }

            // Only Images can be uploaded.
            string ext = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(ext) || !PERMITTED_TYPE_FILE.Split(",".ToCharArray()).Contains(ext))
            {
                return new ValidationResult("This file is not an image");
            }

            // Everything OK.
            return ValidationResult.Success;
        }
    }
}