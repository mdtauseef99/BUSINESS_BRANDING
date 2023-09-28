using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class BusinessJobBLL
    {
        BusinessJobDAL objdal = new BusinessJobDAL();

        public List<BusinessJobModel> GetAllJobs()
        {
            try
            {
                return objdal.GetAllJobs();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<BusinessJobModel> GetAllJobs(int skip, int take, int cid)
        {
            try
            {
                return objdal.GetAllJobs(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BusinessJobModel> GetAllJobs(int skip, int take)
        {
            try
            {
                return objdal.GetAllJobs(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public BusinessJobModel GetJobDetailsById(int id)
        {
            try
            {
                return objdal.GetJobDetailsById(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddBusinessJob(BusinessJobModel model)
        {
            return objdal.AddBusinessJob(model);
        }

        public int AddEditBusinessJob(BusinessJobModel model)
        {
            try
            {
                return objdal.AddEditBusinessJob(model);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int RemoveBusinessJob(int id)
        {
            try
            {
                return objdal.RemoveBusinessJob(id);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }


    }
}
