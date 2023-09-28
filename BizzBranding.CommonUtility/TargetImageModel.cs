using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
   public class TargetImageModel
    {
       public int TargetImageId { get; set; }
        public int? TargetBrandingId { get; set; }
        //[MustBeSelected(ErrorMessage = "Please Select City")]
        public Nullable<int> CityId { get; set; }
        public Nullable<int> IndustryId { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }

        public string URL { get; set; }

        public int? BusinessUserId { get; set; }
        public string BusinessName { get; set; }
        public string CityName { get; set; }
        public string IndustryName { get; set; }
        public int? No_Month { get; set; }

        //public Nullable<System.DateTime> ExpiresOn { get; set; }
        //public IEnumerable<SelectListItem> IndustryNames { get; set; }
        //public IEnumerable<SelectListItem> CountryNames { get; set; }
        //public IEnumerable<SelectListItem> CityNames { get; set; }
        //public IEnumerable<SelectListItem> Industrylist { get; set; }
        //public IEnumerable<string> SelectItems { set; get; }
        //public List<SelectListItem> Industries { set; get; }

        public List<TargetImageModel> TargetImageList { get; set; }
    }
}
