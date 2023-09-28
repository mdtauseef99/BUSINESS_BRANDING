using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.DAL
{
   
   public class CoBrandDAL
    {
       BizzBrandingEntities objdb = new BizzBrandingEntities();

       //public List<CoBrandModel> GetAllCoBrandUsers()
       //{
       //    try
       //    {
       //        return objdb.CoBrandings.Where(x => x.IsActive == true).Select(x => new CoBrandModel
       //        {

       //            CoBrandId = x.CoBrandingId,
       //            BusinessNameA = x.BussinessUser.BusinessName,
       //            BusinessNameB=x.BussinessUser1.BusinessName,
       //            PartnerA = x.PartnerA,
       //            PartnerB = x.PartnerB,
       //            //PartnerA = x.BussinessUser.UserId,
       //            //PartnerB = x.BussinessUser.UserId,
       //           // ActivatedOn = x.ActivatedOn,
       //            //UpdatedDate = x.UpdatedDate,
       //            //CreatedBy = x.CreatedBy,
       //            //CreatedDate = x.CreatedDate,
       //            IsActive = x.IsActive,
       //        }).ToList();
       //    }
       //    catch (Exception)
       //    {
       //        return null;
       //        throw;
       //    }
       //}
       List<CoBrandModel> model = new List<CoBrandModel>();
       public List<CoBrandModel> GetAllCoBrandUsers()
       {
           
           //int userid = Convert.ToInt32(Session["UserId"]);
           // string Emailid = Convert.ToString(Session["EmailId"]);
           //model = (from bu in objdb.BussinessUsers
           //         join Co in objdb.CoBrandings on bu.UserId equals Co.PartnerA
           //         where Co.PartnerA == userid || Co.PartnerB == userid && Co.IsActive == false
           //         select new LandingPageModel
           //         {
           //             BusinessName = bu.BusinessName,
           //             CompanyLogo = bu.CompanyLogo,
           //             PartnerId = Co.PartnerB,
           //             UserId = bu.UserId

           //         }).ToList();
           //return (model);


           model = (from Co in objdb.CoBrandings
                    //where Co.IsActive == false
                    select new CoBrandModel
                    {
                        PartnerA = Co.PartnerA,
                        PartnerB = Co.PartnerB,
                        BusinessNameA = Co.BussinessUser.BusinessName,
                        BusinessNameB=Co.BussinessUser1.BusinessName,
                        CoBrandedName=Co.CoBrandedName,
                        CoBrandId = Co.CoBrandingId,
                        IsActive=Co.IsActive,
                        IsApproved=Co.IsAprroved,
                        ActivatedOn=Co.ActivatedOn,
                        ExpiresOn=Co.ExpiresOn

                    }).ToList();
           return (model);


       }

       public List<CoBrandModel> GetAllCoBrandUsersDetails()
       {
           model = (from Co in objdb.CoBrandings
                    let x = objdb.CoBrandingImages.Where(pi => pi.CoBrandingId == Co.CoBrandingId).FirstOrDefault()
                    //join cp in objdb.CoBrandingImages on Co.CoBrandingId equals cp.CoBrandingId
                    where Co.IsAprroved == true
                    select new CoBrandModel
                    {
                        PartnerA = Co.PartnerA,
                        PartnerB = Co.PartnerB,
                        BusinessNameA = Co.BussinessUser.BusinessName,
                        BusinessNameB = Co.BussinessUser1.BusinessName,
                        CompanyLogo = Co.BussinessUser.CompanyLogo,
                        CompanyLogoB = Co.BussinessUser1.CompanyLogo,
                        CoBrandId = Co.CoBrandingId,
                        IsActive = Co.IsActive,
                        IsApproved = Co.IsAprroved,
                        CoBrandedName = Co.CoBrandedName,
                        CoProductImages = x.CoBrandingImage1,
                        ProductDetails=Co.ProductsDetails,
                        ActivatedOn = Co.ActivatedOn,
                        ExpiresOn = Co.ExpiresOn,
                        CompanyDetailsA=Co.BussinessUser.BusinessDetails,
                        CompanyDetailsB=Co.BussinessUser1.BusinessDetails,
                    }).OrderByDescending(x=>x.CoBrandId).ToList();

           ////foreach (var item in model)
           //{
           //    for (int i = 0; i < cbImages.ToArray().Length; i++)
           //    {
           //        item.CoProductImages = cbImages[i];
           //    }
           //}
           return (model);
       }

       public List<CoBrandModel> GetAllCoBrandUsers(int skip, int take)
       {
           try
           {
               model = (from Co in objdb.CoBrandings
                        //where Co.IsActive == true
                        select new CoBrandModel
                        {
                            PartnerA = Co.PartnerA,
                            PartnerB = Co.PartnerB,
                            BusinessNameA = Co.BussinessUser.BusinessName,
                            BusinessNameB = Co.BussinessUser1.BusinessName,
                            CoBrandId = Co.CoBrandingId,
                            IsActive = Co.IsActive,
                            IsApproved = Co.IsAprroved,
                            CoBrandedName=Co.CoBrandedName,
                            ActivatedOn=Co.ActivatedOn,
                            ExpiresOn=Co.ExpiresOn,
                            Phone=Co.BussinessUser.Phone,
                            TradeEmailId=Co.BussinessUser.TradeEmaiIId,
                            EmailId=Co.BussinessUser.EmailId,
                            Address=Co.BussinessUser.CoAddress

                        }).OrderByDescending(x=>x.CoBrandId).Skip(skip).Take(take).ToList();
               return (model);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public int AddEditCoBrandUsers(CoBrandModel objmodel)
       {
           try
           {

               if (objmodel.CoBrandId== 0)
               {
                   CoBranding objcob = new CoBranding
                   {
                       PartnerA = objmodel.PartnerA,
                       PartnerB = objmodel.PartnerB,
                       //CreatedDate = DateTime.Now,
                       IsActive = objmodel.IsActive,
                       IsAprroved = objmodel.IsApproved,
                       CoBrandedName=objmodel.CoBrandedName,
                       ActivatedOn=objmodel.ActivatedOn,
                       ExpiresOn=DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month)),
                       No_Months=objmodel.No_Month,
                       ProductsDetails=objmodel.ProductDetails+objmodel.BusinessNameB,
                   };
                   objdb.CoBrandings.Add(objcob);
                   objdb.SaveChanges();
                   return objcob.CoBrandingId;
               }
               else
               {
                   var objcob = objdb.CoBrandings.Find(objmodel.CoBrandId);
                   objcob.PartnerA = objmodel.PartnerA;
                   objcob.PartnerB = objmodel.PartnerB;
                   objcob.IsAprroved = objmodel.IsApproved;
                   objcob.CoBrandedName = objmodel.CoBrandedName;
                   objcob.ProductsDetails = objmodel.ProductDetails;
                   objcob.ExpiresOn=DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month));
                   objcob.No_Months = objmodel.No_Month;
                   objcob.IsActive = objmodel.IsActive;
                   objdb.SaveChanges();
                   return objmodel.CoBrandId;
               }
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       public CoBrandModel GetCoBrandUsersById(int id)
       {
           try
           {
               return objdb.CoBrandings.Where(x => x.CoBrandingId == id).Select(x => new CoBrandModel
               {
                   CoBrandId = x.CoBrandingId,
                   PartnerA = x.PartnerA,
                   PartnerB = x.PartnerB,
                   IsApproved = x.IsAprroved,
                   BusinessNameA=x.BussinessUser.BusinessName,
                   BusinessNameB=x.BussinessUser1.BusinessName,
                   CoBrandedName=x.CoBrandedName,
                   ActivatedOn=x.ActivatedOn,
                   ExpiresOn=x.ExpiresOn,
                   ProductDetails=x.ProductsDetails,
                   No_Month=x.No_Months,
                   TradeEmailId=x.BussinessUser.TradeEmaiIId,
                   EmailId=x.BussinessUser.EmailId,
                   Address=x.BussinessUser.CoAddress,
                   Phone=x.BussinessUser.Phone,
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

       public int GetPageCount()
       {
           try
           {
               return objdb.CoBrandings.Select(x => x.CoBrandingId).Count();
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
               var obj = objdb.CoBrandings.Find(id);
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

       public bool ChangeApprovalStatus(int id)
       {
           try
           {
               var obj = objdb.CoBrandings.Find(id);
               if (obj != null && obj.IsAprroved == true)
               {
                   obj.IsAprroved = false;
                   obj.IsActive = false;
                   objdb.SaveChanges();
                   return false;
               }
               else if (obj != null)
               {
                   obj.IsAprroved = true;
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
               var obj = objdb.CoBrandings.Find(id);
               if (obj!=null)
               {
                   objdb.CoBrandings.Remove(obj);
                   objdb.SaveChanges();
               }
               return obj.CoBrandingId;
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

    }
}
