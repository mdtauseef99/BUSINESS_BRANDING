using BizzBranding.CommonUtility;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Models
{
    public class BizUserHomeModel
    {

        public int UserId { get; set; }
        [Required]
        public string BusinessName { get; set; }
        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is not valid")]
        public string EmailId { get; set; }
        [Required]
        public string Password { get; set; }
        [Required(ErrorMessage = "Please confirm your Password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "Please check your password")]
        public string ConfirmPassword { get; set; }
        [Required]
        public string CompanyLogo { get; set; }
        public string CoRegNo { get; set; }
        public string CoAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string TradeEmaiIId { get; set; }
        public string BusinessDetails { get; set; }
        public Nullable<int> IndustryId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string IndustryName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public List<BizUserHomeModel> Users { get; set; }

        public IEnumerable<SelectListItem> User { get; set; }
        public IEnumerable<SelectListItem> IndustryNames { get; set; }
        public IEnumerable<SelectListItem> CountryNames { get; set; }
        public IEnumerable<SelectListItem> CityNames { get; set; }

    }
}