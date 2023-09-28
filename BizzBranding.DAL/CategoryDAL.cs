using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class CategoryDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<CategoryModel> GetAllCategory()
        {
            try
            {
                return objdb.Categories.Where(x => x.IsActive == true).Select(x => new CategoryModel
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
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

        public List<CategoryModel> GetAllCategory(int skip, int take, int cid)
        {
            try
            {
                return objdb.Categories.Where(x => x.CategoryId == cid).Select(x => new CategoryModel
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.CategoryId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CategoryModel> GetAllCategory(int skip, int take)
        {
            try
            {
                return objdb.Categories.Where(x => x.CategoryId != null).Select(x => new CategoryModel
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.CategoryId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditCategory(CategoryModel objmodel)
        {
            try
            {

                if (objmodel.CategoryId == 0)
                {
                    Category objcategory = new Category
                    {
                        CategoryName = objmodel.CategoryName,
                        CategoryId = objmodel.CategoryId,
                        //CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.Categories.Add(objcategory);
                    objdb.SaveChanges();
                    return objcategory.CategoryId;
                }
                else
                {
                    var objcategory = objdb.Categories.Find(objmodel.CategoryId);
                    objcategory.CategoryName= objmodel.CategoryName;
                    objcategory.CategoryId = objmodel.CategoryId;
                    objcategory.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.CategoryId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public CategoryModel GetCategoryById(int id)
        {
            try
            {
                return objdb.Categories.Where(x => x.CategoryId== id).Select(x => new CategoryModel
                {
                    CategoryId= x.CategoryId,
                    CategoryName = x.CategoryName,
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
                return objdb.Categories.Where(x => x.CategoryId!= null)
                            .Select(x => x.CategoryId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<CategoryModel> GetCategoryByParent(int id)
        {
            try
            {
                return objdb.Categories.Where(x => x.CategoryId== id).Select(x => new CategoryModel
                {
                    CategoryId= x.CategoryId,
                    CategoryName = x.CategoryName,
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
                var obj = objdb.Categories.Find(id);
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

        //public List<CountryModel> GetParentCities()
        //{
        //    try
        //    {
        //        return objdb.Cities.Where(x => x.CountryId == null).Select(x => new CityModel
        //        {
        //            CityId = x.CityId,
        //            CityName = x.CityName,
        //            //CreatedBy = x.CreatedBy,
        //            CountryId = x.CountryId,
        //            CreatedDate = x.CreatedDate,
        //            IsActive = x.IsActive
        //        }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //        throw;
        //    }
        //}

        //public List<CourseInstituteMappingModel> GetCourseByInstituteId(int id)
        //{
        //    try
        //    {
        //        return objdb.CourseInstituteMappings.Where(x => x.InstituteID == id).Select(x => new CourseInstituteMappingModel
        //        {
        //            CourseID = x.CourseID,
        //            InstituteID = x.InstituteID,
        //            CourseName = x.Course.CourseName
        //        }).ToList();
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<CourseInstituteMappingModel> GetCourseByInstituteID(int id)
        //{
        //    try
        //    {
        //        List<CourseInstituteMappingModel> courselist = new List<CourseInstituteMappingModel>();
        //        var list = objdb.CourseInstituteMappings.Where(x => x.InstituteID == id).Select(x => new CourseInstituteMappingModel
        //        {
        //            CourseID = x.CourseID,
        //            InstituteID = x.InstituteID

        //        }).ToList();
        //        foreach (var item in list)
        //        {
        //            courselist.Add(objdb.Courses.Where(x => x.CourseID == item.CourseID).Select(x => new CourseInstituteMappingModel
        //            {
        //                CourseID = x.CourseID,
        //                CourseName = x.CourseName

        //            }).SingleOrDefault());
        //        }
        //        return courselist;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public List<CourseModel> GetCourseByParentId(int id)
        //{
        //    try
        //    {
        //        var list = objdb.Courses.Where(x => x.ParentCourseID == id && x.IsActive == true).Select(x => new CourseModel
        //        {
        //            CourseID = x.CourseID,
        //            CourseName = x.CourseName,
        //        }).ToList();
        //        return list;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public CourseModel getparentcoursenamebycourseid(int id)
        //{
        //    try
        //    {
        //        var list = objdb.Courses.Where(x => x.CourseID == id && x.IsActive == true).Select(x => new CourseModel
        //        {
        //            CourseID = x.CourseID,
        //            ParentCourseID = x.ParentCourseID,
        //            CourseName = x.CourseName,
        //        }).FirstOrDefault();
        //        return list;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        //public CourseModel getparentcoursenamebyparentid(int id)
        //{
        //    try
        //    {
        //        //var test objdb.Courses.Where (x=>x.CourseID==
        //        var list = objdb.Courses.Where(x => x.CourseID == id && x.IsActive == true).FirstOrDefault();

        //        var test = objdb.Courses.Where(x => x.CourseID == list.ParentCourseID).Select(x => new CourseModel
        //        {
        //            CourseID = x.CourseID,
        //            ParentCourseID = x.ParentCourseID,
        //            CourseName = x.CourseName,
        //        }).FirstOrDefault();
        //        return test;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}


    }

}
