using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class InvestorPartneringModel
    {
        public int InvestorPartnerId { get; set; }
        public int? BusinessUserId { get; set; }
        public string BusinessName { get; set; }
        [Required(ErrorMessage="Enter Details")]
        public string Details { get; set; }
        public string CompanyLogo { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ApprovedOn { get; set; }
        public int? ApprovedBy { get; set; }
        public bool IsActive { get; set; }
        public string Admin { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }

        public string Phone { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string TradeEmailId { get; set; }

        public int CmsId { get; set; }
        public string CMSTitle { get; set; }
        public string Description { get; set; }

        public int? No_Month { get; set; }
        public Nullable<System.DateTime> ExpiresOn { get; set; }

        public List<InvestorPartneringModel> InvestorPartneringList { get; set; }

    }
}
