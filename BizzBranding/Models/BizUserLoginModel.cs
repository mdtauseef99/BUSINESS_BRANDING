using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizzBranding.Models
{
    public class BizUserLoginModel
    {
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is not valid")]
        public string Email { get; set; }
        [Required(ErrorMessage="Enter Password")]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public string Msg { get; set; }
       // [Required(ErrorMessage="Enter New Password")]
        public string NewPassword { get; set; }

        public List<BizUserLoginModel> IndustryList { get; set; }
        public List<BizUserLoginModel> CityList { get; set; }

    }
}