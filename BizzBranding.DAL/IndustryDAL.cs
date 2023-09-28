using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class IndustryDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<IndustryModel> GetAllIndustry()
        {
            try
            {
                return objdb.Industries.Where(x => x.IsActive == true).Select(x => new IndustryModel
                {
                    IndustryId = x.IndustryId,
                    IndustryName = x.IndustryName,
                    Description = x.Description,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<IndustryModel> GetAllIndustry(int skip, int take, int cid)
        {
            try
            {
                return objdb.Industries.Where(x => x.IndustryId == cid).Select(x => new IndustryModel
                {
                    IndustryId = x.IndustryId,
                    IndustryName = x.IndustryName,
                    Description = x.Description,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.IndustryId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<IndustryModel> GetAllIndustry(int skip, int take)
        {
            try
            {
                return objdb.Industries.Where(x => x.IndustryId != null).Select(x => new IndustryModel
                {
                    IndustryId = x.IndustryId,
                    IndustryName = x.IndustryName,
                    Description = x.Description,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.IndustryId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditIndustry(IndustryModel objmodel)
        {
            try
            {

                if (objmodel.IndustryId == 0)
                {
                    Industry objindustry = new Industry
                    {
                        IndustryId = objmodel.IndustryId,
                        IndustryName = objmodel.IndustryName,
                        Description = objmodel.Description,
                        //CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.Industries.Add(objindustry);
                    objdb.SaveChanges();
                    return objindustry.IndustryId;
                }
                else
                {
                    var objindustry = objdb.Industries.Find(objmodel.IndustryId);
                    objindustry.IndustryName= objmodel.IndustryName;
                    objindustry.IndustryId = objmodel.IndustryId;
                    objindustry.Description = objmodel.Description;
                    objindustry.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.IndustryId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public IndustryModel GetIndustryById(int id)
        {
            try
            {
                return objdb.Industries.Where(x => x.IndustryId== id).Select(x => new IndustryModel
                {
                    IndustryId = x.IndustryId,
                    IndustryName = x.IndustryName,
                    Description = x.Description,
                    //CreatedBy = x.CreatedBy,
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
                return objdb.Industries.Where(x => x.IndustryId!= null)
                            .Select(x => x.IndustryId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<IndustryModel> GetIndustryByParent(int id)
        {
            try
            {
                return objdb.Industries.Where(x => x.IndustryId== id).Select(x => new IndustryModel
                {
                    IndustryId = x.IndustryId,
                    IndustryName = x.IndustryName,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public bool ChangeStatus(int id)
        {
            try
            {
                var obj = objdb.Industries.Find(id);
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
    }

}
