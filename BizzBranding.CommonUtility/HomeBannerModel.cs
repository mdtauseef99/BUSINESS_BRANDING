using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BizzBranding.CommonUtility
{
    public class HomeBannerModel
    {
        public int HomeBannerID { get; set; }
        public int? UserId { get; set; }

        [Required(ErrorMessage = "Please choose any file to upload")]    
        public string BannerImage { get; set; }
        public bool IsActive { get; set; }

        public string CoAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string TradeEmaiIId { get; set; }
        public string EmailId { get; set; }

        public string CompanyURL { get; set; }

        public string BusinessName { get; set; }

        public string BusinessDetails { get; set; }
        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }

        public List<HomeBannerModel> HomeBannerImgList { get; set; }
        public List<IndustryModel> IndustryList { get; set; }
        public List<CityModel> CityList { get; set; }
        public List<BusinessNewsModel> NewsList { get; set; }
    }
}
