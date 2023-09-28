using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class CurrencyDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<CurrencyModel> GetAllCurrency()
        {
            try
            {
                return objdb.Currencies.Where(x => x.IsActive == true).Select(x => new CurrencyModel
                {
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.CurrencyName,
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

        public List<CurrencyModel> GetAllCurrency(int skip, int take, int cid)
        {
            try
            {
                return objdb.Currencies.Where(x => x.CurrencyId== cid).Select(x => new CurrencyModel
                {
                    CurrencyId= x.CurrencyId,
                    CurrencyName= x.CurrencyName,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.CurrencyId== cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CurrencyModel> GetAllCurrency(int skip, int take)
        {
            try
            {
                return objdb.Currencies.Where(x => x.CurrencyId!= null).Select(x => new CurrencyModel
                {
                    CurrencyId= x.CurrencyId,
                    CurrencyName = x.CurrencyName,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.CurrencyId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditCurrency(CurrencyModel objmodel)
        {
            try
            {

                if (objmodel.CurrencyId== 0)
                {
                    Currency objcurrency = new Currency
                    {
                        CurrencyName = objmodel.CurrencyName,
                        CurrencyId = objmodel.CurrencyId,
                        //CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.Currencies.Add(objcurrency);
                    objdb.SaveChanges();
                    return objcurrency.CurrencyId;
                }
                else
                {
                    var objcurrency = objdb.Currencies.Find(objmodel.CurrencyId);
                    objcurrency.CurrencyName= objmodel.CurrencyName;
                    objcurrency.CurrencyId = objmodel.CurrencyId;
                    objcurrency.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.CurrencyId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public CurrencyModel GetCurrencyById(int id)
        {
            try
            {
                return objdb.Currencies.Where(x => x.CurrencyId== id).Select(x => new CurrencyModel
                {
                    CurrencyId= x.CurrencyId,
                    CurrencyName = x.CurrencyName,
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
                return objdb.Currencies.Where(x => x.CurrencyId!= null)
                            .Select(x => x.CurrencyId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<CurrencyModel> GetCurrencyByParent(int id)
        {
            try
            {
                return objdb.Currencies.Where(x => x.CurrencyId== id).Select(x => new CurrencyModel
                {
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.CurrencyName,
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
                var obj = objdb.Currencies.Find(id);
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
