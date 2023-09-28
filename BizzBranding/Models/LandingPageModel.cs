using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBranding.CommonUtility;

namespace BizzBranding.Models
{
    public class LandingPageModel
    {
        public LandingPageModel()
        {
            ConnectedBizList = new List<LandingPageModel>();
            ProductsList = new List<LandingPageModel>();
            CMSList = new List<LandingPageModel>();
            PopupList = new List<LandingPageModel>();
            RequestedEnquiries = new List<LandingPageModel>();
            CompanyProfile = new List<LandingPageModel>();
            PendingRequests = new List<LandingPageModel>();
            CoBrandRequest = new List<LandingPageModel>();
            SuggestedConn = new List<LandingPageModel>();
            RecentJoinee = new List<LandingPageModel>();
            MostConnected = new List<LandingPageModel>();
            IndustryList = new List<IndustryModel>();
            CityList = new List<CityModel>();
            PremiumBottom = new List<LandingPageModel>();
            PremiumLeft = new List<LandingPageModel>();
            PremiumRight = new List<LandingPageModel>();
            NewsUpdates = new List<BusinessNewsModel>();
            AdminHomeNews = new List<NewsModel>();
            BusinessJobs = new List<BusinessJobModel>();
            //RequestList = new List<LandingPageModel>();
            // Info = new List<LandingPageModel>();
        }

        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        public string EmailId { get; set; }
        public int UserId { get; set; }

        /// <new Add>
        public int MembershipID { get; set; }
        /// </new Add end>

        public int? PartnerId { get; set; }
        //public int UserFriendid { get; set; }
        public string CompanyLogo { get; set; }
        //public string Description { get; set; }
        public string Regno { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string ContactPerson { get; set; }
        public string Designation { get; set; }
        public string TradeEmailId { get; set; }
        public int TotalConnectCount { get; set; }
        public int HitCount { get; set; }
        public int TBBPoints { get; set; }
        public int EnquiryCount { get; set; }

        [Required(ErrorMessage = "Enter Name")]
        public string CustomerName { get; set; }

        [Required(ErrorMessage = "Enter Phone")]
        public string CustomerPhone { get; set; }

        [Required(ErrorMessage = "Enter Subject")]
        public string CustSubject { get; set; }

        [Required(ErrorMessage = "Enter Your Enquiry")]
        public string CustEnquiry { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is not valid")]
        public string CustEmailId { get; set; }



        public int ProductId { get; set; }
        public string ProductImage { get; set; }
        public string ProductName { get; set; }
        public string ProductDesc { get; set; }

        public int CmsId { get; set; }
        public string CMSTitle { get; set; }
        public string Description { get; set; }


        /// <New Added>
        /// 
        public string GlobalDescription { get; set; }
        public string CorpDescription { get; set; }
        public string TargDescription { get; set; }
        /// </ended>
        public int FromUserId { get; set; }
        public int ToUserId { get; set; }

        [Required(ErrorMessage = "Businees Name cannot be null")]
        [StringLength(50)]
        public string BusinessName { get; set; }
        public string Subject { get; set; }
        public string BusinessDetails { get; set; }

        public string CompanyTitle { get; set; }
        public string CompanyBanner { get; set; }
        public string CompanyURL { get; set; }

        public int BussinessConnectionId { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }

        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsDesc { get; set; }
        public string NewsImage { get; set; }


        public string BannerName { get; set; }
        public string BannerTitle { get; set; }
        public string BannerDesc { get; set; }
        //public int PopupID { get; set; }
        //public string PopupImage { get; set; }
        //public Nullable<bool> IsActive { get; set; }


        //public int NewsId { get; set; }
        //public string NewsDescription { get; set; }

        //public int ReqId { set; get; }
        //public string ReqLogo { get; set; }
        //public string ReqUsername { get; set; }

        //public string CompanyDetails { get; set; }
        //public string Moreinfo { get; set; }

        [Required(ErrorMessage = "Enter Employee Name")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Enter Details")]
        public string EmpBrandMessage { get; set; }

        [Required(ErrorMessage = "Enter Details")]
        public string FranchiseDetails { get; set; }

        public string Fax { get; set; }
        public List<LandingPageModel> ConnectedBizList { get; set; }
        public List<BusinessJobModel> BusinessJobs { get; set; }
        public List<LandingPageModel> CMSList { get; set; }
        public List<LandingPageModel> ProductsList { get; set; }
        public List<LandingPageModel> PopupList { get; set; }
        public List<LandingPageModel> RequestedEnquiries { set; get; }
        public List<LandingPageModel> CompanyProfile { get; set; }
        public List<LandingPageModel> PendingRequests { get; set; }
        public List<LandingPageModel> CoBrandRequest { get; set; }
        public List<LandingPageModel> SuggestedConn { get; set; }
        public List<LandingPageModel> RecentJoinee { get; set; }
        public List<LandingPageModel> MostConnected { get; set; }
        public List<IndustryModel> IndustryList { get; set; }
        public List<CityModel> CityList { get; set; }


        public IEnumerable<SelectListItem> IndustryNames { get; set; }
        public IEnumerable<SelectListItem> CountryNames { get; set; }
        public IEnumerable<SelectListItem> CityNames { get; set; }

        public IEnumerable<string> SelectItems { set; get; }

        [MustBeSelected(ErrorMessage = "Please Select Country")]
        public Nullable<int> CountryId { get; set; }
        [MustBeSelected(ErrorMessage = "Please Select City")]
        public Nullable<int> CityId { get; set; }
        public Nullable<int> IndustryId { get; set; }


        // public List<LandingPageModel> RequestList { get; set; }
        // public List<LandingPageModel> CompanyDetails { get; set; }
        //public List<LandingPageModel> Info { get; set; }
        public List<LandingPageModel> PremiumBottom { get; set; }
        public List<LandingPageModel> PremiumLeft { get; set; }
        public List<LandingPageModel> PremiumRight { get; set; }
        public List<BusinessNewsModel> NewsUpdates { get; set; }
        public List<NewsModel> AdminHomeNews { get; set; }

        public List<HomeBannerModel> BannerList { get; set; }

        //Neww added
        public List<CoBrandModel> CoBrandList { get; set; }

        //New end added

        public class MustBeTrueAttribute : ValidationAttribute, IClientValidatable // IClientValidatable for client side Validation
        {
            public override bool IsValid(object value)
            {
                return value is bool && (bool)value;
            }
            // Implement IClientValidatable for client side Validation
            public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
            {
                return new ModelClientValidationRule[] { new ModelClientValidationRule { ValidationType = "checkbox", ErrorMessage = this.ErrorMessage } };
            }
        }

        public class MustBeSelected : ValidationAttribute, IClientValidatable // IClientValidatable for client side Validation
        {
            public override bool IsValid(object value)
            {
                if (value == null || (int)value == 0)
                    return false;
                else
                    return true;
            }
            // Implement IClientValidatable for client side Validation
            public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
            {
                return new ModelClientValidationRule[] { new ModelClientValidationRule { ValidationType = "dropdown", ErrorMessage = this.ErrorMessage } };
            }
        }

    }

}