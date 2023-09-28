using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
     public class ProductBLL
     {
         ProductDAL objproductdal = new ProductDAL();

         public List<ProductModel> GetAllProduct()
         {
             try
             {
                 return objproductdal.GetAllProduct();
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
                 return objproductdal.GetAllProduct(skip, take, cid);
             }
             catch (Exception)
             {

                 throw;
             }
         }

         public List<ProductModel> GetAllProductByUserid(int skip, int take, int id)
         {
             try
             {
                 return objproductdal.GetAllProductByUserid(skip, take, id);
             }
             catch (Exception)
             {

                 throw;
             }
         }


         public List<ProductModel> GetAllProduct(int skip, int take)
         {
             try
             {
                 return objproductdal.GetAllProduct(skip, take);
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
                 return objproductdal.AddEditProduct(objmodel);
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
                 return objproductdal.GetProductById(id);
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
                 return objproductdal.GetPageCount();
             }
             catch (Exception)
             {

                 throw;
             }
         }

         public int GetPageCount(int userid)
         {
             try
             {
                 return objproductdal.GetPageCount(userid);
             }
             catch (Exception)
             {

                 throw;
             }
         }

         public List<ProductModel> GetProductByParent(int id)
         {
             try
             {
                 return objproductdal.GetProductByParent(id);
             }
             catch (Exception)
             {
                 return null;
                 throw;
             }
         }

         public int AddProductBLL(ProductModel model)
         {
             return objproductdal.AddProductDLL(model);
         }

         public int InsertProductImagesBLL(ProductImageModel model)
         {
             return objproductdal.InsertProductImagesDLL(model);
         }

         public bool ChangeStatus(int id)
         {
             try
             {
                 return objproductdal.ChangeStatus(id);
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
         //        return objproductdal.GetParentCategories();
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
         //        return objcoursedal.GetCourseByInstituteId(id);
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
         //        return objcoursedal.GetCourseByInstituteID(id);
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
         //        return objcoursedal.GetCourseByParentId(id);
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
         //        return new CourseDAL().getparentcoursenamebycourseid(id);
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
         //        return new CourseDAL { }.getparentcoursenamebyparentid(id);
         //    }
         //    catch (Exception)
         //    {

         //        throw;
         //    }
         //}

        
     }

}
