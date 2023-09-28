using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;


namespace BizzBranding.DAL
{
    public class CorporateBrandingDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();
        List<CorporateBrandingModel> model = new List<CorporateBrandingModel>();

        public int AddEditCorporateBranding(CorporateBrandingModel objmodel)
        {
            try
            {
                if (objmodel.CorporateBrandingId == 0)
                {
                    CorporateBranding objCorporate = new CorporateBranding
                    {
                        //FranchiseId = objmodel.FranchiseId,
                        BusinessUserId = objmodel.BusinessUserId,
                        CreatedOn = DateTime.Now,
                        IsActive = objmodel.IsActive,
                        CorporatingDetails = objmodel.CorporateBrandDetails,
                        ApprovedBy = objmodel.ApprovedBy,
                        ApprovedOn = DateTime.Now,
                        ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month)),
                        No_Months = objmodel.No_Month,
                    };
                    objdb.CorporateBrandings.Add(objCorporate);
                    objdb.SaveChanges();
                    return objCorporate.CorporateId;
                }
                else
                {
                    var obj = objdb.CorporateBrandings.Find(objmodel.CorporateBrandingId);
                    obj.BusinessUserId = objmodel.BusinessUserId;
                    obj.CorporateId = objmodel.CorporateBrandingId;
                    obj.ApprovedOn = DateTime.Now;
                    obj.ApprovedBy = objmodel.ApprovedBy;
                    obj.CorporatingDetails = objmodel.CorporateBrandDetails;
                    obj.IsActive = objmodel.IsActive;
                    obj.ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month));
                    obj.No_Months = objmodel.No_Month;
                    objdb.SaveChanges();
                    return objmodel.CorporateBrandingId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<CorporateBrandingModel> GetAllCorporateBrandingList(int skip, int take)
        {
            try
            {
                model = (from cb in objdb.CorporateBrandings
                         //where Fr.IsActive == true
                         select new CorporateBrandingModel
                         {
                             BusinessUserId = cb.BusinessUserId,
                             CorporateBrandingId = cb.CorporateId,
                             BusinessName = cb.BussinessUser.BusinessName,
                             CorporateBrandDetails = cb.CorporatingDetails,
                             ApprovedBy = cb.ApprovedBy,
                             Admin = cb.Administrator.UserName,
                             CreatedOn = cb.CreatedOn,
                             IsActive = cb.IsActive,
                             ApprovedOn = cb.ApprovedOn,
                             No_Month = cb.No_Months,
                             CompanyLogo = cb.BussinessUser.CompanyLogo,
                             ExpiresOn = cb.ExpiresOn,

                         }).OrderByDescending(x => x.CorporateBrandingId).Skip(skip).Take(take).ToList();
                return (model);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CorporateBrandingModel> GetAllCorporateBrandingList(int skip, int take, int cid)
        {
            try
            {
                return objdb.CorporateBrandings.Where(x => x.CorporateId == cid).Select(x => new CorporateBrandingModel
                {
                    CorporateBrandingId = x.CorporateId,
                    BusinessUserId = x.BusinessUserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    CorporateBrandDetails = x.CorporatingDetails,
                    Admin = x.Administrator.UserName,
                    CreatedOn = x.CreatedOn,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.CorporateBrandingId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public CorporateBrandingModel GetCorporateBrandingDetailsById(int id)
        {
            try
            {
                return objdb.CorporateBrandings.Where(x => x.CorporateId == id).Select(x => new CorporateBrandingModel
                {
                    CorporateBrandingId = x.CorporateId,
                    BusinessUserId = x.BusinessUserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    Admin = x.Administrator.UserName,
                    CorporateBrandDetails = x.CorporatingDetails,
                    CreatedOn = x.CreatedOn,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    IsActive = x.IsActive,
                    No_Month = x.No_Months,
                    ExpiresOn = x.ExpiresOn,
                    CompanyLogo = x.BussinessUser.CompanyLogo,
                    Phone=x.BussinessUser.Phone,
                    Address=x.BussinessUser.CoAddress,
                    TradeEmailId=x.BussinessUser.TradeEmaiIId,
                    EmailId=x.BussinessUser.EmailId
                }).SingleOrDefault();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CorporateBrandingModel> GetCorporateBrandingDetailsByUserId(int id)
        {
            try
            {
                return objdb.CorporateBrandings.Where(x => x.BusinessUserId == id).Select(x => new CorporateBrandingModel
                {
                    CorporateBrandingId = x.CorporateId,
                    BusinessUserId = x.BusinessUserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    Admin = x.Administrator.UserName,
                    CorporateBrandDetails = x.CorporatingDetails,
                    CreatedOn = x.CreatedOn,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    IsActive = x.IsActive,
                    No_Month = x.No_Months,
                    ExpiresOn = x.ExpiresOn,
                    CompanyLogo = x.BussinessUser.CompanyLogo,
                }).OrderByDescending(x=>x.CorporateBrandingId).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CorporateBrandingModel> GetAllCorporateBrandingList()
        {
            model = (from Co in objdb.CorporateBrandings
                     where Co.IsActive == true
                     select new CorporateBrandingModel
                     {
                         BusinessName = Co.BussinessUser.BusinessName,
                         BusinessUserId = Co.BusinessUserId,
                         CorporateBrandingId = Co.CorporateId,
                         CorporateBrandDetails = Co.CorporatingDetails,
                         CompanyLogo = Co.BussinessUser.CompanyLogo,
                         ApprovedOn = Co.ApprovedOn,
                         IsActive = Co.IsActive,
                         CreatedOn = Co.CreatedOn,
                         Admin = Co.Administrator.UserName,
                         No_Month = Co.No_Months,
                         ExpiresOn = Co.ExpiresOn,
                         //ActivatedOn = Co.ActivatedOn,
                         // ExpiresOn = Co.ExpiresOn,

                     }).OrderByDescending(x => x.CorporateBrandingId).ToList();
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

        public bool ChangeCorporateBrandingStatus(int id)
        {
            try
            {
                var obj = objdb.CorporateBrandings.Find(id);
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

        public bool ChangeCorporateBrandingApprovalStatus(int id)
        {
            try
            {
                var obj = objdb.CorporateBrandings.Find(id);
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
                var obj = objdb.CorporateBrandings.Find(id);
                if (obj != null)
                {
                    objdb.CorporateBrandings.Remove(obj);
                    objdb.SaveChanges();
                }
                return obj.CorporateId;
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
                return objdb.CorporateBrandings.Select(x => x.CorporateId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int GetPageCountByUserid(int id)
        {
            try
            {
                return objdb.CorporateBrandings.Select(x => x.BusinessUserId==id).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
    }
}
