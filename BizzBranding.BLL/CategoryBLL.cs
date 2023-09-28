using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class CategoryBLL
    {
        CategoryDAL objcategorydal = new CategoryDAL();

        public List<CategoryModel> GetAllCategory()
        {
            try
            {
                return objcategorydal.GetAllCategory();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CategoryModel> GetAllCategory(int skip, int take, int cid)
        {
            try
            {
                return objcategorydal.GetAllCategory(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<CategoryModel> GetAllCategory(int skip, int take)
        {
            try
            {
                return objcategorydal.GetAllCategory(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditCategory(CategoryModel objmodel)
        {
            try
            {
                return objcategorydal.AddEditCategory(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public CategoryModel GetCategoryById(int id)
        {
            try
            {
                return objcategorydal.GetCategoryById(id);
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
                return objcategorydal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CategoryModel> GetCategoryByParent(int id)
        {
            try
            {
                return objcategorydal.GetCategoryByParent(id);
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
                return objcategorydal.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
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
