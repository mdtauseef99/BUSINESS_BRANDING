using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.DAL
{
   public class FranchiseDAL
    {
       BizzBrandingEntities objdb = new BizzBrandingEntities();
       List<FranchiseModel> model = new List<FranchiseModel>();
       public int AddEditFranchise(FranchiseModel objmodel)
       {
           try
           {
               if (objmodel.FranchiseId == 0)
               {
                   Franchise objFranchise = new Franchise
                   {
                       //FranchiseId = objmodel.FranchiseId,
                       BusinessId = objmodel.BusinessId,
                       CreatedOn = DateTime.Now,
                       IsActive = objmodel.IsActive,
                       Details = objmodel.Details,
                       ApprovedBy = objmodel.ApprovedBy,
                       ApprovedOn =  DateTime.Now,
                       ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month)),
                       No_Months = objmodel.No_Month,
                   };
                   objdb.Franchises.Add(objFranchise);
                   objdb.SaveChanges();
                   return objFranchise.FranchiseId;
               }
               else
               {
                   var objFranchise = objdb.Franchises.Find(objmodel.FranchiseId);
                   objFranchise.BusinessId = objmodel.BusinessId;
                   objFranchise.FranchiseId = objmodel.FranchiseId;
                   objFranchise.ApprovedOn = DateTime.Now;
                   objFranchise.ApprovedBy = objmodel.ApprovedBy;
                   objFranchise.Details = objmodel.Details;
                   objFranchise.IsActive = objmodel.IsActive;
                   objFranchise.ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(objmodel.No_Month));
                   objFranchise.No_Months = objmodel.No_Month;
                   objdb.SaveChanges();
                   return objmodel.FranchiseId;
               }
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       public List<FranchiseModel> GetAllFranchiseeList(int skip, int take)
       {
           try
           {
               model = (from Fr in objdb.Franchises
                        //where Fr.IsActive == true
                        select new FranchiseModel
                        {
                            BusinessId = Fr.BusinessId,
                            FranchiseId = Fr.FranchiseId,
                            BusinessName = Fr.BussinessUser.BusinessName,
                            Details=Fr.Details,
                            ApprovedBy = Fr.ApprovedBy,
                            Admin=Fr.Administrator.UserName,
                            CreatedOn = Fr.CreatedOn,
                            IsActive = Fr.IsActive,
                            ApprovedOn = Fr.ApprovedOn,
                            No_Month=Fr.No_Months,
                            CompanyLogo=Fr.BussinessUser.CompanyLogo,
                            ExpiresOn=Fr.ExpiresOn,

                        }).OrderByDescending(x => x.FranchiseId).Skip(skip).Take(take).ToList();
               return (model);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<FranchiseModel> GetAllFranchiseeList(int skip, int take, int cid)
       {
           try
           {
               return objdb.Franchises.Where(x => x.FranchiseId == cid).Select(x => new FranchiseModel
               {
                   FranchiseId = x.FranchiseId,
                   BusinessId = x.BusinessId,
                   BusinessName = x.BussinessUser.BusinessName,
                   Details=x.Details,
                   Admin = x.Administrator.UserName,
                   CreatedOn = x.CreatedOn,
                   ApprovedOn = x.ApprovedOn,
                   ApprovedBy = x.ApprovedBy,
                   IsActive = x.IsActive,
               }).OrderByDescending(x => x.FranchiseId == cid).Skip(skip).Take(take).ToList();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public FranchiseModel GetFranchiserDetailsById(int id)
       {
           try
           {
               return objdb.Franchises.Where(x => x.FranchiseId == id).Select(x => new FranchiseModel
               {
                   FranchiseId = x.FranchiseId,
                   BusinessId = x.BusinessId,
                   BusinessName = x.BussinessUser.BusinessName,
                   Admin = x.Administrator.UserName,
                   Details = x.Details,
                   CreatedOn = x.CreatedOn,
                   ApprovedOn = x.ApprovedOn,
                   ApprovedBy = x.ApprovedBy,
                   IsActive = x.IsActive,
                   No_Month=x.No_Months,
                   Phone=x.BussinessUser.Phone,
                   EmailId=x.BussinessUser.EmailId,
                   Address=x.BussinessUser.CoAddress,
                   TradeEmailId=x.BussinessUser.TradeEmaiIId,
                   ExpiresOn=x.ExpiresOn,
                   
                   CompanyLogo=x.BussinessUser.CompanyLogo,
               }).SingleOrDefault();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<FranchiseModel> GetFranchiserDetailsByUserId(int id)
       {
           try
           {
               return objdb.Franchises.Where(x => x.BusinessId == id).Select(x => new FranchiseModel
               {
                   FranchiseId = x.FranchiseId,
                   BusinessId = x.BusinessId,
                   BusinessName = x.BussinessUser.BusinessName,
                   Admin = x.Administrator.UserName,
                   Details = x.Details,
                   CreatedOn = x.CreatedOn,
                   ApprovedOn = x.ApprovedOn,
                   ApprovedBy = x.ApprovedBy,
                   IsActive = x.IsActive,
                   No_Month = x.No_Months,
                   ExpiresOn = x.ExpiresOn,
                   CompanyLogo = x.BussinessUser.CompanyLogo,
               }).OrderByDescending(x=>x.FranchiseId).ToList();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }


       public List<FranchiseModel> GetAllFranchiseeList()
       {
           model = (from Co in objdb.Franchises
                    where Co.IsActive == true
                    select new FranchiseModel
                    {
                        BusinessName = Co.BussinessUser.BusinessName,
                        BusinessId = Co.BusinessId,
                        FranchiseId = Co.FranchiseId,
                        Details = Co.Details,
                        CompanyLogo = Co.BussinessUser.CompanyLogo,
                        ApprovedOn = Co.ApprovedOn,
                        IsActive = Co.IsActive,
                        CreatedOn = Co.CreatedOn,
                        Admin = Co.Administrator.UserName,
                        No_Month=Co.No_Months,
                        ExpiresOn=Co.ExpiresOn,
                        //ActivatedOn = Co.ActivatedOn,
                       // ExpiresOn = Co.ExpiresOn,

                    }).OrderByDescending(x => x.FranchiseId).ToList();
           return (model);
       }

       //public List<CoBrandModel> GetAllCoBrandUsersDetails()
       //{
       //    model = (from Co in objdb.CoBrandings
       //             where Co.IsAprroved == true
       //             select new CoBrandModel
       //             {
       //                 PartnerA = Co.PartnerA,
       //                 PartnerB = Co.PartnerB,
       //                 BusinessNameA = Co.BussinessUser.BusinessName,
       //                 BusinessNameB = Co.BussinessUser1.BusinessName,
       //                 CompanyLogo = Co.BussinessUser.CompanyLogo,
       //                 CompanyLogoB = Co.BussinessUser1.CompanyLogo,
       //                 CoBrandId = Co.CoBrandingId,
       //                 IsActive = Co.IsActive,
       //                 IsApproved = Co.IsAprroved,
       //                 CoBrandedName = Co.CoBrandedName,
       //                 ActivatedOn = Co.ActivatedOn,
       //                 ExpiresOn = Co.ExpiresOn,

       //             }).ToList();
       //    return (model);
       //}

       public bool ChangeFranchiseStatus(int id)
       {
           try
           {
               var obj = objdb.Franchises.Find(id);
               if (obj != null && obj.IsActive == true)
               {
                   obj.IsActive = false;
                   objdb.SaveChanges();
                   return false;
               }
               else if (obj != null)
               {
                   obj.IsActive = true;
                   obj.ApprovedOn = DateTime.Now;
                   obj.ExpiresOn = DateTime.Now.AddMonths(3);
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

       public bool ChangeFranchiseApprovalStatus(int id)
       {
           try
           {
               var obj = objdb.Franchises.Find(id);
               if (obj != null && obj.IsActive == true)
               {
                   obj.IsActive = false;
                   objdb.SaveChanges();
                   return false;
               }
               else if (obj != null)
               {
                  // obj.IsAprroved = true;
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
               var obj = objdb.Franchises.Find(id);
               if (obj != null)
               {
                   objdb.Franchises.Remove(obj);
                   objdb.SaveChanges();
               }
               return obj.FranchiseId;
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       public int GetPageCount()
       {
           try
           {
               return objdb.Franchises.Select(x => x.FranchiseId).Count();
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
               return objdb.Franchises.Select(x => x.BusinessId==id).Count();
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }
    }
}
