using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class AccessPlanDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<AccessPlanModel> GetAllAccessPlan()
        {
            try
            {
                return objdb.AccessPlans.Where(x => x.IsActive == true).Select(x => new AccessPlanModel
                {
                    AccessPlanId = x.AccessPlanId,
                    AccessPlanName = x.AccessPlanName,
                    Description = x.Description,
                    MaxNoDeals = x.MaxNoDeals,
                    ProdPerMonth = x.ProdPerMonth,
                    UpdatesPerMonth = x.UpdatesPerMonth,
                    AddPostCharge = x.AddPostCharge,
                    NewsCharge = x.NewsCharge,
                    AddUpdCharge = x.AddUpdCharge,
                    MembershipFee = x.MembershipFee,
                    Validity=x.Validity,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<AccessPlanModel> GetAllAccessPlan(int skip, int take, int cid)
        {
            try
            {
                return objdb.AccessPlans.Where(x => x.AccessPlanId == cid).Select(x => new AccessPlanModel
                {
                    AccessPlanId = x.AccessPlanId,
                    AccessPlanName = x.AccessPlanName,
                    Description = x.Description,
                    MaxNoDeals = x.MaxNoDeals,
                    ProdPerMonth = x.ProdPerMonth,
                    UpdatesPerMonth = x.UpdatesPerMonth,
                    AddPostCharge = x.AddPostCharge,
                    NewsCharge = x.NewsCharge,
                    AddUpdCharge = x.AddUpdCharge,
                    MembershipFee = x.MembershipFee,
                    Validity=x.Validity,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.AccessPlanId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<AccessPlanModel> GetAllAccessPlan(int skip, int take)
        {
            try
            {
                return objdb.AccessPlans.Where(x => x.AccessPlanId != null).Select(x => new AccessPlanModel
                {
                    AccessPlanId = x.AccessPlanId,
                    AccessPlanName = x.AccessPlanName,
                    Description = x.Description,
                    MaxNoDeals = x.MaxNoDeals,
                    ProdPerMonth = x.ProdPerMonth,
                    UpdatesPerMonth = x.UpdatesPerMonth,
                    AddPostCharge = x.AddPostCharge,
                    NewsCharge = x.NewsCharge,
                    AddUpdCharge = x.AddUpdCharge,
                    MembershipFee = x.MembershipFee,
                    Validity=x.Validity,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.AccessPlanId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditAccessPlan(AccessPlanModel objmodel)
        {
            try
            {

                if (objmodel.AccessPlanId == 0)
                {
                    AccessPlan objacplan = new AccessPlan
                    {
                        AccessPlanName = objmodel.AccessPlanName,
                        Description = objmodel.Description,
                        MaxNoDeals = objmodel.MaxNoDeals,
                        ProdPerMonth = objmodel.ProdPerMonth,
                        UpdatesPerMonth = objmodel.UpdatesPerMonth,
                        AddPostCharge = objmodel.AddPostCharge,
                        NewsCharge = objmodel.NewsCharge,
                        AddUpdCharge = objmodel.AddUpdCharge,
                        MembershipFee = objmodel.MembershipFee,
                        Validity=objmodel.Validity,
                        AccessPlanImage=objmodel.AccessPlanImage,
                        CurrencyId=objmodel.CurrencyId,
                        //CreatedBy = x.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive,
                    };
                    objdb.AccessPlans.Add(objacplan);
                    objdb.SaveChanges();
                    return objacplan.AccessPlanId;
                }
                else
                {
                    var objacplan = objdb.AccessPlans.Find(objmodel.AccessPlanId);
                    objacplan.AccessPlanName = objmodel.AccessPlanName;
                    objacplan.Description = objmodel.Description;
                    objacplan.MaxNoDeals = objmodel.MaxNoDeals;
                    objacplan.ProdPerMonth = objmodel.ProdPerMonth;
                    objacplan.UpdatesPerMonth = objmodel.UpdatesPerMonth;
                    objacplan.AddPostCharge = objmodel.AddPostCharge;
                    objacplan.NewsCharge = objmodel.NewsCharge;
                    objacplan.AddUpdCharge = objmodel.AddUpdCharge;
                    objacplan.MembershipFee = objmodel.MembershipFee;
                    objacplan.Validity = objmodel.Validity;
                    objacplan.AccessPlanImage = objmodel.AccessPlanImage;
                    objacplan.CurrencyId = objmodel.CurrencyId;
                    objacplan.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.AccessPlanId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public AccessPlanModel GetAccessPlanById(int id)
        {
            try
            {
                return objdb.AccessPlans.Where(x => x.AccessPlanId == id).Select(x => new AccessPlanModel
                {
                    AccessPlanId = x.AccessPlanId,
                    AccessPlanName = x.AccessPlanName,
                    Description = x.Description,
                    MaxNoDeals = x.MaxNoDeals,
                    ProdPerMonth = x.ProdPerMonth,
                    UpdatesPerMonth = x.UpdatesPerMonth,
                    AddPostCharge = x.AddPostCharge,
                    NewsCharge = x.NewsCharge,
                    AddUpdCharge = x.AddUpdCharge,
                    MembershipFee = x.MembershipFee,
                    Validity=x.Validity,
                    AccessPlanImage = x.AccessPlanImage,
                    CurrencyId = x.CurrencyId,
                    CurrencyName=x.Currency.CurrencyName,
                    //CreatedBy = x.CreatedBy,
                    CreatedDate = x.CreatedDate,
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
                return objdb.AccessPlans.Where(x => x.AccessPlanId != null)
                            .Select(x => x.AccessPlanId).Count();
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
                var obj = objdb.AccessPlans.Find(id);
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

   












































        //public List<AccessPlanModel> GetCategoryByParent(int id)
        //{
        //    try
        //    {
        //        return objdb.Categories.Where(x => x.CategoryId == id).Select(x => new CategoryModel
        //        {
        //            CategoryId = x.CategoryId,
        //            CategoryName = x.CategoryName,
        //        }).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //        throw;
        //    }
        //}

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
