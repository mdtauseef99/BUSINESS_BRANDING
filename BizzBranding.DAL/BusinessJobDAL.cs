using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.DAL
{
   public class BusinessJobDAL
    {
       BizzBrandingEntities objdb = new BizzBrandingEntities();

       public List<BusinessJobModel> GetAllJobs()
       {
           try
           {
               return objdb.BusinessJobs.Where(x => x.IsActive == true).Select(x => new BusinessJobModel
               {
                   BusinessJobId = x.BusinessJobId,
                   BusinessUserId = x.BusinessUserId,
                   JobTitle = x.JobTitle,
                   Description = x.Description,
                   CompanyLogo=x.BussinessUser.CompanyLogo,
                   //CreatedBy = x.CreatedBy,
                   IsActive = x.IsActive,
               }).ToList();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<BusinessJobModel> GetAllJobs(int skip, int take, int cid)
       {
           try
           {
               return objdb.BusinessJobs.Where(x => x.BusinessUserId == cid).Select(x => new BusinessJobModel
               {
                   BusinessJobId = x.BusinessJobId,
                   BusinessUserId = x.BusinessUserId,
                   JobTitle = x.JobTitle,
                   Description = x.Description,
                   CompanyLogo = x.BussinessUser.CompanyLogo,
                   //CreatedBy = x.CreatedBy,
                   IsActive = x.IsActive,
               }).OrderByDescending(x => x.BusinessJobId == cid).Skip(skip).Take(take).ToList();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<BusinessJobModel> GetAllJobs(int skip, int take)
       {
           try
           {
               return objdb.BusinessJobs.Where(x => x.BusinessJobId != null && x.BusinessUserId != null).Select(x => new BusinessJobModel
               {
                   BusinessJobId = x.BusinessJobId,
                   BusinessUserId = x.BusinessUserId,
                   JobTitle = x.JobTitle,
                   Description = x.Description,
                   CompanyLogo = x.BussinessUser.CompanyLogo,
                   //CreatedBy = x.CreatedBy,
                   IsActive = x.IsActive,
               }).OrderByDescending(x => x.BusinessJobId).Skip(skip).Take(take).ToList();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public BusinessJobModel GetJobDetailsById(int id)
       {
           try
           {
               return objdb.BusinessJobs.Where(x => x.BusinessJobId == id).Select(x => new BusinessJobModel
               {
                   BusinessUserId= x.BusinessUserId,
                   BusinessJobId = x.BusinessJobId,
                   Description = x.Description,
                   JobTitle = x.JobTitle,
                   CompanyLogo = x.BussinessUser.CompanyLogo,
                   IsActive = x.IsActive,
               }).SingleOrDefault();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }


       //public int AddEditBusinessJob(BusinessJobModel objmodel)
       //{

       //    try
       //    {
       //        //if (objmodel.CountryId == 0)
       //        //{
       //        //    objmodel.CountryId =null;
       //        //}
       //        if (objmodel.BusinessJobId == 0)
       //        {
       //            Product objpro = new Product
       //            {
       //                ProductName = objmodel.ProductName,
       //                //CreatedBy = objmodel.CreatedBy,
       //                CategoryId = objmodel.CategoryId,
       //                SubCategoryId = objmodel.SubCategoryId,
       //                ProdDetails = objmodel.ProdDetails,
       //                CreatedDate = DateTime.Now,
       //                IsActive = objmodel.IsActive
       //            };
       //            objdb.Products.Add(objpro);
       //            objdb.SaveChanges();
       //            return objpro.ProductId;
       //        }
       //        else
       //        {
       //            var objpro = objdb.Products.Find(objmodel.ProductId);
       //            objpro.ProductName = objmodel.ProductName;
       //            objpro.CategoryId = objmodel.CategoryId;
       //            objpro.SubCategoryId = objmodel.SubCategoryId;
       //            objpro.ProdDetails = objmodel.ProdDetails;
       //            objpro.IsActive = objmodel.IsActive;
       //            objdb.SaveChanges();
       //            return objmodel.ProductId;
       //        }
       //    }
       //    catch (Exception)
       //    {
       //        return 0;
       //        throw;
       //    }
       //}

       public int AddBusinessJob(BusinessJobModel model)
       {
           int res = 0;
           BusinessJob objJob = new BusinessJob();
           objJob.JobTitle = model.JobTitle;
           objJob.Description = model.Description;
           objJob.IsActive = true;
           objJob.BusinessUserId = model.BusinessUserId;
           objdb.BusinessJobs.Add(objJob);
           objdb.SaveChanges();
           res = objJob.BusinessJobId;
           return res;
       }

       public int AddEditBusinessJob(BusinessJobModel model)
       {
           try
           {
               if (model.BusinessJobId == 0)
               {//Insert BusinesNewss
                   BusinessJob Obj = new BusinessJob
                   {
                       JobTitle = model.JobTitle,
                       Description = model.Description,
                       IsActive = true,
                       BusinessUserId = model.BusinessUserId,
                   };
                   objdb.BusinessJobs.Add(Obj);
                   objdb.SaveChanges();
                   return Obj.BusinessJobId;
               }
               else
               {//Update Business News
                   int id = model.BusinessJobId;
                   BusinessJob obj = objdb.BusinessJobs.Find(id);
                   if (obj != null)
                   {
                       obj.JobTitle = model.JobTitle;
                       obj.Description = model.Description;
                       obj.IsActive = true;
                       obj.BusinessUserId = model.BusinessUserId;

                       objdb.SaveChanges();
                       return obj.BusinessJobId;
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

       public int RemoveBusinessJob(int id)
       {
           try
           {
               var obj = objdb.BusinessJobs.Find(id);
               if (obj != null)
               {
                   objdb.BusinessJobs.Remove(obj);
                   objdb.SaveChanges();
               }
               return obj.BusinessJobId;
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }
    }

}
