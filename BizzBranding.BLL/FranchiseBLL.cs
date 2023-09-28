using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
   public class FranchiseBLL
    {
       FranchiseDAL Objdal = new FranchiseDAL();
       public int AddEditFranchise(FranchiseModel objmodel)
       {
           try
           {
               return Objdal.AddEditFranchise(objmodel);
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
               return Objdal.GetAllFranchiseeList(skip, take);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public List<FranchiseModel> GetAllFranchiseeList()
       {
           try
           {
               return Objdal.GetAllFranchiseeList();
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
               return Objdal.GetAllFranchiseeList(skip, take, cid);
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
               return Objdal.GetFranchiserDetailsById(id);
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
               return Objdal.GetFranchiserDetailsByUserId(id);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }
       public bool ChangeFranchiseStatus(int id)
       {
           try
           {
               return Objdal.ChangeFranchiseStatus(id);
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
               return Objdal.ChangeFranchiseApprovalStatus(id);
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

       public int GetPageCountByUserId(int id)
       {
           try
           {
               return Objdal.GetPageCountByUserId(id);
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
               return Objdal.GetPageCount();
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }
    }
}
