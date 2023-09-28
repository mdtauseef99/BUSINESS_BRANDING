using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class ProductDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<ProductModel> GetAllProduct()
        {
            try
            {
                return objdb.Products.Where(x => x.IsActive == true).Select(x => new ProductModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    CategoryId = x.CategoryId,
                    SubCategoryId = x.SubCategoryId,
                    ProdDetails = x.ProdDetails,
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

        public List<ProductModel> GetAllProduct(int skip, int take, int cid)
        {
            try
            {
                return objdb.Products.Where(x => x.CategoryId != null && x.SubCategoryId != null && x.ProductId == cid).Select(x => new ProductModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    CategoryId = x.CategoryId,
                    SubCategoryId = x.SubCategoryId,
                    ProdDetails = x.ProdDetails,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.ProductId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<ProductModel> GetAllProductByUserid(int skip, int take, int id)
        {
            try
            {
                return objdb.Products.Where(x => x.CategoryId != null && x.SubCategoryId != null).Select(x => new ProductModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    CategoryId = x.CategoryId,
                    SubCategoryId = x.SubCategoryId,
                    ProdDetails = x.ProdDetails,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).Where(x=>x.UserId==id).OrderByDescending(x => x.ProductId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<ProductModel> GetAllProduct(int skip, int take)
        {
            try
            {
                return objdb.Products.Where(x => x.CategoryId != null && x.SubCategoryId != null).Select(x => new ProductModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    CategoryId = x.CategoryId,
                    SubCategoryId = x.SubCategoryId,
                    ProdDetails = x.ProdDetails,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.ProductId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditProduct(ProductModel objmodel)
        {

            try
            {
                //if (objmodel.CountryId == 0)
                //{
                //    objmodel.CountryId =null;
                //}
                if (objmodel.ProductId == 0)
                {
                    Product objpro = new Product
                    {
                        ProductName = objmodel.ProductName,
                        //CreatedBy = objmodel.CreatedBy,
                        CategoryId = objmodel.CategoryId,
                        SubCategoryId = objmodel.SubCategoryId,
                        ProdDetails = objmodel.ProdDetails,
                        CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.Products.Add(objpro);
                    objdb.SaveChanges();
                    return objpro.ProductId;
                }
                else
                {
                    var objpro = objdb.Products.Find(objmodel.ProductId);
                    objpro.ProductName = objmodel.ProductName;
                    objpro.CategoryId = objmodel.CategoryId;
                    objpro.SubCategoryId= objmodel.SubCategoryId;
                    objpro.ProdDetails = objmodel.ProdDetails;
                    objpro.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.ProductId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public ProductModel GetProductById(int id)
        {
            try
            {
                return objdb.Products.Where(x => x.ProductId== id).Select(x => new ProductModel
                {
                    SubCategoryId = x.SubCategoryId,
                    ProductName = x.ProductName,
                    CategoryId = x.CategoryId,
                    ProductId = x.ProductId,
                    ProdDetails = x.ProdDetails,
                    CategoryName = x.Category.CategoryName,
                    SubCategoryName= x.SubCategory.SubCatgoryName,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).SingleOrDefault();
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
                return objdb.Products.Where(x => x.Category != null)
                            .Select(x => x.ProductId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int GetPageCount(int userid)
        {
            try
            {
                return objdb.Products.Where(x => x.UserId == userid)
                            .Select(x => x.ProductId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<ProductModel> GetProductByParent(int id)
        {
            try
            {
                return objdb.Products.Where(x => x.ProductId== id).Select(x => new ProductModel
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddProductDLL(ProductModel model)
        {
            int res = 0;
            Product objPro = new Product(); 
            objPro.ProductName = model.ProductName;
            objPro.ProdDetails = model.ProdDetails;
            objPro.IsActive = true;
            objPro.CreatedDate = DateTime.Now;
            objPro.UserId = model.UserId;
            objdb.Products.Add(objPro);
            objdb.SaveChanges();
            res = objPro.ProductId;
            return res;
        }

        public int InsertProductImagesDLL(ProductImageModel model)
        {
            int res = 0;
            ProductImage pimage = new ProductImage();
            pimage.ProdImage = model.ProdImage;
            pimage.ProductId = model.ProductId;
            pimage.CreatedDate = model.CreatedDate;
            pimage.IsActive = model.IsActive;
            objdb.ProductImages.Add(pimage);
            objdb.SaveChanges();
            return res;
        }

      

        public bool ChangeStatus(int id)
        {
            try
            {
                var obj = objdb.Products.Find(id);
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
       

       













        //public List<ProductModel> GetParentCategories()
        //{
        //    try
        //    {
        //        return objdb.SubCategories.Where(x => x.IsActive == true).Select(x => new ProductModel
        //        {
        //            SubCategoryId = x.SubCategoryId,
        //            SubCatgoryName = x.SubCatgoryName,
        //            //CreatedBy = x.CreatedBy,
        //            ParentCategoryId = x.ParentCategoryId,
        //            CategoryName = x.Category.CategoryName,
        //            //CreatedDate = x.CreatedDate,
        //            IsActive = x.IsActive
        //        }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //        throw;
        //    }
        //}

        //public List<CourseInstituteMappingModel> GetCourseByInstituteId(int id)
        //{
        //    try
        //    {
        //        return objdb.CourseInstituteMappings.Where(x => x.InstituteID == id).Select(x => new CourseInstituteMappingModel
        //        {
        //            CourseID = x.CourseID,
        //            InstituteID = x.InstituteID,
        //            CourseName = x.Course.CourseName
        //        }).ToList();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<CourseInstituteMappingModel> GetCourseByInstituteID(int id)
        //{
        //    try
        //    {
        //        List<CourseInstituteMappingModel> courselist = new List<CourseInstituteMappingModel>();
        //        var list = objdb.CourseInstituteMappings.Where(x => x.InstituteID == id).Select(x => new CourseInstituteMappingModel
        //        {
        //            CourseID = x.CourseID,
        //            InstituteID = x.InstituteID

        //        }).ToList();
        //        foreach (var item in list)
        //        {
        //            courselist.Add(objdb.Courses.Where(x => x.CourseID == item.CourseID).Select(x => new CourseInstituteMappingModel
        //            {
        //                CourseID = x.CourseID,
        //                CourseName = x.CourseName

        //            }).SingleOrDefault());
        //        }
        //        return courselist;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<CourseModel> GetCourseByParentId(int id)
        //{
        //    try
        //    {
        //        var list = objdb.Courses.Where(x => x.ParentCourseID == id && x.IsActive == true).Select(x => new CourseModel
        //        {
        //            CourseID = x.CourseID,
        //            CourseName = x.CourseName,
        //        }).ToList();
        //        return list;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public CourseModel getparentcoursenamebycourseid(int id)
        //{
        //    try
        //    {
        //        var list = objdb.Courses.Where(x => x.CourseID == id && x.IsActive == true).Select(x => new CourseModel
        //        {
        //            CourseID = x.CourseID,
        //            ParentCourseID = x.ParentCourseID,
        //            CourseName = x.CourseName,
        //        }).FirstOrDefault();
        //        return list;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public CourseModel getparentcoursenamebyparentid(int id)
        //{
        //    try
        //    {
        //        //var test objdb.Courses.Where (x=>x.CourseID==
        //        var list = objdb.Courses.Where(x => x.CourseID == id && x.IsActive == true).FirstOrDefault();

        //        var test = objdb.Courses.Where(x => x.CourseID == list.ParentCourseID).Select(x => new CourseModel
        //        {
        //            CourseID = x.CourseID,
        //            ParentCourseID = x.ParentCourseID,
        //            CourseName = x.CourseName,
        //        }).FirstOrDefault();
        //        return test;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}



       
    }

}
