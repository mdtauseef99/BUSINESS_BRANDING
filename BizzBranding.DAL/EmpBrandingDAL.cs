using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.DAL
{
   public class EmpBrandingDAL
    {
       BizzBrandingEntities objdb = new BizzBrandingEntities();
       List<EmpBrandingModel> model = new List<EmpBrandingModel>();

       //public int AddEditCoBrandUsers(CoBrandModel objmodel)
       //{
       //    try
       //    {

       //        if (objmodel.CoBrandId == 0)
       //        {
       //            CoBranding objcob = new CoBranding
       //            {
       //                PartnerA = objmodel.PartnerA,
       //                PartnerB = objmodel.PartnerB,
       //                //CreatedDate = DateTime.Now,
       //                IsActive = objmodel.IsActive,
       //                IsAprroved = objmodel.IsApproved,
       //                CoBrandedName = objmodel.CoBrandedName,
       //                ActivatedOn = objmodel.ActivatedOn,
       //                ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month)),
       //                No_Months = objmodel.No_Month,
       //                ProductsDetails = objmodel.ProductDetails + objmodel.BusinessNameB,
       //            };
       //            objdb.CoBrandings.Add(objcob);
       //            objdb.SaveChanges();
       //            return objcob.CoBrandingId;
       //        }
       //        else
       //        {
       //            var objcob = objdb.CoBrandings.Find(objmodel.CoBrandId);
       //            objcob.PartnerA = objmodel.PartnerA;
       //            objcob.PartnerB = objmodel.PartnerB;
       //            objcob.IsAprroved = objmodel.IsApproved;
       //            objcob.CoBrandedName = objmodel.CoBrandedName;
       //            objcob.ProductsDetails = objmodel.ProductDetails;
       //            objcob.ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month));
       //            objcob.No_Months = objmodel.No_Month;
       //            objcob.IsActive = objmodel.IsActive;
       //            objdb.SaveChanges();
       //            return objmodel.CoBrandId;
       //        }
       //    }
       //    catch (Exception)
       //    {
       //        return 0;
       //        throw;
       //    }
       //}

       public int AddEditEmpBranding(EmpBrandingModel objmodel)
       {

           try
           {
               //if (objmodel.CountryId == 0)
               //{
               //    objmodel.CountryId =null;
               //}
               if (objmodel.EmpBrandingId == 0)
               {
                   EmployeeBranding objempbrand = new EmployeeBranding
                   {
                       EmployerId = objmodel.EmployerId,
                       EmpImage = objmodel.EmpImage,
                       EmpName=objmodel.EmpName,
                       Message=objmodel.Message,
                       CreatedOn = DateTime.Now,
                       IsActive = objmodel.IsActive,
                       No_Months=objmodel.No_Month
                   };
                   objdb.EmployeeBrandings.Add(objempbrand);
                   objdb.SaveChanges();
                   return objempbrand.EmpBrandingId;
               }
               else
               {
                   var objpro = objdb.EmployeeBrandings.Find(objmodel.EmpBrandingId);
                   objpro.EmpName = objmodel.EmpName;
                   //objpro.EmpBrandingId = objmodel.EmpBrandingId;
                   objpro.EmployerId = objmodel.EmployerId;
                   objpro.Message = objmodel.Message;
                   objpro.EmpImage = objmodel.EmpImage;
                   objpro.IsActive = objmodel.IsActive;
                   objpro.UpdatedOn = DateTime.Now;
                   //objpro.UpdatedBy = objmodel.EmployerId;
                   //objpro.No_Months = objmodel.No_Month;
                   objdb.SaveChanges();
                   return objmodel.EmpBrandingId;
               }
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       public List<EmpBrandingModel> GetAllEmpBranding()
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


           model = (from Co in objdb.EmployeeBrandings
                    //where Co.IsActive == false
                    select new EmpBrandingModel
                    {
                        EmpBrandingId = Co.EmpBrandingId,
                        EmployerId = Co.EmployerId,
                        EmployerName = Co.BussinessUser.BusinessName,
                        EmpImage=Co.EmpImage,
                        EmpName=Co.EmpName,
                        Message=Co.Message,
                        CreateOn = Co.CreatedOn,
                        IsActive=Co.IsActive,
                        CreatedBy=Co.EmployerId,
                        UpdatedOn=Co.UpdatedOn,
                        UpdateBy=Co.EmployerId,
                        No_Month=Co.No_Months

                    }).ToList();
           return (model);


       }

        public List<EmpBrandingModel> GetAllEmpBrandDetails()
       {
           model = (from Co in objdb.EmployeeBrandings
                    where Co.IsActive == true
                    select new EmpBrandingModel
                    {
                        EmployerId = Co.EmployerId,
                        EmpBrandingId = Co.EmpBrandingId,
                        EmployerName = Co.BussinessUser.BusinessName,
                        EmployerCompanyLogo=Co.BussinessUser.CompanyLogo,
                        EmpName = Co.EmpName,
                        Message=Co.Message,
                        EmpImage=Co.EmpImage,
                        CreatedBy=Co.CreatedBy,
                        CreateOn = Co.CreatedOn,
                        IsActive = Co.IsActive,
                        UpdateBy = Co.UpdatedBy,
                        UpdatedOn=Co.UpdatedOn,
                        No_Month=Co.No_Months
                    }).ToList();
           return (model);
       }

       public List<EmpBrandingModel> GetAllEmployeeBranding(int skip, int take)
       {
           try
           {
               model = (from Co in objdb.EmployeeBrandings
                        //where Co.IsActive == true
                        select new EmpBrandingModel
                        {
                            EmpBrandingId = Co.EmpBrandingId,
                            EmployerId = Co.EmployerId,
                            EmpName = Co.EmpName,
                            EmpImage=Co.EmpImage,
                            Message = Co.Message,
                            IsActive = Co.IsActive,
                            CreateOn=Co.CreatedOn,
                            UpdateBy=Co.UpdatedBy,
                            UpdatedOn=Co.UpdatedOn,
                            EmployerName=Co.BussinessUser.BusinessName,
                            No_Month=Co.No_Months,
                            Description=Co.Message

                        }).OrderByDescending(x => x.EmpBrandingId).Skip(skip).Take(take).ToList();
               return (model);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public EmpBrandingModel GetEmpBrandingById(int id)
       {
           try
           {
               return objdb.EmployeeBrandings.Where(x => x.EmpBrandingId == id).Select(x => new EmpBrandingModel
               {
                   EmpBrandingId = x.EmpBrandingId,
                   EmployerId = x.EmployerId,
                   EmployerName = x.BussinessUser.BusinessName,
                  // IsApproved = x.IsAprroved,
                   EmpName=x.EmpName,
                   No_Month=x.No_Months,
                   EmpImage=x.EmpImage,
                   Message=x.Message,
                   CreatedBy=x.CreatedBy,
                   CreateOn=x.CreatedOn,
                   UpdateBy=x.UpdatedBy,
                   UpdatedOn=x.UpdatedOn,
                   IsActive = x.IsActive,
               }).SingleOrDefault();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<EmpBrandingModel> GetEmpBrandingByUserId(int id)
       {
           try
           {
               return objdb.EmployeeBrandings.Where(x => x.EmployerId == id).Select(x => new EmpBrandingModel
               {
                   EmpBrandingId = x.EmpBrandingId,
                   EmployerId = x.EmployerId,
                   EmployerName = x.BussinessUser.BusinessName,
                   // IsApproved = x.IsAprroved,
                   EmpName = x.EmpName,
                   No_Month = x.No_Months,
                   EmpImage = x.EmpImage,
                   Message = x.Message,
                   CreatedBy = x.CreatedBy,
                   CreateOn = x.CreatedOn,
                   UpdateBy = x.UpdatedBy,
                   UpdatedOn = x.UpdatedOn,
                   IsActive = x.IsActive,
               }).OrderByDescending(x=>x.EmpBrandingId).ToList();
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
               return objdb.EmployeeBrandings.Select(x => x.EmpBrandingId).Count();
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       public int GetPageCountbyUserId(int id)
       {
           try
           {
               return objdb.EmployeeBrandings.Where(x=>x.EmployerId==id).Select(x => x.EmpBrandingId).Count();
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
               var obj = objdb.EmployeeBrandings.Find(id);
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
               var obj = objdb.EmployeeBrandings.Find(id);
               if (obj != null && obj.IsActive == true)
               {
                   obj.IsActive = false;
                   obj.IsActive = false;
                   objdb.SaveChanges();
                   return false;
               }
               else if (obj != null)
               {
                   obj.IsActive = true;
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
               var obj = objdb.EmployeeBrandings.Find(id);
               if (obj!=null)
               {
                   objdb.EmployeeBrandings.Remove(obj);
                   objdb.SaveChanges();
               }
               return obj.EmpBrandingId;
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       



    }
}
