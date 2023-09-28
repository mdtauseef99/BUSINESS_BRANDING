using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BizzBranding.CommonUtility
{
   public class TargetBrandingModel
    {
        public int TargetBrandingId { get; set; }
        //[MustBeSelected(ErrorMessage = "Please Select City")]
        public Nullable<int> CityId { get; set; }
        public Nullable<int> IndustryId { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }

       [DisplayFormat(DataFormatString="{0:d}",ApplyFormatInEditMode=true)]
        public Nullable<System.DateTime> CreatedDate { get; set; }

       [DisplayFormat(DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> UpdateDate { get; set; }

       [DisplayFormat(DataFormatString = "{0:d}")]
        public Nullable<System.DateTime> ExpiresOn { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public string CoAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string TradeEmaiIId { get; set; }
       [Required(ErrorMessage="Required")]
        public string URL { get; set; }
        public string EmailId { get; set; }
        public int? BusinessUserId { get; set; }
        public string BusinessName { get; set; }
        public string CityName { get; set; }
        public string IndustryName { get; set; }
        public int? No_Month { get; set; }

       [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public int? DaysCounter { get; set; }

        public IEnumerable<SelectListItem> IndustryNames { get; set; }
        public IEnumerable<SelectListItem> CountryNames { get; set; }
        public IEnumerable<SelectListItem> CityNames { get; set; }
        public IEnumerable<SelectListItem> Industrylist { get; set; }
        public IEnumerable<string> SelectItems { set; get; }
        public List<SelectListItem> Industries { set; get; }

        public List<TargetBrandingModel> TargetBrandingList { get; set; }
        public IEnumerable<SelectListItem> BusinessUserList { get; set; }
        public IEnumerable<SelectListItem> BusinessNames { get; set; }
        public List<TargetImageModel> TargetImageList { get; set; }
    }
}
