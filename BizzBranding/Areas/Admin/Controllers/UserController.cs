using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBranding.BLL;
using BizzBranding.CommonUtility;
using BizzBranding.Areas.Admin.Models;
using BizzBranding.Areas.Admin.Filters;
using BizzBranding.DAL;
//using PagedList;

namespace BizzBranding.Areas.Admin.Controllers
{
    
    public class UserController : Controller
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();
       
        //
        // GET: /Admin/User/

        public ActionResult Index()
        {
            return View();
        }

        #region User
        public ActionResult Users(BussinessUserModel omodel,int pid = 0)
        {
            int take = 10;
            int skip = take * pid;

            UserWebModel model = new UserWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<UserWebModel> users = new List<UserWebModel>();
            CustomMethods.RoleManagement(CustomMethods.GetPageIDByPageName("User"));
            var userslist = new BussinessUserBLL { }.GetAllUsers(skip, take);
            double count = Convert.ToDouble(new BussinessUserBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (userslist != null)
            {
                model.Users = userslist.Select(x => new UserWebModel
                {
                    UserId = x.UserId,
                    //UserName = x.UserName,
                    BusinessName=x.BusinessName,
                    EmailId = x.EmailId,
                    CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive),
                }).ToList();
            }

            var countuser = new BussinessUserBLL { }.GetAllUsers().Count();
            model.CountUser = countuser.ToString();

            return View(model);
        }

        public ActionResult AddEditUser(int id = 0)
        {
            UserWebModel model = new UserWebModel();
            if (id != 0)
            {
                var objmodel = new BussinessUserBLL { }.GetUserById(id);
                if (objmodel != null)
                {
                    model.UserId = objmodel.UserId;
                    model.BusinessName = objmodel.BusinessName;
                    model.EmailId = objmodel.EmailId;
                    model.Password = objmodel.Password;
                    model.ConfirmPassword = objmodel.ConfirmPassword;
                    model.IsActive = Convert.ToBoolean(objmodel.IsActive);
                    model.ConfirmPassword = objmodel.ConfirmPassword;
                    model.CompanyLogo = objmodel.CompanyLogo;
                    model.CoRegNo = objmodel.CoRegNo;
                    model.CoAddress = objmodel.CoAddress;
                    model.Phone = objmodel.Phone;
                    model.Fax = objmodel.Fax;
                    model.ContactPerson = objmodel.ContactPerson;
                    model.Designation = objmodel.Designation;
                    model.TradeEmaiIId = objmodel.TradeEmaiIId;
                    model.BusinessDetails = objmodel.BusinessDetails;
                    model.IndustryId = objmodel.IndustryId;
                    model.CountryId = objmodel.CountryId;
                    model.CityId = objmodel.CityId;
                    
                }
            }
            CustomMethods.BindIndustry(model);
            CustomMethods.BindCountryWithCity(model);
            CustomMethods.BindCityForUser(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddEditUser(UserWebModel model,HttpPostedFileBase file)
        {
            try
            {
                if (model.UserId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }
                int res = 0;
                string strFileName = "";
                string path = "";
                Random rnd = new Random();
                //if (ModelState.IsValid)
                if (model.BusinessDetails!="" && model.BusinessName!="")
                {
                     var checkDuplicate = objdb.BussinessUsers.Where(x => x.BusinessName == model.BusinessName).FirstOrDefault();
                    if (checkDuplicate == null)
                    {
                        if (file != null)
                        {
                            strFileName = "CoLogo_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                            path = Server.MapPath("~/Content/Images/CoLogo/" + strFileName);
                            res = new BussinessUserBLL { }.AddEditUser(new BussinessUserModel
                            {
                                BusinessName = model.BusinessName,
                                EmailId = model.EmailId,
                                //Password = model.Password,
                                Password = DataEncryption.Encrypt(model.Password.Trim(), "passKey"),
                                ConfirmPassword = model.ConfirmPassword,
                                CoRegNo = model.CoRegNo,
                                CoAddress = model.CoAddress,
                                Phone = model.Phone,
                                Fax = model.Fax,
                                ContactPerson = model.ContactPerson,
                                Designation = model.Designation,
                                TradeEmaiIId = model.TradeEmaiIId,
                                BusinessDetails = model.BusinessDetails,
                                IsActive = true,
                                CreatedDate = model.CreatedDate,
                                //Image = model.Image,
                                CompanyLogo = file == null ? model.CompanyLogo : strFileName,
                                IndustryId = model.IndustryId,
                                CountryId = model.CountryId,
                                CityId = model.CityId,
                            });
                            file.SaveAs(path);
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("Users");
                            }
                        }
                        Session["Error"] = "Record Already Exists";
                        return RedirectToAction("Users");
                    }
                    else
                    {
                        res = new BussinessUserBLL { }.AddEditUser(new BussinessUserModel
                        {
                            UserId=model.UserId,
                            BusinessName = model.BusinessName,
                            EmailId = model.EmailId,
                            Password = model.Password,
                            ConfirmPassword = model.ConfirmPassword,
                            CoRegNo = model.CoRegNo,
                            CoAddress = model.CoAddress,
                            Phone = model.Phone,
                            Fax = model.Fax,
                            ContactPerson = model.ContactPerson,
                            Designation = model.Designation,
                            TradeEmaiIId = model.TradeEmaiIId,
                            BusinessDetails = model.BusinessDetails,
                            IsActive = true,
                            CreatedDate = DateTime.Now,
                            //Image = model.Image,
                            CompanyLogo = file == null ? model.CompanyLogo : strFileName,
                            IndustryId = model.IndustryId,
                            CountryId = model.CountryId,
                            CityId = model.CityId,

                        });
                    }


                    if (res != 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("Users");
                    }
                }
                Session["Error"] = "An error has occured";
                CustomMethods.BindIndustry(model);
                CustomMethods.BindCountryWithCity(model);
                CustomMethods.BindCityForUser(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An error has occured";
                CustomMethods.BindIndustry(model);
                CustomMethods.BindCountryWithCity(model);
                CustomMethods.BindCityForUser(model);
                return View(model);
                throw;
            }
        }

        public ActionResult ViewUser(int id = 0)
        {
            UserWebModel model = new UserWebModel();
            if (id != 0)
            {
                var objmodel = new BussinessUserBLL { }.GetUserById(id);
                if (objmodel != null)
                {
                    model.UserId = objmodel.UserId;
                    model.BusinessName = objmodel.BusinessName;
                    model.EmailId = objmodel.EmailId;
                    model.Password = objmodel.Password;
                    model.ConfirmPassword = objmodel.ConfirmPassword;
                    model.CompanyLogo = objmodel.CompanyLogo;
                    model.CoRegNo = objmodel.CoRegNo;
                    model.CoAddress = objmodel.CoAddress;
                    model.Phone = objmodel.Phone;
                    model.Fax = objmodel.Fax;
                    model.ContactPerson = objmodel.ContactPerson;
                    model.Designation = objmodel.Designation;
                    model.TradeEmaiIId = objmodel.TradeEmaiIId;
                    model.BusinessDetails = objmodel.BusinessDetails;
                    model.IndustryId = objmodel.IndustryId;
                    model.IndustryName = objmodel.IndustryName;
                    model.CountryName = objmodel.CountryName;
                    model.CityName = objmodel.CityName;
                    model.CountryId = objmodel.CountryId;
                    model.CityId = objmodel.CityId;
                    model.IsActive = Convert.ToBoolean(objmodel.IsActive);
                }
            }
            CustomMethods.BindIndustry(model);
            CustomMethods.BindCountryWithCity(model);
            CustomMethods.BindCityForUser(model);
            return View(model);
        }

        public JsonResult ChangeUserStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new BussinessUserBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult GetCity(int id)
        {
            try
            {
                BussinessUserModel usermodelObj = new BussinessUserModel();
                var cityList = new CityBLL { }.GetAllCity().Where(x => x.CountryId == id).Select(x => new CityModel
                {
                    CityId = x.CityId,
                    CityName = x.CityName,
                }).ToList();
                return Json(cityList);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public ActionResult SearchUser(string date, string date1)
        {
            DateTime dt1 = Convert.ToDateTime(date);
            DateTime dt2 = Convert.ToDateTime(date1);
            var omodel = objdb.BussinessUsers.Where(x => x.CreatedDate >= dt1 && x.CreatedDate <= dt2).Select(x => new BussinessUserModel
            {
                UserId = x.UserId,
                BusinessName = x.BusinessName,
                EmailId = x.EmailId,
                CreatedDate = x.CreatedDate,

            }).OrderBy(x => x.CreatedDate).ToList();
            if (omodel != null)
            {
                return View(omodel);
            }
            else
            {
                return RedirectToAction("Users", omodel);

            }

            //return objdb.BussinessUsers.Where(x => x.CreatedDate == stdate && x.CreatedDate == endate).Select(x => new BussinessUserModel
            //{
            //    UserId = x.UserId,
            //    BusinessName = x.BusinessName,
            //    EmailId = x.EmailId,
            //    CreatedDate = x.CreatedDate,

            //}).OrderBy(x => x.CreatedDate).ToList();
        }

        public ActionResult SearchProducts(string date, string date1)
        {
            DateTime dt1 = Convert.ToDateTime(date);
            DateTime dt2 = Convert.ToDateTime(date1);
            var omodel = objdb.Products.Where(x => x.CreatedDate >= dt1 && x.CreatedDate <= dt2).Select(x => new ProductWebModel
            {
                CategoryId = x.CategoryId,
                SubCategoryId = x.SubCategoryId,
                ProductId = x.ProductId,
                ProductName = x.ProductName,
                ProdDetails = x.ProdDetails,
                //CreatedDate = Convert.ToInt32(x.CreatedDate),
                CreatedDate = x.CreatedDate,
                IsActive = x.IsActive

            }).OrderBy(x => x.CreatedDate).ToList();
            if (omodel != null)
            {
                return View(omodel);
            }
            else
            {
                return RedirectToAction("Products","Product");

            }

            //return objdb.BussinessUsers.Where(x => x.CreatedDate == stdate && x.CreatedDate == endate).Select(x => new BussinessUserModel
            //{
            //    UserId = x.UserId,
            //    BusinessName = x.BusinessName,
            //    EmailId = x.EmailId,
            //    CreatedDate = x.CreatedDate,

            //}).OrderBy(x => x.CreatedDate).ToList();
        }

        public ActionResult DeleteUser(int id)
        {
            var User = new BussinessUserBLL { }.DeleteUser(id);
            if (User != 0)
            {
                return RedirectToAction("Users");
            }
            return RedirectToAction("Users");
        }
        #endregion



    }
}
