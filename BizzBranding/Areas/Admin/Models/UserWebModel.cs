using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class UserWebModel
    {
        public int UserId { get; set; }

        [Required(ErrorMessage="BusinessName required")]
        public string BusinessName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is not valid")]
        public string EmailId { get; set; }
        
        [Required(ErrorMessage="Password Required")]
        public string Password { get; set; }
        
        [Required(ErrorMessage = "Please confirm your Password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        
        [MustBeTrue(ErrorMessage = "Please Check the Checkbox")]
        public bool TermsAccepted { get; set; }

        public string TermsConditions { get; set; }

        //[Required(ErrorMessage="Logo is Required")]
        public string CompanyLogo { get; set; }

        public string CoRegNo { get; set; }
        public string CoAddress { get; set; }

        public string CompanyURL {get;set;}

         [Required(ErrorMessage="Telephone Number Required")]
        //[RegularExpression(@"^\(?\)?$", ErrorMessage = "Entered phone format is not valid.")]
         [RegularExpression(@"^[0-9]{0,15}$", ErrorMessage = "PhoneNumber should contain only numbers")]
        public string Phone { get; set; }
        public string Fax { get; set; }

        [Required(ErrorMessage = "Contact Person Required")]
        public string ContactPerson { get; set; }
        public string Designation { get; set; }

        [Required(ErrorMessage = "Trade EmailId Required")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is not valid")]
        public string TradeEmaiIId { get; set; }

        [Required(ErrorMessage = "Enter your Business Details")]
        public string BusinessDetails { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public Nullable<int> IndustryId { get; set; }
        [MustBeSelected(ErrorMessage = "Please Select Country")]
        public Nullable<int> CountryId { get; set; }
        [MustBeSelected(ErrorMessage = "Please Select City")]
        public Nullable<int> CityId { get; set; }

        [Required(ErrorMessage = "Select Industry")]
        public string IndustryName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        
        public bool IsGlobal { get; set; }

        public bool IsActive { get; set; }

        public string CountUser { get; set; }
        public List<UserWebModel> Users { get; set; }
        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public IEnumerable<SelectListItem> User { get; set; }
        public IEnumerable<SelectListItem> IndustryNames { get; set; }
        public IEnumerable<SelectListItem> CountryNames { get; set; }
        public IEnumerable<SelectListItem> CityNames { get; set; }
        public IEnumerable<SelectListItem> Industrylist { get; set; }
        public IEnumerable<string> SelectItems { set; get; }
        public List<SelectListItem> Industries { set; get; }


        public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable // IClientValidatable for client side Validation
        {
            public override bool IsValid(object value)
            {
                return value is bool && (bool)value;
            }
            // Implement IClientValidatable for client side Validation
            public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
            {
                return new ModelClientValidationRule[] { new ModelClientValidationRule { ValidationType = "checkbox", ErrorMessage = this.ErrorMessage } };
            }
        }

        public class MustBeSelected : ValidationAttribute, IClientValidatable // IClientValidatable for client side Validation
        {
            public override bool IsValid(object value)
            {
                if (value == null || (int)value == 0)
                    return false;
                else
                    return true;
            }
            // Implement IClientValidatable for client side Validation
            public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
            {
                return new ModelClientValidationRule[] { new ModelClientValidationRule { ValidationType = "dropdown", ErrorMessage = this.ErrorMessage } };
            }
        }
    }
}