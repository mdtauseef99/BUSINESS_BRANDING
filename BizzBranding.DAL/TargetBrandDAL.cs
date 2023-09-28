using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
   public class TargetBrandDAL
    {
       BizzBrandingEntities objdb = new BizzBrandingEntities();
       List<TargetBrandingModel> model = new List<TargetBrandingModel>();
       public int AddEditTargetBranding(TargetBrandingModel objmodel)
       {

           try
           {
               //if (objmodel.CountryId == 0)
               //{
               //    objmodel.CountryId =null;
               //}
               if (objmodel.TargetBrandingId == 0)
               {
                   TargetBranding objTarget = new TargetBranding
                   {
                       CreatedUserId = objmodel.BusinessUserId,
                       BusinessName = objmodel.BusinessName,
                       CreatedDate = objmodel.CreatedDate,
                       ExpiresOn=objmodel.ExpiresOn,
                       IsActive = objmodel.IsActive,
                       No_Months = objmodel.No_Month,
                       CityId=objmodel.CityId,
                       IndustryId=objmodel.IndustryId,
                       URL=objmodel.URL,
                       
                   };
                   objdb.TargetBrandings.Add(objTarget);
                   objdb.SaveChanges();
                   return objTarget.TargetBrandingId;
               }
               else
               {
                   var objpro = objdb.TargetBrandings.Find(objmodel.TargetBrandingId);
                   objpro.BusinessName = objmodel.BusinessName;
                   //objpro.EmpBrandingId = objmodel.EmpBrandingId;
                   objpro.CreatedUserId = objmodel.BusinessUserId;
                   objpro.IsActive = objmodel.IsActive;
                   objpro.UpdatedDate = DateTime.Now.Date;
                   objpro.IndustryId = objmodel.IndustryId;
                  // objpro.No_Months = objmodel.No_Month;
                   objpro.CreatedDate = objmodel.CreatedDate;
                   objpro.ExpiresOn = objmodel.ExpiresOn;
                   objpro.CityId = objmodel.CityId;
                   objpro.URL = objmodel.URL;
                   objdb.SaveChanges();
                   return objmodel.TargetBrandingId;
               }
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       public List<TargetBrandingModel> GetAllTargetedBrandUsers(int skip, int take)
       {
           try
           {
               model = (from Fr in objdb.TargetBrandings
                        //where Fr.IsActive == true
                        select new TargetBrandingModel
                        {
                            BusinessName = Fr.BussinessUser.BusinessName,
                            BusinessUserId = Fr.BussinessUser.UserId,
                            TargetBrandingId = Fr.TargetBrandingId,
                            CityId = Fr.CityId,
                            CityName = Fr.City.CityName,
                            IndustryName = Fr.Industry.IndustryName,
                            IndustryId=Fr.IndustryId,
                            IsActive = Fr.IsActive,
                            CreatedDate = Fr.CreatedDate,
                            No_Month = Fr.No_Months,
                            URL=Fr.URL,
                            ExpiresOn=Fr.ExpiresOn
                            
                        }).OrderByDescending(x => x.TargetBrandingId).Skip(skip).Take(take).ToList();
               return (model);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<TargetBrandingModel> GetAllTargetedBrandUsers(int skip, int take, int cid)
       {
           try
           {
               return objdb.TargetBrandings.Where(x => x.CreatedUserId == cid).Select(x => new TargetBrandingModel
               {
                    BusinessName = x.BussinessUser.BusinessName,
                    BusinessUserId = x.BussinessUser.UserId,
                    TargetBrandingId = x.TargetBrandingId,
                    CityId = x.CityId,
                    CityName = x.City.CityName,
                    IndustryName = x.Industry.IndustryName,
                    IndustryId = x.IndustryId,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                    No_Month = x.No_Months,
                    URL = x.URL,
                    ExpiresOn=x.ExpiresOn,
               }).OrderByDescending(x => x.TargetBrandingId).Skip(skip).Take(take).ToList();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<TargetBrandingModel> GetAllTargetedBrandUsers()
       {
           try
           {
               return objdb.TargetBrandings.Where(x => x.CreatedUserId!=null).Select(x => new TargetBrandingModel
               {
                   BusinessName = x.BussinessUser.BusinessName,
                   BusinessUserId = x.BussinessUser.UserId,
                   TargetBrandingId = x.TargetBrandingId,
                   CityId = x.CityId,
                   CityName = x.City.CityName,
                   IndustryName = x.Industry.IndustryName,
                   IndustryId = x.IndustryId,
                   IsActive = x.IsActive,
                   CreatedDate = x.CreatedDate,
                   No_Month = x.No_Months,
                   URL = x.URL,
                   ExpiresOn = x.ExpiresOn,
               }).OrderByDescending(x => x.TargetBrandingId).ToList();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       //public List<TargetBrandingModel> GetTargetBrandImageById(int id)
       //{
       //    try
       //    {
       //        return objdb.TargetedImages.Where(x => x.TargetBrandingId == id).Select(x => new TargetBrandingModel
       //        {
       //            BusinessName = x.TargetBranding.BussinessUser.BusinessName,
       //            BusinessUserId=x.TargetBranding.CreatedUserId,
       //            Image=x.Image,

       //        }).ToList();
       //    }
       //    catch (Exception)
       //    {
       //        return null;
       //        throw;
       //    }
       //}

       public TargetBrandingModel GetTargetBrandUsersById(int id)
       {
           try
           {
               return objdb.TargetBrandings.Where(x => x.CreatedUserId == id).Select(x => new TargetBrandingModel
               {
                   TargetBrandingId = x.TargetBrandingId,
                   IndustryId = x.IndustryId,
                   CityId = x.CityId,
                   CreatedDate = x.CreatedDate,
                   UpdateDate = x.UpdatedDate,
                   No_Month = x.No_Months,
                   BusinessUserId = x.CreatedUserId,
                   BusinessName = x.BussinessUser.BusinessName,
                   URL = x.URL,
                   ExpiresOn=x.ExpiresOn,
                   //TradeEmailId = x.BussinessUser.TradeEmaiIId,
                   //EmailId = x.BussinessUser.EmailId,
                   //Address = x.BussinessUser.CoAddress,
                   //Phone = x.BussinessUser.Phone,
                   //CreatedDate = x.CreatedDate,
                   IsActive = x.IsActive,
               }).SingleOrDefault();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public TargetBrandingModel GetTargetBrandByImageId(int id)
       {
           try
           {
               return objdb.TargetBrandings.Where(x => x.TargetBrandingId == id).Select(x => new TargetBrandingModel
               {
                   TargetBrandingId = x.TargetBrandingId,
                   IndustryId = x.IndustryId,
                   CityId = x.CityId,
                   CreatedDate = x.CreatedDate,
                   UpdateDate = x.UpdatedDate,
                   No_Month = x.No_Months,
                   BusinessUserId = x.CreatedUserId,
                   BusinessName = x.BussinessUser.BusinessName,
                   CityName=x.City.CityName,
                   IndustryName=x.Industry.IndustryName,
                   URL = x.URL,
                   ExpiresOn=x.ExpiresOn,
                   TradeEmaiIId = x.BussinessUser.TradeEmaiIId,
                   EmailId = x.BussinessUser.EmailId,
                   CoAddress = x.BussinessUser.CoAddress,
                   Phone = x.BussinessUser.Phone,
                   IsActive = x.IsActive,
                   Fax=x.BussinessUser.Fax
               }).SingleOrDefault();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       //public IList<TargetBrandingModel> GetTargetBrandDataById(int id)
       //{
       //    try
       //    {
       //        return objdb.TargetBrandings.Where(x => x.CreatedUserId == cid).Select(x => new TargetBrandingModel
       //        {
       //            BusinessName = x.BussinessUser.BusinessName,
       //            BusinessUserId = x.BussinessUser.UserId,
       //            TargetBrandingId = x.TargetBrandingId,
       //            CityId = x.CityId,
       //            CityName = x.City.CityName,
       //            IndustryName = x.Industry.IndustryName,
       //            IndustryId = x.IndustryId,
       //            IsActive = x.IsActive,
       //            CreatedDate = x.CreatedDate,
       //            No_Month = x.No_Months,
       //            Image = x.Image,
       //            URL = x.URL
       //        }).OrderByDescending(x => x.TargetBrandingId).Skip(skip).Take(take).ToList();
       //    }
       //    catch (Exception)
       //    {
       //        return null;
       //        throw;
       //    }
       //}

       public int GetPageCount()
       {
           try
           {
               return objdb.TargetBrandings.Select(x => x.TargetBrandingId).Count();
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
               var obj = objdb.TargetBrandings.Find(id);
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

       public int Remove(int id)
       {
           try
           {
               var obj = objdb.TargetBrandings.Find(id);
               if (obj != null)
               {
                   objdb.TargetBrandings.Remove(obj);
                   objdb.SaveChanges();
               }
               return obj.TargetBrandingId;
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

    }
}
