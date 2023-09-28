using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class AccessPlanWebModel
    {
        public int AccessPlanId { get; set; }
         [Required(ErrorMessage = "Please enter Accessplan name")] 
        public string AccessPlanName { get; set; }
         [Required(ErrorMessage = "Please enter AccessPlan Description")] 
        public string Description { get; set; }
        public Nullable<int> MaxNoDeals { get; set; }
        public Nullable<int> ProdPerMonth { get; set; }
        public Nullable<int> UpdatesPerMonth { get; set; }
        public Nullable<decimal> AddPostCharge { get; set; }
        public Nullable<decimal> AddUpdCharge { get; set; }
        public Nullable<decimal> NewsCharge { get; set; }
        public int Validity { get; set; }
        public string CurrencyName { get; set; }
        [Required(ErrorMessage = "Please select currency")] 
        public Nullable<int> CurrencyId { get; set; }
        public Nullable<decimal> MembershipFee { get; set; }
        public string AccessPlanImage { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public List<AccessPlanWebModel> AccessPlans{ get; set; }
        public IEnumerable<SelectListItem> AccessPlan { get; set; }
        public IEnumerable<SelectListItem> CurrencyNames { get; set; }
    }
}