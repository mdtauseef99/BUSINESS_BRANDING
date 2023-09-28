using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
   public class MembershipWithAccessPlanModel
    {
        public int MembershipID { get; set; }
        public int? AccessPlanId { get; set; }
        public int? UserId { get; set; }
        public DateTime? ActivatedOn { get; set; }
        public DateTime? ExpiresOn { get; set; }
        public bool IsActive { get; set; }

        public string AccessPlanName { get; set; }
        public string Description { get; set; }
        public int? MaxNoDeals { get; set; }
        public int? ProdPerMonth { get; set; }
        public int? UpdatesPerMonth { get; set; }
        public decimal? AddPostCharge { get; set; }
        public decimal? AddUpdCharge { get; set; }
        public decimal? NewsCharge { get; set; }
        public int Validity { get; set; }
        public string CurrencyName { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? MembershipFee { get; set; }
    }
}
