using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;
namespace BizzBranding.BLL
{
    public class CMSBLL
    {
        CMSDAL objdal = new CMSDAL();

        public int AddEditCms(CMSModel model)
        {
            try
            {
                return objdal.AddEditCms(model);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
            
        }

        public List<CMSModel> GetAllCms()
        {
            try
            {
                return objdal.GetAllCms();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        public List<CMSModel> GetAllCms(int skip, int take)
        {
            try
            {
                return objdal.GetAllCms(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CMSModel> GetAllCms(int skip, int take,int cid)
        {
            try
            {
                return objdal.GetAllCms(skip, take,cid);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public CMSModel GetCmsById(int id)
        {
            try
            {
                return objdal.GetCmsById(id);
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
