using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
    public class NewsBLL
    {
        NewsDAL objdal = new NewsDAL();

        public int AddEditNews(NewsModel model)
        {
            try
            {
                return objdal.AddEditNews(model);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<NewsModel> GetAllNews()
        {
            try
            {
                return objdal.GetAllNews();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<NewsModel> GetAllNews(int skip, int take)
        {
            try
            {
                return objdal.GetAllNews(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public NewsModel GetNewsById(int id)
        {
            try
            {
                return objdal.GetNewsById(id);
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
    }
}
