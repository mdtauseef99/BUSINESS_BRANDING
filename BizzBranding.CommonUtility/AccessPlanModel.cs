using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class AccessPlanModel
    {
        public int AccessPlanId { get; set; }
         [Required(ErrorMessage = "AccessPlan Name cannot be null")]
        public string AccessPlanName { get; set; }

        [Required(ErrorMessage = "Description cannot be null")]
        public string Description { get; set; }
        public Nullable<int> MaxNoDeals { get; set; }
        public Nullable<int> ProdPerMonth { get; set; }
        public Nullable<int> UpdatesPerMonth { get; set; }
        public Nullable<decimal> AddPostCharge { get; set; }
        public Nullable<decimal> AddUpdCharge { get; set; }
        public Nullable<decimal> NewsCharge { get; set; }
        public int Validity { get; set; }
        public string CurrencyName { get; set; }
        public Nullable<int> CurrencyId { get; set; }
        public Nullable<decimal> MembershipFee { get; set; }
        public string AccessPlanImage { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }

        public List<AccessPlanModel> AccessPlan { get; set; }
    }
}
