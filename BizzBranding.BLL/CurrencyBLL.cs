using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class CurrencyBLL
    {
        CurrencyDAL objcurrencydal = new CurrencyDAL();

        public List<CurrencyModel> GetAllCurrency()
        {
            try
            {
                return objcurrencydal.GetAllCurrency();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CurrencyModel> GetAllCurrency(int skip, int take, int cid)
        {
            try
            {
                return objcurrencydal.GetAllCurrency(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<CurrencyModel> GetAllCurrency(int skip, int take)
        {
            try
            {
                return objcurrencydal.GetAllCurrency(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditCurrency(CurrencyModel objmodel)
        {
            try
            {
                return objcurrencydal.AddEditCurrency(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public CurrencyModel GetCurrencyById(int id)
        {
            try
            {
                return objcurrencydal.GetCurrencyById(id);
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
                return objcurrencydal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CurrencyModel> GetCurrencyByParent(int id)
        {
            try
            {
                return objcurrencydal.GetCurrencyByParent(id);
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
                return objcurrencydal.ChangeStatus(id);
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
