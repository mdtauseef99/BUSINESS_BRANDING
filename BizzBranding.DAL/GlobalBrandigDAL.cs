using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
   public class GlobalBrandigDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();
        List<GlobalBrandingModel> model = new List<GlobalBrandingModel>();

        public int AddEditGlobalBranding(GlobalBrandingModel objmodel)
        {
            try
            {
                if (objmodel.GlobalBrandingId == 0)
                {
                    GlobalBranding obj = new GlobalBranding
                    {
                        //FranchiseId = objmodel.FranchiseId,
                        BusinessUserId = objmodel.BusinessUserId,
                        CreatedOn = DateTime.Now,
                        IsActive = objmodel.IsActive,
                        GlobalBrandingDetails = objmodel.GlobalBrandDetails,
                        ApprovedBy = objmodel.ApprovedBy,
                        ApprovedOn = DateTime.Now,
                        ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month)),
                        No_Months = objmodel.No_Month,
                    };
                    objdb.GlobalBrandings.Add(obj);
                    objdb.SaveChanges();
                    return obj.GlobalBrandingId;
                }
                else
                {
                    var objglobal = objdb.GlobalBrandings.Find(objmodel.GlobalBrandingId);
                    objglobal.BusinessUserId = objmodel.BusinessUserId;
                    objglobal.GlobalBrandingId = objmodel.GlobalBrandingId;
                    objglobal.ApprovedOn = DateTime.Now;
                    objglobal.ApprovedBy = objmodel.ApprovedBy;
                    objglobal.GlobalBrandingDetails = objmodel.GlobalBrandDetails;
                    objglobal.IsActive = objmodel.IsActive;
                    objglobal.ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month));
                    objglobal.No_Months = objmodel.No_Month;
                    objdb.SaveChanges();
                    return objmodel.GlobalBrandingId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<GlobalBrandingModel> GetAllGlobalBrandingList(int skip, int take)
        {
            try
            {
                model = (from ip in objdb.GlobalBrandings
                         //where Fr.IsActive == true
                         select new GlobalBrandingModel
                         {
                             BusinessUserId = ip.BusinessUserId,
                             GlobalBrandingId = ip.GlobalBrandingId,
                             BusinessName = ip.BussinessUser.BusinessName,
                             GlobalBrandDetails = ip.GlobalBrandingDetails,
                             ApprovedBy = ip.ApprovedBy,
                             Admin = ip.Administrator.UserName,
                             CreatedOn = ip.CreatedOn,
                             IsActive = ip.IsActive,
                             ApprovedOn = ip.ApprovedOn,
                             No_Month = ip.No_Months,
                             CompanyLogo = ip.BussinessUser.CompanyLogo,
                             ExpiresOn = ip.ExpiresOn,

                         }).OrderByDescending(x => x.GlobalBrandingId).Skip(skip).Take(take).ToList();
                return (model);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<GlobalBrandingModel> GetAllGlobalBrandingList(int skip, int take, int cid)
        {
            try
            {
                return objdb.GlobalBrandings.Where(x => x.GlobalBrandingId == cid).Select(x => new GlobalBrandingModel
                {
                    GlobalBrandingId = x.GlobalBrandingId,
                    BusinessUserId = x.BusinessUserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    GlobalBrandDetails = x.GlobalBrandingDetails,
                    Admin = x.Administrator.UserName,
                    CreatedOn = x.CreatedOn,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.GlobalBrandingId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public GlobalBrandingModel GetGlobalBrandingDetailsById(int id)
        {
            try
            {
                return objdb.GlobalBrandings.Where(x => x.GlobalBrandingId == id).Select(x => new GlobalBrandingModel
                {
                    GlobalBrandingId = x.GlobalBrandingId,
                    BusinessUserId = x.BusinessUserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    Admin = x.Administrator.UserName,
                    GlobalBrandDetails = x.GlobalBrandingDetails,
                    CreatedOn = x.CreatedOn,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    IsActive = x.IsActive,
                    No_Month = x.No_Months,
                    ExpiresOn = x.ExpiresOn,
                    CompanyLogo = x.BussinessUser.CompanyLogo,
                    Phone = x.BussinessUser.Phone,
                    Address = x.BussinessUser.CoAddress,
                    TradeEmailId = x.BussinessUser.TradeEmaiIId,
                    EmailId = x.BussinessUser.EmailId
                }).SingleOrDefault();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<GlobalBrandingModel> GetGlobalBrandingDetailsByUserId(int id)
        {
            try
            {
                return objdb.GlobalBrandings.Where(x => x.BusinessUserId == id).Select(x => new GlobalBrandingModel
                {
                    GlobalBrandingId = x.GlobalBrandingId,
                    BusinessUserId = x.BusinessUserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    Admin = x.Administrator.UserName,
                    GlobalBrandDetails = x.GlobalBrandingDetails,
                    CreatedOn = x.CreatedOn,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    IsActive = x.IsActive,
                    No_Month = x.No_Months,
                    ExpiresOn = x.ExpiresOn,
                    CompanyLogo = x.BussinessUser.CompanyLogo,
                }).OrderByDescending(x=>x.GlobalBrandingId).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<GlobalBrandingModel> GetAllGlobalBrandingList()
        {
            model = (from Co in objdb.GlobalBrandings
                     where Co.IsActive == true
                     select new GlobalBrandingModel
                     {
                         BusinessName = Co.BussinessUser.BusinessName,
                         BusinessUserId = Co.BusinessUserId,
                         GlobalBrandingId = Co.GlobalBrandingId,
                         GlobalBrandDetails = Co.GlobalBrandingDetails,
                         CompanyLogo = Co.BussinessUser.CompanyLogo,
                         ApprovedOn = Co.ApprovedOn,
                         IsActive = Co.IsActive,
                         CreatedOn = Co.CreatedOn,
                         Admin = Co.Administrator.UserName,
                         No_Month = Co.No_Months,
                         ExpiresOn = Co.ExpiresOn,
                         //ActivatedOn = Co.ActivatedOn,
                         // ExpiresOn = Co.ExpiresOn,

                     }).OrderByDescending(x => x.GlobalBrandingId).ToList();
            return (model);
        }

        //public List<CoBrandModel> GetAllCoBrandUsersDetails()
        //{
        //    model = (from Co in objdb.CoBrandings
        //             where Co.IsAprroved == true
        //             select new CoBrandModel
        //             {
        //                 PartnerA = Co.PartnerA,
        //                 PartnerB = Co.PartnerB,
        //                 BusinessNameA = Co.BussinessUser.BusinessName,
        //                 BusinessNameB = Co.BussinessUser1.BusinessName,
        //                 CompanyLogo = Co.BussinessUser.CompanyLogo,
        //                 CompanyLogoB = Co.BussinessUser1.CompanyLogo,
        //                 CoBrandId = Co.CoBrandingId,
        //                 IsActive = Co.IsActive,
        //                 IsApproved = Co.IsAprroved,
        //                 CoBrandedName = Co.CoBrandedName,
        //                 ActivatedOn = Co.ActivatedOn,
        //                 ExpiresOn = Co.ExpiresOn,

        //             }).ToList();
        //    return (model);
        //}

        public bool ChangeGlobalBrandingStatus(int id)
        {
            try
            {
                var obj = objdb.GlobalBrandings.Find(id);
                if (obj != null && obj.IsActive == true)
                {
                    obj.IsActive = false;
                    objdb.SaveChanges();
                    return false;
                }
                else if (obj != null)
                {
                    obj.IsActive = true;
                    //obj.ApprovedOn = DateTime.Now;
                    //obj.ExpiresOn = DateTime.Now.AddMonths(3);
                    objdb.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        public bool ChangeGlobalBrandingApprovalStatus(int id)
        {
            try
            {
                var obj = objdb.GlobalBrandings.Find(id);
                if (obj != null && obj.IsActive == true)
                {
                    obj.IsActive = false;
                    objdb.SaveChanges();
                    return false;
                }
                else if (obj != null)
                {
                    // obj.IsAprroved = true;
                    obj.IsActive = true;
                    objdb.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public int Remove(int id)
        {
            try
            {
                var obj = objdb.GlobalBrandings.Find(id);
                if (obj != null)
                {
                    objdb.GlobalBrandings.Remove(obj);
                    objdb.SaveChanges();
                }
                return obj.GlobalBrandingId;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int GetPageCount()
        {
            try
            {
                return objdb.GlobalBrandings.Select(x => x.GlobalBrandingId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int GetPageCountByUserId(int id)
        {
            try
            {
                return objdb.GlobalBrandings.Select(x => x.BusinessUserId==id).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
    }
}
