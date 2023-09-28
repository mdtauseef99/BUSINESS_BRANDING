using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
   public class EmpBrandingBLL
    {
       EmpBrandingDAL Objdal = new EmpBrandingDAL();

       public List<EmpBrandingModel> GetAllEmployeeBranding(int skip, int take)
       {
           try
           {
               return Objdal.GetAllEmployeeBranding(skip, take);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<EmpBrandingModel> GetAllEmpBranding()
       {
           try
           {
               return Objdal.GetAllEmpBranding();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<EmpBrandingModel> GetAllEmpBrandDetails()
       {
           try
           {
               return Objdal.GetAllEmpBrandDetails();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public int AddEditEmpBranding(EmpBrandingModel objmodele)
       {
           try
           {
               return Objdal.AddEditEmpBranding(objmodele);
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       public EmpBrandingModel GetEmpBrandingById(int id)
       {
           try
           {
               return Objdal.GetEmpBrandingById(id);
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
               return Objdal.GetEmpBrandingByUserId(id);
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

       public int GetPageCountbyUserId(int id)
       {
           try
           {
               return Objdal.GetPageCountbyUserId(id);
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
