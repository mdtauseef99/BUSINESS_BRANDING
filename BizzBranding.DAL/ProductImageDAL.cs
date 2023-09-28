using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class ProductImageDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<ProductImageModel> GetAllProductImages()
        {
            try
            {
                return objdb.ProductImages.Where(x => x.IsActive == true).Select(x => new ProductImageModel
                {
                    ProductId = x.ProductId,
                    ProdImage = x.ProdImage,
                    ProductImageId = x.Id,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).ToList();
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
                return objdb.ProductImages.Where(x => x.ProductId != null&& x.Id == cid).Select(x => new ProductImageModel
                {
                    ProductId = x.ProductId,
                    ProdImage = x.ProdImage,
                    ProductImageId = x.Id,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.ProductImageId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<ProductImageModel> GetAllProductImages(int skip, int take)
        {
            try
            {
                return objdb.ProductImages.Select(x => new ProductImageModel
                {
                    ProductId = x.ProductId,
                    ProdImage = x.ProdImage,
                    ProductImageId = x.Id,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.ProductImageId).Skip(skip).Take(take).ToList();
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
                //if (objmodel.CountryId == 0)
                //{
                //    objmodel.CountryId =null;
                //}
                if (objmodel.ProductImageId == 0)
                {
                    ProductImage objpro = new ProductImage
                    {
                        Id = objmodel.ProductImageId,
                        //CreatedBy = objmodel.CreatedBy,
                        ProductId = objmodel.ProductId,
                        ProdImage = objmodel.ProdImage,
                        CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.ProductImages.Add(objpro);
                    objdb.SaveChanges();
                    return objpro.Id;
                }
                else
                {
                    var objpro = objdb.ProductImages.Find(objmodel.ProductId);
                    objpro.ProdImage = objmodel.ProdImage;
                    objpro.ProductId = objmodel.ProductId;
                    objpro.Id= objmodel.ProductImageId;
                    objpro.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.ProductImageId;
                }
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
                return objdb.ProductImages.Where(x => x.ProductId== id).Select(x => new ProductImageModel
                {
                    ProductImageId= x.Id,
                    ProdImage = x.ProdImage,
                    ProductId = x.ProductId,
                    ProductName = x.Product.ProductName,
                    CreatedDate = x.CreatedDate,
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
                return objdb.ProductImages.Where(x => x.ProductId!= null)
                            .Select(x => x.Id).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<ProductImageModel> GetProductImageByParent(int id)
        {
            try
            {
                return objdb.ProductImages.Where(x => x.Id == id).Select(x => new ProductImageModel
                {
                    ProductId = x.ProductId,
                }).ToList();
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
                return objdb.ProductImages.Where(x => x.IsActive == true).Select(x => new ProductImageModel
                {
                    ProductImageId = x.Id,
                    ProdImage = x.ProdImage,
                    //CreatedBy = x.CreatedBy,
                    ProductId = x.ProductId,
                    ProductName= x.Product.ProductName,
                    CreatedDate = x.CreatedDate,
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
                var obj = objdb.ProductImages.Find(id);
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
