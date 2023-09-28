using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
namespace BizzBranding.DAL
{
    public class SubCategoryDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<SubCategoryModel> GetAllSubCategory()
        {
            try
            {
                return objdb.SubCategories.Where(x => x.IsActive == true).Select(x => new SubCategoryModel
                {
                    SubCategoryId = x.SubCategoryId,
                    SubCatgoryName = x.SubCatgoryName,
                    ParentCategoryId = x.ParentCategoryId,
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

        public List<SubCategoryModel> GetAllSubCategory(int skip, int take, int cid)
        {
            try
            {
                return objdb.SubCategories.Where(x => x.ParentCategoryId!= null && x.SubCategoryId == cid).Select(x => new SubCategoryModel
                {
                    SubCategoryId = x.SubCategoryId,
                    SubCatgoryName = x.SubCatgoryName,
                    ParentCategoryId = x.ParentCategoryId,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.SubCategoryId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<SubCategoryModel> GetAllSubCategory(int skip, int take)
        {
            try
            {
                return objdb.SubCategories.Where(x => x.ParentCategoryId!= null).Select(x => new SubCategoryModel
                {
                    SubCategoryId = x.SubCategoryId,
                    SubCatgoryName = x.SubCatgoryName,
                    ParentCategoryId = x.ParentCategoryId,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.SubCategoryId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditSubCategory(SubCategoryModel objmodel)
        {

            try
            {
                //if (objmodel.CountryId == 0)
                //{
                //    objmodel.CountryId =null;
                //}
                if (objmodel.SubCategoryId == 0)
                {
                    SubCategory objsub = new SubCategory
                    {
                        SubCatgoryName = objmodel.SubCatgoryName,
                        //CreatedBy = objmodel.CreatedBy,
                        ParentCategoryId = objmodel.ParentCategoryId,
                        //CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.SubCategories.Add(objsub);
                    objdb.SaveChanges();
                    return objsub.SubCategoryId;
                }
                else
                {
                    var objsubc = objdb.SubCategories.Find(objmodel.SubCategoryId);
                    objsubc.SubCatgoryName= objmodel.SubCatgoryName;
                    objsubc.ParentCategoryId = objmodel.ParentCategoryId;
                    objsubc.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.SubCategoryId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public SubCategoryModel GetSubCategoryById(int id)
        {
            try
            {
                return objdb.SubCategories.Where(x => x.SubCategoryId== id).Select(x => new SubCategoryModel
                {
                    SubCategoryId = x.SubCategoryId,
                    SubCatgoryName= x.SubCatgoryName,
                    ParentCategoryId = x.ParentCategoryId,
                    CategoryName = x.Category.CategoryName,
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
                return objdb.SubCategories.Where(x => x.Category != null)
                            .Select(x => x.SubCategoryId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<SubCategoryModel> GetSubCategoryByParent(int id)
        {
            try
            {
                return objdb.SubCategories.Where(x => x.SubCategoryId== id).Select(x => new SubCategoryModel
                {
                    SubCategoryId = x.SubCategoryId,
                    SubCatgoryName = x.SubCatgoryName,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<SubCategoryModel> GetParentCategories()
        {
            try
            {
                return objdb.SubCategories.Where(x => x.IsActive == true).Select(x => new SubCategoryModel
                {
                    SubCategoryId= x.SubCategoryId,
                    SubCatgoryName = x.SubCatgoryName,
                    //CreatedBy = x.CreatedBy,
                    ParentCategoryId= x.ParentCategoryId,
                    CategoryName = x.Category.CategoryName,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public bool ChangeSubCatStatus(int id)
        {
            try
            {
                var obj = objdb.SubCategories.Find(id);
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
