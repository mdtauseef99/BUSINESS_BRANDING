using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class BussinessUserModel
    {
        public int UserId { get; set; }
        public string BusinessName { get; set; }
        public string FromBusinessName { get; set; }
        public string EmailId { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string CompanyLogo { get; set; }
        public string CompanyBanner { get; set; }
        public string CoRegNo { get; set; } 
        public string CoAddress { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string TradeEmaiIId { get; set; }
        public string BusinessDetails { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> IndustryId { get; set; }
        public Nullable<int> CountryId { get; set; }
        public Nullable<int> CityId { get; set; }
        public string IndustryName { get; set; }
        public string CountryName { get; set; }
        public string CityName { get; set; }
        public string GUId { get; set; }
        public bool IsGlobal { get; set; }
        public string CompURL { get; set; }
        public int MemShipId { get; set; }

        public List<BussinessUserModel> BussinessUser { get; set; }
        public List<HomeBannerModel> HomeBannerList { get; set; }
        //public List<BussinessUserModel> GetallFrndNotlist { get; set; }
        //public virtual List<Bussiness> GetallFrndNotlist()
        //{
        //    List<Customer> customerList = new List<Customer>
        //    {
        //    };
        //    return GetallFrndNotlist;
        //}
    }
}
