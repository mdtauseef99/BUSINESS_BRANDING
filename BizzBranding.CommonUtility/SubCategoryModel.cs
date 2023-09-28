using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class SubCategoryModel
    {
        public int SubCategoryId { get; set; }
        public string SubCatgoryName { get; set; }
        public Nullable<int> ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }
        public List<SubCategoryModel> SubCategory { get; set; }
        public List<CategoryModel> Category { get; set; }
    }
}
