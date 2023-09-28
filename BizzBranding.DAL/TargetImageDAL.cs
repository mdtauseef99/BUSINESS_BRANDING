using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
   public class TargetImageDAL
    {
       BizzBrandingEntities objdb = new BizzBrandingEntities();

       public TargetImageModel GetTargetImageByUserId(int id)
       {
           try
           {
               return objdb.TargetedImages.Where(x => x.TargetBrandingId == id).Select(x => new TargetImageModel
               {
                   TargetBrandingId = x.TargetBrandingId,
                   TargetImageId = x.TargetImageId,
                   Image = x.Image,
                   IndustryName = x.TargetBranding.Industry.IndustryName,
                   CityName = x.TargetBranding.City.CityName
               }).SingleOrDefault();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }

       public int InsertTargetAdImagesBLL(TargetImageModel model)
       {
           int res = 0;
           TargetedImage obj = new TargetedImage();
           obj.TargetBrandingId = model.TargetBrandingId;
           obj.Image = model.Image;
           obj.CreatedDate = model.CreatedDate;
           obj.IsActive = model.IsActive;
           objdb.TargetedImages.Add(obj);
           objdb.SaveChanges();
           return res;
       }

       public List<TargetImageModel> GetTargetBrandAdImagesById(int id)
       {
           try
           {
               return objdb.TargetedImages.Where(x => x.TargetBrandingId == id).Select(x => new TargetImageModel
               {
                   Image = x.Image,
                   TargetImageId = x.TargetImageId,
                   TargetBrandingId = x.TargetBrandingId,
                   IsActive = x.IsActive,
               }).ToList();
           }
           catch (Exception)
           {
               return null;
               throw;
           }
       }
    }
}
