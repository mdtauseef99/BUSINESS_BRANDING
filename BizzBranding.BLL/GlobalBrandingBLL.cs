using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
   public class GlobalBrandingBLL
    {

       GlobalBrandigDAL Objdal = new GlobalBrandigDAL();
       public int AddEditGlobalBranding(GlobalBrandingModel objmodel)
        {
            try
            {
                return Objdal.AddEditGlobalBranding(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

       public List<GlobalBrandingModel> GetAllGlobalBrandingList(int skip, int take)
        {
            try
            {
                return Objdal.GetAllGlobalBrandingList(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

       public List<GlobalBrandingModel> GetAllGlobalBrandingList()
        {
            try
            {
                return Objdal.GetAllGlobalBrandingList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

       public List<GlobalBrandingModel> GetAllGlobalBrandingList(int skip, int take, int cid)
        {
            try
            {
                return Objdal.GetAllGlobalBrandingList(skip, take, cid);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public GlobalBrandingModel GetGlobalBrandingDetailsById(int id)
        {
            try
            {
                return Objdal.GetGlobalBrandingDetailsById(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<GlobalBrandingModel> GetGlobalBrandingDetailsByUserId(int id)
        {
            try
            {
                return Objdal.GetGlobalBrandingDetailsByUserId(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }
       
        public bool ChangeGlobalBrandingStatus(int id)
        {
            try
            {
                return Objdal.ChangeGlobalBrandingStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool ChangeGlobalBrandingApprovalStatus(int id)
        {
            try
            {
                return Objdal.ChangeGlobalBrandingApprovalStatus(id);
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
                return Objdal.Remove(id);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int GetPageCount()
        {
            try
            {
                return Objdal.GetPageCount();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int GetPageCountByUserId(int id)
        {
            try
            {
                return Objdal.GetPageCountByUserId(id);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
    }
}
