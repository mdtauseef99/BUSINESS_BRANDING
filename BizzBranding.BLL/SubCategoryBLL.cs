using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;
namespace BizzBranding.BLL
{
    public class SubCategoryBLL
    {
        SubCategoryDAL objsubcategorydal = new SubCategoryDAL();

        public List<SubCategoryModel> GetAllSubCategory()
        {
            try
            {
                return objsubcategorydal.GetAllSubCategory();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<SubCategoryModel> GetAllSubCategory(int skip, int take, int cid)
        {
            try
            {
                return objsubcategorydal.GetAllSubCategory(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<SubCategoryModel> GetAllSubCategory(int skip, int take)
        {
            try
            {
                return objsubcategorydal.GetAllSubCategory(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditSubCategory(SubCategoryModel objmodel)
        {
            try
            {
                return objsubcategorydal.AddEditSubCategory(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public SubCategoryModel GetSubCategoryById(int id)
        {
            try
            {
                return objsubcategorydal.GetSubCategoryById(id);
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
                return objsubcategorydal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<SubCategoryModel> GetSubCategoryByParent(int id)
        {
            try
            {
                return objsubcategorydal.GetSubCategoryByParent(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<SubCategoryModel> GetParentCategories()
        {
            try
            {
                return objsubcategorydal.GetParentCategories();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public bool ChangeSubCatStatus(int id)
        {
            try
            {
                return objsubcategorydal.ChangeSubCatStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

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
