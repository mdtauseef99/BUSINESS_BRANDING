using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
    public class CorporateBrandingBLL
    {

        CorporateBrandingDAL Objdal = new CorporateBrandingDAL();
        public int AddEditCorporateBranding(CorporateBrandingModel objmodel)
        {
            try
            {
                return Objdal.AddEditCorporateBranding(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<CorporateBrandingModel> GetAllCorporateBrandingList(int skip, int take)
        {
            try
            {
                return Objdal.GetAllCorporateBrandingList(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CorporateBrandingModel> GetAllCorporateBrandingList()
        {
            try
            {
                return Objdal.GetAllCorporateBrandingList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CorporateBrandingModel> GetCorporateBrandingDetailsByUserId(int id)
        {
            try
            {
                return Objdal.GetCorporateBrandingDetailsByUserId(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        

        public List<CorporateBrandingModel> GetAllCorporateBrandingList(int skip, int take, int cid)
        {
            try
            {
                return Objdal.GetAllCorporateBrandingList(skip, take, cid);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public CorporateBrandingModel GetCorporateBrandingDetailsById(int id)
        {
            try
            {
                return Objdal.GetCorporateBrandingDetailsById(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public bool ChangeCorporateBrandingStatus(int id)
        {
            try
            {
                return Objdal.ChangeCorporateBrandingStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool ChangeCorporateBrandingApprovalStatus(int id)
        {
            try
            {
                return Objdal.ChangeCorporateBrandingApprovalStatus(id);
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
                return Objdal.GetPageCountByUserid(id);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }
    }
}
