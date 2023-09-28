using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class EmailTemplateWebModel
    {
        public int EmailTempId { get; set; }
        [Required(ErrorMessage = "Please enter TemplateTitle")] 
        public string EmailTempTitle { get; set; }
         [Required(ErrorMessage = "Please enter description")] 
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public List<EmailTemplateWebModel> EmailTemplates { get; set; }
        public IEnumerable<SelectListItem> EmailTemplate { get; set; }
    }
}