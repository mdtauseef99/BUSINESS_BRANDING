using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.DAL
{
    public class InvestorPartnerDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();
        List<InvestorPartneringModel> model = new List<InvestorPartneringModel>();

        public int AddEditInvestorPartnering(InvestorPartneringModel objmodel)
        {
            try
            {
                if (objmodel.InvestorPartnerId == 0)
                {
                    InvestorPartnering objInvestor = new InvestorPartnering
                    {
                        //FranchiseId = objmodel.FranchiseId,
                        BusinessUserId = objmodel.BusinessUserId,
                        CreatedOn = DateTime.Now,
                        IsActive = objmodel.IsActive,
                        Details = objmodel.Details,
                        ApprovedBy = objmodel.ApprovedBy,
                        ApprovedOn = DateTime.Now,
                        ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month)),
                        No_Months = objmodel.No_Month,
                    };
                    objdb.InvestorPartnerings.Add(objInvestor);
                    objdb.SaveChanges();
                    return objInvestor.InvestorPartneringId;
                }
                else
                {
                    var objInvestor = objdb.InvestorPartnerings.Find(objmodel.InvestorPartnerId);
                    objInvestor.BusinessUserId = objmodel.BusinessUserId;
                    objInvestor.InvestorPartneringId = objmodel.InvestorPartnerId;
                    objInvestor.ApprovedOn = DateTime.Now;
                    objInvestor.ApprovedBy = objmodel.ApprovedBy;
                    objInvestor.Details = objmodel.Details;
                    objInvestor.IsActive = objmodel.IsActive;
                    objInvestor.ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month));
                    objInvestor.No_Months = objmodel.No_Month;
                    objdb.SaveChanges();
                    return objmodel.InvestorPartnerId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<InvestorPartneringModel> GetAllInvestorPartnerList(int skip, int take)
        {
            try
            {
                model = (from ip in objdb.InvestorPartnerings
                         //where Fr.IsActive == true
                         select new InvestorPartneringModel
                         {
                             BusinessUserId = ip.BusinessUserId,
                             InvestorPartnerId = ip.InvestorPartneringId,
                             BusinessName = ip.BussinessUser.BusinessName,
                             Details = ip.Details,
                             ApprovedBy = ip.ApprovedBy,
                             Admin = ip.Administrator.UserName,
                             CreatedOn = ip.CreatedOn,
                             IsActive = ip.IsActive,
                             ApprovedOn = ip.ApprovedOn,
                             No_Month = ip.No_Months,
                             CompanyLogo = ip.BussinessUser.CompanyLogo,
                             ExpiresOn = ip.ExpiresOn,

                         }).OrderByDescending(x => x.InvestorPartnerId).Skip(skip).Take(take).ToList();
                return (model);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<InvestorPartneringModel> GetAllInvestorPartnerList(int skip, int take, int cid)
        {
            try
            {
                return objdb.InvestorPartnerings.Where(x => x.InvestorPartneringId == cid).Select(x => new InvestorPartneringModel
                {
                    InvestorPartnerId = x.InvestorPartneringId,
                    BusinessUserId = x.BusinessUserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    Details = x.Details,
                    Admin = x.Administrator.UserName,
                    CreatedOn = x.CreatedOn,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.InvestorPartnerId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public InvestorPartneringModel GetInvestorPartnerDetailsById(int id)
        {
            try
            {
                return objdb.InvestorPartnerings.Where(x => x.InvestorPartneringId== id).Select(x => new InvestorPartneringModel
                {
                    InvestorPartnerId = x.InvestorPartneringId,
                    BusinessUserId = x.BusinessUserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    Admin = x.Administrator.UserName,
                    Details = x.Details,
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

        public List<InvestorPartneringModel> GetInvestorPartnerDetailsByUserId(int id)
        {
            try
            {
                return objdb.InvestorPartnerings.Where(x => x.BusinessUserId == id).Select(x => new InvestorPartneringModel
                {
                    InvestorPartnerId = x.InvestorPartneringId,
                    BusinessUserId = x.BusinessUserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    Admin = x.Administrator.UserName,
                    Details = x.Details,
                    CreatedOn = x.CreatedOn,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    IsActive = x.IsActive,
                    No_Month = x.No_Months,
                    ExpiresOn = x.ExpiresOn,
                    CompanyLogo = x.BussinessUser.CompanyLogo,
                }).OrderByDescending(x=>x.InvestorPartnerId).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<InvestorPartneringModel> GetAllInvestorPartnerList()
        {
            model = (from Co in objdb.InvestorPartnerings
                     where Co.IsActive == true
                     select new InvestorPartneringModel
                     {
                         BusinessName = Co.BussinessUser.BusinessName,
                         BusinessUserId = Co.BusinessUserId,
                         InvestorPartnerId = Co.InvestorPartneringId,
                         Details = Co.Details,
                         CompanyLogo = Co.BussinessUser.CompanyLogo,
                         ApprovedOn = Co.ApprovedOn,
                         IsActive = Co.IsActive,
                         CreatedOn = Co.CreatedOn,
                         Admin = Co.Administrator.UserName,
                         No_Month = Co.No_Months,
                         ExpiresOn = Co.ExpiresOn,
                         //ActivatedOn = Co.ActivatedOn,
                         // ExpiresOn = Co.ExpiresOn,

                     }).OrderByDescending(x => x.InvestorPartnerId).ToList();
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

        public bool ChangeInvestorPartnerStatus(int id)
        {
            try
            {
                var obj = objdb.InvestorPartnerings.Find(id);
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

        public bool ChangeInvestorPartnerApprovalStatus(int id)
        {
            try
            {
                var obj = objdb.InvestorPartnerings.Find(id);
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
                var obj = objdb.InvestorPartnerings.Find(id);
                if (obj != null)
                {
                    objdb.InvestorPartnerings.Remove(obj);
                    objdb.SaveChanges();
                }
                return obj.InvestorPartneringId;
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
                return objdb.InvestorPartnerings.Select(x => x.InvestorPartneringId).Count();
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
                return objdb.InvestorPartnerings.Select(x => x.BusinessUserId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
    }
}
