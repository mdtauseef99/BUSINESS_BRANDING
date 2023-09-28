using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class BusinessNewsDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public int AddEditBusinessNews(BusinessNewsModel model)
        {
            try
            {
                if (model.BusinessNewsId == 0)
                {//Insert BusinesNewss
                    BusinessNew NewsObj = new BusinessNew
                    {
                        NewsTitle = model.NewsTitle,
                        Description = model.Description,
                        IsActive = true,
                        CreatedBy=model.CreatedBy,
                        CreatedOn=DateTime.Now
                    };
                    objdb.BusinessNews.Add(NewsObj);
                    objdb.SaveChanges();
                    return NewsObj.BusinessNewsId;
                }
                else
                {//Update Business News
                    int id = model.BusinessNewsId;
                    BusinessNew obj = objdb.BusinessNews.Find(id);
                    if (obj != null)
                    {
                        obj.NewsTitle = model.NewsTitle;
                        obj.Description = model.Description;
                        obj.IsActive = true;
                        obj.UpdatedBy = model.UpdatedBy;
                        obj.UpdatedOn = DateTime.Now;

                        objdb.SaveChanges();
                        return obj.BusinessNewsId;
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

        public List<BusinessNewsModel> GetAllBusinessNews()
        {
            try
            {
                //return objdb.BusinessNews.Where(x => x.IsActive == true).Select(x => new BusinessNewsModel
                return objdb.BusinessNews.Select(x => new BusinessNewsModel
                {
                    BusinessNewsId = x.BusinessNewsId,
                    NewsTitle = x.NewsTitle,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    CreatedBy=x.CreatedBy
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<BusinessNewsModel> GetAllBusinessNews(int skip, int take)
        {
            try
            {
                return objdb.BusinessNews.Select(x => new BusinessNewsModel
                {
                    BusinessNewsId = x.BusinessNewsId,
                    NewsTitle = x.NewsTitle,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    CreatedBy=x.CreatedBy
                }).OrderBy(x => x.BusinessNewsId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<BusinessNewsModel> GetAllBusinessNews(int id,int skip, int take)
        {
            try
            {
                return objdb.BusinessNews.Select(x => new BusinessNewsModel
                {
                    BusinessNewsId = x.BusinessNewsId,
                    NewsTitle = x.NewsTitle,
                    Description = x.Description,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy
                }).Where(x=>x.CreatedBy==id).OrderBy(x => x.BusinessNewsId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public BusinessNewsModel GetBusinessNewsById(int id)
        {
            try
            {
                return objdb.BusinessNews.Where(x => x.CreatedBy == id).Select(x => new BusinessNewsModel
                {
                    BusinessNewsId = x.BusinessNewsId,
                    NewsTitle = x.NewsTitle,
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

        public BusinessNewsModel GetBusinessNewsByNewsId(int id)
        {
            try
            {
                return objdb.BusinessNews.Where(x => x.BusinessNewsId == id).Select(x => new BusinessNewsModel
                {
                    BusinessNewsId = x.BusinessNewsId,
                    NewsTitle = x.NewsTitle,
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

        public int GetPageCount()
        {
            try
            {
                //return objdb.BusinessNews.Where(x => x.IsActive == true)
                //            .Select(x => x.BusinessNewsId).Count();
                return objdb.BusinessNews.Select(x => x.BusinessNewsId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int GetPageCountByUserid(int id)
        {
            try
            {
                //return objdb.BusinessNews.Where(x => x.IsActive == true)
                //            .Select(x => x.BusinessNewsId).Count();
                return objdb.BusinessNews.Where(x=>x.CreatedBy==id).Select(x => x.BusinessNewsId).Count();
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
                var obj = objdb.BusinessNews.Find(id);
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

        public int Remove(int id)
        {
            try
            {
                var obj = objdb.BusinessNews.Find(id);
                if (obj != null)
                {
                    objdb.BusinessNews.Remove(obj);
                    objdb.SaveChanges();
                }
                return obj.BusinessNewsId;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

   
    }
}
