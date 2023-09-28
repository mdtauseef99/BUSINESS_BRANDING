using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizzBranding.Areas.Admin.Models
{
    public class CmsWebModel
    {
        public CmsWebModel()
        {
            CMS = new List<CmsWebModel>();
        }
        public int CMSId { get; set; }

        [Required(ErrorMessage = "Please enter Title")] 
        public string CMSTitle { get; set; }

        [Required(ErrorMessage = "Please enter description")] 
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public List<CmsWebModel> CMS { get; set; }
        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
    }
}