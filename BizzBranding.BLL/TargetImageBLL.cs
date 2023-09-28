using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
   public class TargetImageBLL
    {
       TargetImageDAL objdal = new TargetImageDAL();

       public TargetImageModel GetTargetImageByUserId(int id)
       {
           try
           {
               return objdal.GetTargetImageByUserId(id);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public int InsertTargetAdImagesBLL(TargetImageModel model)
       {
           return objdal.InsertTargetAdImagesBLL(model);
       }

       public List<TargetImageModel> GetTargetBrandAdImagesById(int id)
       {
           try
           {
               return objdal.GetTargetBrandAdImagesById(id);
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

    
    }
}
