using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class CategoryWebModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name cannot be null")]
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public List<CategoryWebModel> Categories { get; set; }
        public IEnumerable<SelectListItem> Category { get; set; }
        public IEnumerable<SelectListItem> SubCategoryNames { get; set; }
    }
}