using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.DAL
{
    public class SearcbyKeywordDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public SearchwordsModel Search(string txtval)
        {
            SearchwordsModel storemodel = new SearchwordsModel();
            try
            {

                storemodel.searchkeywords = (from s in objdb.BussinessUsers
                                             where s.Industry.IndustryName.StartsWith(txtval) && s.IsActive == true
                                             select new SearchwordsModel
                                             {
                                                 Id = s.UserId,
                                                 UserId = s.UserId,
                                                 EmailId = s.EmailId,
                                                 BusinessName = s.BusinessName,
                                                 CompanyLogo = s.CompanyLogo
                                             }).ToList();




                if (storemodel.searchkeywords.Count == 0)
                {
                    storemodel.searchkeywords = (from s in objdb.BussinessUsers
                                                 where s.BusinessName.StartsWith(txtval) && s.IsActive == true
                                                 select new SearchwordsModel
                                                 {
                                                     Id = s.UserId,
                                                     EmailId = s.EmailId,
                                                     BusinessName = s.BusinessName,
                                                     CompanyLogo = s.CompanyLogo,
                                                     BusineeDetails = s.BusinessDetails
                                                 }).ToList();

                    if (storemodel.searchkeywords.Count != 0)
                    {
                        return storemodel;
                    }
                    else
                    {
                        try
                        {
                            storemodel.searchkeywords = (from s in objdb.BussinessUsers
                                                         where s.City.CityName.StartsWith(txtval) && s.IsActive == true
                                                         select new SearchwordsModel
                                                         {
                                                             Id = s.UserId,
                                                             UserId = s.UserId,
                                                             EmailId = s.EmailId,
                                                             BusinessName = s.BusinessName,
                                                             CompanyLogo = s.CompanyLogo
                                                         }).ToList();
                            return storemodel;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }


                }

                //storemodel.searchkeywords = (from x in objdb.BussinessUsers
                //                             where (x.BusinessName.Contains(txtval) || x.Industry.IndustryName.Contains(txtval)|| x.City.CityName.Contains(txtval)) && x.IsActive==true 
                //                             select new SearchwordsModel
                //                             {
                //                                 Id = x.UserId,
                //                                 EmailId = x.EmailId,
                //                                 BusinessName = x.BusinessName,
                //                                 CompanyLogo = x.CompanyLogo
                //                             }).ToList();
                return storemodel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public SearchwordsModel SearchByAccessPlan(string txtval)
        {
            SearchwordsModel storemodel = new SearchwordsModel();
            try
            {
                storemodel.searchkeywords = (from m in objdb.Memberships
                                             //join a in objdb.Areas on s.AreaId equals a.AreaId
                                             // where a.AreaName.StartsWith(txtval) && a.IsActive == true
                                             where m.BussinessUser.BusinessName.StartsWith(txtval) || m.BussinessUser.EmailId.StartsWith(txtval) || m.AccessPlan.AccessPlanName.StartsWith(txtval)
                                             select new SearchwordsModel
                                             {
                                                 MembershipId=m.MembershipID,
                                                 Id = m.BussinessUser.UserId,
                                                 EmailId = m.BussinessUser.EmailId,
                                                 BusinessName = m.BussinessUser.BusinessName,
                                                 CompanyLogo = m.BussinessUser.CompanyLogo,
                                                 BusineeDetails = m.BussinessUser.BusinessDetails,
                                                 AccessPlanName=m.AccessPlan.AccessPlanName,
                                                 ActivatedOn=m.ActivatedOn,
                                                 ExpiresOn=m.ExpiresOn,
                                                 CreatedDate=m.CreatedDate,
                                                 IsActive=m.IsActive
                                             }).ToList();

                if (storemodel.searchkeywords.Count == 0)
                {
                //    storemodel.searchkeywords = (from s in objdb.BussinessUsers
                //                                 where s.Industry.IndustryName.StartsWith(txtval) && s.IsActive == true
                //                                 select new SearchwordsModel
                //                                 {
                //                                     Id = s.UserId,
                //                                     UserId = s.UserId,
                //                                     EmailId = s.EmailId,
                //                                     BusinessName = s.BusinessName,
                //                                     CompanyLogo = s.CompanyLogo
                //                                 }).ToList();

                //    if (storemodel.searchkeywords.Count != 0)
                //    {
                //        return storemodel;
                //    }
                //    else
                //    {
                //        try
                //        {
                //            storemodel.searchkeywords = (from s in objdb.BussinessUsers
                //                                         where s.City.CityName.StartsWith(txtval) && s.IsActive == true
                //                                         select new SearchwordsModel
                //                                         {
                //                                             Id = s.UserId,
                //                                             UserId = s.UserId,
                //                                             EmailId = s.EmailId,
                //                                             BusinessName = s.BusinessName,
                //                                             CompanyLogo = s.CompanyLogo
                //                                         }).ToList();
                            return storemodel;
                //        }
                //        catch (Exception)
                //        {
                //            throw;
                //        }
                   }
                return storemodel;
                }

                //storemodel.searchkeywords = (from x in objdb.BussinessUsers
                //                             where (x.BusinessName.Contains(txtval) || x.Industry.IndustryName.Contains(txtval)|| x.City.CityName.Contains(txtval)) && x.IsActive==true 
                //                             select new SearchwordsModel
                //                             {
                //                                 Id = x.UserId,
                //                                 EmailId = x.EmailId,
                //                                 BusinessName = x.BusinessName,
                //                                 CompanyLogo = x.CompanyLogo
                //                             }).ToList();
               // return storemodel;
           // }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        public SearchwordsModel SearchMemberByProduct(string txtval)
        {
            SearchwordsModel storemodel = new SearchwordsModel();
            try
            {
                storemodel.searchkeywords = (from m in objdb.Products
                                             //join a in objdb.Areas on s.AreaId equals a.AreaId
                                             // where a.AreaName.StartsWith(txtval) && a.IsActive == true
                                             where m.BussinessUser.BusinessName.StartsWith(txtval) || m.BussinessUser.EmailId.StartsWith(txtval) || m.ProductName.StartsWith(txtval)
                                             select new SearchwordsModel
                                             {
                                                 ProductId=m.ProductId,
                                                 ProdDesc=m.ProdDetails,
                                                 ProductName=m.ProductName,
                                                 //MembershipId = m.MembershipID,
                                                 Id = m.BussinessUser.UserId,
                                                 EmailId = m.BussinessUser.EmailId,
                                                 BusinessName = m.BussinessUser.BusinessName,
                                                 CompanyLogo = m.BussinessUser.CompanyLogo,
                                                 BusineeDetails = m.BussinessUser.BusinessDetails,
                                                 //AccessPlanName = m.AccessPlan.AccessPlanName,
                                                 //ActivatedOn = m.ActivatedOn,
                                                // ExpiresOn = m.ExpiresOn,
                                                // CreatedDate = m.CreatedDate,
                                                 IsActive = m.IsActive
                                             }).ToList();

                if (storemodel.searchkeywords.Count == 0)
                {
                    //    storemodel.searchkeywords = (from s in objdb.BussinessUsers
                    //                                 where s.Industry.IndustryName.StartsWith(txtval) && s.IsActive == true
                    //                                 select new SearchwordsModel
                    //                                 {
                    //                                     Id = s.UserId,
                    //                                     UserId = s.UserId,
                    //                                     EmailId = s.EmailId,
                    //                                     BusinessName = s.BusinessName,
                    //                                     CompanyLogo = s.CompanyLogo
                    //                                 }).ToList();

                    //    if (storemodel.searchkeywords.Count != 0)
                    //    {
                    //        return storemodel;
                    //    }
                    //    else
                    //    {
                    //        try
                    //        {
                    //            storemodel.searchkeywords = (from s in objdb.BussinessUsers
                    //                                         where s.City.CityName.StartsWith(txtval) && s.IsActive == true
                    //                                         select new SearchwordsModel
                    //                                         {
                    //                                             Id = s.UserId,
                    //                                             UserId = s.UserId,
                    //                                             EmailId = s.EmailId,
                    //                                             BusinessName = s.BusinessName,
                    //                                             CompanyLogo = s.CompanyLogo
                    //                                         }).ToList();
                    return storemodel;
                    //        }
                    //        catch (Exception)
                    //        {
                    //            throw;
                    //        }
                }
                return storemodel;
            }

                //storemodel.searchkeywords = (from x in objdb.BussinessUsers
            //                             where (x.BusinessName.Contains(txtval) || x.Industry.IndustryName.Contains(txtval)|| x.City.CityName.Contains(txtval)) && x.IsActive==true 
            //                             select new SearchwordsModel
            //                             {
            //                                 Id = x.UserId,
            //                                 EmailId = x.EmailId,
            //                                 BusinessName = x.BusinessName,
            //                                 CompanyLogo = x.CompanyLogo
            //                             }).ToList();
            // return storemodel;
            // }
            catch (Exception)
            {
                return null;
                throw;
            }

        }

        public SearchwordsModel SearchbyIndustry(int industryId)
        {
            SearchwordsModel storemodel = new SearchwordsModel();
            try
            {
                storemodel.searchkeywords = (from s in objdb.BussinessUsers
                                             where s.IndustryId == industryId //&& s.IsActive == true
                                             select new SearchwordsModel
                                             {
                                                 Id = s.UserId,
                                                 UserId = s.UserId,
                                                 EmailId = s.EmailId,
                                                 BusinessName = s.BusinessName,
                                                 CompanyLogo = s.CompanyLogo,
                                                 BusineeDetails = s.BusinessDetails
                                             }).ToList();
                return storemodel;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string IndustrySearch(int industryId)
        {
            string str = string.Empty;
            try
            {
                str = (from s in objdb.Industries
                       where s.IndustryId == industryId //&& s.IsActive == true
                       select s.IndustryName).FirstOrDefault();
                return str;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public string CitySearch(int Cityid)
        {
            string str = string.Empty;
            try
            {
                str = (from s in objdb.Cities
                       where s.CityId == Cityid //&& s.IsActive == true
                       select s.CityName).FirstOrDefault();
                return str;
            }
            catch (Exception)
            {

                throw;
            }

        }

        public SearchwordsModel SearchCity(int Cityid)
        {
            SearchwordsModel storemodel = new SearchwordsModel();
            try
            {
                storemodel.searchkeywords = (from s in objdb.BussinessUsers
                                             where s.CityId == Cityid // && s.IsActive == true
                                             select new SearchwordsModel
                                             {
                                                 Id = s.UserId,
                                                 UserId = s.UserId,
                                                 EmailId = s.EmailId,
                                                 BusinessName = s.BusinessName,
                                                 CompanyLogo = s.CompanyLogo,
                                                 BusineeDetails=s.BusinessDetails
                                             }).ToList();
                return storemodel;
            }
            catch (Exception)
            {

                throw;
            }

        }

    }
}
