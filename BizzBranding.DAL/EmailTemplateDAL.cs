using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class EmailTemplateDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<EmailTemplateModel> GetAllEmailTemplate()
        {
            try
            {
                return objdb.EmailTemplates.Where(x => x.IsActive == true).Select(x => new EmailTemplateModel
                {
                    EmailTempId = x.EmailTempId,
                    EmailTempTitle = x.EmailTempTitle,
                    Description=x.Description,
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

        public List<EmailTemplateModel> GetAllEmailTemplate(int skip, int take, int cid)
        {
            try
            {
                return objdb.EmailTemplates.Where(x => x.EmailTempId== cid).Select(x => new EmailTemplateModel
                {
                    EmailTempId = x.EmailTempId,
                    EmailTempTitle = x.EmailTempTitle,
                    Description = x.Description,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.EmailTempId == cid).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<EmailTemplateModel> GetAllEmailTemplate(int skip, int take)
        {
            try
            {
                return objdb.EmailTemplates.Where(x => x.EmailTempId!= null).Select(x => new EmailTemplateModel
                {
                    EmailTempId = x.EmailTempId,
                    EmailTempTitle = x.EmailTempTitle,
                    Description = x.Description,
                    //CreatedBy = x.CreatedBy,
                    //CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.EmailTempId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int AddEditEmailTemplate(EmailTemplateModel objmodel)
        {
            try
            {

                if (objmodel.EmailTempId == 0)
                {
                    EmailTemplate objemailtemp = new EmailTemplate
                    {
                        EmailTempId = objmodel.EmailTempId,
                        EmailTempTitle = objmodel.EmailTempTitle,
                        Description = objmodel.Description,
                        //CreatedDate = DateTime.Now,
                        IsActive = objmodel.IsActive
                    };
                    objdb.EmailTemplates.Add(objemailtemp);
                    objdb.SaveChanges();
                    return objemailtemp.EmailTempId;
                }
                else
                {
                    var objemailtemp = objdb.EmailTemplates.Find(objmodel.EmailTempId);
                    objemailtemp.EmailTempTitle= objmodel.EmailTempTitle;
                    objemailtemp.EmailTempId= objmodel.EmailTempId;
                    objemailtemp.Description = objmodel.Description;
                    objemailtemp.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.EmailTempId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public EmailTemplateModel GetEmailTemplateById(int id)
        {
            try
            {
                return objdb.EmailTemplates.Where(x => x.EmailTempId== id).Select(x => new EmailTemplateModel
                {
                    EmailTempId = x.EmailTempId,
                    EmailTempTitle = x.EmailTempTitle,
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
                return objdb.EmailTemplates.Where(x => x.EmailTempId != null)
                            .Select(x => x.EmailTempId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<EmailTemplateModel> GetEmailTemplateByParent(int id)
        {
            try
            {
                return objdb.EmailTemplates.Where(x => x.EmailTempId== id).Select(x => new EmailTemplateModel
                {
                    EmailTempId = x.EmailTempId,
                    EmailTempTitle = x.EmailTempTitle,
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
                var obj = objdb.EmailTemplates.Find(id);
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

        public EmailTemplate GetEmailSettingsByTemplateName(string name)
        {
            return objdb.EmailTemplates.Where(e => e.EmailTempTitle == name && e.IsActive == true).SingleOrDefault();
        }

        public EmailTemplate GetEmailSettingsByTemplateID(int id)
        {
            return objdb.EmailTemplates.Where(e => e.EmailTempId == id && e.IsActive == true).SingleOrDefault();
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
