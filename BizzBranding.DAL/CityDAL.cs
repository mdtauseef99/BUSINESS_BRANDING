using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class CityDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<CityModel> GetAllCity()
        {
            try
            {
                return objdb.Cities.Where(x => x.IsActive == true).Select(x => new CityModel
                {
                    CityId = x.CityId,
                    CityName = x.CityName,
                    CountryId = x.CountryId,
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

        public List<CityModel> GetAllCity(int skip, int take, int cid)
        {
            try
            {
                return objdb.Cities.Where(x => x.CountryId != null && x.CityId == cid).Select(x => new CityModel
                {
                    CityId = x.CityId,
                    CityName = x.CityName,
                    CountryId = x.CountryId,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.CityId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CityModel> GetAllCity(int skip, int take)
        {
            try
            {
                return objdb.Cities.Where(x => x.CountryId != null).Select(x => new CityModel
                {
                    CityId = x.CityId,
                    CityName = x.CityName,
                    CountryId = x.CountryId,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.CityId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditCity(CityModel objmodel)
        {

            try
            {
                //if (objmodel.CountryId == 0)
                //{
                //    objmodel.CountryId =null;
                //}
                if (objmodel.CityId == 0)
                {
                    City objcourse = new City
                    {
                        CityName = objmodel.CityName,
                        //CreatedBy = objmodel.CreatedBy,
                        CountryId = objmodel.CountryId,
                        //CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.Cities.Add(objcourse);
                    objdb.SaveChanges();
                    return objcourse.CityId;
                }
                else
                {
                    var objcourse = objdb.Cities.Find(objmodel.CityId);
                    objcourse.CityName = objmodel.CityName;
                    objcourse.CountryId = objmodel.CountryId;
                    objcourse.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.CityId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public CityModel GetCityById(int id)
        {
            try
            {
                return objdb.Cities.Where(x => x.CityId == id).Select(x => new CityModel
                {
                    CityId = x.CityId,
                    CityName = x.CityName,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
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
                return objdb.Cities.Where(x => x.CountryId != null)
                            .Select(x => x.CityId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<CityModel> GetCityByParent(int id)
        {
            try
            {
                return objdb.Cities.Where(x => x.CityId == id).Select(x => new CityModel
                {
                    CityId = x.CityId,
                    CityName = x.CityName,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CityModel> GetParentCities()
        {
            try
            {
                return objdb.Cities.Where(x => x.IsActive == true).Select(x => new CityModel
                {
                    CityId = x.CityId,
                    CityName = x.CityName,
                    //CreatedBy = x.CreatedBy,
                    CountryId = x.CountryId,
                    CountryName = x.Country.CountryName,
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

        public bool ChangeStatus(int id)
        {
            try
            {
                var obj = objdb.Cities.Find(id);
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
