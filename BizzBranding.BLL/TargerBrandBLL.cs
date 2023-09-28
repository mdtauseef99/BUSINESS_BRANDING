using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
   public class TargerBrandBLL
    {
       TargetBrandDAL objdal = new TargetBrandDAL();

       public int AddEditTargetBranding(TargetBrandingModel objmodele)
       {
           try
           {
               return objdal.AddEditTargetBranding(objmodele);
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       public TargetBrandingModel GetTargetBrandUsersById(int id)
       {
           try
           {
               return objdal.GetTargetBrandByImageId(id);
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
               return objdal.GetTargetBrandByImageId(id);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<TargetBrandingModel> GetAllTargetedBrandUsers(int skip, int take)
       {
           try
           {
               return objdal.GetAllTargetedBrandUsers(skip, take);
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
               return objdal.GetAllTargetedBrandUsers(skip, take, cid);
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
               return objdal.GetAllTargetedBrandUsers();
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
       //        return objdal.GetTargetBrandImageById(id);
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
               return objdal.GetPageCount();
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
               return objdal.ChangeStatus(id);
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
               return objdal.Remove(id);
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }
    }
}
