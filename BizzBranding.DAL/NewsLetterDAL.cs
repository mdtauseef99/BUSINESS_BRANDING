using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;
using System.Data;

namespace BizzBranding.DAL
{
   public class NewsLetterDAL
    {
       BizzBrandingEntities objDb = new BizzBrandingEntities();

       public bool InsertNewsLetter(NewsLetterModel objNewsLetterModel)
       {
           bool res = false;

           try
           {
               NewsLetter objNewsLetter = new NewsLetter();
               objNewsLetter.CreatedBy = 1;
               objNewsLetter.CreatedOn = DateTime.Now;
               objNewsLetter.IsActive = objNewsLetterModel.IsActive;
               //objNewsLetter.NewsCategoryID = objNewsLetterModel.NewsCategoryID;
               objNewsLetter.Title = objNewsLetterModel.Title;
               objNewsLetter.Description = objNewsLetterModel.Description;
               objNewsLetter.CreatedOn = DateTime.Now;
               objDb.NewsLetters.Add(objNewsLetter);
               objDb.SaveChanges();
               res = true;
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return res;
       }

       public bool UpdateNewsLetter(NewsLetterModel objNewsLetterModel)
       {
           bool res = false;

           try
           {
               var objNewsLetter = objDb.NewsLetters.Where(x => x.NewsLetterID == objNewsLetterModel.NewsLetterID).First();

               if (objNewsLetter != null)
               {
                   objNewsLetter.IsActive = objNewsLetterModel.IsActive;
                  // objNewsLetter.NewsCategoryID = objNewsLetterModel.NewsCategoryID;
                   objNewsLetter.Title = objNewsLetterModel.Title;
                   objNewsLetter.Description = objNewsLetterModel.Description;
                   objNewsLetter.UpdatedOn = DateTime.Now;
                   objDb.SaveChanges();
                   res = true;
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return res;
       }

       public int AddEditNewsLetter(NewsLetterModel model)
       {
           try
           {
               if (model.NewsLetterID == 0)
               {
                   NewsLetter NewsLetterObj = new NewsLetter
                   {
                       Title = model.Title,
                       Description = model.Description,
                       IsActive = model.IsActive,
                       // CreatedOn=DateTime.Now
                   };
                   objDb.NewsLetters.Add(NewsLetterObj);
                   objDb.SaveChanges();
                   return NewsLetterObj.NewsLetterID;
               }
               else
               {
                   int id = model.NewsLetterID;
                   NewsLetter obj = objDb.NewsLetters.Find(id);
                   if (obj != null)
                   {
                       obj.Title = model.Title;
                       obj.Description = model.Description;
                       obj.IsActive = model.IsActive;
                       obj.UpdatedOn = DateTime.Now;

                       objDb.SaveChanges();
                       return obj.NewsLetterID;
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

       public List<NewsLetter> GetAllNewsLetters()
       {
           try
           {
               return objDb.NewsLetters.Where(x => x.IsActive == true).OrderByDescending(x => x.NewsLetterID).ToList();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public NewsLetterModel GetNewsLetterById(int id)
       {
           try
           {
               return objDb.NewsLetters.Where(x => x.NewsLetterID == id).Select(x => new NewsLetterModel
               {
                   NewsLetterID = x.NewsLetterID,
                   Title = x.Title,
                   Description = x.Description,
                   IsActive = x.IsActive
               }).SingleOrDefault();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       //public DataTable GetNewsLetterById(int id)
       //{
       //    try
       //    {
       //        var list = objDb.NewsLetters.Where(x => x.NewsLetterID == id && x.IsActive == true).Select(x => new { x.NewsLetterID, x.Title, x.Description, x.CreatedOn, x.IsActive }).ToList();
       //        return CommonClassUtility.LINQToDataTable(list);
       //    }
       //    catch (Exception ex)
       //    {
       //        throw ex;
       //    }
       //}

       public DataTable GetAllNewsLetterDetails(int intSkip, int pagesize, string type)
       {
           DataTable dt = null;
           try
           {
               switch (type)
               {
                   case "All":
                       var allStates = objDb.NewsLetters.Select(x => new { NewsLetterID = x.NewsLetterID, Title = x.Title, CreatedBy = x.Administrator1.UserName, x.CreatedOn, x.IsActive }).OrderByDescending(x => x.NewsLetterID).Skip(intSkip).Take(pagesize).ToList();
                       dt = CommonClassUtility.LINQToDataTable(allStates);
                       break;
                   case "Active":
                       var activeStates = objDb.NewsLetters.Select(x => new { NewsLetterID = x.NewsLetterID, Title = x.Title, CreatedBy = x.Administrator1.UserName, x.CreatedOn, x.IsActive }).Where(x => x.IsActive == true).OrderByDescending(x => x.NewsLetterID).Skip(intSkip).Take(pagesize).ToList();
                       dt = CommonClassUtility.LINQToDataTable(activeStates);
                       break;
                   case "InActive":
                       var inactiveStates = objDb.NewsLetters.Select(x => new { NewsLetterID = x.NewsLetterID, Title = x.Title, CreatedBy = x.Administrator1.UserName, x.CreatedOn, x.IsActive }).Where(x => x.IsActive == false).OrderByDescending(x => x.NewsLetterID).Skip(intSkip).Take(pagesize).ToList();
                       dt = CommonClassUtility.LINQToDataTable(inactiveStates);
                       break;
               }

           }
           catch (Exception ex)
           {
               throw ex;
           }
           return dt;
       }

       public List<NewsLetterModel> GetAllNewsLetterDetails(int skip, int take)
       {
           try
           {
               return objDb.NewsLetters.Select(x => new NewsLetterModel
               {
                   NewsLetterID = x.NewsLetterID,
                   Title = x.Title,
                   Description = x.Description,
                   IsActive = x.IsActive
               }).OrderBy(x => x.NewsLetterID).Skip(skip).Take(take).ToList();
           }
           catch (Exception)
           {
               return null;
               throw;
           }


           //try
           //{
           //    var list = objDb.NewsLetters.Where(x => x.IsActive == true).Select(x => new
           //    {
           //        x.NewsLetterID,
           //        x.Title,
           //       // x.NewsCategory.NewsCategoryName,
           //        x.Administrator1.UserName,
           //        x.CreatedOn,
           //       // x.ValidDate,
           //        x.IsActive,
           //    }).OrderByDescending(x => x.NewsLetterID).Skip(skip).Take(take).ToList();

           //    return CommonClassUtility.LINQToDataTable(list);

           //}
           //catch (Exception Ex)
           //{
           //    return null;
           //}
       }

       public int GetTotalRowsNewsLetter(string type)
       {
           int records = 0;
           try
           {
               switch (type)
               {
                   case "All":
                       records = objDb.NewsLetters.OrderByDescending(x => x.NewsLetterID).Count();
                       break;
                   case "Active":
                       records = objDb.NewsLetters.Where(x => x.IsActive == true).OrderByDescending(p => p.NewsLetterID).Count();
                       break;
                   case "InActive":
                       records = objDb.NewsLetters.Where(x => x.IsActive == false).OrderByDescending(p => p.NewsLetterID).Count();
                       break;
                   default:
                       break;
               }
           }
           catch (Exception)
           {
               throw;
           }
           return records;
       }

       public int GetTotalRowsNewsLetter(DateTime fromdate, DateTime todate, bool status)
       {
           int records = 0;
           try
           {
               records = objDb.NewsLetters.Where(x => x.CreatedOn >= fromdate & x.CreatedOn <= todate & x.IsActive == status).Count();
           }
           catch
           {
               records = 0;
           }
           return records;
       }

       public bool DeleteNewsLetterData(List<int> Id)
       {
           bool res = false;
           try
           {
               foreach (var item in Id)
               {
                   var objNewsLetter = objDb.NewsLetters.Where(x => x.NewsLetterID == item && x.IsActive == true).First();

                   if (objNewsLetter != null)
                   {
                       objNewsLetter.IsActive = false;
                       objDb.SaveChanges();
                       res = true;
                   }
               }
           }
           catch (Exception ex)
           {
               throw ex;
           }
           return res;
       }

       public bool ChangeStatus(List<int> id, bool status)
       {
           bool res = false;
           try
           {
               foreach (var item in id)
               {
                   var obj = objDb.NewsLetters.Where(x => x.NewsLetterID == item).FirstOrDefault();
                   if (obj != null)
                   {
                       obj.IsActive = status;
                       objDb.SaveChanges();
                       res = true;
                   }
               }
           }
           catch (Exception)
           {
               throw;
           }
           return res;
       }

       public bool? CheckStatus(int id)
       {
           bool? res = false;
           try
           {
               NewsLetter objNewsLetter = objDb.NewsLetters.Where(x => x.NewsLetterID == id).Single();

               if (objNewsLetter != null)
               {
                   if (objNewsLetter.IsActive == false)
                   {
                       res = false;
                   }
                   else
                   {
                       res = true;
                   }
               }

               return res;
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public int GetPageCount()
       {
           try
           {
               return objDb.NewsLetters.Where(x => x.IsActive == true)
                           .Select(x => x.NewsLetterID).Count();
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
               var obj = objDb.News.Find(id);
               if (obj != null && obj.IsActive == true)
               {
                   obj.IsActive = false;
                   objDb.SaveChanges();
                   return false;
               }
               else if (obj != null)
               {
                   obj.IsActive = true;
                   objDb.SaveChanges();
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
