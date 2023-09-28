using BizzBranding.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBranding.CommonUtility;
using System.ComponentModel.DataAnnotations;

namespace BizzBranding.Areas.Admin.Models
{
    public class ProductWebModel
    {
        public int ProductId { get; set; }
        [Required(ErrorMessage = "Please enter Product Name")]
        public string ProductName { get; set; }

         [Required(ErrorMessage = "Please select category")]
        public Nullable<int> CategoryId { get; set; }

         [Required(ErrorMessage = "Please select subcategory")]
        public Nullable<int> SubCategoryId { get; set; }

        [Required(ErrorMessage = "Please enter details")]
        public string ProdDetails { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }

        public int UserId { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public List<ProductWebModel> Products{ get; set; }
        public IEnumerable<SelectListItem> Product{ get; set; }
        public IEnumerable<SelectListItem> CategoryNames { get; set; }
        public IEnumerable<SelectListItem> SubCategoryNames { get; set; }
        public List<ProductImageWebModel> ProductImages { get; set; }
        public List<BusinessNewsModel> NewsUpdates { get; set; }
        public List<IndustryModel> IndustryList { get; set; }
        public List<CityModel> CityList { get; set; }
        public string ProdImage { get; set; }
    }

    public class ProductImageWebModel
    {
        public int ProductImgId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProdImage { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ProductName { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public virtual Product Product { get; set; }

        public List<ProductImageWebModel> ProductImages { get; set; }
        public IEnumerable<SelectListItem> ProductImage { get; set; }
        public IEnumerable<SelectListItem> ProductNames { get; set; }

    }
}