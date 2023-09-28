using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class EmailTemplateBLL
    {
        EmailTemplateDAL objemailtempdal = new EmailTemplateDAL();

        public List<EmailTemplateModel> GetAllEmailTemplate()
        {
            try
            {
                return objemailtempdal.GetAllEmailTemplate();
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
                return objemailtempdal.GetAllEmailTemplate(skip, take, cid);
            }
            catch (Exception)
            {

                throw;
            }
        }


        public List<EmailTemplateModel> GetAllEmailTemplate(int skip, int take)
        {
            try
            {
                return objemailtempdal.GetAllEmailTemplate(skip, take);
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
                return objemailtempdal.AddEditEmailTemplate(objmodel);
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
                return objemailtempdal.GetEmailTemplateById(id);
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
                return objemailtempdal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<EmailTemplateModel> GetEmailTemplateByParent(int id)
        {
            try
            {
                return objemailtempdal.GetEmailTemplateByParent(id);
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
                return objemailtempdal.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public EmailTemplate GetEmailSettingsByTemplateName(string name)
        {
            return objemailtempdal.GetEmailSettingsByTemplateName(name);
        }

        public EmailTemplate GetEmailSettingsByTemplateID(int id)
        {
            return objemailtempdal.GetEmailSettingsByTemplateID(id);
        }

























        //public List<CountryModel> GetParentCities()
        //{
        //    try
        //    {
        //        return objcoursedal.GetParentCities();
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
        //        return objcoursedal.GetCourseByInstituteId(id);
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
        //        return objcoursedal.GetCourseByInstituteID(id);
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
        //        return objcoursedal.GetCourseByParentId(id);
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
        //        return new CourseDAL().getparentcoursenamebycourseid(id);
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
        //        return new CourseDAL { }.getparentcoursenamebyparentid(id);
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

    }
}
