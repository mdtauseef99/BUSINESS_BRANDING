using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.DAL
{
    public class HomeBannerDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public int AddEditBanner(HomeBannerModel model)
        {
            try
            {
                if (model.HomeBannerID == 0)
                {//Insert BusinesNewss
                    HomeBanner BannerObj = new HomeBanner
                    {
                        BannerImage = model.BannerImage,
                        UserId = model.UserId,
                        IsActive = false,
                        HomeBannerId = model.HomeBannerID,
                        URL=model.CompanyURL
                    };
                    objdb.HomeBanners.Add(BannerObj);
                    objdb.SaveChanges();
                    return BannerObj.HomeBannerId;
                }
                else
                {//Update Business News
                    int id = model.HomeBannerID;
                    HomeBanner obj = objdb.HomeBanners.Find(id);
                    if (obj != null)
                    {
                        obj.BannerImage = model.BannerImage;
                        obj.UserId = model.UserId;
                        obj.IsActive = false;
                        obj.URL = model.CompanyURL;
                       // obj.HomeBannerId = id;
                        objdb.SaveChanges();
                        return obj.HomeBannerId;
                    }
                    return 0;
                }
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
                return objdb.HomeBanners.Where(x => x.IsActive == true).Select(x => new HomeBannerModel
                {
                    BannerImage = x.BannerImage,
                    HomeBannerID = x.HomeBannerId,
                    IsActive = x.IsActive,
                    UserId = x.UserId,
                    CompanyURL=x.URL,
                    BusinessName=x.BussinessUser.BusinessName
                }).ToList();
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
                return objdb.HomeBanners.Select(x => new HomeBannerModel
                {
                    BannerImage = x.BannerImage,
                    HomeBannerID = x.HomeBannerId,
                    IsActive = x.IsActive,
                    CompanyURL=x.URL,
                    BusinessName=x.BussinessUser.BusinessName,
                    UserId = x.UserId
                }).OrderByDescending(x=>x.UserId).Skip(skip).Take(take).ToList();
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
                return objdb.HomeBanners.Where(x => x.HomeBannerId == id).Select(x => new HomeBannerModel
                {
                    BannerImage = x.BannerImage,
                    HomeBannerID = x.HomeBannerId,
                    IsActive = x.IsActive,
                    CompanyURL=x.URL,
                    UserId = x.UserId,
                    BusinessName=x.BussinessUser.BusinessName
                }).SingleOrDefault();
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
                return objdb.HomeBanners.Where(x => x.UserId == id).Select(x => new HomeBannerModel
                {
                    BannerImage = x.BannerImage,
                    HomeBannerID = x.HomeBannerId,
                    IsActive = x.IsActive,
                    UserId = x.UserId,
                    CompanyURL=x.URL,
                    BusinessName=x.BussinessUser.BusinessName
                }).ToList();
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
                return objdb.HomeBanners.Where(x => x.UserId == id).Select(x => new HomeBannerModel
                {
                    BannerImage = x.BannerImage,
                    HomeBannerID = x.HomeBannerId,
                    IsActive = x.IsActive,
                    UserId = x.UserId,
                    CompanyURL=x.URL,
                    
                    BusinessName = x.BussinessUser.BusinessName
                }).ToList();
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
                return objdb.HomeBanners.Where(x => x.HomeBannerId!=null)
                            .Select(x => x.HomeBannerId).Count();
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
                return objdb.HomeBanners.Where(x => x.UserId==id)
                            .Select(x => x.HomeBannerId).Count();
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
                var obj = objdb.HomeBanners.Find(id);
                if (obj != null && obj.IsActive == true)
                {
                    obj.IsActive = false;
                    objdb.SaveChanges();
                    return false;
                }
                else if (obj != null)
                {
                    obj.IsActive = true;
                    objdb.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }

        //public List<HomeBanner> GetBannerDAL(int UserId)
        //{
        //    try
        //    {
        //        List<HomeBanner> obj = new List<HomeBanner>();
        //        obj = objdb.HomeBanners.Where(x => x.UserId == UserId && x.IsActive==true).Take(6).ToList();
        //        return obj;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }

        //}
        public List<HomeBanner> GetBannerDAL()
        {
            try
            {
                List<HomeBanner> obj = new List<HomeBanner>();
                obj = objdb.HomeBanners.Where(x =>  x.IsActive == true).Take(4).ToList();
                return obj;
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
}
