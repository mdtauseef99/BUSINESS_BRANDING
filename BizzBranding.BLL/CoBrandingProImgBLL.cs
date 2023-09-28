using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
   public class CoBrandingProImgBLL
    {
       CoBrandingProImgDAL objproductimagedal = new CoBrandingProImgDAL();

       public List<CoBrandingProImgModel> GetAllCoBrandProductImages()
        {
            try
            {
                return objproductimagedal.GetAllCoBrandProductImages();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

       public List<CoBrandingProImgModel> GetAllCoBrandProductImages(int skip, int take, int cid)
        {
            try
            {
                return objproductimagedal.GetAllCoBrandProductImages(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


       public List<CoBrandingProImgModel> GetAllCoBrandProductImages(int skip, int take)
        {
            try
            {
                return objproductimagedal.GetAllCoBrandProductImages(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

       public int AddEditCoBrandProductImages(CoBrandingProImgModel objmodel)
        {
            try
            {
                return objproductimagedal.AddEditCoBrandProductImages(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

       public List<CoBrandingProImgModel> GetCoBrandProductImageById(int id)
        {
            try
            {
                return objproductimagedal.GetCoBrandProductImageById(id);
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
                return objproductimagedal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<CoBrandingProImgModel> GetProductImageByParent(int id)
        {
            try
            {
                return objproductimagedal.GetProductImageByParent(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CoBrandingProImgModel> GetParentProductImages()
        {
            try
            {
                return objproductimagedal.GetParentProductImages();
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
                return objproductimagedal.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
