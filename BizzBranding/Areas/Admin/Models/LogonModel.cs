using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizzBranding.Areas.Admin.Models
{
    public class LogonModel
    {
         [Required(ErrorMessage = "Please enter UserName")] 
        public string UserName { get; set; }
         [Required(ErrorMessage = "Please enter Password")] 
        public string Password { get; set; }

        public string Returnurl { get; set; }
    }
}