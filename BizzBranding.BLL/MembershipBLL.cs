using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class MembershipBLL
    {
        MembershipDAL objmembershipdal = new MembershipDAL();

        public List<MembershipModel> GetAllMembership()
        {
            try
            {
                return objmembershipdal.GetAllMembership();
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
                return objmembershipdal.GetAllMembership(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<MembershipModel> GetAllMembership(int skip, int take)
        {
            try
            {
                return objmembershipdal.GetAllMembership(skip, take);
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
                return objmembershipdal.AddEditMembership(objmodel);
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
                return objmembershipdal.GetMembershipById(id);
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
                return objmembershipdal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<MembershipModel> GetMembershipByParent(int id)
        {
            try
            {
                return objmembershipdal.GetMembershipByParent(id);
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
                return objmembershipdal.GetAccessPlanMembership();
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
                return objmembershipdal.GetBusinessUserMembership();
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
                return objmembershipdal.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool UpgradeMembership(int id)
        {
            return objmembershipdal.UpgradeMembership(id);
        }

        public AccessPlan GetAccessPlanByIdBLL(int AccId)
        {
            return objmembershipdal.GetAccessPlanById(AccId);
        }

        public MembershipWithAccessPlanModel CheckMembership(int userid)
        {
            return objmembershipdal.CheckMembership(userid);
        }

        public bool RenewBannerValidityBLL(HomeBannerMappingModel hModel)
        {
            return objmembershipdal.RenewBannerValidityDAL(hModel);
        }

        public bool RenewNewsValidityBLL(NewsMappingModel hModel)
        {
            return objmembershipdal.RenewNewsValidityDAL(hModel);
        }

        public HomeBannerMapping GetHomeBannerValidityBLL()
        {
            return objmembershipdal.GetHomeBannerValidityDAL();
        }

        public NewsMapping GetHomeNewsListBLL()
        {
            return objmembershipdal.GetHomeNewsListDAL();
        }
























        //public List<CountryModel> GetParentCities()
        //{
        //    try
        //    {
        //        return objcoursedal.GetParentCities();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //        throw;
        //    }
        //}

        //public List<CourseInstituteMappingModel> GetCourseByInstituteId(int id)
        //{
        //    try
        //    {
        //        return objcoursedal.GetCourseByInstituteId(id);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<CourseInstituteMappingModel> GetCourseByInstituteID(int id)
        //{
        //    try
        //    {
        //        return objcoursedal.GetCourseByInstituteID(id);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<CourseModel> GetCourseByParentId(int id)
        //{
        //    try
        //    {
        //        return objcoursedal.GetCourseByParentId(id);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public CourseModel getparentcoursenamebycourseid(int id)
        //{
        //    try
        //    {
        //        return new CourseDAL().getparentcoursenamebycourseid(id);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public CourseModel getparentcoursenamebyparentid(int id)
        //{
        //    try
        //    {
        //        return new CourseDAL { }.getparentcoursenamebyparentid(id);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}
    }

}
