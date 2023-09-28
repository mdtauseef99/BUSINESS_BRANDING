using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class SearchwordsModel
    {
        public SearchwordsModel()
        {
            CityList = new List<SearchwordsModel>();
            IndustryList = new List<SearchwordsModel>();
            NewsUpdates = new List<BusinessNewsModel>();
        }
        public int Id { get; set; }
        public int MembershipId { get; set; }
        public int ProductId { get; set; }
        public int ProductImageId { get; set; }

        public string ProductName { get; set; }
        public string ProdDesc { get; set; }

        public string BusinessName { get; set; }
        public string EmailId { get; set; }
        public bool IsActive { get; set; }
        public string CompanyLogo { get; set; }
        public int CityId { get; set; }
        public int IndustryId { get; set; }
        public string CityName { get; set; }
        public string IndustryName { get; set; }
        public string CountryName { get; set; }
        public int UserId { get; set; }
        public int TotalConnectCount { get; set; }
        public string BusineeDetails { get; set; }
        public string MembershipName { get; set; }
        public string AccessPlanName { get; set; }


        public Nullable<System.DateTime> ActivatedOn { get; set; }
        public Nullable<System.DateTime> ExpiresOn { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }


        public List<SearchwordsModel> searchkeywords { get; set; }
        public List<SearchwordsModel> IndustryList { get; set; }
        public List<SearchwordsModel> CityList { get; set; }
        public List<BusinessNewsModel> NewsUpdates { get; set; }

    }
}
