using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;
using System.Data;

namespace BizzBranding.BLL
{
    public class NewsLetterBLL
    {
        NewsLetterDAL objNewsLetterDAL = new NewsLetterDAL();

        public bool InsertNewsLetter(NewsLetterModel objNewsLetterModel)
        {
            try
            {
                return objNewsLetterDAL.InsertNewsLetter(objNewsLetterModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateNewsLetter(NewsLetterModel objNewsLetterModel)
        {
            try
            {
                return objNewsLetterDAL.UpdateNewsLetter(objNewsLetterModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DataTable GetAllNewsLetters()
        {
            DataTable dt = null;

            try
            {
                var list = objNewsLetterDAL.GetAllNewsLetters();
                dt = CommonClassUtility.LINQToDataTable(list);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public NewsLetterModel GetNewsLetterById(int id)
        {
            try
            {
                return objNewsLetterDAL.GetNewsLetterById(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditNewsLetter(NewsLetterModel model)
        {
            try
            {
                return objNewsLetterDAL.AddEditNewsLetter(model);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        //public DataTable GetNewsLetterById(int id)
        //{
        //    DataTable dt = null;

        //    try
        //    {
        //        dt = objNewsLetterDAL.GetNewsLetterById(id);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    return dt;
        //}

        public DataTable GetAllNewsLetterDetails(int intSkip, int pagesize, string type)
        {
            DataTable dt = null;

            try
            {
                dt = objNewsLetterDAL.GetAllNewsLetterDetails(intSkip, pagesize, type);
            }
            catch (Exception)
            {
                throw;
            }
            return dt;
        }

        public List<NewsLetterModel> GetAllNewsLetterDetails(int skip, int take)
        {
            try
            {
                return objNewsLetterDAL.GetAllNewsLetterDetails(skip, take);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetTotalRowsNewsLetter(string type)
        {
            try
            {
                return objNewsLetterDAL.GetTotalRowsNewsLetter(type);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetTotalRowsNewsLetter(DateTime fromdate, DateTime todate, bool status)
        {
            try
            {
                return objNewsLetterDAL.GetTotalRowsNewsLetter(fromdate, todate, status);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ChangeStatus(List<int> id, bool status)
        {
            try
            {
                return objNewsLetterDAL.ChangeStatus(id, status);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteNewsLetterData(List<int> Id)
        {
            try
            {
                return objNewsLetterDAL.DeleteNewsLetterData(Id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool? CheckStatus(int id)
        {
            try
            {
                return objNewsLetterDAL.CheckStatus(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetPageCount()
        {
            try
            {
                return objNewsLetterDAL.GetPageCount();
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
                return objNewsLetterDAL.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
