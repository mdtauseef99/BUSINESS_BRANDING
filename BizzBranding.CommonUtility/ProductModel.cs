using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;


namespace BizzBranding.CommonUtility
{
    public class ProductModel
    {
        public int ProductId { get; set; }
        
        [Required(ErrorMessage="Product Name cannot be null")]
        [StringLength(50)]
        public string ProductName { get; set; }
        public Nullable<int> CategoryId { get; set; }
        public Nullable<int> SubCategoryId { get; set; }

        [Required(ErrorMessage="Product Description cannot be null")]
        public string ProdDetails { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string CategoryName { get; set; }
        public string SubCategoryName { get; set; }
        public int UserId { get; set; }
        public List<ProductModel> Product { get; set; }
    }


    public class ProductImageModel
    {
        public int ProductImageId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProdImage { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public string ProductName { get; set; }
    }
}
