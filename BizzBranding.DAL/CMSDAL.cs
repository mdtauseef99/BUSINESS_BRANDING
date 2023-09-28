using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class CMSDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public int AddEditCms(CMSModel model)
        {
            try
            {
                if (model.CMSId == 0)
                {
                    CM obj = new CM
                    {
                        CMSTitle = model.CMSTitle,
                        Description = model.Description,
                        IsActive = model.IsActive
                    };
                    objdb.CMS.Add(obj);
                    objdb.SaveChanges();
                    return obj.CMSId;
                }
                else
                {
                    CM obj = objdb.CMS.Find(model.CMSId);
                    if (obj != null)
                    {
                        obj.CMSTitle = model.CMSTitle;
                        obj.Description = model.Description;
                        obj.IsActive = model.IsActive;

                        objdb.SaveChanges();
                        return obj.CMSId;
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

        public List<CMSModel> GetAllCms()
        {
            try
            {
                return objdb.CMS.Select(x => new CMSModel
                {
                    CMSId = x.CMSId,
                    CMSTitle = x.CMSTitle,
                    Description = x.Description,
                    IsActive = x.IsActive
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CMSModel> GetAllCms(int skip, int take)
        {
            try
            {
                return objdb.CMS.Select(x => new CMSModel
                {
                    CMSId = x.CMSId,
                    CMSTitle = x.CMSTitle,
                    Description = x.Description,
                    IsActive = x.IsActive
                }).OrderBy(x => x.CMSId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public CMSModel GetCmsById(int id)
        {
            try
            {
                return objdb.CMS.Where(x => x.CMSId == id).Select(x => new CMSModel
                {
                    CMSId = x.CMSId,
                    CMSTitle = x.CMSTitle,
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
        public List<CMSModel> GetAllCms(int skip, int take,int cid)
        {
            try
            {
                return objdb.CMS.Where(x=>x.CMSId==cid).Select(x => new CMSModel
                {
                    CMSId = x.CMSId,
                    CMSTitle = x.CMSTitle,
                    Description = x.Description,
                    IsActive = x.IsActive
                }).OrderByDescending(x => x.CMSId==cid).Skip(skip).Take(take).ToList();
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
                var obj = objdb.CMS.Find(id);
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

        public int GetPageCount()
        {
            try
            {
                return objdb.CMS.Where(x => x.IsActive == true)
                            .Select(x => x.CMSId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

    }
}
