using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class CategoryModel
    {
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "Category Name cannot be null")]
        public string CategoryName { get; set; }
        public bool IsActive { get; set; }

        public List<CategoryModel> Category { get; set; }
    }
}
