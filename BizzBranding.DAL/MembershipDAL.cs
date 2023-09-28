using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class MembershipDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<MembershipModel> GetAllMembership()
        {
            try
            {
                return objdb.Memberships.Where(x => x.IsActive == true).Select(x => new MembershipModel
                {
                    MembershipID= x.MembershipID,
                    AccessPlanId = x.AccessPlanId,
                    UserId=x.UserId,
                    ExpiresOn = x.ExpiresOn,
                    ActivatedOn = x.ActivatedOn,
                    UpdatedDate = x.UpdatedDate,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<MembershipModel> GetAllMembership(int skip, int take, int cid)
        {
            try
            {
                return objdb.Memberships.Where(x => x.AccessPlanId != null &&x.UserId != null&& x.MembershipID == cid).Select(x => new MembershipModel
                {
                    BusinessName=x.BussinessUser.BusinessName,
                    AccessPlanName=x.AccessPlan.AccessPlanName,
                    MembershipID = x.MembershipID,
                    AccessPlanId = x.AccessPlanId,
                    UserId = x.UserId,
                    ExpiresOn = x.ExpiresOn,
                    ActivatedOn = x.ActivatedOn,
                    UpdatedDate = x.UpdatedDate,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.MembershipID == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<MembershipModel> GetAllMembership(int skip, int take)
        {
            try
            {
                return objdb.Memberships.Where(x => x.AccessPlanId != null).Select(x => new MembershipModel
                {
                    BusinessName = x.BussinessUser.BusinessName,
                    AccessPlanName = x.AccessPlan.AccessPlanName,
                    MembershipID = x.MembershipID,
                    AccessPlanId = x.AccessPlanId,
                    UserId = x.UserId,
                    ExpiresOn = x.ExpiresOn,
                    ActivatedOn = x.ActivatedOn,
                    UpdatedDate = x.UpdatedDate,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate =x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.MembershipID).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditMembership(MembershipModel objmodel)
        {

            try
            {

                if (objmodel.MembershipID == 0)
                {
                    Membership objmem = new Membership
                    {
                        ActivatedOn = objmodel.ActivatedOn,
                        ExpiresOn = objmodel.ExpiresOn,
                        AccessPlanId = objmodel.AccessPlanId,
                        UserId = objmodel.UserId,
                        UpdatedDate = objmodel.UpdatedDate,
                        //CreatedBy = x.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive,
                    };
                    objdb.Memberships.Add(objmem);
                    objdb.SaveChanges();
                    return objmem.MembershipID;
                }
                else
                {
                    var objmem = objdb.Memberships.Find(objmodel.MembershipID);
                    objmem.ActivatedOn = objmodel.ActivatedOn;
                    objmem.ExpiresOn = objmodel.ExpiresOn;
                    objmem.AccessPlanId = objmodel.AccessPlanId;
                    objmem.UserId = objmodel.UserId;
                    objmem.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.MembershipID;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public MembershipModel GetMembershipById(int id)
        {
            try
            {
                return objdb.Memberships.Where(x => x.MembershipID == id).Select(x => new MembershipModel
                {
                    MembershipID = x.MembershipID,
                    ActivatedOn = x.ActivatedOn,
                    ExpiresOn = x.ExpiresOn,
                    AccessPlanId = x.AccessPlanId,
                    AccessPlanName=x.AccessPlan.AccessPlanName,
                    UserId = x.UserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    UpdatedDate = x.UpdatedDate,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                   
                }).SingleOrDefault();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int GetPageCount()
        {
            try
            {
                return objdb.Memberships.Where(x => x.UserId != null&&x.AccessPlanId!=null)
                            .Select(x => x.MembershipID).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<MembershipModel> GetMembershipByParent(int id)
        {
            try
            {
                return objdb.Memberships.Where(x => x.MembershipID == id).Select(x => new MembershipModel
                {
                    MembershipID = x.MembershipID,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<MembershipModel> GetAccessPlanMembership()
        {
            try
            {
                return objdb.Memberships.Where(x => x.IsActive == true).Select(x => new MembershipModel
                {
                    MembershipID= x.MembershipID,
                    //CreatedBy = x.CreatedBy,
                    AccessPlanId = x.AccessPlanId,
                    AccessPlanName= x.AccessPlan.AccessPlanName,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<MembershipModel> GetBusinessUserMembership()
        {
            try
            {
                return objdb.Memberships.Where(x => x.IsActive == true).Select(x => new MembershipModel
                {
                    MembershipID= x.MembershipID,
                    //CreatedBy = x.CreatedBy,
                    UserId = x.UserId,
                    BusinessName = x.BussinessUser.BusinessName,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public bool ChangeStatus(int id)
        {
            try
            {
                var obj = objdb.Memberships.Find(id);
                if (obj != null && obj.IsActive == true)
                {
                    obj.IsActive = false;
                    objdb.SaveChanges();
                    return false;
                }
                else if (obj != null)
                {
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

        public bool UpgradeMembership(int id)
        {
            bool res = false;

            var mem = objdb.Memberships.Where(x => x.UserId == id).FirstOrDefault();
            if (mem != null)
            {
                var AccessPlan = objdb.AccessPlans.Where(x => x.AccessPlanId == 20).FirstOrDefault();
                mem.AccessPlanId = AccessPlan.AccessPlanId;
                mem.ExpiresOn = DateTime.Now.AddDays(AccessPlan.Validity);
                mem.UpdatedDate = DateTime.Now;
                objdb.SaveChanges();
                return true;

            }
            else
            {
                return false;
            }
        }

        public AccessPlan GetAccessPlanById(int id)
        {
            var objAccPlan = objdb.AccessPlans.Where(s => s.AccessPlanId == id).FirstOrDefault();
            return objAccPlan;
        }

        public MembershipWithAccessPlanModel CheckMembership(int id)
        {

            var mem = objdb.Memberships.Where(x => x.UserId == id).Select(x => new MembershipWithAccessPlanModel
            {
                MembershipFee = x.AccessPlan.MembershipFee,
                UpdatesPerMonth = x.AccessPlan.UpdatesPerMonth,
                NewsCharge = x.AccessPlan.NewsCharge,
                AddPostCharge = x.AccessPlan.AddPostCharge,
                AddUpdCharge = x.AccessPlan.AddUpdCharge,
                MaxNoDeals = x.AccessPlan.MaxNoDeals,
                Validity = x.AccessPlan.Validity,
                CurrencyId = x.AccessPlan.CurrencyId,
                ProdPerMonth = x.AccessPlan.ProdPerMonth,

                UserId = x.UserId,
                ActivatedOn = x.ActivatedOn,
                ExpiresOn = x.ExpiresOn,
                AccessPlanId = x.AccessPlanId,
                MembershipID = x.MembershipID,

            }).FirstOrDefault();

            return mem;
        }

        public bool RenewBannerValidityDAL(HomeBannerMappingModel hModel)
        {
            bool res = false;
            try
            {
                HomeBannerMapping objmodel = new HomeBannerMapping();
                objmodel.AccessPlanId = hModel.AccessPlanId;
                objmodel.UserId = hModel.UserId;
                objmodel.ValidityStart = hModel.ValidityStart;
                objmodel.ValidityEnd = hModel.ValidityEnd;
                objmodel.IsActive = hModel.IsActive;
                objdb.HomeBannerMappings.Add(objmodel);
                objdb.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return res;
                throw;
            }

           
        }

        public bool RenewNewsValidityDAL(NewsMappingModel hModel)
        {
            bool res = false;
            try
            {
                NewsMapping objmodel = new NewsMapping();
                objmodel.AccessPlanId = hModel.AccessPlanId;
                objmodel.UserId = hModel.UserId;
                objmodel.ValidityStart = hModel.ValidityStart;
                objmodel.ValidityEnd = hModel.ValidityEnd;
                objmodel.IsActive = hModel.IsActive;
                objdb.NewsMappings.Add(objmodel);
                objdb.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return res;
                throw;
            }


        }

        public HomeBannerMapping GetHomeBannerValidityDAL()
        {
            try
            {
                DateTime CompareDate = DateTime.Now.Date;
                HomeBannerMapping objmodel = (from home in objdb.HomeBannerMappings where home.ValidityStart <= CompareDate && home.ValidityEnd >= CompareDate && home.IsActive==true select home).FirstOrDefault();
                return objmodel;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public NewsMapping GetHomeNewsListDAL()
        {
            try
            {DateTime CompareDate = DateTime.Now.Date;
                NewsMapping objnews = (from news in objdb.NewsMappings where news.ValidityStart<=CompareDate && news.ValidityEnd>=CompareDate select news).FirstOrDefault();
                return objnews;
            }
            catch (Exception)
            {

                throw;
            }
        }

    }

}
