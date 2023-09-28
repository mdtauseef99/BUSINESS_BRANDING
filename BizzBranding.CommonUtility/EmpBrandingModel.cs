using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BizzBranding.CommonUtility;

namespace BizzBranding.CommonUtility
{
   public class EmpBrandingModel
    {
       public int EmpBrandingId { get; set; }
       public int? EmployerId { get; set; }
       [Required(ErrorMessage="Enter Employee Name")]
       public string EmpName { get; set; }
       //[MustBeSelected(ErrorMessage = "Please Select City")]
       public Nullable<int> CityId { get; set; }
       public Nullable<int> IndustryId { get; set; }

       public string EmpImage { get; set; }
       public string EmployerName { get; set; }
       
       [Required(ErrorMessage="Enter Details")]
       public string Message { get; set; }
       public Nullable<System.DateTime> CreateOn { get; set; }
       public int? CreatedBy { get; set; }
       public Nullable<System.DateTime> UpdatedOn { get; set; }
       public int? UpdateBy { get; set; }
       public bool IsActive { get; set; }
       public string EmployerCompanyLogo { get; set; }
       public int CmsId { get; set; }
       public string CMSTitle { get; set; }
       public string Description { get; set; }

       public IEnumerable<SelectListItem> IndustryNames { get; set; }
       public IEnumerable<SelectListItem> CountryNames { get; set; }
       public IEnumerable<SelectListItem> CityNames { get; set; }

       public int Pagecount { get; set; }
       public int PageID { get; set; }
       public int Current { get; set; }

       public int? No_Month { get; set; }


       public List<EmpBrandingModel> EmpBrandingList { get; set; }

       //public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable // IClientValidatable for client side Validation
       //{
       //    public override bool IsValid(object value)
       //    {
       //        return value is bool && (bool)value;
       //    }
       //    // Implement IClientValidatable for client side Validation
       //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
       //    {
       //        return new ModelClientValidationRule[] { new ModelClientValidationRule { ValidationType = "checkbox", ErrorMessage = this.ErrorMessage } };
       //    }
       //}

       //public class MustBeSelected : ValidationAttribute, IClientValidatable // IClientValidatable for client side Validation
       //{
       //    public override bool IsValid(object value)
       //    {
       //        if (value == null || (int)value == 0)
       //            return false;
       //        else
       //            return true;
       //    }
       //    // Implement IClientValidatable for client side Validation
       //    public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
       //    {
       //        return new ModelClientValidationRule[] { new ModelClientValidationRule { ValidationType = "dropdown", ErrorMessage = this.ErrorMessage } };
       //    }
       //}
    }
}
