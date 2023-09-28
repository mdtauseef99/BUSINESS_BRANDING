using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class CityBLL
    {
        CityDAL objcitydal = new CityDAL();

        public List<CityModel> GetAllCity()
        {
            try
            {
                return objcitydal.GetAllCity();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CityModel> GetAllCity(int skip, int take, int cid)
        {
            try
            {
                return objcitydal.GetAllCity(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<CityModel> GetAllCity(int skip, int take)
        {
            try
            {
                return objcitydal.GetAllCity(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditCity(CityModel objmodel)
        {
            try
            {
                return objcitydal.AddEditCity(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public CityModel GetCityById(int id)
        {
            try
            {
                return objcitydal.GetCityById(id);
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
                return objcitydal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CityModel> GetCityByParent(int id)
        {
            try
            {
                return objcitydal.GetCityByParent(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CityModel> GetParentCities()
        {
            try
            {
                return objcitydal.GetParentCities();
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
                return objcitydal.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

    }
}
