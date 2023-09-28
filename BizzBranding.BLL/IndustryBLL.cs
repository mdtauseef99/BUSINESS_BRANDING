using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class IndustryBLL
    {
        IndustryDAL objindustrydal = new IndustryDAL();

        public List<IndustryModel> GetAllIndustry()
        {
            try
            {
                return objindustrydal.GetAllIndustry();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<IndustryModel> GetAllIndustry(int skip, int take, int cid)
        {
            try
            {
                return objindustrydal.GetAllIndustry(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<IndustryModel> GetAllIndustry(int skip, int take)
        {
            try
            {
                return objindustrydal.GetAllIndustry(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditIndustry(IndustryModel objmodel)
        {
            try
            {
                return objindustrydal.AddEditIndustry(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public IndustryModel GetIndustryById(int id)
        {
            try
            {
                return objindustrydal.GetIndustryById(id);
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
                return objindustrydal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<IndustryModel> GetIndustryByParent(int id)
        {
            try
            {
                return objindustrydal.GetIndustryByParent(id);
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
                return objindustrydal.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }


    }

}
