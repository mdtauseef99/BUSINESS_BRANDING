using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.DAL
{
   public class CoBrandingProImgDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<CoBrandingProImgModel> GetAllCoBrandProductImages()
        {
            try
            {
                return objdb.CoBrandingImages.Where(x => x.IsActive == true).Select(x => new CoBrandingProImgModel
                {
                    Id = x.Id,
                    CoBrandingId = x.CoBrandingId,
                    CoBrandProdImage = x.CoBrandingImage1,
                    //CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    IsActive = x.IsActive,
                }).ToList();
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
                return objdb.CoBrandingImages.Where(x => x.Id != null && x.Id == cid).Select(x => new CoBrandingProImgModel
                {
                    Id = x.Id,
                    CoBrandProdImage = x.CoBrandingImage1,
                    CoBrandingId = x.CoBrandingId,
                    //CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.Id == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CoBrandingProImgModel> GetAllCoBrandProductImages(int skip, int take)
        {
            try
            {
                return objdb.CoBrandingImages.Select(x => new CoBrandingProImgModel
                {
                    Id = x.Id,
                    CoBrandProdImage = x.CoBrandingImage1,
                    CoBrandingId = x.CoBrandingId,
                    //CreatedBy = x.CreatedBy,
                    CreatedOn = x.CreatedOn,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.Id).Skip(skip).Take(take).ToList();
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
                //if (objmodel.CountryId == 0)
                //{
                //    objmodel.CountryId =null;
                //}
                if (objmodel.Id == 0)
                {
                    CoBrandingImage obBrandImg = new CoBrandingImage
                    {
                        Id = objmodel.Id,
                        //CreatedBy = objmodel.CreatedBy,
                        CoBrandingId = objmodel.CoBrandingId,
                        CoBrandingImage1 = objmodel.CoBrandProdImage,
                        CreatedOn = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.CoBrandingImages.Add(obBrandImg);
                    objdb.SaveChanges();
                    return obBrandImg.Id;
                }
                else
                {
                    var objpro = objdb.CoBrandingImages.Find(objmodel.Id);
                    objpro.CoBrandingImage1 = objmodel.CoBrandProdImage;
                    objpro.CoBrandingId = objmodel.CoBrandingId;
                    objpro.Id = objmodel.Id;
                    objpro.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.Id;
                }
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
                return objdb.CoBrandingImages.Where(x => x.CoBrandingId == id).Select(x => new CoBrandingProImgModel
                {
                    Id = x.Id,
                    CoBrandProdImage = x.CoBrandingImage1,
                    CoBrandingId = x.CoBrandingId,
                    //ProductName = x.Product.ProductName,
                    CreatedOn = x.CreatedOn,
                    IsActive = x.IsActive,
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
                return objdb.CoBrandingImages.Where(x => x.Id != null)
                            .Select(x => x.Id).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<CoBrandingProImgModel> GetProductImageByParent(int id)
        {
            try
            {
                return objdb.CoBrandingImages.Where(x => x.Id == id).Select(x => new CoBrandingProImgModel
                {
                    CoBrandingId = x.CoBrandingId,
                }).ToList();
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
                return objdb.CoBrandingImages.Where(x => x.IsActive == true).Select(x => new CoBrandingProImgModel
                {
                    Id = x.Id,
                    CoBrandProdImage = x.CoBrandingImage1,
                    //CreatedBy = x.CreatedBy,
                    CoBrandingId = x.CoBrandingId,
                    //ProductName = x.Product.ProductName,
                    CreatedOn = x.CreatedOn,
                    IsActive = x.IsActive
                }).ToList();
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
                var obj = objdb.CoBrandingImages.Find(id);
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
    }
}
