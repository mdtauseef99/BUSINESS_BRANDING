using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class AccessPlanBLL
    {
        AccessPlanDAL objaccessplandal = new AccessPlanDAL();

        public List<AccessPlanModel> GetAllAccessPlan()
        {
            try
            {
                return objaccessplandal.GetAllAccessPlan();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<AccessPlanModel> GetAllAccessPlan(int skip, int take, int cid)
        {
            try
            {
                return objaccessplandal.GetAllAccessPlan(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<AccessPlanModel> GetAllAccessPlan(int skip, int take)
        {
            try
            {
                return objaccessplandal.GetAllAccessPlan(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditAccessPlan(AccessPlanModel objmodel)
        {
            try
            {
                return objaccessplandal.AddEditAccessPlan(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public AccessPlanModel GetAccessPlanById(int id)
        {
            try
            {
                return objaccessplandal.GetAccessPlanById(id);
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
                return objaccessplandal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ChangeStatus(int id)
        {
            try
            {
                return objaccessplandal.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

    

































        //public List<AccessPlanModel> GetCategoryByParent(int id)
        //{
        //    try
        //    {
        //        return objaccessplandal.GetCategoryByParent(id);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //        throw;
        //    }
        //}

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
