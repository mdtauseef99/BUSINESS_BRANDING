using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class NewsDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public int AddEditNews(NewsModel model)
        {
            try
            {
                if (model.NewsId == 0)
                {
                    News NewsObj = new News
                    {
                        NewsTitle = model.NewsTitle,
                        Description = model.NewsDesc,
                        IsActive = model.IsActive,
                        NewsImage=model.NewsImage,
                        CreatedOn=DateTime.Now,
                        CreatedByUserId = model.CreatedByUserId,
                        CreatedByAdminId = model.CreatedByAdminId
                    };
                    objdb.News.Add(NewsObj);
                    objdb.SaveChanges();
                    return NewsObj.NewsId;
                }
                else
                {
                    int id = model.NewsId;
                    News obj = objdb.News.Find(id);
                    if (obj != null)
                    {
                        obj.NewsTitle = model.NewsTitle;
                        obj.Description = model.NewsDesc;
                        obj.IsActive = model.IsActive;
                        obj.UpdatedOn = DateTime.Now;
                        obj.NewsImage = model.NewsImage;
                        obj.CreatedByUserId = model.CreatedByUserId;
                        objdb.SaveChanges();
                        return obj.NewsId;
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

        public List<NewsModel> GetAllNews()
        {
            try
            {
                return objdb.News.Where(x=>x.IsActive==true).Select(x => new NewsModel
                {
                    NewsId = x.NewsId,
                    NewsTitle = x.NewsTitle,
                    NewsDesc = x.Description,
                    IsActive = x.IsActive,
                    NewsImage=x.NewsImage
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<NewsModel> GetAllNews(int skip, int take)
        {
            try
            {
                return objdb.News.Select(x => new NewsModel
                {
                    NewsId = x.NewsId,
                    NewsTitle = x.NewsTitle,
                    NewsDesc = x.Description,
                    IsActive = x.IsActive,
                    //NewsImage=x.NewsImage
                }).OrderByDescending(x => x.NewsId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public NewsModel GetNewsById(int id)
        {
            try
            {
                return objdb.News.Where(x => x.NewsId == id).Select(x => new NewsModel
                {
                    NewsId = x.NewsId,
                    NewsTitle = x.NewsTitle,
                    NewsDesc = x.Description,
                    IsActive = x.IsActive,
                    NewsImage=x.NewsImage
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
                return objdb.News.Where(x => x.IsActive == true)
                            .Select(x => x.NewsId).Count();
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
                var obj = objdb.News.Find(id);
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
