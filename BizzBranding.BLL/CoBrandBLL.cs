using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
   public class CoBrandBLL
    {
       CoBrandDAL Objdal = new CoBrandDAL();

       public List<CoBrandModel> GetAllCoBrandUsers()
       {
           try
           {
               return Objdal.GetAllCoBrandUsers();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<CoBrandModel> GetAllCoBrandUsers(int skip, int take)
       {
           try
           {
               return Objdal.GetAllCoBrandUsers(skip, take);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<CoBrandModel> GetAllCoBrandUsersDetails()
       {
           try
           {
               return Objdal.GetAllCoBrandUsersDetails();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public int AddEditCoBrandUsers(CoBrandModel objmodele)
       {
           try
           {
               return Objdal.AddEditCoBrandUsers(objmodele);
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
               return Objdal.GetCoBrandUsersById(id);
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
               return Objdal.GetPageCount();
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
               return Objdal.ChangeStatus(id);
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
               return Objdal.ChangeApprovalStatus(id);
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
               return Objdal.Remove(id);
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

    }
}
