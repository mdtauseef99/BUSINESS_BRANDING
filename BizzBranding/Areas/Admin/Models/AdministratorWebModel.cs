using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class AdministratorWebModel
    {
        public AdministratorWebModel()
        {
            Administrator = new List<AdministratorWebModel>();
        }
        public int AdminId { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        public string UserName { get; set; }


        [Required(ErrorMessage = "Password is required"), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm your Password"), DataType(DataType.Password)]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Please check your password")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ExpiryDate { get; set; }
        [Display(Name = "Role")]
        public int RoleID { get; set; }

        [Display(Name = "Subject")]
        public int SubjectID { get; set; }
        public bool IsActive { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public string Subject { get; set; }
        public string Role { get; set; }
        public string Membership { get; set; }

        public List<AdministratorWebModel> Administrator { get; set; }
        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
    }
}