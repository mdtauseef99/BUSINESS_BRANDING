using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
    public class HomeBannerBLL
    {
        HomeBannerDAL objdal = new HomeBannerDAL();

        public int AddEditBanner(HomeBannerModel model)
        {
            try
            {
                return objdal.AddEditBanner(model);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<HomeBannerModel> GetAllBannerImages()
        {
            try
            {
                return objdal.GetAllBannerImages();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<HomeBannerModel> GetAllBannerImages(int skip, int take)
        {
            try
            {
                return objdal.GetAllBannerImages(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public HomeBannerModel GetBannerImageById(int id)
        {
            try
            {
                return objdal.GetBannerImageById(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<HomeBannerModel> GetBannerImageByIUserId(int skip, int take, int id)
        {
            try
            {
                return objdal.GetBannerImageByIUserId(skip,take,id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<HomeBannerModel> GetBannerImageByIUserId(int id)
        {
            try
            {
                return objdal.GetBannerImageByIUserId(id);
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

        public int GetPageCountByUserId(int id)
        {
            try
            {
                return objdal.GetPageCountByUserId(id);
            }
            catch (Exception)
            {
                return 0;
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

        //public List<HomeBanner> GetBannerBLL(int UserId)
        //{
        //    try
        //    {
        //        return objdal.GetBannerDAL(UserId);
        //    }
        //    catch (Exception)
        //    {
                
        //        throw;
        //    }
        //}

        public List<HomeBanner> GetBannerBLL()
        {
            try
            {
                return objdal.GetBannerDAL();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
