using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class CountryBLL
    {
        CountryDAL objcountrydal = new CountryDAL();

        public List<CountryModel> GetAllCountry()
        {
            try
            {
                return objcountrydal.GetAllCountry();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CountryModel> GetAllCountry(int skip, int take, int cid)
        {
            try
            {
                return objcountrydal.GetAllCountry(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<CountryModel> GetAllCountry(int skip, int take)
        {
            try
            {
                return objcountrydal.GetAllCountry(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditCountry(CountryModel objmodel)
        {
            try
            {
                return objcountrydal.AddEditCountry(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public CountryModel GetCountryById(int id)
        {
            try
            {
                return objcountrydal.GetCountryById(id);
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
                return objcountrydal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CountryModel> GetCountryByParent(int id)
        {
            try
            {
                return objcountrydal.GetCountryByParent(id);
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
                return objcountrydal.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

    }
}
