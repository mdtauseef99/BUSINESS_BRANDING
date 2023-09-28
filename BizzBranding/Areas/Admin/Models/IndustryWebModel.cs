using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class IndustryWebModel
    {
        public int IndustryId { get; set; }
         [Required(ErrorMessage = "Please enter Industry Name")] 
        public string IndustryName { get; set; }
         [Required(ErrorMessage = "Please enter Description")] 
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public List<IndustryWebModel> Industries { get; set; }
        public IEnumerable<SelectListItem> Industry { get; set; }
    }
}