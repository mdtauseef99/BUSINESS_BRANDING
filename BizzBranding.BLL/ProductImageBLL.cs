using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class ProductImageBLL
    {
        ProductImageDAL objproductimagedal = new ProductImageDAL();

        public List<ProductImageModel> GetAllProductImages()
        {
            try
            {
                return objproductimagedal.GetAllProductImages();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<ProductImageModel> GetAllProductImages(int skip, int take, int cid)
        {
            try
            {
                return objproductimagedal.GetAllProductImages(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<ProductImageModel> GetAllProductImages(int skip, int take)
        {
            try
            {
                return objproductimagedal.GetAllProductImages(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditProductImages(ProductImageModel objmodel)
        {
            try
            {
                return objproductimagedal.AddEditProductImages(objmodel);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<ProductImageModel> GetProductImageById(int id)
        {
            try
            {
                return objproductimagedal.GetProductImageById(id);
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

        public List<ProductImageModel> GetProductImageByParent(int id)
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

        public List<ProductImageModel> GetParentProductImages()
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
