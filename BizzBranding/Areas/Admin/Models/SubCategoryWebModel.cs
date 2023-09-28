using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class SubCategoryWebModel
    {
         [Required(ErrorMessage = "Please select subcategory")] 
        public int SubCategoryId { get; set; }
        [Required(ErrorMessage = "Please enter valid Subcategory name")] 
        public string SubCatgoryName { get; set; }
         [Required(ErrorMessage = "Please select category")]
        public Nullable<int> ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public List<SubCategoryWebModel> SubCategories { get; set; }
        public IEnumerable<SelectListItem> SubCategory { get; set; }
        public IEnumerable<SelectListItem> CategoryNames { get; set; }

    }
}