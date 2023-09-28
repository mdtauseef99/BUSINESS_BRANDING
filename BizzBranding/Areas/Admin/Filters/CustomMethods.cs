using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BizzBranding.BLL;
using System.Xml.Linq;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Filters
{
    static public class CustomMethods
    {
        internal static void BindRoles<T>(T model)
        {
            try
            {
                var roleslist = new RolesBLL { }.GetAllRoles();
                if (roleslist != null)
                {
                    model.GetType().GetProperty("Roles").SetValue(model, roleslist.Select(x => new SelectListItem { Value = x.RoleId.ToString(), Text = x.RoleName }));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static void BindSubategory<T>(T model)
        {
            try
            {
                var subcat = new SubCategoryBLL { }.GetAllSubCategory();

                if (subcat != null)
                {
                    model.GetType().GetProperty("SubCategory").SetValue(model, subcat.Select(x => new SelectListItem { Value = x.SubCategoryId.ToString(), Text = x.SubCatgoryName}));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public static void BindParentCategory<T>(T model)
        {
            try
            {
                var categories = new CategoryBLL { }.GetAllCategory();
                if (categories != null)
                {
                    model.GetType().GetProperty("CategoryNames").SetValue(model, categories.Select(x => new SelectListItem { Value = x.CategoryId.ToString(), Text = x.CategoryName}));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void BindSubCategory<T>(T model)
        {
            try
            {
                var categories = new SubCategoryBLL { }.GetAllSubCategory();
                if (categories != null)
                {
                    model.GetType().GetProperty("SubCategoryNames").SetValue(model, categories.Select(x => new SelectListItem { Value = x.SubCategoryId.ToString(), Text = x.SubCatgoryName}));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void BindAccessPlanForMembership<T>(T model)
        {
            try
            {
                var acplan = new AccessPlanBLL { }.GetAllAccessPlan();
                if (acplan != null)
                {
                    model.GetType().GetProperty("AccessPlanNames").SetValue(model, acplan.Select(x => new SelectListItem { Value = x.AccessPlanId.ToString(), Text = x.AccessPlanName }));
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static void BindBusinessUser<T>(T model)
        {
            try
            {
                var bus = new BussinessUserBLL { }.GetAllUsers();
                if (bus != null)
                {
                    model.GetType().GetProperty("BusinessNames").SetValue(model, bus.Select(x => new SelectListItem { Value = x.UserId.ToString(), Text = x.BusinessName }));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static void BindCurrencyForMembership<T>(T model)
        {
            try
            {
                var currency = new CurrencyBLL { }.GetAllCurrency();
                if (currency != null)
                {
                    model.GetType().GetProperty("CurrencyNames").SetValue(model, currency.Select(x => new SelectListItem { Value = x.CurrencyId.ToString(), Text = x.CurrencyName }));
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static void BindIndustry<T>(T model)
        {
            try
            {
                var industry = new IndustryBLL { }.GetAllIndustry();
                if (industry != null)
                {
                    model.GetType().GetProperty("IndustryNames").SetValue(model, industry.Select(x => new SelectListItem { Value = x.IndustryId.ToString(), Text = x.IndustryName }));
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public static void BindCity<T>(T model)
        {
            try
            {
                var cities = new CityBLL { }.GetAllCity();

                if (cities != null)
                {
                    model.GetType().GetProperty("City").SetValue(model, cities.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.CityName }));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void BindCountryWithCity<T>(T model)
        {
            try
            {
                var cities = new CountryBLL { }.GetAllCountry();
                if (cities != null)
                {
                    model.GetType().GetProperty("CountryNames").SetValue(model, cities.Select(x => new SelectListItem { Value = x.CountryId.ToString(), Text = x.CountryName }));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void BindCityForUser<T>(T model)
        {
            try
            {
                var city = new CityBLL { }.GetAllCity();
                if (city != null)
                {
                    model.GetType().GetProperty("CityNames").SetValue(model, city.Select(x => new SelectListItem { Value = x.CityId.ToString(), Text = x.CityName }));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void BindProduct<T>(T model)
        {
            try
            {
                var product = new ProductBLL { }.GetAllProduct();
                if (product != null)
                {
                    model.GetType().GetProperty("ProductNames").SetValue(model, product.Select(x => new SelectListItem { Value = x.ProductId.ToString(), Text = x.ProductName}));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void BindPartnerA<T>(T model)
        {
            try
            {
                var partnera = new BussinessUserBLL { }.GetAllUsers();
                if (partnera != null)
                {
                    model.GetType().GetProperty("PartnerAList").SetValue(model, partnera.Select(x => new SelectListItem { Value = x.UserId.ToString(), Text = x.BusinessName}));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void BindPartnerB<T>(T model)
        {
            try
            {
                var partnera = new BussinessUserBLL { }.GetAllUsers();
                if (partnera != null)
                {
                    model.GetType().GetProperty("PartnerBList").SetValue(model, partnera.Select(x => new SelectListItem { Value = x.UserId.ToString(), Text = x.BusinessName }));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static void BindCoBrandedUsers<T>(T model)
        {
            try
            {
                var CoBraUsers = new CoBrandBLL { }.GetAllCoBrandUsers();
                if (CoBraUsers != null)
                {
                    model.GetType().GetProperty("CoBrandedUsersList").SetValue(model, CoBraUsers.Select(x => new SelectListItem { Value = x.CoBrandId.ToString(), Text = x.CoBrandedName }));
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal static void RoleManagement(int pageid)
        {
            try
            {
                int roleid = Convert.ToInt32(HttpContext.Current.Session["RoleId"]);
                var rm = new RolesBLL { }.GetRoleManagement(pageid, roleid);
                if (rm != null)
                {
                    HttpContext.Current.Session["Add"] = rm.Add;
                    HttpContext.Current.Session["Edit"] = rm.Edit;
                    HttpContext.Current.Session["Delete"] = rm.Delete;
                    HttpContext.Current.Session["View"] = rm.View;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal static int GetPageIDByPageName(string pagename)
        {
            try
            {
                string str = HttpContext.Current.Server.MapPath("~/Areas/Admin/RoleManagement.xml");
                var xml = XDocument.Load(str);
                var id = xml.Root.Elements("Page").Where(x => (string)x.Attribute("Name") == pagename)
                                .Select(x => x.Attribute("Id").Value).SingleOrDefault();
                return Convert.ToInt32(id);
            }
            catch (Exception)
            {

                throw;
            }
        }

        internal static bool ValidateRoles(string pagename)
        {
            try
            {
                RoleManagement(GetPageIDByPageName(pagename));
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

































        //public static void BindMemberships<T>(T model)
        //{
        //    try
        //    {
        //        var memberships = new MembershipBLL { }.GetAllMembership();
        //        if (memberships != null)
        //        {
        //            model.GetType().GetProperty("Memberships").SetValue(model, memberships.Select(x => new SelectListItem { Value = x.MembershipID.ToString(), Text = x.MembershipName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void BindMembershipsForInstructor<T>(T model)
        //{
        //    try
        //    {
        //        var memberships = new MembershipBLL { }.GetAllMembership();
        //        if (memberships != null)
        //        {
        //            var instmemberships = memberships.Where(x => x.MemberShipType == 1).ToList();
        //            if (instmemberships.Count > 0)
        //            {
        //                model.GetType().GetProperty("Memberships").SetValue(model, instmemberships.Select(x => new SelectListItem { Value = x.MembershipID.ToString(), Text = x.MembershipName }));
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void BindMembershipsForUser<T>(T model)
        //{
        //    try
        //    {
        //        var memberships = new MembershipBLL { }.GetAllMembership();
        //        if (memberships != null)
        //        {
        //            // var usermemberships = memberships.Where(x => x.MemberShipType == 2).ToList();
        //            //if (usermemberships.Count > 0)
        //            //{
        //            //    model.GetType().GetProperty("Memberships").SetValue(model, usermemberships.Select(x => new SelectListItem { Value = x.MembershipID.ToString(), Text = x.MembershipName }));
        //            //}
        //            if (memberships.Count > 0)
        //            {
        //                model.GetType().GetProperty("Memberships").SetValue(model, memberships.Select(x => new SelectListItem { Value = x.MembershipID.ToString(), Text = x.MembershipName }));
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        //public static void BindDepartmentDropdown<T>(T model,int cityId)
        //{
        //    try
        //    {
        //        var Departments = new DepartmentBLL { }.GetAllDepartmentbyCityId(cityId);
        //        if (Departments != null)
        //        {
        //            model.GetType().GetProperty("DepartmentList").SetValue(model, Departments.Select(x => new SelectListItem { Value = x.DepartmentId.ToString(), Text = x.DepartmentName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void BindUserTypes<T>(T model)
        //{
        //    try
        //    {
        //        var usertypes = new UserTypeBLL { }.GetAllUserTypes();
        //        if (usertypes != null)
        //        {
        //            model.GetType().GetProperty("UserTypes").SetValue(model, usertypes.Select(x => new SelectListItem { Value = x.UserTypeID.ToString(), Text = x.UserTypeName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void BindQuestionPatterns<T>(T model)
        //{
        //    try
        //    {
        //        var questionpatterns = new QuestionPaternBLL { }.GetAllQuestionPattern();
        //        if (questionpatterns != null)
        //        {
        //            model.GetType().GetProperty("QuestionPatterns").SetValue(model, questionpatterns
        //                            .Select(x => new SelectListItem { Value = x.PatternID.ToString(), Text = x.PatternName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void BindSubjects<T>(T model)
        //{
        //    try
        //    {
        //        var subjects = new SubjectBLL { }.GetAllSubjects();
        //        if (subjects != null)
        //        {
        //            model.GetType().GetProperty("Subjects").SetValue(model, subjects
        //                            .Select(x => new SelectListItem { Value = x.SubjectID.ToString(), Text = x.SubjectName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void BindStatus<T>(T model)
        //{
        //    try
        //    {
        //        var status = new StatusBLL { }.GetAllStatus();
        //        if (status != null)
        //        {
        //            model.GetType().GetProperty("AllStatus").SetValue(model, status
        //                            .Select(x => new SelectListItem { Value = x.StatusID.ToString(), Text = x.StatusName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void BindTestInstruction<T>(T model)
        //{
        //    try
        //    {
        //        var testinstruction = new TestInstructionBLL { }.GetAllTestInstructions();
        //        if (testinstruction != null)
        //        {
        //            model.GetType().GetProperty("TestInstructions").SetValue(model, testinstruction
        //                            .Select(x => new SelectListItem { Value = x.TestInstructionID.ToString(), Text = x.TestInstructionName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void BindOnlineTest<T>(T model)
        //{
        //    try
        //    {
        //        var onlinetestdetails = new OnlineTestDetailsBLL { }.GetAllOnlineTestDetails();
        //        if (onlinetestdetails != null)
        //        {
        //            model.GetType().GetProperty("OnlineTests").SetValue(model, onlinetestdetails
        //                            .Select(x => new SelectListItem { Value = x.TestID.ToString(), Text = x.TestName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public static void BindInstitute<T>(T model)
        //{
        //    try
        //    {
        //        var institute = new InstituteBLL { }.GetAllInstitutes();
        //        if (institute != null)
        //        {
        //            model.GetType().GetProperty("Institute").SetValue(model, institute
        //                            .Select(x => new SelectListItem { Value = x.InstituteID.ToString(), Text = x.InstituteName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}



        //public static bool SendMail(string MailTo, string Body, string Subject, string file)
        //{
        //    try
        //    {
        //        SmtpClient serv = new SmtpClient();
        //        MailMessage msg = new MailMessage();
        //        if (file != "")
        //        {
        //            string[] files = file.Split(',');
        //            for (int i = 0; i < files.Length; i++)
        //            {
        //                if (files[i] != "")
        //                {
        //                    Attachment attach = new Attachment(files[i]);
        //                    msg.Attachments.Add(attach);
        //                }
        //            }
        //        }
        //        msg.To.Add(MailTo);
        //        msg.Body = "body " + Body;
        //        msg.Subject = Subject;
        //        msg.IsBodyHtml = true;
        //        msg.From = new MailAddress("globicleb2b@gmail.com");
        //        serv.Host = "smtp.gmail.com";
        //        serv.Port = 587;
        //        serv.UseDefaultCredentials = true;
        //        serv.Credentials = new System.Net.NetworkCredential("globicleb2b@gmail.com", "Reset@357");
        //        serv.EnableSsl = true;
        //        serv.Send(msg);
        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }
        //}

        //public static void GetTestName<T>(T model)
        //{
        //    try
        //    {
        //        var courses = new OnlineTestBLL { }.GetTestName();

        //        if (courses != null)
        //        {
        //            model.GetType().GetProperty("testname").SetValue(model, courses.Where(x => x.TestID != null).Select(x => new SelectListItem { Value = x.TestID.ToString(), Text = x.TestName }));
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

    }
}

     
