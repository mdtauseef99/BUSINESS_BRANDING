using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBranding.Areas.Admin.Models;
using BizzBranding.CommonUtility;
using BizzBranding.Models;
using BizzBranding.BLL;
using BizzBranding.Areas.Admin.Filters;
using BizzBranding.DAL;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;
using System.IO;
using System.Web.Security;
using System.Text.RegularExpressions;
using System.Globalization;
using BizzBranding.Filters;
using System.Collections;
using System.Configuration;
using System.Web.Optimization;
using System.Web.Routing;
using System.Text;
using System.Web.UI;
using System.Threading.Tasks;
using System.Web.SessionState;



namespace BizzBranding.Controllers
{
    public class HomeController : Controller
    {

        ////public IFormsAuthenticationService FormsService { get; set; }
        BizzBrandingEntities objdb = new BizzBrandingEntities();
        LandingPageModel _HomeModel = new LandingPageModel();

        public static int iVal = 0;

        [CustumAuthorize.AllowAnonymous]
        public ActionResult FilteredBrandsbyIndustry(int id)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            _HomeModel.PremiumBottom = (from mem in objdb.Memberships
                                        where mem.AccessPlanId == 20 && mem.BussinessUser.IndustryId == id && mem.BussinessUser.IsActive == true && mem.ExpiresOn > DateTime.Now
                                        select new LandingPageModel
                                        {
                                            TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == mem.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == mem.UserId && ct.IsActive == true select ct.UserId).Count(),
                                            CreatedDate = mem.BussinessUser.CreatedDate,
                                            UserId = mem.BussinessUser.UserId,
                                            CompanyLogo = mem.BussinessUser.CompanyLogo,
                                            BusinessName = mem.BussinessUser.BusinessName,
                                            // PartnerId = mem.BusinessPartnerId
                                        }).OrderByDescending(x => x.TotalConnectCount).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();

            _HomeModel.MostConnected = (from bu in objdb.Memberships
                                        where bu.BussinessUser.IndustryId == id && bu.BussinessUser.IsActive == true
                                        select new LandingPageModel
                                        {
                                            TotalConnectCount = (from mo in objdb.BussinessUsers where mo.UserId == bu.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == bu.UserId && ct.IsActive == true select ct.UserId).Count(),
                                            CreatedDate = bu.CreatedDate,
                                            UserId = bu.BussinessUser.UserId,
                                            CompanyLogo = bu.BussinessUser.CompanyLogo,
                                            BusinessName = bu.BussinessUser.BusinessName,
                                            //PartnerId = bu.BusinessPartnerId
                                        }).OrderByDescending(x => x.TotalConnectCount).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();

            _HomeModel.RecentJoinee = (from bu in objdb.BussinessUsers
                                       where bu.IndustryId == id && bu.IsActive == true
                                       select new LandingPageModel
                                       {
                                           // BussinessConnectionId=bu.Id,
                                           TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == bu.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == bu.UserId && ct.IsActive == true select ct.UserId).Count(),
                                           CreatedDate = bu.CreatedDate,
                                           UserId = bu.UserId,
                                           CompanyLogo = bu.CompanyLogo,
                                           BusinessName = bu.BusinessName,
                                           //PartnerId = bu.BusinessPartnerId
                                       }).OrderByDescending(x => x.CreatedDate).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();

            _HomeModel.IndustryList = (from Ind in objdb.Industries
                                       where Ind.IsActive == true
                                       select new IndustryModel
                                       {
                                           IndustryName = Ind.IndustryName,
                                           IndustryId = Ind.IndustryId

                                       }).Take(7).ToList();
            _HomeModel.CityList = (from ct in objdb.Cities
                                   where ct.IsActive == true
                                   select new CityModel
                                   {
                                       CityName = ct.CityName,
                                       CityId = ct.CityId

                                   }).Take(7).ToList();

            var NewsList = (from UserNews in objdb.BusinessNews
                            where UserNews.IsActive == true && UserNews.CreatedBy == userid
                            select new BusinessNewsModel
                            {
                                BusinessNewsId = UserNews.BusinessNewsId,
                                NewsTitle = UserNews.NewsTitle,
                                Description = UserNews.Description

                            }).ToList();
            NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
            _HomeModel.NewsUpdates = NewsList;

            return View(_HomeModel);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult FooterLinks()
        {
            ViewBag.IndustryList = (from Ind in objdb.Industries
                                    where Ind.IsActive == true
                                    select new IndustryModel
                                    {
                                        IndustryName = Ind.IndustryName,
                                        IndustryId = Ind.IndustryId
                                    }).Take(5).ToList();
            ViewBag.CityList = (from ct in objdb.Cities
                                where ct.IsActive == true
                                select new CityModel
                                {
                                    CityName = ct.CityName,
                                    CityId = ct.CityId

                                }).Take(5).ToList();

            return PartialView("_FooterLayout");
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult FilteredByCity(int id)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            _HomeModel.PremiumBottom = (from mem in objdb.Memberships

                                        where mem.AccessPlanId == 20 && mem.BussinessUser.CityId == id && mem.BussinessUser.IsActive == true && mem.ExpiresOn > DateTime.Now
                                        select new LandingPageModel
                                        {
                                            TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == mem.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == mem.UserId && ct.IsActive == true select ct.UserId).Count(),
                                            CreatedDate = mem.BussinessUser.CreatedDate,
                                            UserId = mem.BussinessUser.UserId,
                                            CompanyLogo = mem.BussinessUser.CompanyLogo,
                                            BusinessName = mem.BussinessUser.BusinessName,
                                            // PartnerId = mem.BusinessPartnerId
                                        }).OrderByDescending(x => x.TotalConnectCount).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();
            _HomeModel.MostConnected = (from bu in objdb.Memberships
                                        where bu.BussinessUser.CityId == id && bu.BussinessUser.IsActive == true
                                        select new LandingPageModel
                                        {
                                            TotalConnectCount = (from mo in objdb.BussinessUsers where mo.UserId == bu.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == bu.UserId && ct.IsActive == true select ct.UserId).Count(),
                                            CreatedDate = bu.CreatedDate,
                                            UserId = bu.BussinessUser.UserId,
                                            CompanyLogo = bu.BussinessUser.CompanyLogo,
                                            BusinessName = bu.BussinessUser.BusinessName,
                                            //PartnerId = bu.BusinessPartnerId
                                        }).OrderByDescending(x => x.TotalConnectCount).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();

            _HomeModel.RecentJoinee = (from bu in objdb.BussinessUsers
                                       where bu.CityId == id && bu.IsActive == true
                                       select new LandingPageModel
                                       {
                                           // BussinessConnectionId=bu.Id,
                                           TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == bu.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == bu.UserId && ct.IsActive == true select ct.UserId).Count(),
                                           CreatedDate = bu.CreatedDate,
                                           UserId = bu.UserId,
                                           CompanyLogo = bu.CompanyLogo,
                                           BusinessName = bu.BusinessName,
                                           //PartnerId = bu.BusinessPartnerId
                                       }).OrderByDescending(x => x.CreatedDate).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();

            _HomeModel.IndustryList = (from Ind in objdb.Industries
                                       where Ind.IsActive == true
                                       select new IndustryModel
                                       {
                                           IndustryName = Ind.IndustryName,
                                           IndustryId = Ind.IndustryId

                                       }).Take(7).ToList();
            _HomeModel.CityList = (from ct in objdb.Cities
                                   where ct.IsActive == true
                                   select new CityModel
                                   {
                                       CityName = ct.CityName,
                                       CityId = ct.CityId

                                   }).Take(7).ToList();

            var NewsList = (from UserNews in objdb.BusinessNews
                            where UserNews.IsActive == true && UserNews.CreatedBy == userid
                            select new BusinessNewsModel
                            {
                                BusinessNewsId = UserNews.BusinessNewsId,
                                NewsTitle = UserNews.NewsTitle,
                                Description = UserNews.Description

                            }).ToList();
            NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
            _HomeModel.NewsUpdates = NewsList;

            return View(_HomeModel);
        }


        [CustumAuthorize.AllowAnonymous]
        public ActionResult Index()
        {
            //LandingPageModel model = new LandingPageModel();
            int userid = Convert.ToInt32(Session["UserId"]);
            ViewBag.Message = "Welcome to Business Branding";
            ViewBag.MetaKeywords = "Business,compare businesses,business directory,business services comparison,business ideas,b2b,";
            ViewBag.MetaDescription = " Business-to-business (B2B) describes commerce transactions between businesses. Business branding is one of the most comprehensive business directory that offers thousands of company business listings.";
            DateTime dt = DateTime.Now.AddDays(1).AddMinutes(-1);
            // int userid = Convert.ToInt32(Session["UserId"]);

            _HomeModel.PremiumBottom = (from mem in objdb.Memberships
                                        //where mem.AccessPlanId == 26 && mem.BussinessUser.IsActive == true && mem.ExpiresOn > DateTime.Now
                                        where mem.AccessPlanId == 26 && mem.BussinessUser.IsActive == true 
                                select new LandingPageModel
                                {
                                    // BussinessConnectionId=bu.Id,
                                    TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == mem.UserId && mo.IsActive == true select mo.UserId).Count()
                                    + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == mem.UserId && ct.IsActive == true select ct.UserId).Count(),
                                    CreatedDate = mem.BussinessUser.CreatedDate,
                                    UserId = mem.BussinessUser.UserId,
                                    CompanyLogo = mem.BussinessUser.CompanyLogo,
                                    BusinessName = mem.BussinessUser.BusinessName,
                                    // PartnerId = mem.BusinessPartnerId
                                }).OrderByDescending(x => x.TotalConnectCount).Take(12).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();

            _HomeModel.PremiumLeft = (from mem in objdb.Memberships

                                      //where mem.AccessPlanId == 20 && mem.BussinessUser.IsActive == true && mem.ExpiresOn > DateTime.Now
                                      where mem.AccessPlanId == 20 && mem.BussinessUser.IsActive == true
                                      select new LandingPageModel
                                      {
                                          // BussinessConnectionId=bu.Id,
                                          TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == mem.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == mem.UserId && ct.IsActive == true select ct.UserId).Count(),
                                          CreatedDate = mem.BussinessUser.CreatedDate,
                                          UserId = mem.BussinessUser.UserId,
                                          CompanyLogo = mem.BussinessUser.CompanyLogo,
                                          CompanyBanner = mem.BussinessUser.BannerImage,
                                          BusinessName = mem.BussinessUser.BusinessName,
                                          BusinessDetails = mem.BussinessUser.BusinessDetails
                                          // PartnerId = mem.BusinessPartnerId
                                      }).OrderByDescending(x => x.TotalConnectCount).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();

            //_HomeModel.PremiumRight = (from mem in objdb.Memberships

            //                           //where mem.AccessPlanId == 25 && mem.BussinessUser.IsActive == true && mem.ExpiresOn > DateTime.Now
            //                           where mem.AccessPlanId == 25 && mem.BussinessUser.IsActive == true 
            //                           select new LandingPageModel
            //                           {
            //                               // BussinessConnectionId=bu.Id,
            //                               TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == mem.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == mem.UserId && ct.IsActive == true select ct.UserId).Count(),
            //                               CreatedDate = mem.BussinessUser.CreatedDate,
            //                               UserId = mem.BussinessUser.UserId,
            //                               CompanyLogo = mem.BussinessUser.CompanyLogo,
            //                               CompanyBanner = mem.BussinessUser.BannerImage,
            //                               BusinessName = mem.BussinessUser.BusinessName,
            //                               BusinessDetails = mem.BussinessUser.BusinessDetails
            //                               // PartnerId = mem.BusinessPartnerId
            //                           }).OrderByDescending(x => x.TotalConnectCount).Take(1).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();


            _HomeModel.PremiumRight = (from mem in objdb.Memberships

                                       //where mem.AccessPlanId == 25 && mem.BussinessUser.IsActive == true && mem.ExpiresOn > DateTime.Now
                                       where mem.AccessPlanId == 25 && mem.BussinessUser.IsActive == true
                                       select new LandingPageModel
                                       {
                                           // BussinessConnectionId=bu.Id,
                                           //TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == mem.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == mem.UserId && ct.IsActive == true select ct.UserId).Count(),
                                           CreatedDate = mem.BussinessUser.CreatedDate,
                                           UserId = mem.BussinessUser.UserId,
                                           CompanyLogo = mem.BussinessUser.CompanyLogo,
                                           CompanyBanner = mem.BussinessUser.BannerImage,
                                           BusinessName = mem.BussinessUser.BusinessName,
                                           BusinessDetails = mem.BussinessUser.BusinessDetails,
                                           CompanyURL=mem.BussinessUser.URL,
                                           MembershipID=mem.MembershipID
                                           // PartnerId = mem.BusinessPartnerId
                                       }).OrderByDescending(x => x.MembershipID).Take(1).ToList();

            _HomeModel.RecentJoinee = (from bu in objdb.BussinessUsers

                                       where bu.IsActive == true
                                       select new LandingPageModel
                                       {
                                           // BussinessConnectionId=bu.Id,
                                           TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == bu.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == bu.UserId && ct.IsActive == true select ct.UserId).Count(),
                                           CreatedDate = bu.CreatedDate,
                                           UserId = bu.UserId,
                                           CompanyLogo = bu.CompanyLogo,
                                           BusinessName = bu.BusinessName,
                                           //PartnerId = bu.BusinessPartnerId
                                       }).OrderByDescending(x => x.CreatedDate).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList(); //.OrderByDescending(x => x.TotalConnectCount).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();
          
            _HomeModel.MostConnected = (from bu in objdb.Memberships
                                        //where bu.IsActive == true && bu.ExpiresOn > DateTime.Now
                                        where bu.IsActive == true 
                                        select new LandingPageModel
                                        {
                                            TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == bu.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == bu.UserId && ct.IsActive == true select ct.UserId).Count(),
                                            CreatedDate = bu.CreatedDate,
                                            UserId = bu.BussinessUser.UserId,
                                            CompanyLogo = bu.BussinessUser.CompanyLogo,
                                            BusinessName = bu.BussinessUser.BusinessName,
                                            // PartnerId = bu.BusinessPartnerId
                                        }).OrderByDescending(x => x.TotalConnectCount).Take(12).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();

            _HomeModel.IndustryList = (from Ind in objdb.Industries
                                       where Ind.IsActive == true
                                       select new IndustryModel
                                       {
                                           IndustryName = Ind.IndustryName,
                                           IndustryId = Ind.IndustryId

                                       }).Take(7).ToList();
            _HomeModel.CityList = (from ct in objdb.Cities
                                   where ct.IsActive == true
                                   select new CityModel
                                   {
                                       CityName = ct.CityName,
                                       CityId = ct.CityId

                                   }).Take(7).ToList();
            // Checking News Validity
            NewsMapping NewMap = new NewsMapping();
            NewMap = new MembershipBLL { }.GetHomeNewsListBLL();
            List<BusinessNewsModel> NewsList = new List<BusinessNewsModel>();
            List<NewsModel> AdminNewsList = new List<NewsModel>();



            AdminNewsList = (from ns in objdb.News
                             where ns.IsActive == true
                             select new NewsModel
                             {
                                 NewsId = ns.NewsId,
                                 NewsTitle = ns.NewsTitle,
                                 NewsDesc = ns.Description,
                                 NewsImage = ns.NewsImage,
                                 CreateOn = ns.CreatedOn
                             }).OrderByDescending(x => x.NewsId).ToList();
            AdminNewsList.ForEach(s => s.NewsDesc = Regex.Replace(s.NewsDesc, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));


            _HomeModel.AdminHomeNews = AdminNewsList;

            // Removed Part//

            HomeBannerMapping HBM = new HomeBannerMapping();
            HBM = new MembershipBLL { }.GetHomeBannerValidityBLL();

            //if (HBM != null)
            //{
            //    List<HomeBanner> objhome = new List<HomeBanner>();
            //    objhome = new HomeBannerBLL { }.GetBannerBLL(Convert.ToInt32(HBM.UserId));
            //    if (objhome != null)
            //    {

            //    }
            //    ViewBag.Banner = objhome;
            //}
            //ViewBag.IsHome = "Yes";


            List<HomeBanner> objhome = new List<HomeBanner>();
            objhome = new HomeBannerBLL { }.GetBannerBLL();
            if (objhome != null)
            {

            }
            ViewBag.Banner = objhome;

            ViewBag.IsHome = "Yes";

            _HomeModel.BannerList = (from Ban in objdb.HomeBanners
                                     where Ban.IsActive == true && Ban.BussinessUser.IsActive==true
                                     select new HomeBannerModel
                                     {
                                         HomeBannerID=Ban.HomeBannerId,
                                         BannerImage = Ban.BannerImage,
                                         CompanyURL = Ban.URL,
                                         BusinessDetails=Ban.BussinessUser.BusinessDetails,
                                         BusinessName=Ban.BussinessUser.BusinessName

                                         //BannerDesc=Ban.ba 
                                     }).OrderByDescending(x => x.HomeBannerID).Take(4).ToList();
        // New code  added//
            var objmodelCoBrand = new CMSBLL { }.GetCmsById(16);
            if (objmodelCoBrand != null)
            {
                _HomeModel.CMSTitle = objmodelCoBrand.CMSTitle;
                //_HomeModel.Description = objmodel.Description;
                _HomeModel.Description = System.Text.RegularExpressions.Regex.Replace(objmodelCoBrand.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty);
            }


            var modelGlobalBrand = new CMSBLL { }.GetCmsById(11);
            if (modelGlobalBrand != null)
            {
                //model.CMSTitle = modelGlobalBrand.CMSTitle;
                _HomeModel.GlobalDescription = System.Text.RegularExpressions.Regex.Replace(modelGlobalBrand.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty);
            }

            var CorpBrand = new CMSBLL { }.GetCmsById(10);
            if (CorpBrand != null)
            {
                _HomeModel.CorpDescription = System.Text.RegularExpressions.Regex.Replace(CorpBrand.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty);
            }

            var TargBrand = new CMSBLL { }.GetCmsById(3);
            if (TargBrand != null)
            {
                _HomeModel.TargDescription = System.Text.RegularExpressions.Regex.Replace(TargBrand.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty);
            }

        //new code ending


            return View(_HomeModel);
        }


        [CustumAuthorize.AllowAnonymous]
        public ActionResult ViewAllBrands(int iVal = 0, int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            _HomeModel.PageID = pid;
            _HomeModel.Current = pid + 1;
            if (iVal == 20)
            {
                _HomeModel.PremiumBottom = (from mem in objdb.Memberships

                                            where mem.AccessPlanId == 20 && mem.BussinessUser.IsActive == true && mem.ExpiresOn > DateTime.Now
                                            select new LandingPageModel
                                            {
                                                // BussinessConnectionId=bu.Id,
                                                TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == mem.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == mem.UserId && ct.IsActive == true select ct.UserId).Count(),
                                                CreatedDate = mem.BussinessUser.CreatedDate,
                                                UserId = mem.BussinessUser.UserId,
                                                CompanyLogo = mem.BussinessUser.CompanyLogo,
                                                BusinessName = mem.BussinessUser.BusinessName,
                                                BusinessDetails = mem.BussinessUser.BusinessDetails
                                                // PartnerId = mem.BusinessPartnerId
                                            }).OrderByDescending(x => x.TotalConnectCount).ToList().GroupBy(p => p.UserId).Select(f => f.First()).Skip(skip).Take(take).ToList();
                ViewBag.SearchKey = "Corporate Brand";
            }
            else if (iVal == 19)
            {
                _HomeModel.MostConnected = (from bu in objdb.Memberships
                                            where bu.IsActive == true && bu.ExpiresOn > DateTime.Now
                                            select new LandingPageModel
                                            {
                                                TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == bu.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == bu.UserId && ct.IsActive == true select ct.UserId).Count(),
                                                CreatedDate = bu.CreatedDate,
                                                UserId = bu.BussinessUser.UserId,
                                                CompanyLogo = bu.BussinessUser.CompanyLogo,
                                                BusinessName = bu.BussinessUser.BusinessName,
                                                BusinessDetails = bu.BussinessUser.BusinessDetails
                                                // PartnerId = bu.BusinessPartnerId
                                            }).OrderByDescending(x => x.TotalConnectCount).ToList().GroupBy(p => p.UserId).Select(f => f.First()).Skip(skip).Take(take).ToList();
                ViewBag.SearchKey = "Most Connected";
            }
            else
            {
                _HomeModel.RecentJoinee = (from bu in objdb.BussinessUsers

                                           where bu.IsActive == true
                                           select new LandingPageModel
                                           {
                                               // BussinessConnectionId=bu.Id,
                                               TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == bu.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == bu.UserId && ct.IsActive == true select ct.UserId).Count(),
                                               CreatedDate = bu.CreatedDate,
                                               UserId = bu.UserId,
                                               CompanyLogo = bu.CompanyLogo,
                                               BusinessName = bu.BusinessName,
                                               BusinessDetails = bu.BusinessDetails
                                               //PartnerId = bu.BusinessPartnerId
                                           }).OrderByDescending(x => x.CreatedDate).ToList().GroupBy(p => p.UserId).Select(f => f.First()).Skip(skip).Take(take).ToList(); //.OrderByDescending(x => x.TotalConnectCount).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();
                ViewBag.SearchKey = "Recent Joinee";
            }

            _HomeModel.IndustryList = (from Ind in objdb.Industries
                                       where Ind.IsActive == true
                                       select new IndustryModel
                                       {
                                           IndustryName = Ind.IndustryName,
                                           IndustryId = Ind.IndustryId,
                                          

                                       }).Take(7).ToList();
            _HomeModel.CityList = (from ct in objdb.Cities
                                   where ct.IsActive == true
                                   select new CityModel
                                   {
                                       CityName = ct.CityName,
                                       CityId = ct.CityId

                                   }).Take(7).ToList();


            //        throw;
            return View(_HomeModel);
        }
        //  return View();
        ////}

        [CustumAuthorize.AllowAnonymous]
        public ActionResult About(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            ViewBag.Message = "Business Branding-AboutUs";

            ViewBag.MetaKeywords = "b2b marketing,b2b websites,b2b portal,b2b business directory";
            ViewBag.MetaDescription = "Business branding is known for the in depth knowledge of how to make your business more profitable. We help you to make more benefit from your business.";
            //SearchwordsModel searchObj = new SearcbyKeywordBLL { }.Search(searchValue);
            var objmodel = new CMSBLL { }.GetCmsById(id);
            if (objmodel != null)
            {
                _HomeModel.IndustryList = (from Ind in objdb.Industries
                                           where Ind.IsActive == true
                                           select new IndustryModel
                                           {
                                               IndustryName = Ind.IndustryName,
                                               IndustryId = Ind.IndustryId

                                           }).Take(7).ToList();
                _HomeModel.CityList = (from ct in objdb.Cities
                                       where ct.IsActive == true
                                       select new CityModel
                                       {
                                           CityName = ct.CityName,
                                           CityId = ct.CityId

                                       }).Take(7).ToList();

                _HomeModel.CMSTitle = objmodel.CMSTitle;
                _HomeModel.Description = objmodel.Description;

                //UserProfile News
                //var NewsList = (from UserNews in objdb.BusinessNews
                //                where UserNews.IsActive == true && UserNews.CreatedBy == userid
                //                select new BusinessNewsModel
                //                {
                //                    BusinessNewsId = UserNews.BusinessNewsId,
                //                    NewsTitle = UserNews.NewsTitle,
                //                    Description = UserNews.Description

                //                }).ToList();
                //NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                //_HomeModel.NewsUpdates = NewsList;

                // Admin News
                NewsMapping NewMap = new NewsMapping();
                NewMap = new MembershipBLL { }.GetHomeNewsListBLL();
                List<BusinessNewsModel> NewsList = new List<BusinessNewsModel>();
                List<BusinessNewsModel> AdminNewsList = new List<BusinessNewsModel>();

                if (NewMap != null)
                {
                    NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == NewMap.UserId
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).OrderByDescending(x => x.BusinessNewsId).Take(2).ToList();
                    NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));


                }
                AdminNewsList = (from ns in objdb.News
                                 where ns.IsActive == true
                                 select new BusinessNewsModel
                                 {
                                     BusinessNewsId = ns.NewsId,
                                     NewsTitle = ns.NewsTitle,
                                     Description = ns.Description

                                 }).OrderByDescending(x => x.BusinessNewsId).Take(5).ToList();
                AdminNewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));

                foreach (var item in NewsList)
                {
                    AdminNewsList.Add(item);
                }
                _HomeModel.NewsUpdates = AdminNewsList;
            }

            return View(_HomeModel);

        }

        public RedirectResult Link()
        {
            return Redirect("www.google.com");
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult Contact(int id = 0)
        {
            CustomerEnquiriesModel objmodel = new CustomerEnquiriesModel();
            if (iVal == 1)
            {
                ViewBag.Msg = "Your Enquiry Details Sent Successfully to Administrator";
                iVal = 0;
            }
            int userid = Convert.ToInt32(Session["UserId"]);
            if (userid > 0)
            {
                var obj = new BussinessUserBLL { }.GetUserById(userid);
                ViewBag.LoggedInUser = true;
                objmodel.CustEmailId = obj.EmailId;
                objmodel.CustomerName = obj.BusinessName;
                objmodel.CustomerPhone = obj.Phone;
            }
            ViewBag.Message = "Business Branding-Contact Us";

            ViewBag.MetaKeywords = "b2b market place,b2b business directory";
            ViewBag.MetaDescription = "Feel free to contact Business Branding. We are always ready to help you with best business solutions.";


            return View(objmodel);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult TargetBrandingCMS(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            ViewBag.Message = "Business Branding-Target Branding";

            ViewBag.MetaKeywords = "b2b marketing,b2b websites,b2b portal,b2b business directory,b2b websites,b2b portal,b2b business directory,business networking,qualified business referrals,business marketing,marketing resources";
            ViewBag.MetaDescription = "Global Branding: Business Branding features one of the best business to business (B2B) services. We focus on providing best business to business (B2B) services for your business.";
            var objmodel = new CMSBLL { }.GetCmsById(id);
            if (objmodel != null)
            {
                _HomeModel.CMSTitle = objmodel.CMSTitle;
                _HomeModel.Description = objmodel.Description;
            }
            return View(_HomeModel);
        }

        [CustumAuthorize.AllowAnonymous]
        [HttpPost]
        public ActionResult Contact(CustomerEnquiriesModel model)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            try
            {
                if (Session["UserId"]!=null)
                {
                    int res1 = 0;
                    res1 = new CustomerEnquiryBLL { }.AddEditCustomerEnquiry(new CustomerEnquiriesModel
                    {
                        LoggedInUserId = userid,
                        CustSubject = model.CustSubject,
                        CustEnquiry = model.CustEnquiry,
                        CreatedDate = DateTime.Now.Date,
                    });
                    if (res1 != 0)
                    {
                        iVal = 1;
                        //Session["Success"] = "Successfully Updated The User";
                        return RedirectToAction("Contact", "Home");
                    }

                }
                else
                {
                    int res = 0;
                    res = new CustomerEnquiryBLL { }.AddEditCustomerEnquiry(new CustomerEnquiriesModel
                    {
                        CustomerName = model.CustomerName,
                        LoggedInUserId = userid,
                        CustomerPhone = model.CustomerPhone,
                        CustSubject = model.CustSubject,
                        CustEmailId = model.CustEmailId,
                        CustEnquiry = model.CustEnquiry,
                        CreatedDate = DateTime.Now.Date,
                    });
                    if (res != 0)
                    {
                        iVal = 1;

                        //Session["Success"] = "Successfully Updated The User";
                        return RedirectToAction("Contact", "Home");
                    }
                }
                
            }

            catch (Exception e)
            {
                throw e;
            }
            return RedirectToAction("UsersProfile", "Home");
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult Services(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            ViewBag.Message = "Business Branding-Services";

            ViewBag.MetaKeywords = "small business ideas,business to business marketing,business development,b to b resources,b to b association";
            ViewBag.MetaDescription = " Services: Business Branding features one of the best business to business (B2B) services. We focus on providing best business to business (B2B) services for your business.";
            //SearchwordsModel searchObj = new SearcbyKeywordBLL { }.Search(searchValue);
            var objmodel = new CMSBLL { }.GetCmsById(id);
            if (objmodel != null)
            {
                _HomeModel.IndustryList = (from Ind in objdb.Industries
                                           where Ind.IsActive == true
                                           select new IndustryModel
                                           {
                                               IndustryName = Ind.IndustryName,
                                               IndustryId = Ind.IndustryId

                                           }).Take(7).ToList();
                _HomeModel.CityList = (from ct in objdb.Cities
                                       where ct.IsActive == true
                                       select new CityModel
                                       {
                                           CityName = ct.CityName,
                                           CityId = ct.CityId
                                       }).Take(7).ToList();
                _HomeModel.CMSTitle = objmodel.CMSTitle;
                _HomeModel.Description = objmodel.Description;

                var NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == userid
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                _HomeModel.NewsUpdates = NewsList;
            }

            return View(_HomeModel);
        }
        
        [CustumAuthorize.AllowAnonymous]
        public ActionResult OurWork(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            ViewBag.Message = "Business Branding-Our Work";

            ViewBag.MetaKeywords = "business directory india,b2b portal,business networking group";
            ViewBag.MetaDescription = "Business Branding features one of the best business to business (B2B) services. We focus on providing best business to business services for your business.";
            //SearchwordsModel searchObj = new SearcbyKeywordBLL { }.Search(searchValue);
            var objmodel = new CMSBLL { }.GetCmsById(id);
            if (objmodel != null)
            {
                _HomeModel.IndustryList = (from Ind in objdb.Industries
                                           where Ind.IsActive == true
                                           select new IndustryModel
                                           {
                                               IndustryName = Ind.IndustryName,
                                               IndustryId = Ind.IndustryId

                                           }).Take(7).ToList();
                _HomeModel.CityList = (from ct in objdb.Cities
                                       where ct.IsActive == true
                                       select new CityModel
                                       {
                                           CityName = ct.CityName,
                                           CityId = ct.CityId

                                       }).Take(7).ToList();
                _HomeModel.CMSTitle = objmodel.CMSTitle;
                _HomeModel.Description = objmodel.Description;

                var NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == userid
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                _HomeModel.NewsUpdates = NewsList;
            }

            return View(_HomeModel);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult GlobalBranding(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            ViewBag.Message = "Business Branding-Global Branding";

            ViewBag.MetaKeywords = "b2b marketing,b2b websites,b2b portal,b2b business directory,b2b websites,b2b portal,b2b business directory,business networking,qualified business referrals,business marketing,marketing resources";
            ViewBag.MetaDescription = "Global Branding: Business Branding features one of the best business to business (B2B) services. We focus on providing best business to business (B2B) services for your business.";
            var objmodel = new CMSBLL { }.GetCmsById(id);
            if (objmodel != null)
            {
                _HomeModel.IndustryList = (from Ind in objdb.Industries
                                           where Ind.IsActive == true
                                           select new IndustryModel
                                           {
                                               IndustryName = Ind.IndustryName,
                                               IndustryId = Ind.IndustryId

                                           }).Take(7).ToList();
                _HomeModel.CityList = (from ct in objdb.Cities
                                       where ct.IsActive == true
                                       select new CityModel
                                       {
                                           CityName = ct.CityName,
                                           CityId = ct.CityId

                                       }).Take(7).ToList();
                _HomeModel.CMSTitle = objmodel.CMSTitle;
                _HomeModel.Description = objmodel.Description;

                //User Profile News
                //var NewsList = (from UserNews in objdb.BusinessNews
                //                where UserNews.IsActive == true && UserNews.CreatedBy == userid
                //                select new BusinessNewsModel
                //                {
                //                    BusinessNewsId = UserNews.BusinessNewsId,
                //                    NewsTitle = UserNews.NewsTitle,
                //                    Description = UserNews.Description

                //                }).ToList();
                //NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                //_HomeModel.NewsUpdates = NewsList;

                NewsMapping NewMap = new NewsMapping();
                NewMap = new MembershipBLL { }.GetHomeNewsListBLL();
                List<BusinessNewsModel> NewsList = new List<BusinessNewsModel>();
                List<BusinessNewsModel> AdminNewsList = new List<BusinessNewsModel>();

                if (NewMap != null)
                {
                    NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == NewMap.UserId
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).OrderByDescending(x => x.BusinessNewsId).Take(2).ToList();
                    NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));


                }
                AdminNewsList = (from ns in objdb.News
                                 where ns.IsActive == true
                                 select new BusinessNewsModel
                                 {
                                     BusinessNewsId = ns.NewsId,
                                     NewsTitle = ns.NewsTitle,
                                     Description = ns.Description

                                 }).OrderByDescending(x => x.BusinessNewsId).Take(5).ToList();
                AdminNewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));

                foreach (var item in NewsList)
                {
                    AdminNewsList.Add(item);
                }
                _HomeModel.NewsUpdates = AdminNewsList;
            }
            return View(_HomeModel);

        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult CMS(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            ViewBag.Message = "Business Branding-Global Branding";

            ViewBag.MetaKeywords = "b2b marketing,b2b websites,b2b portal,b2b business directory,b2b websites,b2b portal,b2b business directory,business networking,qualified business referrals,business marketing,marketing resources";
            ViewBag.MetaDescription = "Global Branding: Business Branding features one of the best business to business (B2B) services. We focus on providing best business to business (B2B) services for your business.";
            var objmodel = new CMSBLL { }.GetCmsById(id);
            if (objmodel != null)
            {

                _HomeModel.CMSTitle = objmodel.CMSTitle;
                _HomeModel.Description = objmodel.Description;

                // Checking News Validity
                NewsMapping NewMap = new NewsMapping();
                NewMap = new MembershipBLL { }.GetHomeNewsListBLL();
                List<BusinessNewsModel> NewsList = new List<BusinessNewsModel>();
                List<BusinessNewsModel> AdminNewsList = new List<BusinessNewsModel>();

                if (NewMap != null)
                {
                    NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == NewMap.UserId
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).OrderByDescending(x => x.BusinessNewsId).Take(2).ToList();
                    NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));


                }
                AdminNewsList = (from ns in objdb.News
                                 where ns.IsActive == true
                                 select new BusinessNewsModel
                                 {
                                     BusinessNewsId = ns.NewsId,
                                     NewsTitle = ns.NewsTitle,
                                     Description = ns.Description

                                 }).OrderByDescending(x => x.BusinessNewsId).Take(5).ToList();
                AdminNewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));

                foreach (var item in NewsList)
                {
                    AdminNewsList.Add(item);
                }
                _HomeModel.NewsUpdates = AdminNewsList;
            }
            return View(_HomeModel);
        }

        [CustumAuthorize.AllowAnonymous]
        public JsonResult GetCity(int id)
        {
            try
            {
                BussinessUserModel usermodelObj = new BussinessUserModel();
                var cityList = new CityBLL { }.GetAllCity().Where(x => x.CountryId == id).Select(x => new CityModel
                {
                    CityId = x.CityId,
                    CityName = x.CityName,
                }).ToList();
                return Json(cityList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [CustumAuthorize.AllowAnonymous]
        [HttpGet]
        public ActionResult UserRegistration(int id = 0)
        {
            if (Convert.ToBoolean(TempData["Error"]) == true)
            {
                TempData["AlertMessage"] = "User Error";
            }
            //BizUserHomeModel UModel = new BizUserHomeModel();
            UserWebModel model = new UserWebModel();
            if (id != 0)
            {
                var objmodel = new BussinessUserBLL { }.GetUserById(id);
                if (objmodel != null)
                {
                    var obj = new CMSBLL { }.GetCmsById(22);
                    model.TermsConditions = obj.Description;
                    model.UserId = objmodel.UserId;
                    model.BusinessName = objmodel.BusinessName;
                    model.EmailId = objmodel.EmailId;
                    model.Password = objmodel.Password;
                    model.ConfirmPassword = objmodel.ConfirmPassword;
                    model.IsActive = Convert.ToBoolean(objmodel.IsActive);
                    model.ConfirmPassword = objmodel.ConfirmPassword;
                    model.CompanyLogo = objmodel.CompanyLogo;
                    model.CoRegNo = objmodel.CoRegNo;
                    model.CoAddress = objmodel.CoAddress;
                    model.Phone = objmodel.Phone;
                    model.Fax = objmodel.Fax;
                    model.ContactPerson = objmodel.ContactPerson;
                    model.Designation = objmodel.Designation;
                    model.TradeEmaiIId = objmodel.TradeEmaiIId;
                    model.BusinessDetails = objmodel.BusinessDetails;
                    model.IndustryId = objmodel.IndustryId;
                    model.CountryId = objmodel.CountryId;
                    model.CityId = objmodel.CityId;
                    model.IsGlobal = Convert.ToBoolean(objmodel.IsGlobal);
                    
                }
            }
            if (iVal == 1)
            {
                //ViewBag.Msg = "Email Already exists";
                iVal = 0;
            }
            CustomMethods.BindIndustry(model);
            CustomMethods.BindCountryWithCity(model);
            CustomMethods.BindCityForUser(model);
            //var list = new List<SelectListItem>();
            //foreach (Industry sub in objdb.Industries)
            //{
            //    SelectListItem select = new SelectListItem()
            //    {
            //        Text = sub.IndustryName,
            //        Value = sub.IndustryId.ToString(),
            //        Selected = sub.IsActive
            //    };
            //    list.Add(select);
            //}
            //UserIndustryMappingModel view = new UserIndustryMappingModel();
            //model.Industrylist = list;

            return View(model);
        }

        string guid = Guid.NewGuid().ToString();
        string uPwd = string.Empty;
        MailMessage msg;
        string ActivationUrl = string.Empty;
        string emailId = string.Empty;

        [HttpPost]

        [CustumAuthorize.AllowAnonymous]
        public ActionResult UserRegistration(UserWebModel model, HttpPostedFileBase file, IEnumerable<string> SelectItems)
        {
            Guid temp = Guid.Parse(guid);
            //string confirmationGuid = user.ProviderUserKey.ToString();
            var checkduplicate = objdb.BussinessUsers.Where(x => x.UserId == model.UserId || x.EmailId == model.EmailId).Select(x =>
                 new BussinessUserModel { UserId = x.UserId, EmailId = x.EmailId }).SingleOrDefault();
            if (checkduplicate == null)
            {
                try
                {
                    int res = 0;
                    string strFileName = "";
                    string path = "";
                    Random rnd = new Random();
                    //if (ModelState.IsValid)
                    //{
                    if (file != null)
                    {
                        strFileName = "CoLogo_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                        path = Server.MapPath("~/Content/Images/CoLogo/" + strFileName);


                        res = new BussinessUserBLL { }.AddEditUser(new BussinessUserModel
                        {
                            BusinessName = model.BusinessName,
                            EmailId = model.EmailId,
                            Password = DataEncryption.Encrypt(model.Password.Trim(), "passKey"),
                            ConfirmPassword = model.ConfirmPassword,
                            CompanyLogo = strFileName,
                            CoRegNo = model.CoRegNo,
                            CoAddress = model.CoAddress,
                            Phone = model.Phone,
                            Fax = model.Fax,
                            ContactPerson = model.ContactPerson,
                            Designation = model.Designation,
                            TradeEmaiIId = model.TradeEmaiIId,
                            BusinessDetails = model.BusinessDetails,
                            IndustryId = model.IndustryId,
                            CountryId = model.CountryId,
                            CityId = model.CityId,
                            IsActive = true,
                            CompURL = model.CompanyURL,
                            CreatedDate = DateTime.Now,
                            GUId = guid,

                            IsGlobal = model.IsGlobal
                        });
                        file.SaveAs(path);
                        int iRes = 0;
                        if (SelectItems != null)
                        {
                            foreach (var item in SelectItems)
                            {
                                UserIndustryMappingModel mod = new UserIndustryMappingModel();
                                mod.UserId = res;
                                mod.IndustryId = Convert.ToInt32(item);
                                iRes = new BussinessUserBLL { }.AddUserIndutryMapping(mod);
                            }
                        }

                        var getuser = new BussinessUserBLL { }.GetUserById(res);
                        
                        #region Send Email
                        msg = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        emailId = model.EmailId;
                        //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                        //uPwd = DataEncryption.Decrypt(password, "passKey");
                        uPwd = model.Password.Trim();
                        //sender email address
                        msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                        //Receiver email address
                        msg.To.Add(emailId);
                        msg.Subject = "Congratulations You have Successfully Registered";
                        string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                        ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                        //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                        // ActivationUrl = Server.HtmlEncode(link);
                        //msg.Body = "Hi " + model.BusinessName + "!\n" +
                        // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                        // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                        var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(1);
                        var test = emailtemp.Description + "<br />" + ActivationUrl;
                        string mailbod = MailBody(test, getuser);
                        msg.Body = mailbod;
                        msg.IsBodyHtml = true;
                        smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                        smtp.Port = 25;
                        smtp.Host = "mail.FRMT.IN";
                        //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                        //smtp.Port = 2525;
                        //smtp.Host = "retail.smtp.com";
                        smtp.EnableSsl = false;
                        smtp.Send(msg);
                        #endregion

                        if (res != 0)
                        {
                            iVal = 1;
                            TempData["AlertMessage"] = "Successfully Registered Check Email to Activate your Account";
                            // ViewBag.Msg = "Successfully Registered Check your Email to Activate your account";
                            // Session["Success"] = "Successfully Registered Check your Email to Activate your account";
                            //ViewBag.Msg = "Successfully Registered";
                            return RedirectToAction("Login", "Home");
                        }
                    }
                    else
                    {
                        res = new BussinessUserBLL { }.AddEditUser(new BussinessUserModel
                        {
                            BusinessName = model.BusinessName,
                            EmailId = model.EmailId,
                            Password = DataEncryption.Encrypt(model.Password.Trim(), "passKey"),
                            ConfirmPassword = model.ConfirmPassword,
                            CompanyLogo = strFileName,
                            CoRegNo = model.CoRegNo,
                            CoAddress = model.CoAddress,
                            Phone = model.Phone,
                            Fax = model.Fax,
                            ContactPerson = model.ContactPerson,
                            Designation = model.Designation,
                            TradeEmaiIId = model.TradeEmaiIId,
                            BusinessDetails = model.BusinessDetails,
                            IndustryId = model.IndustryId,
                            CountryId = model.CountryId,
                            CityId = model.CityId,
                            CompURL = model.CompanyURL,
                            CreatedDate = DateTime.Now,
                            IsActive = true,
                        });
                        file.SaveAs(path);
                        int iRes = 0;
                        if (SelectItems != null)
                        {
                            foreach (var item in SelectItems)
                            {
                                UserIndustryMappingModel mod = new UserIndustryMappingModel();
                                mod.UserId = res;
                                mod.IndustryId = Convert.ToInt32(item);
                                iRes = new BussinessUserBLL { }.AddUserIndutryMapping(mod);
                            }
                        }

                        var getuser = new BussinessUserBLL { }.GetUserById(res);

                        #region Send Email
                        msg = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        emailId = model.EmailId;
                        //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                        //uPwd = DataEncryption.Decrypt(password, "passKey");
                        uPwd = model.Password.Trim();
                        //sender email address
                        msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                        //Receiver email address
                        msg.To.Add(emailId);
                        msg.Subject = "Congratulations You have Successfully Registered";
                        string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                        ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                        //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                        // ActivationUrl = Server.HtmlEncode(link);
                        //msg.Body = "Hi " + model.BusinessName + "!\n" +
                        // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                        // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                        var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(1);
                        var test = emailtemp.Description + "<br />" + ActivationUrl;
                        string mailbod = MailBody(test, getuser);
                        msg.Body = mailbod;
                        msg.IsBodyHtml = true;
                        //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                        //smtp.Port = 2525;
                        //smtp.Host = "retail.smtp.com";
                        smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                        smtp.Port = 25;
                        smtp.Host = "mail.FRMT.IN";

                        smtp.EnableSsl = false;
                        smtp.Send(msg);
                        #endregion

                        if (res != 0)
                        {
                            iVal = 1;
                            TempData["AlertMessage"] = "Successfully Registered Check Email to Activate your Account";
                            // ViewBag.Msg = "Successfully Registered Check your Email to Activate your account";
                            // Session["Success"] = "Successfully Added The User";
                            return RedirectToAction("Login", "Home");
                        }
                    }                   
                    //}
                    //Session["Error"] = "Please try again after some time.";
                    //TempData["AlertMessage"] = "Invalid EmailId or Password";
                    //CustomMethods.BindIndustry(model);
                    //CustomMethods.BindCountryWithCity(model);
                    //CustomMethods.BindCityForUser(model);
                    //return RedirectToAction("UserRegistration", "Home");
                }
                catch (Exception e)
                {
                    TempData["AlertMessage"] = e.ToString();
                    //CustomMethods.BindIndustry(model);
                    //CustomMethods.BindCountryWithCity(model);
                    //CustomMethods.BindCityForUser(model);
                    return RedirectToAction("UserRegistration", "Home");
                    
                }
            }
            //else
            //{
            //    iVal = 1;
            //    TempData["AlertMessage"] = "User Already Exists";
            //    // Session["error"] = "Email already exists";
            //    return RedirectToAction("UserRegistration");
            //}
            return View(model);
        }

        public string MailBody(string bodywithkey, object obj)
        {
            try
            {
                var mailbod = bodywithkey;
                // var userobj = getpass;
                var keys = new List<string>();
                string[] k = {"Username", "EmailId", "Password", "Birthdate", "BusinessName", "LastName" ,"FromBusinessName"};
                
                keys.AddRange(k);
                foreach (var s in keys)
                {
                    if (mailbod.Contains(s))
                    {
                        //availkeys.Add(s);
                        var val = obj.GetType().GetProperty(s).GetValue(obj, null);
                        if (val == null)
                        {
                            val = "Sir";
                        }
                        mailbod = mailbod.Replace("[" + s + "]", val.ToString());
                    }
                }
                return mailbod;
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        public string MailBody(string bodywithkey, object obj,object obj1)
        {
            try
            {
                var mailbod = bodywithkey;
                // var userobj = getpass;
                var keys = new List<string>();
                string[] k = { "Username", "EmailId", "Password", "Birthdate", "BusinessName", "LastName" };
                string[] y = { "EmailId", "Password", "Birthdate", "FromBusinessName", "LastName"};
                keys.AddRange(k);
                keys.AddRange(y);
                foreach (var s in keys)
                {
                    if (mailbod.Contains(s))
                    {
                        //availkeys.Add(s);
                        var val = obj.GetType().GetProperty(s).GetValue(obj, null);
                        var val2 = obj1.GetType().GetProperty("FromBusinessName").GetValue(obj1, null);
                        if (val == null && val2==null)
                        {
                            val = "Sir";
                        }
                        mailbod = mailbod.Replace("[" + s + "]", val.ToString());
                        mailbod = mailbod.Replace("[FromBusinessName]", val2.ToString());
                    }
                }
                return mailbod;
            }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        [CustumAuthorize.AllowAnonymous]
        public JsonResult CheckDuplicate(string EmailId)
        {
            bool result = false;

            try
            {
                result = new BussinessUserBLL { }.CheckDuplicate(EmailId.Trim());

                if (result == true)
                {
                    return Json(result);
                }
                else
                {
                    return Json(result);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string Decrypt(string str)
        {
            str = str.Replace(" ", "+");
            string DecryptKey = "2013;[pnuLIT)WebCodeExpert";
            byte[] byKey = { };
            byte[] IV = { 18, 52, 86, 120, 144, 171, 205, 239 };
            byte[] inputByteArray = new byte[str.Length];
            // ICryptoTransform transform = new TripleDESCryptoServiceProvider().CreateDecryptor(bytKey, iv);
            byKey = System.Text.Encoding.UTF8.GetBytes(DecryptKey.Substring(0, 8));
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            inputByteArray = Convert.FromBase64String(str.Replace(" ", "+"));
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(byKey, IV), CryptoStreamMode.Write);
            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();
            System.Text.Encoding encoding = System.Text.Encoding.UTF8;
            return encoding.GetString(ms.ToArray());
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult News()
        {
            try
            {
                List<NewsModel> news = new List<NewsModel>();
                int skip = 0;
                int take = 5;
                news = new NewsBLL { }.GetAllNews(skip, take);
                news.ForEach(x => x.NewsDesc = Regex.Replace(x.NewsDesc, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));
                if (news != null)
                {
                    return PartialView("_NewsSection", news);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return View();
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult Login(string returnurl, string msg = null)
        {
            //if (iVal == 1)
            //{
            //    ViewBag.Msg = "Successfully Registered Check your Email to Activate your account";
            //    iVal = 0;
            //}
            try
            {
                BizUserLoginModel model = new BizUserLoginModel();
                model.ReturnUrl = returnurl;
                ViewBag.ReturnUrl = returnurl;
                model.Msg = msg;
                return View(model);
            }
            catch (Exception)
            {
                TempData["AlertMessage"] = "Invalid EmailId or Password";
                ViewBag.msg = "Invalid User";
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [CustumAuthorize.AllowAnonymous]
        public ActionResult Login(BizUserLoginModel model, string returnurl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var Recordexist = new BussinessUserBLL { }.IsEmailExists(model.Email);
                    if (Recordexist != null)
                    {
                        TempData["AlertMessage"] = "First Check Email to activate your Account";
                        return View();
                        //ViewBag.Msg = "Check out your mail to activate your account";
                        //iVal = 0;
                    }
                    //else
                    //{
                    //    ViewBag.Msg = "Incorrect Password";
                    //    return RedirectToAction("Login", "Home");
                    //}
                    var user = new BussinessUserBLL { }.UserLogin(model.Email, DataEncryption.Encrypt(model.Password.Trim(), "passKey"));
                    if (user != null)
                    {
                        Session["UserId"] = user.UserId;
                        Session["EmailId"] = user.EmailId;
                        Session["AccessPlandId"] = user.MemShipId;
                        Session["BusinessName"] = user.BusinessName;
                        if (model.ReturnUrl != null)
                        {
                            return Redirect(model.ReturnUrl);
                        }
                        //Session.Timeout = 6000;
                        return RedirectToAction("UsersProfile", "Home");
                    }

                }
                TempData["Error"] = true;
                //ViewBag.Msg = "Invalid Username or password...";
                //Session["Error"] = "Invalid Username or password...";
                //return View();
                return RedirectToAction("Login", "Home");
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [CustumAuthorize.AllowAnonymous]
        public JsonResult HomeLogin(string email, string pwd)
        {
            try
            {
                var user = new BussinessUserBLL { }.UserLogin(email, DataEncryption.Encrypt(pwd.Trim(), "passKey"));
                if (user != null)
                {
                    Session["UserId"] = user.UserId;
                    Session["EmailId"] = user.EmailId;
                    Session["AccessPlandId"] = user.MemShipId;
                    Session["BusinessName"] = user.BusinessName;
                    return Json(true);
                }
                return Json(false);
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }

        public ActionResult HomeBanner(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            HomeBannerModel model = new HomeBannerModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            var BannerList = new HomeBannerBLL { }.GetBannerImageByIUserId(skip, take, userid);
            double count = Convert.ToDouble(new HomeBannerBLL { }.GetPageCountByUserId(userid));
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (BannerList != null)
            {
                model.HomeBannerImgList = BannerList.Where(x => x.UserId == userid).Select(x => new HomeBannerModel
                {
                    HomeBannerID = x.HomeBannerID,
                    BannerImage = x.BannerImage,
                    UserId = x.UserId,
                    CompanyURL=x.CompanyURL,
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();

                model.IndustryList = (from Ind in objdb.Industries
                                      where Ind.IsActive == true
                                      select new IndustryModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId

                                      }).Take(7).ToList();
                model.CityList = (from ct in objdb.Cities
                                  where ct.IsActive == true
                                  select new CityModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId

                                  }).Take(7).ToList();
                var NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.CreatedBy == userid
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                model.NewsList = NewsList;
                //_HomeModel.NewsTitle = BusinessnewsList.NewsTitle;
                //_HomeModel.NewsDesc = BusinessnewsList.Description;

            }

            return View(model);
        }

        public ActionResult UpdateHomeBanner(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            try
            {
                HomeBannerModel model = new HomeBannerModel();
                if (id != 0)
                {
                    var objmodel = new HomeBannerBLL { }.GetBannerImageById(id);
                    if (objmodel != null)
                    {
                        model.HomeBannerID = objmodel.HomeBannerID;
                        model.BannerImage = objmodel.BannerImage;
                        model.CompanyURL = objmodel.CompanyURL;
                        model.UserId = objmodel.UserId;
                    }
                }
                model.IndustryList = (from Ind in objdb.Industries
                                      where Ind.IsActive == true
                                      select new IndustryModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId

                                      }).Take(7).ToList();
                model.CityList = (from ct in objdb.Cities
                                  where ct.IsActive == true
                                  select new CityModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId

                                  }).Take(7).ToList();
                var NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == userid
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                model.NewsList = NewsList;
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateHomeBanner(HomeBannerModel model, IEnumerable<HttpPostedFileBase> file)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var getuser = new BussinessUserBLL { }.GetUserById(userid);
            try
            {
                int res = 0;
                string strFileName = "";
                string path = "";
                Random rnd = new Random();

                string strURL = model.CompanyURL;
                string Suburl = strURL.Substring(0, 4);
                if (Suburl == "http")
                {
                    strURL = model.CompanyURL;
                }
                else
                {
                    strURL ="http://"+ model.CompanyURL;
                }

                if (file != null)
                {
                    foreach (var item in file)
                    {
                        if (item != null)
                        {


                            string sname = item.FileName;
                            strFileName = "HomeBanner" + rnd.Next(10000, 10000000) + "." + "jpg";
                            path = Server.MapPath("~/Scripts/jquery.bxslider/images/" + strFileName);
                            res = new HomeBannerBLL { }.AddEditBanner(new HomeBannerModel
                            {
                                UserId = userid,
                                IsActive = model.IsActive,
                                BannerImage = strFileName,
                                HomeBannerID = model.HomeBannerID,
                                //CompanyURL=model.CompanyURL
                                CompanyURL = strURL
                            });
                            item.SaveAs(path);
                            #region Send Email
                            msg = new MailMessage();
                            SmtpClient smtp = new SmtpClient();
                            emailId = "ameer.pasha@thebusinessbranding.com";
                            //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                            //uPwd = DataEncryption.Decrypt(password, "passKey");
                            //uPwd = model.Password.Trim();
                            //sender email address
                            msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                            //Receiver email address
                            msg.To.Add(emailId);
                            msg.Subject = "Big Hoarding Banner Request";
                            //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                            //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                            //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                            // ActivationUrl = Server.HtmlEncode(link);
                            //msg.Body = "Hi " + model.BusinessName + "!\n" +
                            // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                            // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                            var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(10);
                            var test = emailtemp.Description + "<br />" + ActivationUrl;
                            string mailbod = MailBody(test, getuser);
                            msg.Body = mailbod;
                            msg.IsBodyHtml = true;
                            smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                            smtp.Port = 25;
                            smtp.Host = "mail.FRMT.IN";
                            //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                            //smtp.Port = 2525;
                            //smtp.Host = "retail.smtp.com";
                            smtp.EnableSsl = false;
                            smtp.Send(msg);

                            TempData["success"] = "Home Banner Updated Successfully!";

                            #endregion
                        }
                        else
                        {
                            res = new HomeBannerBLL { }.AddEditBanner(new HomeBannerModel
                            {
                                UserId = userid,
                                IsActive = model.IsActive,
                                //BannerImage = strFileName,
                                HomeBannerID = model.HomeBannerID,
                                //CompanyURL=model.CompanyURL
                                CompanyURL = strURL
                            });
                            //item.SaveAs(path);
                            #region Send Email
                            msg = new MailMessage();
                            SmtpClient smtp = new SmtpClient();
                            emailId = "ameer.pasha@thebusinessbranding.com";
                            //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                            //uPwd = DataEncryption.Decrypt(password, "passKey");
                            //uPwd = model.Password.Trim();
                            //sender email address
                            msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                            //Receiver email address
                            msg.To.Add(emailId);
                            msg.Subject = "Big Hoarding Banner Request";
                            //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                            //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                            //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                            // ActivationUrl = Server.HtmlEncode(link);
                            //msg.Body = "Hi " + model.BusinessName + "!\n" +
                            // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                            // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                            var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(10);
                            var test = emailtemp.Description + "<br />" + ActivationUrl;
                            string mailbod = MailBody(test, getuser);
                            msg.Body = mailbod;
                            msg.IsBodyHtml = true;
                            smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                            smtp.Port = 25;
                            smtp.Host = "mail.FRMT.IN";
                            //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                            //smtp.Port = 2525;
                            //smtp.Host = "retail.smtp.com";
                            smtp.EnableSsl = false;
                            smtp.Send(msg);

                            TempData["success"] = "Home Banner Updated Successfully!";

                            #endregion
                        }
                    }
                    return RedirectToAction("HomeBanner");
                }
                return RedirectToAction("HomeBanner");
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public JsonResult ChangeHomeBannerStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new HomeBannerBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult UsersProfile(BussinessUserModel model)
        {
            if (TempData["success"] == "EmpBranding")
            {
                TempData["AlertMessage"] = "Employee Branding Request successfully Sent";
            }
            if (TempData["success"] == "FranchiseRequest")
            {
                TempData["AlertMessage"] = "Franchise Request successfully Sent";
            }
            if (TempData["success"] == "InvestorPartneringRequest")
            {
                TempData["AlertMessage"] = "Investor Partnering Request successfully Sent";
            }
            if (TempData["success"] == "CorporateBrandingRequest")
            {
                TempData["AlertMessage"] = "Corporate Branding Request successfully Sent";
            }
            if (TempData["success"] == "GlobalBrandingRequest")
            {
                TempData["AlertMessage"] = "Global Branding Request successfully Sent";
            }
            else if (Convert.ToBoolean(TempData["Error"]) == true)
            {
                TempData["AlertMessage"] = "Please Try Again After Some Time";
            }
            _HomeModel = new LandingPageModel();
            int userid = Convert.ToInt32(Session["UserId"]);
            string Emailid = Convert.ToString(Session["EmailId"]);
            int AccessPlanId = Convert.ToInt32(Session["AccessPlandId"]);
            var objmodel = new BussinessUserBLL { }.GetUserById(userid);
            var TargeBrandData = new TargetImageBLL { }.GetTargetImageByUserId(userid);
            //if (objmodel.CityId ==TargeBrandData.CityId)
            //{

            //}
            //else if (objmodel.IndustryId==TargeBrandData.IndustryId)
            //{

            //}

            if (objmodel != null)
            {
                List<LandingPageModel> objfriendlist = new List<LandingPageModel>();
                List<LandingPageModel> objfriendlist1 = new List<LandingPageModel>();
                LandingPageModel objFriendNotinList = new LandingPageModel();
                List<LandingPageModel> objFriendNotinListNew = new List<LandingPageModel>();
                try
                {
                    // USER HAVING CONTACTS
                    objfriendlist = (from bissUser in objdb.BussinessUsers
                                     join bissCon in objdb.BusinessConnections
                                     on bissUser.UserId equals bissCon.BusinessPartnerId
                                     where bissCon.UserId == userid && bissCon.IsActive == true
                                     select new LandingPageModel
                                     {
                                         PartnerId = bissCon.BusinessPartnerId.HasValue ? bissCon.BusinessPartnerId.Value : 0,
                                         BusinessName = bissUser.BusinessName,
                                         EmailId = bissUser.EmailId,
                                         CompanyLogo = bissUser.CompanyLogo,
                                         BusinessDetails = bissUser.BusinessDetails//Regex.Replace(bissUser.BusinessDetails, @"<[^>]*>", String.Empty)
                                     }).Take(2).ToList();
                    objfriendlist.ForEach(x => x.BusinessDetails = Regex.Replace(x.BusinessDetails, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                    foreach (LandingPageModel user in objfriendlist)
                    {
                        objfriendlist1.Add(user);
                    }
                }
                catch (Exception ex) { }
                try
                {
                    // CURRENT USER ADDED BY OTEHR USERS
                    objfriendlist = (from bissUser in objdb.BussinessUsers
                                     join bissCon in objdb.BusinessConnections
                                     on bissUser.UserId equals bissCon.UserId
                                     where bissCon.BusinessPartnerId == userid && bissCon.IsActive == true
                                     select new LandingPageModel
                                     {
                                         PartnerId = bissCon.UserId.HasValue ? bissCon.UserId.Value : 0,
                                         BusinessName = bissUser.BusinessName,
                                         EmailId = bissUser.EmailId,
                                         CompanyLogo = bissUser.CompanyLogo,
                                         BusinessDetails = bissUser.BusinessDetails//Regex.Replace(bissUser.BusinessDetails, @"<[^>]*>", String.Empty)

                                     }).Take(2).ToList();
                    objfriendlist.ForEach(x => x.BusinessDetails = Regex.Replace(x.BusinessDetails, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                    foreach (LandingPageModel user in objfriendlist)
                    {
                        objfriendlist1.Add(user);
                    }
                }
                catch (Exception ex) { }


                _HomeModel.CompanyBanner = objmodel.CompanyBanner;
                _HomeModel.CompanyLogo = objmodel.CompanyLogo;
                _HomeModel.BusinessName = objmodel.BusinessName;
                _HomeModel.UserId = objmodel.UserId;
                _HomeModel.ConnectedBizList = objfriendlist1;
                _HomeModel.ContactPerson = objmodel.ContactPerson;
                _HomeModel.Regno = objmodel.CoRegNo;
                _HomeModel.Phone = objmodel.Phone;
                _HomeModel.Designation = objmodel.Designation;
                _HomeModel.Address = objmodel.CoAddress;
                _HomeModel.TradeEmailId = objmodel.TradeEmaiIId;
                _HomeModel.BusinessDetails = objmodel.BusinessDetails;
                _HomeModel.EnquiryCount = objdb.Messages.Where(x => x.MsgTo == userid && x.ToStatus == true && x.IsEnquiry == true && x.Status == true).Select(x => x.MessageId).Count();

                #region UserSelectedIndustrysuggestionList

                List<UserIndustryMappingModel> UserSelectedIndustry = new List<UserIndustryMappingModel>();
                List<LandingPageModel> UserSelected = new List<LandingPageModel>();
                List<LandingPageModel> UserSelectednew = new List<LandingPageModel>();

                UserSelectedIndustry = (from bi in objdb.UserIndustryMappings
                                        where bi.UserId == userid
                                        select new UserIndustryMappingModel
                                        {
                                            IndustryId = bi.IndustryId.HasValue ? bi.IndustryId.Value : 0,
                                            UserId = userid,
                                        }).ToList();
                if (UserSelectedIndustry != null)
                {
                    foreach (var item in UserSelectedIndustry)
                    {
                        UserSelected = (from bu in objdb.BussinessUsers
                                        where bu.IndustryId == item.IndustryId
                                        select new LandingPageModel
                                        {
                                            BusinessName = bu.BusinessName,
                                            CompanyLogo = bu.CompanyLogo,
                                            BusinessDetails = bu.BusinessDetails,
                                            UserId = bu.UserId,
                                            //IndustryId=ind.IndustryId,
                                            //IndustryName=ind.IndustryName,
                                        }).ToList();

                        UserSelectednew.AddRange(UserSelected);
                    }
                }

                //List<BussinessUser> UserList = new List<BussinessUser>();
                //UserList = (from bus in objdb.BussinessUsers where bus.UserId != userid select bus).ToList();
                //foreach (BussinessUser buser in UserList)
                foreach (var buser in UserSelectednew)
                {
                    int F_id = buser.UserId;
                    var u = (from us in objdb.BusinessConnections.Where(p => p.UserId == userid && p.BusinessPartnerId == F_id) select us).SingleOrDefault();
                    if (u == null)
                    {
                        u = (from us in objdb.BusinessConnections.Where(p => p.UserId == F_id && p.BusinessPartnerId == userid) select us).SingleOrDefault();
                        if (u == null)
                        {
                            // This condition is for friends not in contacts
                            try
                            {
                                objFriendNotinList = (from bu in objdb.BussinessUsers

                                                      where bu.UserId == F_id
                                                      select new LandingPageModel
                                                      {
                                                          UserId = bu.UserId,
                                                          PartnerId = bu.UserId,
                                                          BusinessName = bu.BusinessName,
                                                          CompanyLogo = bu.CompanyLogo,
                                                          BusinessDetails = bu.BusinessDetails//Regex.Replace(bu.BusinessDetails, @"<[^>]*>", String.Empty)
                                                      }).SingleOrDefault();
                            }
                            catch (Exception ex) { }
                            if (!objFriendNotinListNew.Contains(objFriendNotinList))
                                objFriendNotinListNew.Add(objFriendNotinList);
                        }
                    }
                }
                #endregion


                objFriendNotinListNew.ForEach(x => x.BusinessDetails = Regex.Replace(x.BusinessDetails, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                _HomeModel.SuggestedConn = objFriendNotinListNew.Take(4).ToList();

                _HomeModel.ProductsList = (from pr in objdb.Products
                                           let x = objdb.ProductImages.Where(pi => pi.ProductId == pr.ProductId).FirstOrDefault()
                                           where pr.UserId == userid
                                           select new LandingPageModel
                                           {
                                               ProductId = pr.ProductId,
                                               ProductName = pr.ProductName,
                                               ProductDesc = pr.ProdDetails,
                                               UserId = pr.UserId.HasValue ? pr.UserId.Value : 0,
                                               ProductImage = x == null ? null : x.ProdImage
                                               //ProductImage =pr.ProductImages.Select(x=>x.ProdImage).FirstOrDefault()
                                           }).ToList();

                //All news irrelevant to Active status are shown on userprofile page or else only isactive=true can also be displayed
                var NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.CreatedBy == userid
                                select new BusinessNewsModel

        {
            BusinessNewsId = UserNews.BusinessNewsId,
            NewsTitle = UserNews.NewsTitle,
            Description = UserNews.Description

        }).OrderByDescending(x => x.BusinessNewsId).Take(5).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                _HomeModel.NewsUpdates = NewsList;


                _HomeModel.PendingRequests = (from bc in objdb.BusinessConnections
                                              join bu in objdb.BussinessUsers
                                              on bc.UserId equals bu.UserId
                                              where bc.UserId != userid && bc.BusinessPartnerId == userid && bc.IsActive == false
                                              select new LandingPageModel
                                              {
                                                  BusinessName = bu.BusinessName,
                                                  CompanyLogo = bu.CompanyLogo,
                                                  PartnerId = bc.BusinessPartnerId,
                                                  UserId = bu.UserId,
                                                  BussinessConnectionId = bc.Id
                                              }).Take(3).ToList();

                _HomeModel.CoBrandRequest = (from Co in objdb.CoBrandings
                                             join bu in objdb.BussinessUsers
                                             on Co.PartnerA equals bu.UserId
                                             where Co.PartnerA != userid && Co.PartnerB == userid && Co.IsActive == false
                                             select new LandingPageModel
                                             {
                                                 BusinessName = bu.BusinessName,
                                                 CompanyLogo = bu.CompanyLogo,
                                                 PartnerId = Co.PartnerA,
                                                 //UserId = Co.PartnerA

                                             }).Take(3).ToList();
                return View(_HomeModel);
            }
            return View();
        }

        public ActionResult EditCompanyProfile(BussinessUserModel model)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var objmodel = new BussinessUserBLL { }.GetUserById(userid);
            return PartialView("_EditComp");

        }

        public ActionResult ChangeLogoandBanner()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            LandingPageModel obj = new LandingPageModel();
            var getuserdata = new BussinessUserBLL { }.GetUserById(userid);
            if (getuserdata != null)
            {
                obj.BusinessDetails = getuserdata.BusinessDetails;
                obj.BusinessName = getuserdata.BusinessName;
                obj.CompanyBanner = getuserdata.CompanyBanner;
                obj.CompanyLogo = getuserdata.CompanyLogo;
            }
            return View(obj);
        }

        public ActionResult EditProfile(BussinessUserModel model)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            LandingPageModel obj = new LandingPageModel();
            var getuserdata = new BussinessUserBLL { }.GetUserById(userid);
            if (getuserdata != null)
            {
                obj.BusinessDetails = getuserdata.BusinessDetails;
                obj.BusinessName = getuserdata.BusinessName;
                obj.Phone = getuserdata.Phone;
                obj.Regno = getuserdata.CoRegNo;
                obj.ContactPerson = getuserdata.ContactPerson;
                obj.Address = getuserdata.CoAddress;
                obj.Designation = getuserdata.Designation;
                obj.TradeEmailId = getuserdata.TradeEmaiIId;
                obj.IndustryId = getuserdata.IndustryId;
                obj.CountryId = getuserdata.CountryId;
                obj.CityId = getuserdata.CityId;
                obj.Fax = getuserdata.Fax;
                obj.CompanyURL = getuserdata.CompURL;
                
            }
            CustomMethods.BindIndustry(obj);
            CustomMethods.BindCountryWithCity(obj);
            CustomMethods.BindCityForUser(obj);
            return View(obj);
        }

         [CustumAuthorize.AllowAnonymous]
        public JsonResult GetAds()
        {
            int id = Convert.ToInt32(Session["VisitorId"]);
            // var ads = new BussinessUserBLL{}.GetAllAdsHome();
            var ads = new BussinessUserBLL { }.GetAllAdsHomebyUserId(id);
            //var getbanner = new List<Banner>();
            //if (cseason != null)
            //{
            //    getbanner = _objBanner.GetBannerDetBySeasonId(cseason.ID);
            //}
            var name = new List<string>();
            if (ads.Count > 0)
            {
                name.AddRange(ads.Select(a => a.Image + "~" + a.URL));
            }
            return Json(name, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewMoreFrndNotList(int? page, string currentFilter, int pid = 0, int cid = 0)
        {
            //ViewBag.CurrentSort = sortOrder;
            int take = 10;
            int skip = take * pid;

            _HomeModel = new LandingPageModel();
            _HomeModel.PageID = pid;
            _HomeModel.Current = pid + 1;

            int userid = Convert.ToInt32(Session["UserId"]);
            string Emailid = Convert.ToString(Session["EmailId"]);

            LandingPageModel objFriendNotinList = new LandingPageModel();
            List<LandingPageModel> objFriendNotinListNew = new List<LandingPageModel>();

            List<BussinessUser> UserList = new List<BussinessUser>();
            UserList = (from bus in objdb.BussinessUsers where bus.UserId != userid select bus).ToList();
            foreach (BussinessUser buser in UserList)
            {
                int F_id = buser.UserId;
                var u = (from us in objdb.BusinessConnections.Where(p => p.UserId == userid && p.BusinessPartnerId == F_id) select us).SingleOrDefault();
                if (u == null)
                {
                    u = (from us in objdb.BusinessConnections.Where(p => p.UserId == F_id && p.BusinessPartnerId == userid) select us).SingleOrDefault();
                    if (u == null)
                    {
                        // This condition is for friends not in contacts
                        try
                        {
                            objFriendNotinList = (from bu in objdb.BussinessUsers

                                                  where bu.UserId == F_id
                                                  select new LandingPageModel
                                                  {
                                                      UserId = bu.UserId,
                                                      PartnerId = bu.UserId,
                                                      BusinessName = bu.BusinessName,
                                                      CompanyLogo = bu.CompanyLogo,
                                                      BusinessDetails = bu.BusinessDetails
                                                  }).SingleOrDefault();
                        }
                        catch (Exception ex) { }
                        if (!objFriendNotinListNew.Contains(objFriendNotinList))
                            objFriendNotinListNew.Add(objFriendNotinList);
                    }

                }
            }

            //objFriendNotinListNew.ForEach(x => x.BusinessDetails = Regex.Replace(x.BusinessDetails, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
            objFriendNotinListNew.ForEach(x => x.BusinessDetails=x.BusinessDetails);

            double count = Convert.ToDouble(objFriendNotinListNew.Count);
            var res = count / take;
            _HomeModel.Pagecount = (int)Math.Ceiling(res);
            _HomeModel.SuggestedConn = objFriendNotinListNew.Skip(skip).Take(take).ToList();

            _HomeModel.IndustryList = (from Ind in objdb.Industries
                                       where Ind.IsActive == true
                                       select new IndustryModel
                                       {
                                           IndustryName = Ind.IndustryName,
                                           IndustryId = Ind.IndustryId

                                       }).Take(7).ToList();
            _HomeModel.CityList = (from ct in objdb.Cities
                                   where ct.IsActive == true
                                   select new CityModel
                                   {
                                       CityName = ct.CityName,
                                       CityId = ct.CityId

                                   }).Take(7).ToList();
            return View(_HomeModel);
        }

        public ActionResult ViewAllRequest()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            string Emailid = Convert.ToString(Session["EmailId"]);
            _HomeModel.PendingRequests = (from bc in objdb.BusinessConnections
                                          join bu in objdb.BussinessUsers
                                          on bc.UserId equals bu.UserId
                                          where bc.UserId != userid && bc.BusinessPartnerId == userid && bc.IsActive == false
                                          select new LandingPageModel
                                          {
                                              BusinessName = bu.BusinessName,
                                              CompanyLogo = bu.CompanyLogo,
                                              PartnerId = bc.BusinessPartnerId,
                                              UserId = bu.UserId,
                                              BussinessConnectionId = bc.Id
                                          }).ToList();
            return View(_HomeModel);
        }

        public ActionResult ViewAllCoBrandRequest()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            string Emailid = Convert.ToString(Session["EmailId"]);
            _HomeModel.CoBrandRequest = (from Co in objdb.CoBrandings
                                         join bu in objdb.BussinessUsers
                                         on Co.PartnerA equals bu.UserId
                                         where Co.PartnerA != userid && Co.PartnerB == userid && Co.IsActive == false
                                         select new LandingPageModel
                                         {
                                             BusinessName = bu.BusinessName,
                                             CompanyLogo = bu.CompanyLogo,
                                             PartnerId = Co.PartnerA,
                                             //UserId = Co.PartnerA

                                         }).ToList();
            return View(_HomeModel);
        }

        public ActionResult ViewAllConnList()
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            string Emailid = Convert.ToString(Session["EmailId"]);
            List<LandingPageModel> objfriendlist = new List<LandingPageModel>();
            List<LandingPageModel> objfriendlist1 = new List<LandingPageModel>();

            try
            {
                // USER HAVING CONTACTS
                objfriendlist = (from bissUser in objdb.BussinessUsers
                                 join bissCon in objdb.BusinessConnections
                                 on bissUser.UserId equals bissCon.BusinessPartnerId
                                 where bissCon.UserId == userid && bissCon.IsActive == true
                                 select new LandingPageModel
                                 {
                                     PartnerId = bissCon.BusinessPartnerId.HasValue ? bissCon.BusinessPartnerId.Value : 0,
                                     BusinessName = bissUser.BusinessName,
                                     EmailId = bissUser.EmailId,
                                     CompanyLogo = bissUser.CompanyLogo,
                                     BusinessDetails = bissUser.BusinessDetails
                                 }).ToList();
                objfriendlist.ForEach(x => x.BusinessDetails = Regex.Replace(x.BusinessDetails, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                foreach (LandingPageModel user in objfriendlist)
                {
                    objfriendlist1.Add(user);
                }
            }
            catch (Exception ex) { }
            try
            {
                //// CURRENT USER ADDED BY OTEHR USERS
                objfriendlist = (from bissUser in objdb.BussinessUsers
                                 join bissCon in objdb.BusinessConnections
                                 on bissUser.UserId equals bissCon.UserId
                                 where bissCon.BusinessPartnerId == userid && bissCon.IsActive == true
                                 select new LandingPageModel
                                 {
                                     PartnerId = bissCon.UserId.HasValue ? bissCon.UserId.Value : 0,
                                     BusinessName = bissUser.BusinessName,
                                     EmailId = bissUser.EmailId,
                                     CompanyLogo = bissUser.CompanyLogo,
                                     BusinessDetails = bissUser.BusinessDetails
                                 }).ToList();
                objfriendlist.ForEach(x => x.BusinessDetails = Regex.Replace(x.BusinessDetails, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                foreach (LandingPageModel user in objfriendlist)
                {
                    objfriendlist1.Add(user);
                }
            }
            catch (Exception ex) { }

            _HomeModel.ConnectedBizList = objfriendlist1;

            return View(_HomeModel);
        }

        [HttpPost]
        public ActionResult UpdateProfile(LandingPageModel model, HttpPostedFileBase bannerfile, HttpPostedFileBase logofile)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var objmodel = new BussinessUserBLL { }.GetUserById(userid);
            //var checkduplicate = objdb.BussinessUsers.Where(x => x.UserId == objmodel.UserId || x.BusinessName == objmodel.BusinessName).Select(x =>
            //     new BussinessUserModel { UserId = x.UserId, BusinessName = x.BusinessName }).SingleOrDefault();
            if (objmodel != null)
            {
                try
                {
                    int res = 0;
                    string strFileName = "";
                    string strBannerName = "";
                    string path = "";
                    string path1 = "";
                    Random rnd = new Random();
                    //if (ModelState.IsValid)
                    //{

                    if (logofile != null)
                    {
                        strFileName = "CoLogo_" + rnd.Next(100, 100000000) + "." + logofile.FileName.Split('.')[1].ToString();
                        path = Server.MapPath("~/Content/Images/CoLogo/" + strFileName);
                        objmodel.CompanyLogo = strFileName;
                    }
                    if (bannerfile != null)
                    {
                        strBannerName = "CoBanner_" + rnd.Next(100, 100000000) + "." + bannerfile.FileName.Split('.')[1].ToString();
                        path1 = Server.MapPath("~/Content/Images/CoBanner/" + strBannerName);

                        objmodel.CompanyBanner = strBannerName;
                    }

                    objmodel.BusinessName = model.BusinessName;


                    objmodel.UpdatedDate = DateTime.Now;
                    res = new BussinessUserBLL { }.AddEditUser(objmodel);
                    if (bannerfile != null)
                    {
                        bannerfile.SaveAs(path1);
                    }
                    if (logofile != null)
                    {
                        logofile.SaveAs(path);
                    }
                    if (res != 0)
                    {
                        TempData["AlertMessage"] = "Banner and Logo Updated successfully";
                       
                        Session["Success"] = "Successfully Updated The User";
                        return RedirectToAction("UsersProfile", "Home");
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return RedirectToAction("UsersProfile", "Home");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult UpdateProfileTabs(LandingPageModel model, IEnumerable<string> SelectItems)
        {
            int res = 0;
            int uid = Convert.ToInt32(Session["UserId"]);
            var objmodel = new BussinessUserBLL { }.GetUserById(uid);
            if (objmodel != null)
            {
                objmodel.BusinessDetails = model.BusinessDetails;
                objmodel.CoRegNo = model.Regno;
                objmodel.ContactPerson = model.ContactPerson;
                objmodel.IndustryId = model.IndustryId;
                objmodel.CountryId = model.CountryId;
                objmodel.CityId = model.CityId;
                objmodel.Phone = model.Phone;
                objmodel.Fax = model.Fax;
                objmodel.CoAddress = model.Address;
                objmodel.CompURL = model.CompanyURL;
                objmodel.Designation = model.Designation;
                objmodel.TradeEmaiIId = model.TradeEmailId;
                objmodel.UpdatedDate = DateTime.Now;
                int iRes = 0;
                if (SelectItems != null)
                {
                    foreach (var item in SelectItems)
                    {
                        UserIndustryMappingModel mod = new UserIndustryMappingModel();
                        mod.UserId = uid;
                        mod.IndustryId = Convert.ToInt32(item);
                        iRes = new BussinessUserBLL { }.AddUserIndutryMapping(mod);
                    }
                }
                res = new BussinessUserBLL { }.AddEditUser(objmodel);
                if (res != 0)
                {
                    TempData["AlertMessage"] = "Successfully Updated Profile";
                    //Session["Success"] = "Successfully Updated The User";
                    return RedirectToAction("UsersProfile", "Home");
                }

            }
            return View();
        }

        public ActionResult BusinessNewsList(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            BusinessNewsModel model = new BusinessNewsModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            var BusinessnewsList = new BusinessNewsBLL { }.GetAllBusinessNews(userid, skip, take);
            double count = Convert.ToDouble(new BusinessNewsBLL { }.GetPageCountByUserid(userid));
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (BusinessnewsList != null)
            {
                model.BusinessNewsList = BusinessnewsList.Where(x => x.CreatedBy == userid).Select(x => new BusinessNewsModel
                {
                    BusinessNewsId = x.BusinessNewsId,
                    NewsTitle = x.NewsTitle,
                    Description = x.Description,
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).OrderByDescending(x => x.BusinessNewsId).ToList();

                model.IndustryList = (from Ind in objdb.Industries
                                      where Ind.IsActive == true
                                      select new IndustryModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId

                                      }).Take(7).ToList();
                model.CityList = (from ct in objdb.Cities
                                  where ct.IsActive == true
                                  select new CityModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId

                                  }).Take(7).ToList();


            }

            return View(model);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult ViewallAdminNews(int pid = 0)
        {
            NewsMapping NewMap = new NewsMapping();
            NewMap = new MembershipBLL { }.GetHomeNewsListBLL();
            List<BusinessNewsModel> NewsList = new List<BusinessNewsModel>();
            List<BusinessNewsModel> AdminNewsList = new List<BusinessNewsModel>();

            if (NewMap != null)
            {
                NewsList = (from UserNews in objdb.BusinessNews
                            where UserNews.IsActive == true && UserNews.CreatedBy == NewMap.UserId
                            select new BusinessNewsModel
                            {
                                BusinessNewsId = UserNews.BusinessNewsId,
                                NewsTitle = UserNews.NewsTitle,
                                Description = UserNews.Description

                            }).OrderByDescending(x => x.BusinessNewsId).Take(2).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));


            }
            AdminNewsList = (from ns in objdb.News
                             where ns.IsActive == true
                             select new BusinessNewsModel
                             {
                                 BusinessNewsId = ns.NewsId,
                                 NewsTitle = ns.NewsTitle,
                                 NewsImage=ns.NewsImage,
                                 Description = ns.Description

                             }).OrderByDescending(x => x.BusinessNewsId).ToList();
            AdminNewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));

            foreach (var item in NewsList)
            {
                AdminNewsList.Add(item);
            }
            _HomeModel.NewsUpdates = AdminNewsList;


            return View(_HomeModel);
        }


        public ActionResult CreateHomePageNews()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateHomePageNews(NewsModel model, HttpPostedFileBase file)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
                try
                {
                    int res = 0;
                    string strFileName = "";
                    string path = "";
                    Random rnd = new Random();
                    if (file != null)
                    {
                        strFileName = "HomePageNewsImg_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                        path = Server.MapPath("~/Areas/Admin/ProjectImages/HomepageNews/" + strFileName);
                        res = new NewsBLL { }.AddEditNews(new NewsModel
                        {
                            NewsTitle = model.NewsTitle,
                            CreatedByUserId = userid,
                            NewsImage = strFileName,
                            NewsDesc = model.NewsDesc,
                            CreateOn = DateTime.Now,
                            IsActive = false,
                        });
                        file.SaveAs(path);

                        if (res != 0)
                        {
                            iVal = 1;
                            TempData["AlertMessage"] = "Succesfully HomePage News Request Sent to Admin to Approve.";

                            return RedirectToAction("BusinessNewsList", "Home");
                        }
                    }
                    else
                    {
                        res = new NewsBLL { }.AddEditNews(new NewsModel
                        {
                            NewsTitle = model.NewsTitle,
                            CreatedByUserId = userid,
                            NewsImage = strFileName,
                            NewsDesc = model.NewsDesc,
                            CreateOn = DateTime.Now,
                            IsActive = true,
                        });
                        file.SaveAs(path);
                        if (res != 0)
                        {
                            iVal = 1;
                            TempData["AlertMessage"] = "Succesfully HomePage News Request Sent to Admin to Approve.";

                            return RedirectToAction("BusinessNewsList", "Home");
                        }
                    }
                    TempData["Error"] = true;
                    //Session["Error"] = "Please try again after some time.";
                    return RedirectToAction("BusinessNewsList", "Home");
                }
                catch (Exception)
                {
                    return RedirectToAction("BusinessNewsList", "Home");
                    throw;
                }
                //TempData["AlertMessage"] = false;
                return RedirectToAction("BusinessNewsList", "Home");
            // return View();
        }

        public ActionResult UpdateBusinessNews(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            try
            {
                BusinessNewsModel model = new BusinessNewsModel();
                if (id != 0)
                {
                    var objmodel = new BusinessNewsBLL { }.GetBusinessNewsByNewsId(id);
                    if (objmodel != null)
                    {
                        model.BusinessNewsId = objmodel.BusinessNewsId;
                        model.NewsTitle = objmodel.NewsTitle;
                        model.Description = objmodel.Description;
                    }
                }
                model.IndustryList = (from Ind in objdb.Industries
                                      where Ind.IsActive == true
                                      select new IndustryModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId

                                      }).Take(7).ToList();
                model.CityList = (from ct in objdb.Cities
                                  where ct.IsActive == true
                                  select new CityModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId

                                  }).Take(7).ToList();
                var NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == userid
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                model.BusinessNewsList = NewsList;
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateBusinessNews(BusinessNewsModel model)
        {
            try
            {
                int userid = Convert.ToInt32(Session["UserId"]);
                if (ModelState.IsValid)
                {
                    if (model.BusinessNewsId == 0)
                    {
                        var checkDuplicate = objdb.BusinessNews.Where(x => x.NewsTitle == model.NewsTitle).SingleOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new BusinessNewsBLL { }.AddEditBusinessNews(new BusinessNewsModel
                            {
                                BusinessNewsId = model.BusinessNewsId,
                                NewsTitle = model.NewsTitle,
                                Description = model.Description,
                                CreatedBy = userid,
                                CreatedDate = DateTime.Now
                                // IsActive = model.IsActive,
                            });
                            if (res != 0)
                            {
                                //Session["Success"] = "Successfully Added The Record";
                                TempData["AlertMessage"] = "Successfully Added The Record";
                                return RedirectToAction("BusinessNewsList");
                            }
                        }
                        //Session["Error"] = "Record Already Exists";
                        TempData["AlertMessage"] = "Record Already Exists";
                        return RedirectToAction("BusinessNewsList");
                    }
                    else
                    {
                        int res = new BusinessNewsBLL { }.AddEditBusinessNews(new BusinessNewsModel
                        {
                            BusinessNewsId = model.BusinessNewsId,
                            NewsTitle = model.NewsTitle,
                            Description = model.Description,
                            UpdatedBy = userid,
                            UpdatedDate = DateTime.Now
                            //IsActive = model.IsActive,
                        });
                        if (res != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("BusinessNewsList");
                        }
                    }
                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult RemoveBusinessNews(int id)
        {
            var User = new BusinessNewsBLL { }.Remove(id);
            if (User != 0)
            {
                return RedirectToAction("BusinessNewsList");
            }
            return RedirectToAction("BusinessNewsList");
        }

        public ActionResult ViewBusinessNews(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            BusinessNewsModel model = new BusinessNewsModel();
            if (id != 0)
            {
                BusinessNewsModel Objmodel = new BusinessNewsBLL { }.GetBusinessNewsById(id);
                if (Objmodel != null)
                {
                    model.BusinessNewsId = Objmodel.BusinessNewsId;
                    model.NewsTitle = Objmodel.NewsTitle;
                    model.Description = Objmodel.Description;
                }
                model.IndustryList = (from Ind in objdb.Industries
                                      where Ind.IsActive == true
                                      select new IndustryModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId

                                      }).Take(7).ToList();
                model.CityList = (from ct in objdb.Cities
                                  where ct.IsActive == true
                                  select new CityModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId

                                  }).Take(7).ToList();
                var NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == userid
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                model.BusinessNewsList = NewsList;

            }
            return View(model);
        }

        public ActionResult ProductList(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            ProductWebModel model = new ProductWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<ProductWebModel> Products = new List<ProductWebModel>();
            var ProdList = new ProductBLL { }.GetAllProduct(skip, take);
            double count = Convert.ToDouble(new ProductBLL { }.GetPageCount(userid));
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (ProdList != null)
            {
                var objacplan = new ProductImageBLL { }.GetProductImageById(userid);
                double countimg = Convert.ToDouble(objacplan.Count);
                var Prod = (from pr in objdb.Products
                            let x = objdb.ProductImages.Where(pi => pi.ProductId == pr.ProductId).FirstOrDefault()
                            where pr.UserId == userid
                            select new ProductWebModel
                            {
                                ProductName = pr.ProductName,
                                ProdImage = x == null ? null : x.ProdImage,
                                CreatedDate = pr.CreatedDate,
                                ProdDetails = pr.ProdDetails,
                                ProductId = pr.ProductId,
                                IsActive = pr.IsActive
                            }).OrderByDescending(x => x.ProductId).ToList();

                model.IndustryList = (from Ind in objdb.Industries
                                      where Ind.IsActive == true
                                      select new IndustryModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId

                                      }).Take(7).ToList();
                model.CityList = (from ct in objdb.Cities
                                  where ct.IsActive == true
                                  select new CityModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId

                                  }).Take(7).ToList();
                var NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == userid
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                model.NewsUpdates = NewsList;
                model.Products = Prod;
            }

            return View(model);
        }

        public ActionResult EditProduct(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            try
            {
                ProductWebModel model = new ProductWebModel();
                if (id != 0)
                {
                    var objmodel = new ProductBLL { }.GetProductById(id);
                    if (objmodel != null)
                    {
                        // model.SubCategoryId = objmodel.SubCategoryId;
                        model.ProductName = objmodel.ProductName;
                        // model.CategoryId = objmodel.CategoryId;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                        model.ProductId = objmodel.ProductId;
                        model.ProdDetails = objmodel.ProdDetails;
                        model.IsActive = Convert.ToBoolean(objmodel.IsActive);
                    }
                }
                model.IndustryList = (from Ind in objdb.Industries
                                      where Ind.IsActive == true
                                      select new IndustryModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId

                                      }).Take(7).ToList();
                model.CityList = (from ct in objdb.Cities
                                  where ct.IsActive == true
                                  select new CityModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId

                                  }).Take(7).ToList();

                var NewsList = (from UserNews in objdb.BusinessNews
                                where UserNews.IsActive == true && UserNews.CreatedBy == userid
                                select new BusinessNewsModel
                                {
                                    BusinessNewsId = UserNews.BusinessNewsId,
                                    NewsTitle = UserNews.NewsTitle,
                                    Description = UserNews.Description

                                }).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                model.NewsUpdates = NewsList;
                return View(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult UpdateProduct(ProductWebModel model, IEnumerable<HttpPostedFileBase> file)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            Random rnd = new Random();
            string strImgName = "";
            string path = "";
            try
            {
                //if (ModelState.IsValid)
                //{
                if (model.ProductId == 0)
                {
                    var checkDuplicate = objdb.Products.Where(x => x.ProductName == model.ProductName).SingleOrDefault();
                    if (checkDuplicate == null)
                    {
                        int res = new ProductBLL { }.AddEditProduct(new ProductModel
                        {
                            //SubCategoryId = model.SubCategoryId,
                            ProductName = model.ProductName,
                            ProdDetails = model.ProdDetails,
                            // CategoryId = model.CategoryId,
                            ProductId = model.ProductId,
                            IsActive = model.IsActive,
                            CreatedDate = model.CreatedDate,
                        });
                        if (res != 0)
                        {
                            foreach (var item in file)
                            {
                                if (item != null)
                                {
                                    ProductImageModel objProductImage = new ProductImageModel();
                                    string sname = item.FileName;
                                    strImgName = "ProImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                                    path = Server.MapPath("~/Images/ProductImages/" + strImgName);
                                    objProductImage.ProdImage = strImgName;
                                    objProductImage.CreatedDate = DateTime.Now;
                                    objProductImage.IsActive = true;
                                    objProductImage.ProductId = res;
                                    int result = new ProductBLL { }.InsertProductImagesBLL(objProductImage);
                                    item.SaveAs(path);
                                }
                            }
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("ProductList");
                        }
                        Session["Error"] = "An Error has occured";
                        return View(model);
                    }
                }
                else
                {
                    int data = new ProductBLL { }.AddEditProduct(new ProductModel
                    {
                        //SubCategoryId = model.SubCategoryId,
                        ProductName = model.ProductName,
                        ProdDetails = model.ProdDetails,
                        //CategoryId = model.CategoryId,
                        ProductId = model.ProductId,
                        IsActive = model.IsActive,
                        CreatedDate = model.CreatedDate,
                    });
                    if (data != 0)
                    {
                        foreach (var item in file)
                        {
                            if (item != null)
                            {
                                ProductImageModel objProductImage = new ProductImageModel();
                                string sname = item.FileName;
                                strImgName = "ProImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                                path = Server.MapPath("~/Areas/Admin/ProjectImages/ProductImage/" + strImgName);
                                objProductImage.ProdImage = strImgName;
                                objProductImage.CreatedDate = DateTime.Now;
                                objProductImage.IsActive = true;
                                objProductImage.ProductId = data;
                                int result = new ProductBLL { }.InsertProductImagesBLL(objProductImage);
                                item.SaveAs(path);
                            }
                            else
                            {
                                Session["Success"] = "Successfully Updated The Record";
                                return RedirectToAction("ProductList");
                            }
                        }
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("ProductList");
                    }
                }
                Session["Error"] = "Record Already Exists";
                return RedirectToAction("EditProduct");

            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return RedirectToAction("ProductList");
                throw;
            }
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult VisitorPage(int id, int iValSess = 0)
        {
            string ipAddress;
            Session["VisitorId"] = id;
            try
            {
                ipAddress = (new WebClient()).DownloadString("http://checkip.dyndns.org/");
                ipAddress = (new Regex(@"\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}"))
                             .Matches(ipAddress)[0].ToString();
                ViewBag.objIpAddress = new BussinessUserBLL { }.VisitcountBLL(id, ipAddress);
            }
            catch (Exception)
            {

            }

            if (iValSess == 0)
            {
                Session["ReturnMsg"] = null;
            }
            var objmodel = new BussinessUserBLL { }.GetUserById(id);
            if (objmodel != null)
            {

                List<LandingPageModel> objfriendlist = new List<LandingPageModel>();
                List<LandingPageModel> objfriendlist1 = new List<LandingPageModel>();
                try
                {
                    // USER HAVING CONTACTS
                    objfriendlist = (from bissUser in objdb.BussinessUsers
                                     join bissCon in objdb.BusinessConnections
                                     on bissUser.UserId equals bissCon.BusinessPartnerId
                                     where bissCon.UserId == id && bissCon.IsActive == true
                                     select new LandingPageModel
                                     {

                                         PartnerId = bissCon.BusinessPartnerId.HasValue ? bissCon.BusinessPartnerId.Value : 0,
                                         BusinessName = bissUser.BusinessName,
                                         EmailId = bissUser.EmailId,
                                         CompanyLogo = bissUser.CompanyLogo,
                                         BusinessDetails = bissUser.BusinessDetails

                                     }).ToList();
                    objfriendlist.ForEach(x => x.BusinessDetails = Regex.Replace(x.BusinessDetails, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                    foreach (LandingPageModel user in objfriendlist)
                    {
                        objfriendlist1.Add(user);
                    }
                }
                catch (Exception ex) { }


                _HomeModel.TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == id && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == id && ct.IsActive == true select ct.UserId).Count();

                _HomeModel.CompanyBanner = objmodel.CompanyBanner;
                _HomeModel.CompanyLogo = objmodel.CompanyLogo;
                _HomeModel.BusinessName = objmodel.BusinessName;
                _HomeModel.UserId = objmodel.UserId;
                _HomeModel.ConnectedBizList = objfriendlist1;
                _HomeModel.ContactPerson = objmodel.ContactPerson;
                _HomeModel.Regno = objmodel.CoRegNo;
                _HomeModel.Phone = objmodel.Phone;
                _HomeModel.Designation = objmodel.Designation;
                _HomeModel.Address = objmodel.CoAddress;
                _HomeModel.TradeEmailId = objmodel.TradeEmaiIId;
                _HomeModel.BusinessDetails = objmodel.BusinessDetails;
                _HomeModel.CompanyURL = objmodel.CompURL;
                

                int userid = Convert.ToInt32(Session["UserId"]);
                int F_id = objmodel.UserId;
                if (userid == id)
                {
                    ViewBag.LoggedinProfile = true;
                }

                var u = (from us in objdb.BusinessConnections.Where(p => p.UserId == userid && p.BusinessPartnerId == F_id) select us).SingleOrDefault();
                if (u == null)
                {
                    u = (from us in objdb.BusinessConnections.Where(p => p.UserId == F_id && p.BusinessPartnerId == userid) select us).SingleOrDefault();
                    if (u == null)
                    {
                        // Not in business Connection
                        ViewBag.IsConnected = 0;
                    }
                    else
                    {
                        if (u.IsActive == false && u.UserId == userid)
                        {
                            // Request sent but not accepted ----- Diplay pending and Cancel
                            ViewBag.IsConnected = 1;
                        }
                        else if (u.IsActive == false && u.BusinessPartnerId == userid)
                        {
                            // Request sent but not accepted ----- Display Accept or Reject
                            ViewBag.IsConnected = 3;
                        }
                        else
                        {
                            // User in Business Connection
                            ViewBag.IsConnected = 2;

                        }
                    }

                }
                else
                {
                    if (u.IsActive == false && u.UserId == userid)
                    {
                        // Request sent but not accepted ----- Diplay pending and Cancel
                        ViewBag.IsConnected = 1;
                    }
                    else if (u.IsActive == false && u.BusinessPartnerId == userid)
                    {
                        // Request sent but not accepted ----- Display Accept or Reject
                        ViewBag.IsConnected = 3;
                    }
                    else
                    {
                        // User in Business Connection
                        ViewBag.IsConnected = 2;

                    }

                }
                // Below code for cobranding....
                var co = (from Co in objdb.CoBrandings.Where(p => p.PartnerA == userid && p.PartnerB == F_id) select Co).SingleOrDefault();
                if (co == null)
                {
                    co = (from Co in objdb.CoBrandings.Where(p => p.PartnerA == F_id && p.PartnerB == userid) select Co).SingleOrDefault();
                    if (co == null)
                    {
                        ViewBag.IsCoBranded = 0;
                    }
                    else if (co.IsActive == false && co.PartnerA == userid)
                    {
                        // Request sent but not accepted ----- Diplay pending and Cancel
                        ViewBag.IsCoBranded = 1;
                    }
                    else if (co.IsActive == false && co.PartnerB == userid)
                    {
                        // Request sent but not accepted ----- Display Accept or Reject
                        ViewBag.IsCoBranded = 3;
                    }
                    else
                    {
                        // User in Business Connection
                        ViewBag.IsCoBranded = 2;

                    }
                }
                else
                {
                    if (co.IsActive == false && co.PartnerA == userid)
                    {
                        // Request sent but not accepted ----- Diplay pending and Cancel
                        ViewBag.IsCoBranded = 1;
                    }
                    else if (co.IsActive == false && co.PartnerB == userid)
                    {
                        // Request sent but not accepted ----- Display Accept or Reject
                        ViewBag.IsCoBranded = 3;
                    }
                    else
                    {
                        // User in Business Connection
                        ViewBag.IsCoBranded = 2;
                    }
                }

                // Checking News Validity
                NewsMapping NewMap = new NewsMapping();
                NewMap = new MembershipBLL { }.GetHomeNewsListBLL();
                var ProfileNews = new BusinessNewsBLL { }.GetBusinessNewsById(id);
                List<BusinessNewsModel> NewsList = new List<BusinessNewsModel>();
                List<BusinessNewsModel> AdminNewsList = new List<BusinessNewsModel>();



                NewsList = (from UserNews in objdb.BusinessNews
                            where UserNews.IsActive == true && UserNews.CreatedBy == id
                            select new BusinessNewsModel
                            {
                                BusinessNewsId = UserNews.BusinessNewsId,
                                NewsTitle = UserNews.NewsTitle,
                                Description = UserNews.Description

                            }).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
                _HomeModel.NewsUpdates = NewsList;
                _HomeModel.ProductsList = (from pr in objdb.Products
                                           let x = objdb.ProductImages.Where(pi => pi.ProductId == pr.ProductId).FirstOrDefault()
                                           where pr.UserId == id
                                           select new LandingPageModel
                                           {
                                               ProductId = pr.ProductId,
                                               ProductName = pr.ProductName,
                                               ProductDesc = pr.ProdDetails,
                                               UserId = pr.UserId.HasValue ? pr.UserId.Value : 0,
                                               ProductImage = x == null ? null : x.ProdImage
                                               //ProductImage =pr.ProductImages.Select(x=>x.ProdImage).FirstOrDefault()
                                           }).ToList();

                _HomeModel.BusinessJobs = (from jobs in objdb.BusinessJobs
                                           where jobs.BusinessUserId == id
                                           select new BusinessJobModel
                                           {
                                               BusinessJobId = jobs.BusinessJobId,
                                               JobTitle = jobs.JobTitle,
                                               Description = jobs.Description,
                                               BusinessUserId = jobs.BusinessUserId
                                           }).ToList();
              
                //ViewBag.MemName = (from mem in objdb.Memberships where mem.UserId == id select mem.AccessPlan.AccessPlanName).SingleOrDefault();
                //ViewBag.AcccessPlanId = (from mem in objdb.Memberships where mem.UserId == id select mem.AccessPlan.AccessPlanId).SingleOrDefault();
                ViewBag.AcccessPlanId = (from mem in objdb.Memberships where mem.UserId == id && mem.AccessPlanId == 25 select mem.AccessPlan.AccessPlanId).SingleOrDefault();
                ViewBag.MemName = (from mem in objdb.Memberships where mem.UserId == id && mem.AccessPlanId == 25 select mem.AccessPlan.AccessPlanName).SingleOrDefault();

                if (Session["EmailId"] != null)
                {
                    ViewBag.LoginEmail = Session["EmailId"].ToString();
                    ViewBag._uId = Session["UserId"].ToString();
                    
                }


                return View(_HomeModel);
            }
            return View();
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult VisitProDetail(int id)
        {
            ProductModel obj = new ProductModel();
            List<ProductImageModel> objimg = new List<ProductImageModel>();

            obj = new ProductDAL { }.GetProductById(id);
            objimg = new ProductImageDAL { }.GetProductImageById(id);
            ViewBag.objImg = objimg;
            return PartialView("_VisitProDetail", obj);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult CoBrandedUserList(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            CoBrandModel model = new CoBrandModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            model.CoBrandList = new CoBrandBLL { }.GetAllCoBrandUsersDetails();
            double count = Convert.ToDouble(new CoBrandBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.CoBrandList != null)
            {
                var objmodel = new CMSBLL { }.GetCmsById(16);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }


            }

            return View(model);
        }

        public ActionResult CoBrandRequest(int id)
        {
            string uPwd = string.Empty;
            MailMessage msg;
            string ActivationUrl = string.Empty;
            string emailId = string.Empty;
            string guid = Guid.NewGuid().ToString();
            Guid temp = Guid.Parse(guid);
            CoBrandModel objmodel = new CoBrandModel();
            int userid = Convert.ToInt32(Session["UserId"]);
            string Business = Convert.ToString(Session["BusinessName"]);

            var chckDuplicate = objdb.CoBrandings.Where(x => x.PartnerA == userid && x.PartnerB == id).SingleOrDefault();
            if (chckDuplicate == null)
            {
                chckDuplicate = objdb.CoBrandings.Where(x => x.PartnerA == id && x.PartnerB == userid).SingleOrDefault();
                if (chckDuplicate == null)
                {
                    CoBranding objCoBrand = new CoBranding();
                    objCoBrand.PartnerA = userid;
                    objCoBrand.PartnerB = id;
                    objCoBrand.CreatedOn = DateTime.Now;
                    objCoBrand.IsActive = false;
                    var cobNname = (from bu in objdb.BussinessUsers where bu.UserId == id select bu.BusinessName).FirstOrDefault();
                    objCoBrand.CoBrandedName = Business + "/" + cobNname.ToString();
                    objCoBrand.IsAprroved = false;
                    objdb.CoBrandings.Add(objCoBrand);
                    objdb.SaveChanges();

                    var getuser = new BussinessUserBLL { }.GetUserById(id);
                    #region Send Email
                    msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    emailId = getuser.EmailId;
                    //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                    //uPwd = DataEncryption.Decrypt(password, "passKey");
                    uPwd = getuser.Password.Trim();
                    //sender email address
                    msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                    //Receiver email address
                    msg.To.Add(emailId);
                    msg.Subject = "Congratulations You have Successfully Registered";
                    string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                    ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                    //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                    // ActivationUrl = Server.HtmlEncode(link);
                    //msg.Body = "Hi " + model.BusinessName + "!\n" +
                    // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                    // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                    var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(6);
                    var test = emailtemp.Description + "<br />" + ActivationUrl;
                    string mailbod = MailBody(test, getuser);
                    msg.Body = mailbod;
                    msg.IsBodyHtml = true;
                    smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                    smtp.Port = 25;
                    smtp.Host = "mail.FRMT.IN";
                    //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                    //smtp.Port = 2525;
                    //smtp.Host = "retail.smtp.com";
                    smtp.EnableSsl = false;
                    smtp.Send(msg);
                    #endregion
                    TempData["AlertMessage"] = "Co-Brand Business Request Successfully sent";
                    //Session["ReturnMsg"] = "Co Brand Request sent to Business";
                }

            }
            //return RedirectToAction("UsersProfile");
            return RedirectToAction("VisitorPage", "Home", new { @id = id, @iValSess = 1 });
        }




        public ActionResult SendRequest(int id)
        {
            string uPwd = string.Empty;
            MailMessage msg;
            string ActivationUrl = string.Empty;
            string emailId = string.Empty;
            string guid = Guid.NewGuid().ToString();
            Guid temp = Guid.Parse(guid);
            int userid = Convert.ToInt32(Session["UserId"]);
            var chckDuplicate = objdb.BusinessConnections.Where(x => x.UserId == userid && x.BusinessPartnerId == id).SingleOrDefault();

            if (chckDuplicate == null)
            {
                chckDuplicate = objdb.BusinessConnections.Where(x => x.UserId == id && x.BusinessPartnerId == userid).SingleOrDefault();
                if (chckDuplicate == null)
                {
                    BusinessConnection objbizzCon = new BusinessConnection();
                    objbizzCon.UserId = userid;
                    objbizzCon.BusinessPartnerId = id;
                    objbizzCon.CreatedDate = DateTime.Now;
                    objbizzCon.IsActive = false;
                    objdb.BusinessConnections.Add(objbizzCon);
                    objdb.SaveChanges();

                    var getuser = new BussinessUserBLL { }.GetUserById(id);
                    var getfromuserdata = new BussinessUserBLL { }.GetUserByEmailId(Convert.ToString(Session["EmailId"]));
                    
                    #region Send Email
                    msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    emailId = getuser.EmailId;
                    //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                    //uPwd = DataEncryption.Decrypt(password, "passKey");
                    uPwd = getuser.Password.Trim();
                    //sender email address
                    msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                    //Receiver email address
                    msg.To.Add(emailId);
                    msg.Subject = "Congratulations You have Successfully Registered";
                    string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                    ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                    //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                    // ActivationUrl = Server.HtmlEncode(link);
                    //msg.Body = "Hi " + model.BusinessName + "!\n" +
                    // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                    // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                    var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(5);
                    var test = emailtemp.Description + "<br />" + ActivationUrl;
                    string mailbod = MailBody(test, getuser, getfromuserdata);
                    msg.Body = mailbod;
                    msg.IsBodyHtml = true;
                    smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                    smtp.Port = 25;
                    smtp.Host = "mail.FRMT.IN";
                    smtp.EnableSsl = false;
                    smtp.Send(msg);
                    #endregion
                    TempData["AlertMessage"] = "Business Request Successfully Sent";

                }

            }
            //return RedirectToAction("UsersProfile");
            return RedirectToAction("VisitorPage", "Home", new { @id = id, @iValSess = 1 });
        }



        private string FetchUserId(string emailId)
        {
            //string UserID = Convert.ToString(cmd.ExecuteScalar());
            var userid = (from r in objdb.BussinessUsers where r.EmailId == emailId select r.UserId).FirstOrDefault();
            string UserId = Convert.ToString(userid);
            return UserId;
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult ActivateUrl(string id)
        {
            try
            {
                if (id != null)
                {
                    //userid = Convert.ToInt32(Request.QueryString["UserID"]);
                    Guid temp = Guid.Parse(guid);
                    var update = objdb.BussinessUsers.Where(x => x.GUId == id).SingleOrDefault();

                    if (update != null)
                    {
                        update.IsActive = true;
                        objdb.SaveChanges();
                        return RedirectToAction("Login", "Home");
                    }

                    return RedirectToAction("Login", "Home");
                }

            }
            catch (Exception)
            {
                
            }
            return View();
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            BussinessUserModel userDetail = new BussinessUserModel();
            return View(userDetail);
        }

        public ActionResult PopUpOnClick()
        {
            _HomeModel.PopupList = (from x in objdb.BussinessUsers
                                    select new LandingPageModel
                                    {
                                        CompanyLogo = x.CompanyLogo
                                    }).ToList();
            return View(_HomeModel);
        }

        public ActionResult ChangePassword()
        {
            BizUserLoginModel LoginDetails = new BizUserLoginModel();
            LoginDetails.Email = Convert.ToString(Session["EmailId"]);
            //int userid = Convert.ToInt32(Session["UserId"]);
            return View(LoginDetails);
        }

        [HttpPost]
        public ActionResult ChangePassword(BizUserLoginModel model)
        {
            BussinessUserModel userDetail = new BussinessUserModel();
            string emailid = Convert.ToString(Session["EmailId"]);
            int userid = Convert.ToInt32(Session["UserId"]);
            //string password = Convert.ToString(Session["Password"]);
            string uPwd = string.Empty;
            //string msg = Convert.ToString(TempData["msg"]);
            var User = (from user in objdb.BussinessUsers
                        where user.EmailId == emailid && user.IsActive == true
                        select user).FirstOrDefault();
            if (User != null)
            {
                //string spassword = Convert.ToString(Session["Password"]);
                string spassword = DataEncryption.Decrypt(User.Password.Trim(), "passKey");
                if (model.Password == spassword)
                {
                    User.UserId = userid;
                    uPwd = DataEncryption.Encrypt(model.NewPassword.Trim(), "passKey");
                    User.Password = uPwd;
                    objdb.SaveChanges();
                    TempData["AlertMessage"] = "Password Successfully Changed";
                    return RedirectToAction("Login", "Home");
                }
                return View(userDetail);
            }
            else
            {
                return RedirectToAction("Login", "Home", TempData["AlertMessage"] = "Try after Some Time");
            }
        }

        [HttpPost]
        [CustumAuthorize.AllowAnonymous]
        public ActionResult ForgotPassword(BussinessUserModel usermodel)
        {

            MailMessage msg;
            string ActivationUrl = string.Empty;
            string emailId = string.Empty;
            string uPwd = string.Empty;
            string checkemail = (from chk in objdb.BussinessUsers where chk.EmailId == usermodel.EmailId select chk.EmailId).FirstOrDefault();
            if (checkemail != null)
            {
                var getuser = new BussinessUserBLL { }.GetUserByEmailId(checkemail);
                string paswd = DataEncryption.Decrypt(getuser.Password, "passKey");
                getuser.Password = paswd;
                msg = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                emailId = usermodel.EmailId;
                string password = (from userpasswrd in objdb.BussinessUsers where userpasswrd.EmailId == emailId select userpasswrd.Password).FirstOrDefault();
                uPwd = DataEncryption.Decrypt(password, "passKey");
                //sender email address
                msg.From = new MailAddress("no_reply@thebusinessbranding.com");//amit.arora.rocks72@gmail.com
                //Receiver email address
                msg.To.Add(emailId);
                msg.Subject = "Forgot Password Recovery";
                ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/Login");
                var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(2);
                var test = emailtemp.Description + "<br />" + ActivationUrl;
                string mailbod = MailBody(test, getuser);
                msg.Body = mailbod;
                //msg.Body = "your emailid:  " + emailId + "<br>your Password:  " + uPwd + "\n" +
                //  " Please <a href='" + ActivationUrl + "'>click here to Login</a>";
                msg.IsBodyHtml = true;
                smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                smtp.Port = 2525;
                smtp.Host = "retail.smtp.com";
                smtp.EnableSsl = false;
                smtp.Send(msg);
                TempData["AlertMessage"] = "Password Sent to the Registered EmailID.";
                return RedirectToAction("Login", "Home");
            }
            TempData["AlertMessage"] = "EmailId Doesn't Exist";
            // return View(usermodel);
            // ViewBag.Msg = "An email has been sent to your account with login details";
            return View();
        }

        public ActionResult Verify(string ID)
        {
            if (string.IsNullOrEmpty(ID) || (!Regex.IsMatch(ID, @"[0-9a-f]{8}\-([0-9a-f]{4}\-){3}[0-9a-f]{12}")))
            {
                TempData["tempMessage"] = "The user account is not valid. Please try clicking the link in your email again.";
                return View();
            }

            else
            {

                int userid = Convert.ToInt32(Request.QueryString["UserID"]);
                Guid temp = Guid.Parse(guid);
                var update = objdb.BussinessUsers.Single(x => x.UserId == userid && x.GUId == guid);
                objdb.SaveChanges();
                return RedirectToAction("Index", "Home");

            }
        }

        public ActionResult PendingBizzRequest()
        {
            int uid = Convert.ToInt32(Session["userid"]);
            ViewData["Userdetails"] = "";
            ViewData["UserName"] = "";
            try
            {
                var userdetails = objdb.BussinessUsers.Where(p => p.UserId == uid).SingleOrDefault();
                ViewData["Userdetails"] = userdetails;
                // ViewData["UserName"] = userdetails.FirstName;
            }
            catch (Exception ex) { }

            List<FriendListEntity> objfriendlistpending = new List<FriendListEntity>();
            ViewData["PendingFriendlist"] = objfriendlistpending;
            try
            {
                objfriendlistpending = (from u in objdb.BussinessUsers
                                        join f in objdb.BusinessConnections
                                        on u.UserId equals f.UserId
                                        where f.IsActive == false && f.BusinessPartnerId == uid
                                        select new FriendListEntity
                                        {
                                            Username = u.BusinessName,
                                            Image_Name = u.CompanyLogo,
                                            Fid = f.BusinessPartnerId,
                                            Uid = u.UserId,
                                            UserFriendid = f.Id,
                                            //Usertype = u.UserTypeID
                                        }).ToList();
                ViewData["PendingFriendlist"] = objfriendlistpending;
            }
            catch (Exception ex) { }

            return View();
        }

        public ActionResult AddRequestedFriends(PersonViewModel fid)
        {
            // int fid = Convert.ToInt32(Obj.Message.ToString());
            int friendid = Convert.ToInt32(Request["fid"]);
            // Sitesettingdetails(); popularserch();
            if (HttpContext.Session["userid"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            int uid = Convert.ToInt32(Session["userid"]);
            try
            {
                var objuser = (from f in objdb.BusinessConnections where f.UserId == friendid && f.BusinessPartnerId == uid select f).Single();
                if (objuser.Id > 0)
                {
                    objuser.IsActive = true;
                    objdb.SaveChanges();
                }
            }
            catch (Exception ex) { }

            List<FriendListEntity> objfriendlistpending = new List<FriendListEntity>();
            ViewData["PendingFriendlist"] = objfriendlistpending;
            try
            {
                objfriendlistpending = (from u in objdb.BussinessUsers
                                        join f in objdb.BusinessConnections
                                        on u.UserId equals f.UserId
                                        where f.IsActive == false && f.BusinessPartnerId == uid
                                        select new FriendListEntity
                                        {
                                            Username = u.BusinessName,
                                            Image_Name = u.CompanyLogo,
                                            Fid = f.BusinessPartnerId,
                                            Uid = u.UserId,
                                            UserFriendid = f.Id,
                                            //Usertype = u.UserTypeID
                                        }).ToList();
                ViewData["PendingFriendlist"] = objfriendlistpending;
            }
            catch (Exception ex) { }

            return View();
        }

        public ActionResult AcceptRequest(int id)
        {
            int uid = Convert.ToInt32(Session["userid"]);
            try
            {
                //Chect Request Already Sent...
                var Req = objdb.BusinessConnections.Where(x => x.UserId == id && x.BusinessPartnerId == uid).SingleOrDefault();
                if (Req != null)
                {
                    Req.IsActive = true;
                    objdb.SaveChanges();
                    TempData["AlertMessage"] = "Business Request Accepted ";
                    //Session["ReturnMsg"] = "You are now connected to Business";
                }

            }
            catch (Exception ex) { }
            return RedirectToAction("UsersProfile");
            //return RedirectToAction("VisitorPage", "Home", new { @id = id, @iValSess = 1 });
        }

        public ActionResult AcceptCoBrandRequest(int id)
        {
            int uid = Convert.ToInt32(Session["userid"]);
            var AdminMail = new AdministratorBLL { }.GetAdministratorById(11);
            try
            {
                //Check Request Already Sent...
                var CoBrandReq = objdb.CoBrandings.Where(x => x.PartnerA == id && x.PartnerB == uid).SingleOrDefault();
                if (CoBrandReq != null)
                {
                    CoBrandReq.IsActive = true;
                    objdb.SaveChanges();

                    var getuser = new BussinessUserBLL { }.GetUserById(id);
                    #region Send Email
                    msg = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    emailId = AdminMail.Email;
                    //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                    //uPwd = DataEncryption.Decrypt(password, "passKey");
                    uPwd = getuser.Password.Trim();
                    //sender email address
                    msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                    //Receiver email address
                    msg.To.Add(emailId);
                    msg.Subject = "Congratulations You have Successfully Registered";
                    string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                    ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                    //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                    // ActivationUrl = Server.HtmlEncode(link);
                    //msg.Body = "Hi " + model.BusinessName + "!\n" +
                    // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                    // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                    var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(6);
                    var test = emailtemp.Description + "<br />" + ActivationUrl;
                    string mailbod = MailBody(test, getuser);
                    msg.Body = mailbod;
                    msg.IsBodyHtml = true;
                    smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                    smtp.Port = 2525;
                    smtp.Host = "retail.smtp.com";
                    smtp.EnableSsl = false;
                    smtp.Send(msg);
                    #endregion
                    TempData["AlertMessage"] = "Co-Brand Business Request Accepted ";


                }

            }
            catch (Exception ex) { }
            return RedirectToAction("UsersProfile");
            //return RedirectToAction("UsersProfile", "Home", new { @id = uid }); //, @iValSess = 1 }
        }

        public ActionResult RejectCoBrandRequest(int id)
        {
            int uid = Convert.ToInt32(Session["userid"]);
            try
            {
                //Chect Request Already Sent...
                var Req = (from re in objdb.CoBrandings where re.PartnerB == id && re.PartnerA == uid || re.PartnerB == uid && re.PartnerA == id select re).SingleOrDefault();
                if (Req != null)
                {

                    var RecExist = (from cbi in objdb.CoBrandingImages where cbi.CoBrandingId == Req.CoBrandingId select cbi).SingleOrDefault();
                    if (RecExist != null)
                    {
                        objdb.CoBrandingImages.Remove(RecExist);
                    }
                    objdb.CoBrandings.Remove(Req);
                    objdb.SaveChanges();
                    TempData["AlertMessage"] = "Co-Brand Business Request Rejected ";
                    // Session["ReturnMsg"] = "You have rejected Co-Branded Request";
                }


            }
            catch (Exception ex) { }
            //return RedirectToAction("UsersProfile");
            return RedirectToAction("UsersProfile", "Home", new { @id = uid, @iValSess = 1 });
        }

        public ActionResult RejectRequest(int id)
        {
            int uid = Convert.ToInt32(Session["userid"]);
            try
            {
                //Chect Request Already Sent...
                var Req = (from re in objdb.BusinessConnections where re.UserId == id && re.BusinessPartnerId == uid select re).SingleOrDefault();
                if (Req != null)
                {
                    objdb.BusinessConnections.Remove(Req);
                    objdb.SaveChanges();
                    TempData["AlertMessage"] = "Business Request Rejected ";
                    // Session["ReturnMsg"] = "You have rejectet Business Request";
                }
                var chkDupReq = (from re in objdb.BusinessConnections where re.BusinessPartnerId == id && re.UserId == uid select re).SingleOrDefault();
                if (chkDupReq != null)
                {
                    objdb.BusinessConnections.Remove(chkDupReq);
                    objdb.SaveChanges();
                    TempData["AlertMessage"] = "Business Request Rejected ";
                    //Session["ReturnMsg"] = "You have rejected Business Request";
                }

            }
            catch (Exception ex) { }
            //return RedirectToAction("UsersProfile");
            return RedirectToAction("UsersProfile", "Home", new { @id = id, @iValSess = 1 });
        }

        public ActionResult CancelRequest(int id)
        {
            int uid = Convert.ToInt32(Session["userid"]);
            try
            {
                //Chect Request Already Sent...
                var Req = (from re in objdb.BusinessConnections where re.UserId == uid && re.BusinessPartnerId == id select re).SingleOrDefault();
                if (Req != null)
                {
                    objdb.BusinessConnections.Remove(Req);
                    objdb.SaveChanges();
                    TempData["AlertMessage"] = "Business Request Cancelled ";
                    //Session["ReturnMsg"] = "You have cancel sent Request to Business";
                }

            }
            catch (Exception ex) { }
            //return RedirectToAction("UsersProfile");
            return RedirectToAction("VisitorPage", "Home", new { @id = id, @iValSess = 1 });
        }

        public ActionResult CancelCoRequest(int id)
        {
            int uid = Convert.ToInt32(Session["userid"]);
            try
            {
                //Chect Request Already Sent...
                var CoReq = (from re in objdb.CoBrandings where re.PartnerA == uid && re.PartnerB == id select re).SingleOrDefault();
                if (CoReq != null)
                {
                    objdb.CoBrandings.Remove(CoReq);
                    objdb.SaveChanges();
                    TempData["AlertMessage"] = "Co-Brand Business Request Cancelled ";
                    //Session["ReturnMsg"] = "You have cancel sent Co-Brand Request to Business";
                }

            }
            catch (Exception ex) { }
            //return RedirectToAction("UsersProfile");
            return RedirectToAction("VisitorPage", "Home", new { @id = id, @iValSess = 1 });
        }

        public ActionResult Logout()
        {
            try
            {
                Response.Buffer = true;
                // Response.ExiresAbsolute = DateTime.Now.AddDays(-1d);
                Response.Expires = -1500;
                Response.CacheControl = "no-cache";
                // Response.Cache.SetNotStore(); 
                Session.Abandon();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception)
            {

                throw;
            }
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult Searching(string searchValue)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            SearchwordsModel searchObj = new SearcbyKeywordBLL { }.Search(searchValue);

            searchObj.IndustryList = (from Ind in objdb.Industries
                                      where Ind.IsActive == true
                                      select new SearchwordsModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId
                                      }).ToList();

            searchObj.CityList = (from ct in objdb.Cities
                                  where ct.IsActive == true
                                  select new SearchwordsModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId
                                  }).ToList();
            ViewBag.SearchKey = searchValue;


            NewsMapping NewMap = new NewsMapping();
            NewMap = new MembershipBLL { }.GetHomeNewsListBLL();
            List<BusinessNewsModel> NewsList = new List<BusinessNewsModel>();
            List<BusinessNewsModel> AdminNewsList = new List<BusinessNewsModel>();

            if (NewMap != null)
            {
                NewsList = (from UserNews in objdb.BusinessNews
                            where UserNews.IsActive == true && UserNews.CreatedBy == NewMap.UserId
                            select new BusinessNewsModel
                            {
                                BusinessNewsId = UserNews.BusinessNewsId,
                                NewsTitle = UserNews.NewsTitle,
                                Description = UserNews.Description

                            }).OrderByDescending(x => x.BusinessNewsId).Take(2).ToList();
                NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));


            }
            AdminNewsList = (from ns in objdb.News
                             where ns.IsActive == true
                             select new BusinessNewsModel
                             {
                                 BusinessNewsId = ns.NewsId,
                                 NewsTitle = ns.NewsTitle,
                                 Description = ns.Description

                             }).OrderByDescending(x => x.BusinessNewsId).Take(5).ToList();
            AdminNewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));

            foreach (var item in NewsList)
            {
                AdminNewsList.Add(item);
            }
            searchObj.NewsUpdates = AdminNewsList;
            return View(searchObj);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult SearchbyIndustry(int id)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            SearchwordsModel searchObj = new SearcbyKeywordBLL { }.SearchbyIndustry(id);

            searchObj.IndustryList = (from Ind in objdb.Industries
                                      //where Ind.IsActive == true
                                      select new SearchwordsModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId

                                      }).ToList();
            searchObj.CityList = (from ct in objdb.Cities
                                  //where ct.IsActive == true
                                  select new SearchwordsModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId

                                  }).ToList();

            var NewsList = (from UserNews in objdb.BusinessNews
                            where UserNews.IsActive == true && UserNews.CreatedBy == userid
                            select new BusinessNewsModel
                            {
                                BusinessNewsId = UserNews.BusinessNewsId,
                                NewsTitle = UserNews.NewsTitle,
                                Description = UserNews.Description

                            }).ToList();
            NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
            searchObj.NewsUpdates = NewsList;
            string IndustryName = new SearcbyKeywordBLL { }.IndustrySearch(id);
            ViewBag.SearchKey = IndustryName;
            return PartialView("Searching", searchObj);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult SearchByCity(int CityId)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            SearchwordsModel Objsearch = new SearcbyKeywordBLL { }.SearchCity(CityId);
            Objsearch.IndustryList = (from Ind in objdb.Industries
                                      where Ind.IsActive == true
                                      select new SearchwordsModel
                                      {
                                          IndustryName = Ind.IndustryName,
                                          IndustryId = Ind.IndustryId

                                      }).ToList();
            Objsearch.CityList = (from ct in objdb.Cities
                                  where ct.IsActive == true
                                  select new SearchwordsModel
                                  {
                                      CityName = ct.CityName,
                                      CityId = ct.CityId

                                  }).ToList();
            var NewsList = (from UserNews in objdb.BusinessNews
                            where UserNews.IsActive == true && UserNews.CreatedBy == userid
                            select new BusinessNewsModel
                            {
                                BusinessNewsId = UserNews.BusinessNewsId,
                                NewsTitle = UserNews.NewsTitle,
                                Description = UserNews.Description

                            }).ToList();
            NewsList.ForEach(s => s.Description = Regex.Replace(s.Description, @"<[^>]+>|&nbsp;|&quot;", String.Empty));
            Objsearch.NewsUpdates = NewsList;
            string CityName = new SearcbyKeywordBLL { }.CitySearch(CityId);
            ViewBag.SearchKey = CityName;
            return View("Searching", Objsearch);
        }

        public interface IFormsAuthenticationService
        {
            void SignIn(string userName, bool createPersistentCookie);
            void SignOut();
        }

        public class FormsAuthenticationService : IFormsAuthenticationService
        {
            public void SignIn(string userName, bool createPersistentCookie)
            {
                if (String.IsNullOrEmpty(userName)) throw new ArgumentException("Value cannot be null or empty.", "userName");

                FormsAuthentication.SetAuthCookie(userName, createPersistentCookie);
            }

            public void SignOut()
            {
                FormsAuthentication.SignOut();
            }
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult AddProduct()
        {
            ProductModel model = new ProductModel();

            return View(model);

        }


        [HttpPost]
        public ActionResult AddProduct(ProductModel model, IEnumerable<HttpPostedFileBase> imgFile)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            Random rnd = new Random();
            string path = "";
            model.UserId = userid;
            int resutlId = new ProductBLL { }.AddProductBLL(model);
            if (resutlId > 0)
            {
                foreach (var item in imgFile)
                {
                    if (item != null)
                    {
                        ProductImageModel objProductImage = new ProductImageModel();
                        string sname = item.FileName;
                        string strImgName = "ProImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                        path = Server.MapPath("~/Areas/Admin/ProjectImages/ProductImage/" + strImgName);
                        objProductImage.ProdImage = strImgName;
                        objProductImage.CreatedDate = DateTime.Now;
                        objProductImage.IsActive = true;
                        objProductImage.ProductId = resutlId;
                        int result = new ProductBLL { }.InsertProductImagesBLL(objProductImage);
                        item.SaveAs(path);
                        TempData["AlertMessage"] = "Successfully Product Added";
                        return RedirectToAction("UsersProfile", "Home");
                    }
                }
            }
            TempData["AlertMessage"] = "Try Again after some time";
            return RedirectToAction("UsersProfile", "Home");
        }

       // [CustumAuthorize.AllowAnonymous]
        public ActionResult AddJob()
        {
            if (HttpContext.Session["UserId"] != null)
            {
                int userid = Convert.ToInt32(Session["UserId"]);
                MembershipWithAccessPlanModel res = new MembershipBLL { }.CheckMembership(userid);


                BusinessJobModel model = new BusinessJobModel();
                return View();
                //return PartialView("_AddJob", model);
            }
            else
            {
                BizUserLoginModel LogModel = new BizUserLoginModel();
                //return PartialView("_SessionExpire", LogModel);
                return RedirectToAction("Login", "Home");
            }

        }

        [HttpPost]
        public ActionResult AddJob(BusinessJobModel model)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            Random rnd = new Random();
            model.BusinessUserId = userid;
            int resutlId = new BusinessJobBLL { }.AddBusinessJob(model);
            if (resutlId > 0)
            {
                TempData["AlertMessage"] = "Successfully Posted Job to your visited page";
                return RedirectToAction("UsersProfile", "Home");
                
            }
            TempData["AlertMessage"] = "Try Again after some time";
            return RedirectToAction("UsersProfile", "Home");
        }

        public ActionResult ManageJobs(int pid = 0)
        {
            BusinessJobModel objmodel = new BusinessJobModel();
            int take = 10;
            int skip = take * pid;
            int LoginUserId = Convert.ToInt32(Session["UserId"]);
            var JobList = new BusinessJobBLL { }.GetAllJobs(skip,take,LoginUserId);
            if (JobList!=null)
            {
                objmodel.BusinessJobList = JobList.Where(x => x.BusinessUserId == LoginUserId).Select(x=> new BusinessJobModel
                    {
                        BusinessJobId=x.BusinessJobId,
                        BusinessUserId=x.BusinessUserId,
                        JobTitle=x.JobTitle,
                        Description=x.Description,
                        IsActive=x.IsActive
                    }).ToList();
                return View(objmodel);
            }
            return View();
        }

        public ActionResult UpdateJob(int id = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            try
            {
                BusinessJobModel obj = new BusinessJobModel();
                if (id!= 0)
                {
                    var getJobdata = new BusinessJobBLL { }.GetJobDetailsById(id);
                    if (getJobdata!=null)
                    {
                        obj.BusinessJobId = getJobdata.BusinessJobId;
                        obj.JobTitle = getJobdata.JobTitle;
                        obj.Description = getJobdata.Description;
                        obj.IsActive = getJobdata.IsActive;
                    }
                }
                return View(obj);
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateJob(BusinessJobModel model)
        {
            try
            {
                int userid = Convert.ToInt32(Session["UserId"]);
                if (ModelState.IsValid)
                {
                    if (model.BusinessJobId==0)
                    {
                        return RedirectToAction("AddJob", "Home");
                    }
                    else
                    {
                        int res = new BusinessJobBLL { }.AddEditBusinessJob(new BusinessJobModel
                        {
                            BusinessJobId=model.BusinessJobId,
                            BusinessUserId=userid,
                            JobTitle=model.JobTitle,
                            Description=model.Description,
                            IsActive=model.IsActive
                        });
                        if (res!=0)
                        {
                            TempData["AlertMessage"] = "Successfully Job Added";
                            return RedirectToAction("ManageJobs", "Home");
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
            return View();
        }

        public ActionResult RemoveBusinessJobs(int id)
        {
            var User = new BusinessJobBLL { }.RemoveBusinessJob(id);
            if (User != 0)
            {
                return RedirectToAction("BusinessNewsList");
            }
            return RedirectToAction("BusinessNewsList");
        }

        //[CustumAuthorize.AllowAnonymous]
        //public ActionResult CorporateBrandList()
        //{
        //    _HomeModel.PremiumBottom = (from mem in objdb.Memberships
        //                               where mem.AccessPlanId == 20 && mem.BussinessUser.IsActive == true && mem.ExpiresOn > DateTime.Now
        //                               select new LandingPageModel
        //                               {
        //                                   // BussinessConnectionId=bu.Id,
        //                                   TotalConnectCount = (from mo in objdb.BusinessConnections where mo.UserId == mem.UserId && mo.IsActive == true select mo.UserId).Count() + (from ct in objdb.BusinessConnections where ct.BusinessPartnerId == mem.UserId && ct.IsActive == true select ct.UserId).Count(),
        //                                   CreatedDate = mem.BussinessUser.CreatedDate,
        //                                   UserId = mem.BussinessUser.UserId,
        //                                   CompanyLogo = mem.BussinessUser.CompanyLogo,
        //                                   BusinessName = mem.BussinessUser.BusinessName,
        //                                   // PartnerId = mem.BusinessPartnerId
        //                               }).OrderByDescending(x => x.TotalConnectCount).Take(9).ToList().GroupBy(p => p.UserId).Select(f => f.First()).ToList();
        //    var objmodel = new CMSBLL { }.GetCmsById(10);
        //    if (objmodel != null)
        //    {
        //        _HomeModel.CMSTitle = objmodel.CMSTitle;
        //        _HomeModel.Description = objmodel.Description;
        //    }
        //    //var objmodel = new CMSBLL { }.GetCmsById(id);
        //    //if (objmodel != null)
        //    //{
        //    //    _HomeModel.CMSTitle = objmodel.CMSTitle;
        //    //    _HomeModel.Description = objmodel.Description;
        //    //}
        //    return View(_HomeModel);
        //}


        [CustumAuthorize.AllowAnonymous]
        public ActionResult CorporateBrandList(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            CorporateBrandingModel model = new CorporateBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            model.CorporateBrandingList = new CorporateBrandingBLL { }.GetAllCorporateBrandingList();
            double count = Convert.ToDouble(new InvestorPartnerBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.CorporateBrandingList != null)
            {

                var objmodel = new CMSBLL { }.GetCmsById(10);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }
            }

            return View(model);
        }

        public ActionResult EmpBranding()
        {
            EmpBrandingModel model = new EmpBrandingModel();
            int userid = Convert.ToInt32(Session["UserId"]);
            model.EmployerId = userid;
            CustomMethods.BindIndustry(model);
            CustomMethods.BindCityForUser(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult EmpBranding(LandingPageModel objmodel, HttpPostedFileBase file)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var checkduplicate = objdb.EmployeeBrandings.Where(x => x.EmpName == objmodel.EmployeeName).Select(x =>
                new EmpBrandingModel { EmployerId = x.EmployerId, EmpName = x.EmpName }).SingleOrDefault();
            if (checkduplicate == null)
            {
                try
                {
                    int res = 0;
                    string strFileName = "";
                    string path = "";
                    Random rnd = new Random();
                    if (file != null)
                    {
                        strFileName = "EmployeeBrandingImg_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                        path = Server.MapPath("~/Images/EmployeeBrandingImages/" + strFileName);
                        res = new EmpBrandingBLL { }.AddEditEmpBranding(new EmpBrandingModel
                        {
                            EmpName = objmodel.EmployeeName,
                            EmployerId = userid,
                            EmpImage = strFileName,
                            Message = objmodel.EmpBrandMessage,
                            CreatedBy = userid,
                            CreateOn = DateTime.Now,
                            IsActive = true,
                        });
                        file.SaveAs(path);

                        if (res != 0)
                        {
                            iVal = 1;
                            TempData["success"] = "EmpBranding";

                            return RedirectToAction("UsersProfile", "Home");
                        }

                    }
                    else
                    {
                        res = new EmpBrandingBLL { }.AddEditEmpBranding(new EmpBrandingModel
                        {
                            EmpName = objmodel.EmployeeName,
                            EmployerId = userid,
                            EmpImage = strFileName,
                            Message = objmodel.EmpBrandMessage,
                            CreatedBy = userid,
                            CreateOn = DateTime.Now,
                            IsActive = true,
                        });
                        file.SaveAs(path);
                        if (res != 0)
                        {
                            iVal = 1;
                            TempData["success"] = "EmpBranding";
                            // ViewBag.Msg = "Successfully Registered Check your Email to Activate your account";
                            // Session["Success"] = "Successfully Added The User";
                            return RedirectToAction("UsersProfile", "Home");
                        }
                    }
                    TempData["Error"] = true;
                    //Session["Error"] = "Please try again after some time.";
                    return RedirectToAction("UsersProfile", "Home");

                }
                catch (Exception)
                {
                    return RedirectToAction("UsersProfile", "Home");
                    throw;
                }
            }
            TempData["Error"] = false;
            return RedirectToAction("UsersProfile", "Home");
            // return View();
        }

        public ActionResult TargetBranding()
        {
            TargetBrandingModel model = new TargetBrandingModel();
            int userid = Convert.ToInt32(Session["UserId"]);
            model.BusinessUserId = userid;
            CustomMethods.BindIndustry(model);
            CustomMethods.BindCityForUser(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult TargetBranding(TargetBrandingModel objmodel, IEnumerable<HttpPostedFileBase> file)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var getuser = new BussinessUserBLL { }.GetUserById(userid);
            string emailid = Convert.ToString(Session["EmailId"]);
            var AdminMail = new AdministratorBLL { }.GetAdministratorById(11);
            var checkduplicate = objdb.TargetBrandings.Where(x => x.BusinessName == objmodel.BusinessName).Select(x =>
                new TargetBrandingModel { BusinessUserId = x.CreatedUserId, BusinessName = x.BusinessName }).SingleOrDefault();
            if (checkduplicate == null)
            {
                try
                {
                    int res = 0;
                    string strImgName = "";
                    string path = "";
                    Random rnd = new Random();

                        res = new TargerBrandBLL { }.AddEditTargetBranding(new TargetBrandingModel
                        {
                            BusinessName = objmodel.BusinessName,
                            BusinessUserId = userid,
                            //Image = strImgName,
                            CreatedDate = DateTime.Now,
                            CityId = objmodel.CityId,
                            IndustryId = objmodel.IndustryId,
                            IsActive = false,
                            No_Month = 1,
                            URL = objmodel.URL

                        });
                        if (res != 0)
                        {
                            foreach (var item in file)
                            {
                                if (item != null)
                                {
                                    TargetImageModel objImage = new TargetImageModel();
                                    string sname = item.FileName;
                                    strImgName = "TargetAdImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                                    path = Server.MapPath("~/Images/TargetBrandingImages/" + strImgName);
                                    objImage.Image = strImgName;
                                    objImage.CreatedDate = objmodel.CreatedDate;
                                    objImage.IsActive = true;
                                    objImage.TargetBrandingId = res;
                                    int result = new TargetImageBLL { }.InsertTargetAdImagesBLL(objImage);
                                    item.SaveAs(path);
                                }
                            }
                            iVal = 1;
                           

                            #region Send Email
                            msg = new MailMessage();
                            SmtpClient smtp = new SmtpClient();
                            emailId = AdminMail.Email;
                            //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                            //uPwd = DataEncryption.Decrypt(password, "passKey");
                            //uPwd = model.Password.Trim();
                            //sender email address
                            msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                            //Receiver email address
                            msg.To.Add(emailId);
                            msg.Subject = "Target Branding Request";
                            //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                            //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                            //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                            // ActivationUrl = Server.HtmlEncode(link);
                            //msg.Body = "Hi " + model.BusinessName + "!\n" +
                            // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                            // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                            var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(7);
                            var test = emailtemp.Description + "<br />" + ActivationUrl;
                            string mailbod = MailBody(test, getuser);
                            msg.Body = mailbod;
                            msg.IsBodyHtml = true;
                            //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                            //smtp.Port = 2525;
                            //smtp.Host = "retail.smtp.com";
                            smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                            smtp.Port = 25;
                            smtp.Host = "mail.FRMT.IN";
                            smtp.EnableSsl = false;
                            smtp.Send(msg);
                            #endregion

                            TempData["AlertMessage"] = "Target Branding Request successfully Sent";
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("UsersProfile", "Home");
                        }
                    //}
                    else
                    {
                        res = new TargerBrandBLL { }.AddEditTargetBranding(new TargetBrandingModel
                        {
                            BusinessName = objmodel.BusinessName,
                            BusinessUserId = userid,
                            Image = strImgName,
                            UpdateDate = DateTime.Now,
                            IndustryId = objmodel.IndustryId,
                            CityId = objmodel.CityId,
                            IsActive = false,
                        });
                        //file.SaveAs(path);
                        if (res != 0)
                        {
                            #region Send Email
                            msg = new MailMessage();
                            SmtpClient smtp = new SmtpClient();
                            emailId = AdminMail.Email;
                            //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                            //uPwd = DataEncryption.Decrypt(password, "passKey");
                            //uPwd = model.Password.Trim();
                            //sender email address
                            msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                            //Receiver email address
                            msg.To.Add(emailId);
                            msg.Subject = "Target Branding Request";
                            //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                            //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                            //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                            // ActivationUrl = Server.HtmlEncode(link);
                            //msg.Body = "Hi " + model.BusinessName + "!\n" +
                            // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                            // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                            var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(7);
                            var test = emailtemp.Description + "<br />" + ActivationUrl;
                            string mailbod = MailBody(test, getuser);
                            msg.Body = mailbod;
                            msg.IsBodyHtml = true;
                            smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                            smtp.Port = 2525;
                            smtp.Host = "retail.smtp.com";
                            smtp.EnableSsl = false;
                            smtp.Send(msg);
                            #endregion
                            iVal = 1;
                            TempData["AlertMessage"] = "Request Sent Successfully to Admin";

                            return RedirectToAction("UsersProfile", "Home");
                        }
                    }
                    TempData["Error"] = true;
                    //Session["Error"] = "Please try again after some time.";
                    return RedirectToAction("UsersProfile", "Home");

                }
                catch (Exception)
                {
                    return RedirectToAction("UsersProfile", "Home");
                    throw;
                }
            }
            TempData["Error"] = false;
            return RedirectToAction("UsersProfile", "Home");
            // return View();
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult EmpBrandingList(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            EmpBrandingModel model = new EmpBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            model.EmpBrandingList = new EmpBrandingBLL { }.GetAllEmpBrandDetails();
            double count = Convert.ToDouble(new EmpBrandingBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.EmpBrandingList != null)
            {
                var objmodel = new CMSBLL { }.GetCmsById(13);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }

            }

            return View(model);
        }

        public ActionResult FranchiseRequest()
        {
            FranchiseModel model = new FranchiseModel();
            int userid = Convert.ToInt32(Session["UserId"]);
            return View();
        }

        [HttpPost]
        public ActionResult FranchiseRequest(FranchiseModel objmodel)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var getuser = new BussinessUserBLL { }.GetUserById(userid);
            var getAdminId = new AdministratorBLL { }.GetAdministratorById(11);
            var checkduplicate = objdb.Franchises.Where(x => x.BusinessId == objmodel.BusinessId).Select(x =>
                new FranchiseModel { BusinessId = x.BusinessId, Details = x.Details }).SingleOrDefault();
            if (checkduplicate == null)
            {
                try
                {
                    int res = 0;
                    res = new FranchiseBLL { }.AddEditFranchise(new FranchiseModel
                    {
                        BusinessId = userid,
                        Details = objmodel.Details,
                        CreatedOn = DateTime.Now,
                        IsActive = false,
                    });
                    if (res != 0)
                    {
                        #region Send Email
                        msg = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        emailId = getAdminId.Email;
                        //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                        //uPwd = DataEncryption.Decrypt(password, "passKey");
                        //uPwd = model.Password.Trim();
                        //sender email address
                        msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                        //Receiver email address
                        msg.To.Add(emailId);
                        msg.Subject = "Franchise Request";
                        //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                        //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                        //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                        // ActivationUrl = Server.HtmlEncode(link);
                        //msg.Body = "Hi " + model.BusinessName + "!\n" +
                        // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                        // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                        var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(8);
                        var test = emailtemp.Description + "<br />" + ActivationUrl;
                        string mailbod = MailBody(test, getuser);
                        msg.Body = mailbod;
                        msg.IsBodyHtml = true;
                        //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                        //smtp.Port = 2525;
                        //smtp.Host = "retail.smtp.com";
                        smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                        smtp.Port = 25;
                        smtp.Host = "mail.FRMT.IN";

                        smtp.EnableSsl = false;
                        smtp.Send(msg);
                        #endregion
                        iVal = 1;
                      
                        TempData["success"] = "FranchiseRequest";

                        TempData["AlertMessage"] = "Franchise Request successfully sent";

                        return RedirectToAction("UsersProfile", "Home");
                    }
                    else
                    {
                        res = new FranchiseBLL { }.AddEditFranchise(new FranchiseModel
                        {
                            BusinessId = userid,
                            //FranchiseId = objmodel.BusinessId,
                            Details = objmodel.Details,
                            IsActive = false,
                        });
                        if (res != 0)
                        {
                            #region Send Email
                            msg = new MailMessage();
                            SmtpClient smtp = new SmtpClient();
                            emailId = getAdminId.Email;
                            //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                            //uPwd = DataEncryption.Decrypt(password, "passKey");
                            //uPwd = model.Password.Trim();
                            //sender email address
                            msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                            //Receiver email address
                            msg.To.Add(emailId);
                            msg.Subject = "Franchise Request";
                            //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                            //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                            //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                            // ActivationUrl = Server.HtmlEncode(link);
                            //msg.Body = "Hi " + model.BusinessName + "!\n" +
                            // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                            // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                            var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(8);
                            var test = emailtemp.Description + "<br />" + ActivationUrl;
                            string mailbod = MailBody(test, getuser);
                            msg.Body = mailbod;
                            msg.IsBodyHtml = true;
                            //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                            //smtp.Port = 2525;
                            //smtp.Host = "retail.smtp.com";
                            smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                            smtp.Port = 25;
                            smtp.Host = "mail.FRMT.IN";

                            smtp.EnableSsl = false;
                            smtp.Send(msg);
                            #endregion
                            iVal = 1;
                          
                            TempData["success"] = "FranchiseRequest";

                            TempData["AlertMessage"] = "Franchise Request successfully sent";
                            return RedirectToAction("UsersProfile", "Home");
                        }
                    }
                    TempData["Error"] = true;

                    return RedirectToAction("UsersProfile", "Home");

                }
                catch (Exception)
                {
                    return RedirectToAction("UsersProfile", "Home");
                    throw;
                }
            }

            return View();
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult FranchiserList(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            FranchiseModel model = new FranchiseModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            model.FranchiseeList = new FranchiseBLL { }.GetAllFranchiseeList();
            double count = Convert.ToDouble(new FranchiseBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.FranchiseeList != null)
            {
                var objmodel = new CMSBLL { }.GetCmsById(18);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }

                //model.CoBrandList = CoBrandlist.Where(x => x.IsApproved == true).Select(x => new CoBrandModel
                //{
                //    CoBrandId = x.CoBrandId,
                //    BusinessNameA=x.BusinessNameA,
                //    BusinessNameB=x.BusinessNameB,
                //    CompanyLogo=x.CompanyLogo,
                //    PartnerA=x.PartnerA,
                //    PartnerB=x.PartnerB,
                //    IsActive = Convert.ToBoolean(x.IsActive)
                //}).ToList();

                //model.CoBrandList = (from bissUser in objdb.BusinessConnections
                //                  join cobra in objdb.CoBrandings
                //                  on bissUser.UserId equals cobra.PartnerA 
                //                  where cobra.PartnerA == userid || cobra.PartnerB==userid && cobra.IsActive == true
                //               select new CoBrandModel
                //                  {
                //                      CoBrandId = cobra.CoBrandingId,
                //                      BusinessNameA = cobra.BussinessUser.BusinessName,
                //                      BusinessNameB = cobra.BussinessUser.BusinessName,
                //                      PartnerA=cobra.BussinessUser.UserId,
                //                      PartnerB=cobra.BussinessUser1.UserId,
                //                      CompanyLogo = cobra.BussinessUser.CompanyLogo,
                //                  }).ToList();
            }

            return View(model);
        }

        public ActionResult InvestorPartneringRequest()
        {
            InvestorPartneringModel model = new InvestorPartneringModel();
            int userid = Convert.ToInt32(Session["UserId"]);
            return View();
        }

        [HttpPost]
        public ActionResult InvestorPartneringRequest(InvestorPartneringModel objmodel)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var getuser = new BussinessUserBLL { }.GetUserById(userid);
            var getAdminId = new AdministratorBLL { }.GetAdministratorById(11);
            var checkduplicate = objdb.InvestorPartnerings.Where(x => x.BusinessUserId == objmodel.BusinessUserId).Select(x =>
                new InvestorPartneringModel { BusinessUserId = x.BusinessUserId, Details = x.Details }).SingleOrDefault();
            if (checkduplicate == null)
            {
                try
                {
                    int res = 0;

                    res = new InvestorPartnerBLL { }.AddEditInvestorPartnering(new InvestorPartneringModel
                    {
                        BusinessUserId = userid,
                        Details = objmodel.Details,
                        CreatedOn = DateTime.Now,
                        IsActive = false,
                    });
                    if (res != 0)
                    {
                        #region Send Email
                        msg = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        emailId = getAdminId.Email;
                        //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                        //uPwd = DataEncryption.Decrypt(password, "passKey");
                        //uPwd = model.Password.Trim();
                        //sender email address
                        msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                        //Receiver email address
                        msg.To.Add(emailId);
                        msg.Subject = "Investors Partnering Request";
                        //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                        //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                        //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                        // ActivationUrl = Server.HtmlEncode(link);
                        //msg.Body = "Hi " + model.BusinessName + "!\n" +
                        // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                        // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                        var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(4);
                        var test = emailtemp.Description + "<br />" + ActivationUrl;
                        string mailbod = MailBody(test, getuser);
                        msg.Body = mailbod;
                        msg.IsBodyHtml = true;
                        //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                        //smtp.Port = 2525;
                        //smtp.Host = "retail.smtp.com";
                        smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                        smtp.Port = 25;
                        smtp.Host = "mail.FRMT.IN";

                        smtp.EnableSsl = false;
                        smtp.Send(msg);
                        #endregion
                        iVal = 1;
                        TempData["success"] = "InvestorPartneringRequest";
                        //ViewBag.Msg = "Successfully Investor Partnering Request sent";
                        // Session["Success"] = "Successfully Registered Check your Email to Activate your account";
                        //ViewBag.Msg = "Successfully Registered";
                        return RedirectToAction("UsersProfile", "Home");
                    }
                    else
                    {
                        res = new InvestorPartnerBLL { }.AddEditInvestorPartnering(new InvestorPartneringModel
                        {
                            BusinessUserId = userid,
                            InvestorPartnerId = objmodel.InvestorPartnerId,
                            Details = objmodel.Details,
                            IsActive = false,
                        });
                        if (res != 0)
                        {
                            #region Send Email
                            msg = new MailMessage();
                            SmtpClient smtp = new SmtpClient();
                            emailId = getAdminId.Email;
                            //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                            //uPwd = DataEncryption.Decrypt(password, "passKey");
                            //uPwd = model.Password.Trim();
                            //sender email address
                            msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                            //Receiver email address
                            msg.To.Add(emailId);
                            msg.Subject = "Investors Partnering Request";
                            //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                            //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                            //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                            // ActivationUrl = Server.HtmlEncode(link);
                            //msg.Body = "Hi " + model.BusinessName + "!\n" +
                            // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                            // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                            var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(4);
                            var test = emailtemp.Description + "<br />" + ActivationUrl;
                            string mailbod = MailBody(test, getuser);
                            msg.Body = mailbod;
                            msg.IsBodyHtml = true;
                            //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                            //smtp.Port = 2525;
                            //smtp.Host = "retail.smtp.com";
                            smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                            smtp.Port = 25;
                            smtp.Host = "mail.FRMT.IN";

                            smtp.EnableSsl = false;
                            smtp.Send(msg);
                            #endregion
                            iVal = 1;
                            TempData["success"] = "InvestorPartneringRequest";

                            return RedirectToAction("UsersProfile", "Home");
                        }
                    }
                    TempData["Error"] = true;
                    //Session["Error"] = "Please try again after some time.";
                    return RedirectToAction("UsersProfile", "Home");

                }
                catch (Exception)
                {
                    return RedirectToAction("UsersProfile", "Home");
                    throw;
                }
            }

            return View();
        }

        public ActionResult CorporateBrandingRequest()
        {
            CorporateBrandingModel model = new CorporateBrandingModel();
            int userid = Convert.ToInt32(Session["UserId"]);
            return View();
        }

        [HttpPost]
        public ActionResult CorporateBrandingRequest(CorporateBrandingModel objmodel)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var getuser = new BussinessUserBLL { }.GetUserById(userid);
            var AdminMail = new AdministratorBLL { }.GetAdministratorById(11);
            var checkduplicate = objdb.CorporateBrandings.Where(x => x.BusinessUserId == objmodel.BusinessUserId).Select(x =>
                new CorporateBrandingModel { BusinessUserId = x.BusinessUserId, CorporateBrandDetails = x.CorporatingDetails }).SingleOrDefault();
            if (checkduplicate == null)
            {
                try
                {
                    int res = 0;
                    res = new CorporateBrandingBLL { }.AddEditCorporateBranding(new CorporateBrandingModel
                    {
                        BusinessUserId = userid,
                        CorporateBrandDetails = objmodel.CorporateBrandDetails,
                        CreatedOn = DateTime.Now,
                        IsActive = false,
                    });
                    if (res != 0)
                    {
                        #region Send Email
                        msg = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        emailId = AdminMail.Email;
                        //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                        //uPwd = DataEncryption.Decrypt(password, "passKey");
                        //uPwd = model.Password.Trim();
                        //sender email address
                        msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                        //Receiver email address
                        msg.To.Add(emailId);
                        msg.Subject = "Corporate Branding Request";
                        //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                        //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                        //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                        // ActivationUrl = Server.HtmlEncode(link);
                        //msg.Body = "Hi " + model.BusinessName + "!\n" +
                        // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                        // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                        var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(12);
                        var test = emailtemp.Description + "<br />" + ActivationUrl;
                        string mailbod = MailBody(test, getuser);
                        msg.Body = mailbod;
                        msg.IsBodyHtml = true;
                        //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                        //smtp.Port = 2525;
                        //smtp.Host = "retail.smtp.com";
                        smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                        smtp.Port = 25;
                        smtp.Host = "mail.FRMT.IN";

                        smtp.EnableSsl = false;
                        smtp.Send(msg);
                        #endregion
                        iVal = 1;
                        TempData["Success"] = "CorporateBrandingRequest";
                        //ViewBag.Msg = "Corporate Branding Request Successfully Sent";
                        // Session["Success"] = "Successfully Registered Check your Email to Activate your account";
                        //ViewBag.Msg = "Successfully Registered";
                        return RedirectToAction("UsersProfile", "Home");
                    }
                    else
                    {
                        res = new CorporateBrandingBLL { }.AddEditCorporateBranding(new CorporateBrandingModel
                        {
                            BusinessUserId = userid,
                            CorporateBrandingId = objmodel.CorporateBrandingId,
                            CorporateBrandDetails = objmodel.CorporateBrandDetails,
                            IsActive = false,
                        });
                        if (res != 0)
                        {
                            #region Send Email
                            msg = new MailMessage();
                            SmtpClient smtp = new SmtpClient();
                            emailId =AdminMail.Email;

                            msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                            //Receiver email address
                            msg.To.Add(emailId);
                            msg.Subject = "Corporate Branding Request";
                         
                            var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(12);
                            var test = emailtemp.Description + "<br />" + ActivationUrl;
                            string mailbod = MailBody(test, getuser);
                            msg.Body = mailbod;
                            msg.IsBodyHtml = true;
                            //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                            //smtp.Port = 2525;
                            //smtp.Host = "retail.smtp.com";
                            smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                            smtp.Port = 25;
                            smtp.Host = "mail.FRMT.IN";

                            smtp.EnableSsl = false;
                            smtp.Send(msg);
                            #endregion
                            iVal = 1;
                            TempData["Success"] = "CorporateBrandingRequest";

                            return RedirectToAction("UsersProfile", "Home");
                        }
                    }
                    TempData["Error"] = true;
                    //Session["Error"] = "Please try again after some time.";
                    return RedirectToAction("UsersProfile", "Home");

                }
                catch (Exception)
                {
                    return RedirectToAction("UsersProfile", "Home");
                    throw;
                }
            }

            return View();
        }

        public ActionResult GlobalBrandingRequest()
        {
            GlobalBrandingModel model = new GlobalBrandingModel();
            int userid = Convert.ToInt32(Session["UserId"]);
            return View();
        }

        [HttpPost]
        public ActionResult GlobalBrandingRequest(GlobalBrandingModel objmodel)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            var getuser = new BussinessUserBLL { }.GetUserById(userid);
            var AdminMail = new AdministratorBLL { }.GetAdministratorById(11);
            var checkduplicate = objdb.GlobalBrandings.Where(x => x.BusinessUserId == objmodel.BusinessUserId).Select(x =>
                new GlobalBrandingModel { BusinessUserId = x.BusinessUserId, GlobalBrandDetails = x.GlobalBrandingDetails }).SingleOrDefault();
            if (checkduplicate == null)
            {
                try
                {
                    int res = 0;
                    res = new GlobalBrandingBLL { }.AddEditGlobalBranding(new GlobalBrandingModel
                    {
                        BusinessUserId = userid,
                        GlobalBrandDetails = objmodel.GlobalBrandDetails,
                        CreatedOn = DateTime.Now,
                        IsActive = false,
                    });
                    if (res != 0)
                    {
                        #region Send Email
                        msg = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        emailId = AdminMail.Email;
                        //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                        //uPwd = DataEncryption.Decrypt(password, "passKey");
                        //uPwd = model.Password.Trim();
                        //sender email address
                        msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                        //Receiver email address
                        msg.To.Add(emailId);
                        msg.Subject = "Global Branding Request";
                        //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                        //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                        //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                        // ActivationUrl = Server.HtmlEncode(link);
                        //msg.Body = "Hi " + model.BusinessName + "!\n" +
                        // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                        // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                        var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(11);
                        var test = emailtemp.Description + "<br />" + ActivationUrl;
                        string mailbod = MailBody(test, getuser);
                        msg.Body = mailbod;
                        msg.IsBodyHtml = true;
                        //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                        //smtp.Port = 2525;
                        //smtp.Host = "retail.smtp.com";
                        smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                        smtp.Port = 25;
                        smtp.Host = "mail.FRMT.IN";

                        smtp.EnableSsl = false;
                        smtp.Send(msg);
                        #endregion
                        iVal = 1;
                        TempData["success"] = "GlobalBrandingRequest";

                        return RedirectToAction("UsersProfile", "Home");
                    }
                    else
                    {
                        res = new GlobalBrandingBLL { }.AddEditGlobalBranding(new GlobalBrandingModel
                        {
                            BusinessUserId = userid,
                            GlobalBrandingId = objmodel.GlobalBrandingId,
                            GlobalBrandDetails = objmodel.GlobalBrandDetails,
                            IsActive = false,
                        });
                        if (res != 0)
                        {
                            #region Send Email
                            msg = new MailMessage();
                            SmtpClient smtp = new SmtpClient();
                            emailId = AdminMail.Email;
                            //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
                            //uPwd = DataEncryption.Decrypt(password, "passKey");
                            //uPwd = model.Password.Trim();
                            //sender email address
                            msg.From = new MailAddress("no_reply@thebusinessbranding.com");
                            //Receiver email address
                            msg.To.Add(emailId);
                            msg.Subject = "Global Branding Request";
                            //string link = "http://thebusinessbranding.com/Home/ActivateUrl?id=" + guid;
                            //ActivationUrl = "To Activate Account <a href='" + link + "'> Click here</a>";
                            //ActivationUrl = Server.HtmlEncode("http://thebusinessbranding.com/Home/ActivateUrl?id="+ guid);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
                            // ActivationUrl = Server.HtmlEncode(link);
                            //msg.Body = "Hi " + model.BusinessName + "!\n" +
                            // "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
                            // " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br>your Password: " + uPwd;

                            var emailtemp = new EmailTemplateBLL { }.GetEmailSettingsByTemplateID(11);
                            var test = emailtemp.Description + "<br />" + ActivationUrl;
                            string mailbod = MailBody(test, getuser);
                            msg.Body = mailbod;
                            msg.IsBodyHtml = true;
                            //smtp.Credentials = new NetworkCredential("ameer.pasha@thebusinessbranding.com", "fa34729f");
                            //smtp.Port = 2525;
                            //smtp.Host = "retail.smtp.com";
                            smtp.Credentials = new NetworkCredential("no_reply@FRMT.IN", "Farzana@123");
                            smtp.Port = 25;
                            smtp.Host = "mail.FRMT.IN";

                            smtp.EnableSsl = false;
                            smtp.Send(msg);
                            #endregion
                            iVal = 1;
                            TempData["success"] = "GlobalBrandingRequest";

                            return RedirectToAction("UsersProfile", "Home");
                        }
                    }
                    //Session["Error"] = "Please try again after some time.";
                    TempData["Error"] = true;
                    return RedirectToAction("UsersProfile", "Home");

                }
                catch (Exception)
                {
                    return RedirectToAction("UsersProfile", "Home");
                    throw;
                }
            }

            return View();
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult InvestorPartnerList(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            InvestorPartneringModel model = new InvestorPartneringModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            model.InvestorPartneringList = new InvestorPartnerBLL { }.GetAllInvestorPartnerList();
            double count = Convert.ToDouble(new InvestorPartnerBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.InvestorPartneringList != null)
            {

                var objmodel = new CMSBLL { }.GetCmsById(19);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }

            }

            return View(model);
        }


        [CustumAuthorize.AllowAnonymous]
        public ActionResult GlobalBrandList(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            GlobalBrandingModel model = new GlobalBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            model.GlobalBrandingList = new GlobalBrandingBLL { }.GetAllGlobalBrandingList();
            double count = Convert.ToDouble(new GlobalBrandingBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.GlobalBrandingList != null)
            {

                var objmodel = new CMSBLL { }.GetCmsById(11);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }
            }
            return View(model);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult Company()
        {
            return View();
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult EmployeeForm()
        {
            return View();
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult NewsDetails(int id)
        {
            NewsModel HomeNews = new NewsModel();
            List<NewsModel> AdminNewsList = new List<NewsModel>();
            var objmodel = new NewsBLL { }.GetNewsById(id);
            if (objmodel != null)
            {
                HomeNews.NewsId = objmodel.NewsId;
                HomeNews.NewsTitle = objmodel.NewsTitle;
                HomeNews.NewsDesc = objmodel.NewsDesc;
                HomeNews.NewsImage = objmodel.NewsImage;
            }
            AdminNewsList = (from ns in objdb.News
                             where ns.IsActive == true
                             select new NewsModel
                             {
                                 NewsId = ns.NewsId,
                                 NewsTitle = ns.NewsTitle,
                                 NewsDesc = ns.Description,
                                 NewsImage = ns.NewsImage


                             }).OrderByDescending(x => x.NewsId).Take(10).ToList();
            AdminNewsList.ForEach(s => s.NewsDesc = Regex.Replace(s.NewsDesc, @"<[^>]+>|&nbsp;|&quot;|&#39;", String.Empty));

            //foreach (var item in NewsList)
            //{
            //    AdminNewsList.Add(item);
            //}
            HomeNews.NewsList = AdminNewsList;

            return View(HomeNews);
        }

        #region All Business Request Status Listing

        [CustumAuthorize.AllowAnonymous]
        public ActionResult EmployeeBrandStatus(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            EmpBrandingModel model = new EmpBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            model.EmpBrandingList = new EmpBrandingBLL { }.GetEmpBrandingByUserId(userid);
            double count = Convert.ToDouble(new EmpBrandingBLL { }.GetPageCountbyUserId(userid));
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.EmpBrandingList != null)
            {
                var objmodel = new CMSBLL { }.GetCmsById(13);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }
                //model.CoBrandList = CoBrandlist.Where(x => x.IsApproved == true).Select(x => new CoBrandModel
                //{
                //    CoBrandId = x.CoBrandId,
                //    BusinessNameA=x.BusinessNameA,
                //    BusinessNameB=x.BusinessNameB,
                //    CompanyLogo=x.CompanyLogo,
                //    PartnerA=x.PartnerA,
                //    PartnerB=x.PartnerB,
                //    IsActive = Convert.ToBoolean(x.IsActive)
                //}).ToList();

                //model.CoBrandList = (from bissUser in objdb.BusinessConnections
                //                  join cobra in objdb.CoBrandings
                //                  on bissUser.UserId equals cobra.PartnerA 
                //                  where cobra.PartnerA == userid || cobra.PartnerB==userid && cobra.IsActive == true
                //               select new CoBrandModel
                //                  {
                //                      CoBrandId = cobra.CoBrandingId,
                //                      BusinessNameA = cobra.BussinessUser.BusinessName,
                //                      BusinessNameB = cobra.BussinessUser.BusinessName,
                //                      PartnerA=cobra.BussinessUser.UserId,
                //                      PartnerB=cobra.BussinessUser1.UserId,
                //                      CompanyLogo = cobra.BussinessUser.CompanyLogo,
                //                  }).ToList();
            }

            return View(model);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult FranchiseRequestStatus(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            FranchiseModel model = new FranchiseModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            model.FranchiseeList = new FranchiseBLL { }.GetFranchiserDetailsByUserId(userid);
            double count = Convert.ToDouble(new FranchiseBLL { }.GetPageCountByUserId(userid));
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.FranchiseeList != null)
            {
                var objmodel = new CMSBLL { }.GetCmsById(18);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }
            }

            return View(model);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult InvestorPartnerRequestStatus(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            InvestorPartneringModel model = new InvestorPartneringModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            model.InvestorPartneringList = new InvestorPartnerBLL { }.GetInvestorPartnerDetailsByUserId(userid);
            double count = Convert.ToDouble(new InvestorPartnerBLL { }.GetPageCountByUserId(userid));
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.InvestorPartneringList != null)
            {

                var objmodel = new CMSBLL { }.GetCmsById(19);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }
                //model.CoBrandList = CoBrandlist.Where(x => x.IsApproved == true).Select(x => new CoBrandModel
                //{
                //    CoBrandId = x.CoBrandId,
                //    BusinessNameA=x.BusinessNameA,
                //    BusinessNameB=x.BusinessNameB,
                //    CompanyLogo=x.CompanyLogo,
                //    PartnerA=x.PartnerA,
                //    PartnerB=x.PartnerB,
                //    IsActive = Convert.ToBoolean(x.IsActive)
                //}).ToList();

                //model.CoBrandList = (from bissUser in objdb.BusinessConnections
                //                  join cobra in objdb.CoBrandings
                //                  on bissUser.UserId equals cobra.PartnerA 
                //                  where cobra.PartnerA == userid || cobra.PartnerB==userid && cobra.IsActive == true
                //               select new CoBrandModel
                //                  {
                //                      CoBrandId = cobra.CoBrandingId,
                //                      BusinessNameA = cobra.BussinessUser.BusinessName,
                //                      BusinessNameB = cobra.BussinessUser.BusinessName,
                //                      PartnerA=cobra.BussinessUser.UserId,
                //                      PartnerB=cobra.BussinessUser1.UserId,
                //                      CompanyLogo = cobra.BussinessUser.CompanyLogo,
                //                  }).ToList();
            }

            return View(model);
        }

        [CustumAuthorize.AllowAnonymous]
        public ActionResult CorporateBrandRequestStatus(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            CorporateBrandingModel model = new CorporateBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            model.CorporateBrandingList = new CorporateBrandingBLL { }.GetCorporateBrandingDetailsByUserId(userid);
            double count = Convert.ToDouble(new CorporateBrandingBLL { }.GetPageCountByUserId(userid));
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.CorporateBrandingList != null)
            {

                var objmodel = new CMSBLL { }.GetCmsById(10);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }
            }

            return View(model);
        }   

        [CustumAuthorize.AllowAnonymous]
        public ActionResult GlobalBrandRequestStatus(int pid = 0)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            int take = 10;
            int skip = take * pid;
            GlobalBrandingModel model = new GlobalBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<BusinessNewsModel> Businessnews = new List<BusinessNewsModel>();
            model.GlobalBrandingList = new GlobalBrandingBLL { }.GetGlobalBrandingDetailsByUserId(userid);
            double count = Convert.ToDouble(new GlobalBrandingBLL { }.GetPageCountByUserId(userid));
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (model.GlobalBrandingList != null)
            {

                var objmodel = new CMSBLL { }.GetCmsById(11);
                if (objmodel != null)
                {
                    model.CMSTitle = objmodel.CMSTitle;
                    model.Description = objmodel.Description;
                }
            }
            return View(model);
        }

        #endregion

        #region Integration With PayU

        public string action1 = string.Empty;
        public string hash1 = string.Empty;
        public string txnid1 = string.Empty;


        public ActionResult UpgradeMembership(string amount)
        {
            try
            {
                try
                {
                    Session["AccessPlanId"] = 20;
                    string _returnSuccesUrl = "http://localhost:19297/Home/ReturnSuccessPayU";
                    string _returnFailureUrl = "http://localhost:19297/Home/ReturnFailurePayU";
                    string _returnCancel = "http://localhost:19297/Home/ReturnCancelPayU";

                    var key = ConfigurationManager.AppSettings["MERCHANT_KEY"];

                    int userid = Convert.ToInt32(Session["UserId"]);
                    BussinessUser objUser = new BussinessUserBLL { }.GetBusinessUserDetailsBLL(userid);

                    string[] hashVarsSeq;
                    string hash_string = string.Empty;

                    Random rnd = new Random();
                    string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
                    txnid1 = strHash.ToString().Substring(0, 20);

                    string AmountForm = Convert.ToDecimal(amount).ToString("g29");// eliminating trailing zeros
                    // hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|'); // spliting hash sequence from config
                    hash_string = "";
                    // string str = "C0Dr8m|04f34c514a1d86b96739|100|info|first|test@gmail.com|||||||||||";
                    string str = string.Empty;
                    str += key + "|";
                    str += txnid1 + "|";
                    str += AmountForm + "|";
                    str += objUser.BusinessName + "|";
                    str += objUser.BusinessName + "|";
                    str += objUser.EmailId + "|";
                    str += "||||||||||";


                    //foreach (string hash_var in hashVarsSeq)
                    //{
                    //    if (hash_var == "key")
                    //    {
                    //        hash_string = hash_string + ConfigurationManager.AppSettings["MERCHANT_KEY"];
                    //        hash_string = hash_string + '|';
                    //    }
                    //    else if (hash_var == "txnid")
                    //    {
                    //        hash_string = hash_string + txnid1;
                    //        hash_string = hash_string + '|';
                    //    }
                    //    else if (hash_var == "amount")
                    //    {
                    //        hash_string = hash_string + Convert.ToDecimal(Request.Form[hash_var]).ToString("g29");
                    //        hash_string = hash_string + '|';
                    //    }
                    //    else
                    //    {

                    //        hash_string = hash_string + (Request.Form[hash_var] != null ? Request.Form[hash_var] : "");// isset if else
                    //        hash_string = hash_string + '|';
                    //    }
                    //}

                    hash_string += ConfigurationManager.AppSettings["SALT"];// appending SALT

                    hash1 = Generatehash512(str + hash_string).ToLower();         //generating hash
                    action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";// setting URL              


                    if (!string.IsNullOrEmpty(hash1))
                    {
                        //str = "C0Dr8m|04f34c514a1d86b96739|100|info|first|test@gmail.com|||||||||||";

                        System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                        data.Add("hash", hash1);
                        data.Add("txnid", txnid1);
                        data.Add("key", key);

                        data.Add("amount", AmountForm);
                        data.Add("firstname", objUser.BusinessName.Trim());
                        data.Add("email", objUser.EmailId.Trim());
                        data.Add("phone", objUser.Phone.Trim());
                        data.Add("productinfo", objUser.BusinessName.Trim());
                        data.Add("surl", _returnSuccesUrl.Trim());
                        data.Add("furl", _returnFailureUrl.Trim());
                        data.Add("lastname", objUser.BusinessName.Trim());
                        data.Add("curl", _returnCancel.Trim());

                        //str = "C0Dr8m|04f34c514a1d86b96739|100|info|first|test@gmail.com|||||||||||";

                        //data.Add("hash", hash1);
                        //data.Add("txnid", "04f34c514a1d86b96739");
                        //data.Add("key", "C0Dr8m");
                        //string AmountForm = Convert.ToDecimal(amount.Trim()).ToString("g29");// eliminating trailing zeros
                        //data.Add("amount", "100");
                        //data.Add("firstname", "first");
                        //data.Add("email", "test@gmail.com");
                        //data.Add("phone", "9701217014");
                        //data.Add("productinfo", "info");
                        //data.Add("surl", _returnSuccesUrl.Trim());
                        //data.Add("furl", _returnFailureUrl.Trim());
                        //data.Add("lastname", objUser.BusinessName.Trim());
                        //data.Add("curl", _returnCancel.Trim());



                        string strForm = PreparePOSTForm(action1, data);
                        // Page.Controls.Add(new LiteralControl(strForm));
                        //return RedirectToAction("ReturnCancelPayU", new { @str = strForm });

                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                PaymentTransactionModel mod = new PaymentTransactionModel();
                                mod.TransationStatus = false;
                                mod.TransationId = txnid1;
                                mod.Amount = Convert.ToDecimal(AmountForm);
                                mod.UserId = userid;
                                bool res = new PaymentTransactionBLL { }.InsertPaymentTransactionBLL(mod);
                                // insert into database
                                // transactionid amount etc details 
                                // status sent ,payment gateway status=false    

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        });

                        return Json(strForm);

                    }

                    else
                    {
                        //no hash

                    }

                }

                catch (Exception ex)
                {
                    Response.Write("<span style='color:red'>" + ex.Message + "</span>");

                }

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }

        private string PreparePOSTForm(string url, System.Collections.Hashtable data)      // post form
        {
            //Set a name for the form
            string formID = "PostForm";
            //Build the form using the specified data to be posted.
            StringBuilder strForm = new StringBuilder();
            strForm.Append("<form id=\"" + formID + "\" name=\"" +
                           formID + "\" action=\"" + url +
                           "\" method=\"POST\">");

            foreach (System.Collections.DictionaryEntry key in data)
            {

                strForm.Append("<input type=\"hidden\" name=\"" + key.Key +
                               "\" value=\"" + key.Value + "\">");
            }


            strForm.Append("</form>");
            //Build the JavaScript which will do the Posting operation.
            StringBuilder strScript = new StringBuilder();
            strScript.Append("<script language='javascript'>");
            strScript.Append("var v" + formID + " = document." +
                             formID + ";");
            strScript.Append("v" + formID + ".submit();");
            strScript.Append("</script>");
            //Return the form and the script concatenated.
            //(The order is important, Form then JavaScript)
            return strForm.ToString() + strScript.ToString();
        }

        public string Generatehash512(string text)
        {

            byte[] message = Encoding.UTF8.GetBytes(text);

            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";
            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;

        }

        public ActionResult ReturnSuccessPayU()
        {
            var form = Request.Form;
            int userid = Convert.ToInt32(Session["UserId"]);
            if ((Convert.ToInt32(Session["AccessPlanId"]) == 20))
            {
                if (form["status"] == "success")
                {
                    PaymentTransactionModel mod = new PaymentTransactionModel();
                    mod.PaymentReturnId = form["mihpayid"];
                    mod.TransationStatus = true;
                    mod.TransationId = form["txnid"];
                    mod.MessageFromGateway = form["unmappedstatus"];
                    bool update = new PaymentTransactionBLL { }.UpdatePaymentTransactionByIdBLL(mod);
                    bool res = new MembershipBLL { }.UpgradeMembership(userid);
                }
            }
            else if (Convert.ToInt32(Session["AccessPlanId"]) == 21)
            {
                if (form["status"] == "success")
                {
                    PaymentTransactionModel mod = new PaymentTransactionModel();
                    mod.PaymentReturnId = form["mihpayid"];
                    mod.TransationStatus = true;
                    mod.TransationId = form["txnid"];
                    mod.MessageFromGateway = form["unmappedstatus"];
                    bool update = new PaymentTransactionBLL { }.UpdatePaymentTransactionByIdBLL(mod);
                    HomeBannerMappingModel objmod = new HomeBannerMappingModel();
                    objmod.AccessPlanId = 21;
                    objmod.UserId = userid;
                    objmod.ValidityStart = Convert.ToDateTime(DateTime.ParseExact(Session["ValiStard"].ToString(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")); ;
                    objmod.ValidityEnd = Convert.ToDateTime(DateTime.ParseExact(Session["ValiEnd"].ToString(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")); ;
                    objmod.IsActive = true;
                    bool res = new MembershipBLL { }.RenewBannerValidityBLL(objmod);
                }
            }
            else if (Convert.ToInt32(Session["AccessPlanId"]) == 22)
            {
                if (form["status"] == "success")
                {
                    PaymentTransactionModel mod = new PaymentTransactionModel();
                    mod.PaymentReturnId = form["mihpayid"];
                    mod.TransationStatus = true;
                    mod.TransationId = form["txnid"];
                    mod.MessageFromGateway = form["unmappedstatus"];
                    bool update = new PaymentTransactionBLL { }.UpdatePaymentTransactionByIdBLL(mod);
                    NewsMappingModel objmod = new NewsMappingModel();
                    objmod.AccessPlanId = 21;
                    objmod.UserId = userid;
                    objmod.ValidityStart = Convert.ToDateTime(DateTime.ParseExact(Session["ValiStard"].ToString(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")); ;
                    objmod.ValidityEnd = Convert.ToDateTime(DateTime.ParseExact(Session["ValiEnd"].ToString(), "dd/MM/yyyy", null).ToString("MM/dd/yyyy")); ;
                    objmod.IsActive = true;
                    bool res = new MembershipBLL { }.RenewNewsValidityBLL(objmod);
                }
            }


            return View();
        }

        public ActionResult ReturnFailurePayU()
        {
            var form = Request.Form;
            int userid = Convert.ToInt32(Session["UserId"]);
            PaymentTransactionModel mod = new PaymentTransactionModel();
            mod.PaymentReturnId = form["mihpayid"];
            mod.TransationId = form["txnid"];
            mod.MessageFromGateway = form["unmappedstatus"];
            bool update = new PaymentTransactionBLL { }.UpdatePaymentTransactionByIdBLL(mod);
            return View();
        }

        public ActionResult ReturnCancelPayU()
        {
            var form = Request.Form;
            int userid = Convert.ToInt32(Session["UserId"]);
            //if (Convert.ToInt32(Session["AccessPlanId"])==20)
            //{
            PaymentTransactionModel mod = new PaymentTransactionModel();
            mod.PaymentReturnId = form["mihpayid"];
            mod.TransationId = form["txnid"];
            mod.MessageFromGateway = form["unmappedstatus"];
            bool update = new PaymentTransactionBLL { }.UpdatePaymentTransactionByIdBLL(mod);
            //}
            //else if (Convert.ToInt32(Session["AccessPlanId"]) == 21)
            //{

            //}
            //else if (Convert.ToInt32(Session["AccessPlanId"]) == 22)
            //{
            //}

            return View();
        }


        public ActionResult RenewalBannerMembership()
        {
            AccessPlan accessP = new MembershipBLL { }.GetAccessPlanByIdBLL(21);
            ViewBag.Money = accessP.MembershipFee;
            ViewBag.Currency = accessP.Currency.CurrencyName;
            ViewBag.Validity = accessP.Validity;
            DateTime dt = new PaymentTransactionBLL { }.BannerAvailabilityBLL();
            if (dt != Convert.ToDateTime("1980-01-01"))
            {
                if (dt.Date <= DateTime.Now.Date)
                {
                    ViewBag.Date = DateTime.Now;

                }
                else
                {
                    ViewBag.Date = dt.AddDays(1);
                }
            }

            return View();
        }

        public ActionResult RenewalBannerPopup()
        {
            if (HttpContext.Session["UserId"] != null)
            {
                AccessPlan accessP = new MembershipBLL { }.GetAccessPlanByIdBLL(21);
                ViewBag.Money = accessP.MembershipFee;
                ViewBag.Currency = accessP.Currency.CurrencyName;
                ViewBag.Validity = accessP.Validity;
                DateTime dt = new PaymentTransactionBLL { }.BannerAvailabilityBLL();
                if (dt != Convert.ToDateTime("1980-01-01"))
                {
                    if (dt.Date <= DateTime.Now.Date)
                    {
                        ViewBag.Date = DateTime.Now;

                    }
                    else
                    {
                        ViewBag.Date = dt.AddDays(1);
                    }

                }
                return PartialView("_RenewBigHoarding");
            }
            else
            {
                BizUserLoginModel LogModel = new BizUserLoginModel();
                return PartialView("_SessionExpire", LogModel);
            }
        }

        public ActionResult UpdradeBannerMembership(string amount, string sDate, string eDate)
        {
            try
            {
                try
                {
                    Session["AccessPlanId"] = 21;
                    Session["ValiStard"] = sDate;
                    Session["ValiEnd"] = eDate;
                    string _returnSuccesUrl = "http://localhost:19297/Home/ReturnSuccessPayU";
                    string _returnFailureUrl = "http://localhost:19297/Home/ReturnFailurePayU";
                    string _returnCancel = "http://localhost:19297/Home/ReturnCancelPayU";

                    var key = ConfigurationManager.AppSettings["MERCHANT_KEY"];

                    int userid = Convert.ToInt32(Session["UserId"]);
                    BussinessUser objUser = new BussinessUserBLL { }.GetBusinessUserDetailsBLL(userid);

                    string[] hashVarsSeq;
                    string hash_string = string.Empty;

                    Random rnd = new Random();
                    string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
                    txnid1 = strHash.ToString().Substring(0, 20);

                    string AmountForm = Convert.ToDecimal(amount).ToString("g29");// eliminating trailing zeros
                    // hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|'); // spliting hash sequence from config
                    hash_string = "";
                    // string str = "C0Dr8m|04f34c514a1d86b96739|100|info|first|test@gmail.com|||||||||||";
                    string str = string.Empty;
                    str += key + "|";
                    str += txnid1 + "|";
                    str += AmountForm + "|";
                    str += objUser.BusinessName + "|";
                    str += objUser.BusinessName + "|";
                    str += objUser.EmailId + "|";
                    str += "||||||||||";



                    hash_string += ConfigurationManager.AppSettings["SALT"];// appending SALT

                    hash1 = Generatehash512(str + hash_string).ToLower();         //generating hash
                    action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";// setting URL              


                    if (!string.IsNullOrEmpty(hash1))
                    {
                        //str = "C0Dr8m|04f34c514a1d86b96739|100|info|first|test@gmail.com|||||||||||";

                        System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                        data.Add("hash", hash1);
                        data.Add("txnid", txnid1);
                        data.Add("key", key);

                        data.Add("amount", AmountForm);
                        data.Add("firstname", objUser.BusinessName.Trim());
                        data.Add("email", objUser.EmailId.Trim());
                        data.Add("phone", objUser.Phone.Trim());
                        data.Add("productinfo", objUser.BusinessName.Trim());
                        data.Add("surl", _returnSuccesUrl.Trim());
                        data.Add("furl", _returnFailureUrl.Trim());
                        data.Add("lastname", objUser.BusinessName.Trim());
                        data.Add("curl", _returnCancel.Trim());




                        string strForm = PreparePOSTForm(action1, data);
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                PaymentTransactionModel mod = new PaymentTransactionModel();
                                mod.TransationStatus = false;
                                mod.TransationId = txnid1;
                                mod.Amount = Convert.ToDecimal(AmountForm);
                                mod.UserId = userid;
                                bool res = new PaymentTransactionBLL { }.InsertPaymentTransactionBLL(mod);
                                // insert into database
                                // transactionid amount etc details 
                                // status sent ,payment gateway status=false    

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        });

                        return Json(strForm);

                    }

                    else
                    {
                        //no hash

                    }

                }

                catch (Exception ex)
                {
                    Response.Write("<span style='color:red'>" + ex.Message + "</span>");

                }

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }

        public ActionResult RenewNewsValidity()
        {
            AccessPlan accessP = new MembershipBLL { }.GetAccessPlanByIdBLL(22);
            ViewBag.Money = accessP.MembershipFee;
            ViewBag.Currency = accessP.Currency.CurrencyName;
            ViewBag.Validity = accessP.Validity;
            DateTime dt = new PaymentTransactionBLL { }.NewsAvailabilityBLL();
            if (dt != Convert.ToDateTime("1980-01-01"))
            {
                if (dt.Date <= DateTime.Now.Date)
                {
                    ViewBag.Date = DateTime.Now;
                }
                else
                {
                    ViewBag.Date = dt.AddDays(1);
                }


            }
            return View();
        }

        public ActionResult UpdradeNewsMembership(string amount, string sDate, string eDate)
        {
            try
            {
                try
                {
                    Session["AccessPlanId"] = 21;
                    Session["ValiStard"] = sDate;
                    Session["ValiEnd"] = eDate;
                    string _returnSuccesUrl = "http://localhost:19297/Home/ReturnSuccessPayU";
                    string _returnFailureUrl = "http://localhost:19297/Home/ReturnFailurePayU";
                    string _returnCancel = "http://localhost:19297/Home/ReturnCancelPayU";

                    var key = ConfigurationManager.AppSettings["MERCHANT_KEY"];

                    int userid = Convert.ToInt32(Session["UserId"]);
                    BussinessUser objUser = new BussinessUserBLL { }.GetBusinessUserDetailsBLL(userid);

                    string[] hashVarsSeq;
                    string hash_string = string.Empty;

                    Random rnd = new Random();
                    string strHash = Generatehash512(rnd.ToString() + DateTime.Now);
                    txnid1 = strHash.ToString().Substring(0, 20);

                    string AmountForm = Convert.ToDecimal(amount).ToString("g29");// eliminating trailing zeros
                    // hashVarsSeq = ConfigurationManager.AppSettings["hashSequence"].Split('|'); // spliting hash sequence from config
                    hash_string = "";
                    // string str = "C0Dr8m|04f34c514a1d86b96739|100|info|first|test@gmail.com|||||||||||";
                    string str = string.Empty;
                    str += key + "|";
                    str += txnid1 + "|";
                    str += AmountForm + "|";
                    str += objUser.BusinessName + "|";
                    str += objUser.BusinessName + "|";
                    str += objUser.EmailId + "|";
                    str += "||||||||||";



                    hash_string += ConfigurationManager.AppSettings["SALT"];// appending SALT

                    hash1 = Generatehash512(str + hash_string).ToLower();         //generating hash
                    action1 = ConfigurationManager.AppSettings["PAYU_BASE_URL"] + "/_payment";// setting URL              


                    if (!string.IsNullOrEmpty(hash1))
                    {
                        //str = "C0Dr8m|04f34c514a1d86b96739|100|info|first|test@gmail.com|||||||||||";

                        System.Collections.Hashtable data = new System.Collections.Hashtable(); // adding values in gash table for data post
                        data.Add("hash", hash1);
                        data.Add("txnid", txnid1);
                        data.Add("key", key);

                        data.Add("amount", AmountForm);
                        data.Add("firstname", objUser.BusinessName.Trim());
                        data.Add("email", objUser.EmailId.Trim());
                        data.Add("phone", objUser.Phone.Trim());
                        data.Add("productinfo", objUser.BusinessName.Trim());
                        data.Add("surl", _returnSuccesUrl.Trim());
                        data.Add("furl", _returnFailureUrl.Trim());
                        data.Add("lastname", objUser.BusinessName.Trim());
                        data.Add("curl", _returnCancel.Trim());




                        string strForm = PreparePOSTForm(action1, data);
                        Task.Factory.StartNew(() =>
                        {
                            try
                            {
                                PaymentTransactionModel mod = new PaymentTransactionModel();
                                mod.TransationStatus = false;
                                mod.TransationId = txnid1;
                                mod.Amount = Convert.ToDecimal(AmountForm);
                                mod.UserId = userid;
                                bool res = new PaymentTransactionBLL { }.InsertPaymentTransactionBLL(mod);
                                // insert into database
                                // transactionid amount etc details 
                                // status sent ,payment gateway status=false    

                            }
                            catch (Exception)
                            {

                                throw;
                            }
                        });

                        return Json(strForm);

                    }

                    else
                    {
                        //no hash

                    }

                }

                catch (Exception ex)
                {
                    Response.Write("<span style='color:red'>" + ex.Message + "</span>");

                }

                return Json(true);
            }
            catch (Exception)
            {
                return Json(false);
                throw;
            }
        }

        #endregion

    }
}
