using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using System.Web.Mvc;

namespace BizzBranding.CommonUtility
{
   public class CoBrandModel
    {
        public int CoBrandId { get; set; }

        //[Required(ErrorMessage = "Select User")]
        public int? PartnerA { get; set; }

        //[Required(ErrorMessage = "Select User")]
        public int? PartnerB { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public int UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsApproved { get; set; }
        public string BusinessNameA { get; set; }
        public string BusinessNameB { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyLogoB { get; set; }
        public string CoProductImages { get; set; }
        public Nullable<System.DateTime> ActivatedOn { get; set; }
        public Nullable<System.DateTime> ExpiresOn { get; set; }
        public int? No_Month { get; set; }
        public string CompanyDetailsA { get; set; }
        public string CompanyDetailsB { get; set; }

        public string Phone { get; set; }
        public string EmailId { get; set; }
        public string Address { get; set; }
        public string TradeEmailId { get; set; }

        public int CmsId { get; set; }
        public string CMSTitle { get; set; }
        public string Description { get; set; }

       //[Required(ErrorMessage="Name your Co-Branding")]
        public string CoBrandedName { get; set; }

       [Required(ErrorMessage="Provide Details")]
        public string ProductDetails { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }

        public List<CoBrandModel> CoBrandList { get; set; }
        public List<CoBrandingProImgModel> CoBrandProductImages { get; set; }
        public IEnumerable<SelectListItem> PartnerAList { get; set; }
        public IEnumerable<SelectListItem> PartnerBList { get; set; }
        public IEnumerable<SelectListItem> CoBrandedUsersList { get; set; }

    }
}
