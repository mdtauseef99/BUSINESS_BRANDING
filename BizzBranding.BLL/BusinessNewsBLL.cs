using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class BusinessNewsBLL
    {
        BusinessNewsDAL objdal = new BusinessNewsDAL();

        public int AddEditBusinessNews(BusinessNewsModel model)
        {
            try
            {
                return objdal.AddEditBusinessNews(model);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<BusinessNewsModel> GetAllBusinessNews()
        {
            try
            {
                return objdal.GetAllBusinessNews();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<BusinessNewsModel> GetAllBusinessNews(int skip, int take)
        {
            try
            {
                return objdal.GetAllBusinessNews(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
        public List<BusinessNewsModel> GetAllBusinessNews(int id,int skip, int take)
        {
            try
            {
                return objdal.GetAllBusinessNews(id,skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public BusinessNewsModel GetBusinessNewsById(int id)
        {
            try
            {
                return objdal.GetBusinessNewsById(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public BusinessNewsModel GetBusinessNewsByNewsId(int id)
        {
            try
            {
                return objdal.GetBusinessNewsByNewsId(id);
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
                return objdal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetPageCountByUserid(int id)
        {
            try
            {
                return objdal.GetPageCountByUserid(id);
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
                return objdal.ChangeStatus(id);
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
                return objdal.Remove(id);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
    }
}
