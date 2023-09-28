using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBranding.Areas.Admin.Models;
using BizzBranding.BLL;
using BizzBranding.CommonUtility;
using BizzBranding.Areas.Admin.Filters;
using System.Xml.Linq;
using BizzBranding.DAL;
using System.Data;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace BizzBranding.Areas.Admin.Controllers
{
    public class MasterDataController : Controller
    {
        //
        // GET: /Admin/MasterData/
        BizzBrandingEntities objdb = new BizzBrandingEntities();
        public static int iVal = 0;
        public ActionResult Index()
        {
            return View();
        }

        #region SubCategory
        public ActionResult SubCategory(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            SubCategoryWebModel model = new SubCategoryWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<SubCategoryWebModel> Courses = new List<SubCategoryWebModel>();
            CustomMethods.ValidateRoles("SubCategory");
            var SubCatlist = new SubCategoryBLL { }.GetAllSubCategory(skip, take);


            model.SubCategory = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new SubCategoryBLL { }.GetAllSubCategory(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.SubCategories = sortedlist.Select(x => new SubCategoryWebModel
                {
                    ParentCategoryId = x.ParentCategoryId,
                    SubCategoryId = x.SubCategoryId,
                    SubCatgoryName = x.SubCatgoryName,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (SubCatlist != null)
                {
                    double count = Convert.ToDouble(new SubCategoryBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.SubCategories = SubCatlist.Select(x => new SubCategoryWebModel
                    {
                        SubCategoryId = x.SubCategoryId,
                        SubCatgoryName = x.SubCatgoryName,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            CustomMethods.BindParentCategory(model);
            return View(model);
        }

        public ActionResult AddEditSubCategory(int id = 0)
        {
            CustomMethods.ValidateRoles("SubCategory");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            SubCategoryWebModel SubCatModel = new SubCategoryWebModel();
            if (id != 0)
            {
                var objcity = new SubCategoryBLL { }.GetSubCategoryById(id);
                if (objcity != null)
                {
                    SubCatModel.SubCategoryId = objcity.SubCategoryId;
                    SubCatModel.SubCatgoryName = objcity.SubCatgoryName;
                    SubCatModel.ParentCategoryId = objcity.ParentCategoryId;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    SubCatModel.IsActive = Convert.ToBoolean(objcity.IsActive);
                }
            }
            CustomMethods.BindParentCategory(SubCatModel);
            return View(SubCatModel);
        }

        [HttpPost]
        public ActionResult AddEditSubCategory(SubCategoryWebModel model)
        {
            try
            {
                if (model.SubCategoryId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.SubCategoryId == 0)
                    {
                        var checkDuplicate = objdb.SubCategories.Where(x => x.SubCatgoryName == model.SubCatgoryName && x.ParentCategoryId == model.ParentCategoryId).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new SubCategoryBLL { }.AddEditSubCategory(new SubCategoryModel
                            {
                                SubCategoryId = model.SubCategoryId,
                                SubCatgoryName = model.SubCatgoryName,
                                //CourseDetails = model.CourseDetails,
                                ParentCategoryId = model.ParentCategoryId,
                                IsActive = model.IsActive,
                                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("SubCategory");
                            }

                        }
                    }
                    else
                    {
                        int result = new SubCategoryBLL { }.AddEditSubCategory(new SubCategoryModel
                        {
                            SubCategoryId = model.SubCategoryId,
                            SubCatgoryName = model.SubCatgoryName,
                            ParentCategoryId = model.ParentCategoryId,
                            IsActive = model.IsActive,
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("SubCategory");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("SubCategory");
                }
                Session["Error"] = "An Error has occured";
                CustomMethods.BindParentCategory(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                CustomMethods.BindParentCategory(model);
                return View(model);
                throw;
            }
        }

        public ActionResult ViewSubCategory(int id = 0)
        {
            CustomMethods.ValidateRoles("SubCategory");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            SubCategoryWebModel subcat = new SubCategoryWebModel();
            if (id != 0)
            {
                var objsubcat = new SubCategoryBLL { }.GetSubCategoryById(id);
                if (objsubcat != null)
                {
                    subcat.SubCategoryId = objsubcat.SubCategoryId;
                    subcat.SubCatgoryName = objsubcat.SubCatgoryName;
                    subcat.ParentCategoryId = objsubcat.ParentCategoryId;  //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    subcat.CategoryName = objsubcat.CategoryName;
                    subcat.IsActive = Convert.ToBoolean(objsubcat.IsActive);
                }
            }
            CustomMethods.BindSubategory(subcat);
            return View(subcat);
        }

        public JsonResult ChangeSubCatStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new SubCategoryBLL { }.ChangeSubCatStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }
        #endregion

        #region Category
        public ActionResult Category(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            CategoryWebModel model = new CategoryWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<CategoryWebModel> Categories = new List<CategoryWebModel>();
            CustomMethods.ValidateRoles("Category");
            var Categorylist = new CategoryBLL { }.GetAllCategory(skip, take);
            model.Category = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new CategoryBLL { }.GetAllCategory(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.Categories = sortedlist.Select(x => new CategoryWebModel
                {
                    CategoryId = x.CategoryId,
                    CategoryName = x.CategoryName,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (Categorylist != null)
                {
                    double count = Convert.ToDouble(new CategoryBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.Categories = Categorylist.Select(x => new CategoryWebModel
                    {
                        CategoryId = x.CategoryId,
                        CategoryName = x.CategoryName,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            //CustomMethods.BindParentCourse(model);
            return View(model);
        }

        public ActionResult AddEditCategory(int id = 0)
        {
            CustomMethods.ValidateRoles("Category");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CategoryWebModel CategoryModel = new CategoryWebModel();
            if (id != 0)
            {
                var objcategory = new CategoryBLL { }.GetCategoryById(id);
                if (objcategory != null)
                {
                    CategoryModel.CategoryId = objcategory.CategoryId;
                    CategoryModel.CategoryName = objcategory.CategoryName;
                    CategoryModel.IsActive = Convert.ToBoolean(objcategory.IsActive);
                }
            }
            //CustomMethods.BindParentCourse(CountryModel);
            return View(CategoryModel);
        }

        [HttpPost]
        public ActionResult AddEditCategory(CategoryWebModel model)
        {
            try
            {
                if (model.CategoryId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.CategoryId == 0)
                    {
                        var checkDuplicate = objdb.Categories.Where(x => x.CategoryName == model.CategoryName).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new CategoryBLL { }.AddEditCategory(new CategoryModel
                            {
                                CategoryId = model.CategoryId,
                                CategoryName = model.CategoryName,
                                IsActive = model.IsActive,
                                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("Category");
                            }

                        }
                    }

                    else
                    {
                        var checkDuplicate = objdb.Categories.Where(x => x.CategoryName == model.CategoryName).FirstOrDefault();

                        if (checkDuplicate == null)
                        {
                            int result = new CategoryBLL { }.AddEditCategory(new CategoryModel
                                            {
                                                CategoryId = model.CategoryId,
                                                CategoryName = model.CategoryName,
                                                IsActive = model.IsActive,
                                            });
                            if (result != 0)
                            {
                                Session["Success"] = "Successfully Updated The Record";
                                return RedirectToAction("Category");
                            }
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("Category");
                }
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindParentCourse(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindParentCourse(model);
                return View(model);
                throw;
            }
        }

        public ActionResult ViewCategory(int id = 0)
        {
            CustomMethods.ValidateRoles("Category");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CategoryWebModel category = new CategoryWebModel();
            if (id != 0)
            {
                var objcategory = new CategoryBLL { }.GetCategoryById(id);
                if (objcategory != null)
                {
                    category.CategoryName = objcategory.CategoryName;
                    category.CategoryId = objcategory.CategoryId;
                    //category.CategoryId = objcategory.CategoryId == null ? 0 : Convert.ToInt32(objcategory.CategoryId);
                    category.IsActive = Convert.ToBoolean(objcategory.IsActive);
                }
            }
            //CustomMethods.BindParentCourse(country);
            return View(category);
        }

        public JsonResult ChangeCategoryStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new CategoryBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }
        #endregion

        #region City
        public ActionResult City(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            CityWebModel model = new CityWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<CityWebModel> Courses = new List<CityWebModel>();
            CustomMethods.ValidateRoles("City");
            var Citieslist = new CityBLL { }.GetAllCity(skip, take);


            model.City = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new CityBLL { }.GetAllCity(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.Cities = sortedlist.Select(x => new CityWebModel
                {
                    CountryId = x.CountryId,
                    CityId = x.CityId,
                    CityName = x.CityName,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (Citieslist != null)
                {
                    double count = Convert.ToDouble(new CityBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.Cities = Citieslist.Select(x => new CityWebModel
                    {
                        CityId = x.CityId,
                        CityName = x.CityName,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            CustomMethods.BindCountryWithCity(model);
            return View(model);
        }

        public ActionResult AddEditCity(int id = 0)
        {
            CustomMethods.ValidateRoles("City");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CityWebModel CityModel = new CityWebModel();
            if (id != 0)
            {
                var objcity = new CityBLL { }.GetCityById(id);
                if (objcity != null)
                {
                    CityModel.CityId = objcity.CityId;
                    CityModel.CityName = objcity.CityName;
                    CityModel.CountryId = objcity.CountryId;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    CityModel.IsActive = Convert.ToBoolean(objcity.IsActive);
                }
            }
            CustomMethods.BindCountryWithCity(CityModel);
            return View(CityModel);
        }

        [HttpPost]
        public ActionResult AddEditCity(CityWebModel model)
        {
            try
            {
                if (model.CityId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.CityId == 0)
                    {
                        var checkDuplicate = objdb.Cities.Where(x => x.CityName == model.CityName).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new CityBLL { }.AddEditCity(new CityModel
                            {
                                CityId = model.CityId,
                                CityName = model.CityName,
                                //CourseDetails = model.CourseDetails,
                                CountryId = model.CountryId,
                                IsActive = model.IsActive,
                                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("City");
                            }

                        }
                    }

                    else
                    {
                        int result = new CityBLL { }.AddEditCity(new CityModel
                        {
                            CityId = model.CityId,
                            CityName = model.CityName,
                            CountryId = model.CountryId,
                            IsActive = model.IsActive,
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("City");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("City");
                }
                Session["Error"] = "An Error has occured";
                CustomMethods.BindCountryWithCity(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                CustomMethods.BindCountryWithCity(model);
                return View(model);
                throw;
            }
        }

        public ActionResult ViewCity(int id = 0)
        {
            CustomMethods.ValidateRoles("City");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CityWebModel city = new CityWebModel();
            if (id != 0)
            {
                var objcity = new CityBLL { }.GetCityById(id);
                if (objcity != null)
                {
                    city.CityId = objcity.CityId;
                    city.CityName = objcity.CityName;
                    city.CountryId = objcity.CountryId;  //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    city.CountryName = objcity.CountryName;
                    city.IsActive = Convert.ToBoolean(objcity.IsActive);
                }
            }
            CustomMethods.BindCity(city);
            return View(city);
        }

        public JsonResult ChangeCityStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new CityBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }
        #endregion

        #region Country
        public ActionResult Country(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            CountryWebModel model = new CountryWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<CountryWebModel> Countries = new List<CountryWebModel>();
            CustomMethods.ValidateRoles("Country");
            var Countrylist = new CountryBLL { }.GetAllCountry(skip, take);


            model.Country = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new CountryBLL { }.GetAllCountry(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.Countries = sortedlist.Select(x => new CountryWebModel
                {
                    CountryId = x.CountryId,
                    CountryName = x.CountryName,
                    //CountryCode = x.CountryCode,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (Countrylist != null)
                {
                    double count = Convert.ToDouble(new CountryBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.Countries = Countrylist.Select(x => new CountryWebModel
                    {
                        CountryId = x.CountryId,
                        //CountryCode = x.CountryCode,
                        CountryName = x.CountryName,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            //CustomMethods.BindParentCourse(model);
            return View(model);
        }

        public ActionResult AddEditCountry(int id = 0)
        {
            CustomMethods.ValidateRoles("Country");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CountryWebModel CountryModel = new CountryWebModel();
            if (id != 0)
            {
                var objcountry = new CountryBLL { }.GetCountryById(id);
                if (objcountry != null)
                {
                    CountryModel.CountryId = objcountry.CountryId;
                    CountryModel.CountryName = objcountry.CountryName;
                    //CountryModel.CountryCode = objcountry.CountryCode;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    CountryModel.IsActive = Convert.ToBoolean(objcountry.IsActive);
                }
            }
            //CustomMethods.BindParentCourse(CountryModel);
            return View(CountryModel);
        }

        [HttpPost]
        public ActionResult AddEditCountry(CountryWebModel model)
        {
            try
            {
                if (model.CountryId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.CountryId == 0)
                    {
                        var checkDuplicate = objdb.Countries.Where(x => x.CountryName == model.CountryName).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new CountryBLL { }.AddEditCountry(new CountryModel
                            {
                                CountryId = model.CountryId,
                                CountryName = model.CountryName,
                                //CountryCode = model.CountryCode,
                                IsActive = model.IsActive,
                                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("Country");
                            }

                        }
                    }

                    else
                    {
                        int result = new CountryBLL { }.AddEditCountry(new CountryModel
                        {
                            CountryId = model.CountryId,
                            CountryName = model.CountryName,
                            IsActive = model.IsActive,
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("Country");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("Country");
                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult ViewCountry(int id = 0)
        {
            CustomMethods.ValidateRoles("Country");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CountryWebModel country = new CountryWebModel();
            if (id != 0)
            {
                var objcountry = new CountryBLL { }.GetCountryById(id);
                if (objcountry != null)
                {
                    //country.CountryCode = objcountry.CountryCode;
                    country.CountryName = objcountry.CountryName;
                    country.CountryId = objcountry.CountryId == null ? 0 : Convert.ToInt32(objcountry.CountryId);
                    country.IsActive = Convert.ToBoolean(objcountry.IsActive);
                }
            }
            return View(country);
        }

        public JsonResult ChangeCountryStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new CountryBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }
        #endregion

        #region EmailTemplate
        public ActionResult EmailTemplate(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            EmailTemplateWebModel model = new EmailTemplateWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<EmailTemplateWebModel> emailtemps = new List<EmailTemplateWebModel>();
            CustomMethods.ValidateRoles("EmailTemplate");
            var EmailTemplist = new EmailTemplateBLL { }.GetAllEmailTemplate(skip, take);


            model.EmailTemplate = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new EmailTemplateBLL { }.GetAllEmailTemplate(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.EmailTemplates = sortedlist.Select(x => new EmailTemplateWebModel
                {
                    EmailTempId = x.EmailTempId,
                    EmailTempTitle = x.EmailTempTitle,
                    Description = x.Description,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (EmailTemplist != null)
                {
                    double count = Convert.ToDouble(new CategoryBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.EmailTemplates = EmailTemplist.Select(x => new EmailTemplateWebModel
                    {
                        EmailTempId = x.EmailTempId,
                        EmailTempTitle = x.EmailTempTitle,
                        Description = x.Description,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult AddEditEmailTemplate(int id = 0)
        {
            CustomMethods.ValidateRoles("EmailTemplate");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            EmailTemplateWebModel EmailTempModel = new EmailTemplateWebModel();
            if (id != 0)
            {
                var objemailtemp = new EmailTemplateBLL { }.GetEmailTemplateById(id);
                if (objemailtemp != null)
                {
                    EmailTempModel.EmailTempId = objemailtemp.EmailTempId;
                    EmailTempModel.EmailTempTitle = objemailtemp.EmailTempTitle;
                    EmailTempModel.Description = objemailtemp.Description;
                    EmailTempModel.IsActive = Convert.ToBoolean(objemailtemp.IsActive);
                }
            }
            return View(EmailTempModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddEditEmailTemplate(EmailTemplateWebModel model)
        {
            try
            {
                if (model.EmailTempId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.EmailTempId == 0)
                    {
                        var checkDuplicate = objdb.EmailTemplates.Where(x => x.EmailTempTitle == model.EmailTempTitle).SingleOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new EmailTemplateBLL { }.AddEditEmailTemplate(new EmailTemplateModel
                            {
                                EmailTempId = model.EmailTempId,
                                EmailTempTitle = model.EmailTempTitle,
                                Description = model.Description,
                                IsActive = model.IsActive,
                                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("EmailTemplate");
                            }
                        }
                    }

                    else
                    {
                        int result = new EmailTemplateBLL { }.AddEditEmailTemplate(new EmailTemplateModel
                        {
                            EmailTempId = model.EmailTempId,
                            EmailTempTitle = model.EmailTempTitle,
                            Description = model.Description,
                            IsActive = model.IsActive,
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("EmailTemplate");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("EmailTemplate");

                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult ViewEmailTemplate(int id = 0)
        {
            CustomMethods.ValidateRoles("EmailTemplate");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            EmailTemplateWebModel emailtemp = new EmailTemplateWebModel();
            if (id != 0)
            {
                var objemailtemp = new EmailTemplateBLL { }.GetEmailTemplateById(id);
                if (objemailtemp != null)
                {
                    emailtemp.EmailTempTitle = objemailtemp.EmailTempTitle;
                    emailtemp.EmailTempId = objemailtemp.EmailTempId == null ? 0 : Convert.ToInt32(objemailtemp.EmailTempId);
                    emailtemp.Description = objemailtemp.Description;
                    emailtemp.IsActive = Convert.ToBoolean(objemailtemp.IsActive);
                }
            }
            return View(emailtemp);
        }

        public JsonResult ChangeEmailTemptStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new EmailTemplateBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }
        #endregion

        #region Industry
        public ActionResult Industry(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            IndustryWebModel model = new IndustryWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<IndustryWebModel> industry = new List<IndustryWebModel>();
            CustomMethods.ValidateRoles("Industry");
            var Industrylist = new IndustryBLL { }.GetAllIndustry(skip, take);


            model.Industry = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new IndustryBLL { }.GetAllIndustry(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.Industries = sortedlist.Select(x => new IndustryWebModel
                {
                    IndustryId = x.IndustryId,
                    IndustryName = x.IndustryName,
                    Description = x.Description,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (Industrylist != null)
                {
                    double count = Convert.ToDouble(new IndustryBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.Industries = Industrylist.Select(x => new IndustryWebModel
                    {
                        IndustryId = x.IndustryId,
                        IndustryName = x.IndustryName,
                        Description = x.Description,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult AddEditIndustry(int id = 0)
        {
            CustomMethods.ValidateRoles("Industry");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            IndustryWebModel IndustryModel = new IndustryWebModel();
            if (id != 0)
            {
                var objindustry = new IndustryBLL { }.GetIndustryById(id);
                if (objindustry != null)
                {
                    IndustryModel.IndustryId = objindustry.IndustryId;
                    IndustryModel.IndustryName = objindustry.IndustryName;
                    IndustryModel.Description = objindustry.Description;
                    IndustryModel.IsActive = Convert.ToBoolean(objindustry.IsActive);
                }
            }
            return View(IndustryModel);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddEditIndustry(IndustryWebModel model)
        {
            try
            {

                if (model.IndustryId == 0)
                {

                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.IndustryId == 0)
                    {
                        var checkDuplicate = objdb.Industries.Where(x => x.IndustryName == model.IndustryName).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new IndustryBLL { }.AddEditIndustry(new IndustryModel
                            {
                                IndustryId = model.IndustryId,
                                IndustryName = model.IndustryName,
                                Description = model.Description,
                                IsActive = model.IsActive,
                                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("Industry");
                            }
                        }
                    }
                    else
                    {
                        int result = new IndustryBLL { }.AddEditIndustry(new IndustryModel
                        {
                            IndustryId = model.IndustryId,
                            IndustryName = model.IndustryName,
                            Description = model.Description,
                            IsActive = model.IsActive
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("Industry");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("Industry");
                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult ViewIndustry(int id = 0)
        {
            CustomMethods.ValidateRoles("Industry");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            IndustryWebModel industry = new IndustryWebModel();
            if (id != 0)
            {
                var objindustry = new IndustryBLL { }.GetIndustryById(id);
                if (objindustry != null)
                {
                    industry.IndustryName = objindustry.IndustryName;
                    industry.IndustryId = objindustry.IndustryId == null ? 0 : Convert.ToInt32(objindustry.IndustryId);
                    industry.Description = objindustry.Description;
                    industry.IsActive = Convert.ToBoolean(objindustry.IsActive);
                }
            }
            return View(industry);
        }

        public JsonResult ChangeIndustryStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new IndustryBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }
        #endregion

        #region Currency
        public ActionResult Currency(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            CurrencyWebModel model = new CurrencyWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<CurrencyWebModel> Currencies = new List<CurrencyWebModel>();
            CustomMethods.ValidateRoles("Currency");
            var Currencylist = new CurrencyBLL { }.GetAllCurrency(skip, take);


            model.Currency = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new CurrencyBLL { }.GetAllCurrency(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.Currencies = sortedlist.Select(x => new CurrencyWebModel
                {
                    CurrencyId = x.CurrencyId,
                    CurrencyName = x.CurrencyName,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (Currencylist != null)
                {
                    double count = Convert.ToDouble(new CurrencyBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.Currencies = Currencylist.Select(x => new CurrencyWebModel
                    {
                        CurrencyId = x.CurrencyId,
                        CurrencyName = x.CurrencyName,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult AddEditCurrency(int id = 0)
        {
            CustomMethods.ValidateRoles("Currency");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CurrencyWebModel CurrencyModel = new CurrencyWebModel();
            if (id != 0)
            {
                var objcurrency = new CurrencyBLL { }.GetCurrencyById(id);
                if (objcurrency != null)
                {
                    CurrencyModel.CurrencyId = objcurrency.CurrencyId;
                    CurrencyModel.CurrencyName = objcurrency.CurrencyName;
                    CurrencyModel.IsActive = Convert.ToBoolean(objcurrency.IsActive);
                }
            }
            return View(CurrencyModel);
        }

        [HttpPost]
        public ActionResult AddEditCurrency(CurrencyWebModel model)
        {
            try
            {
                if (model.CurrencyId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.CurrencyId == 0)
                    {
                        var checkDuplicate = objdb.Currencies.Where(x => x.CurrencyName == model.CurrencyName).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new CurrencyBLL { }.AddEditCurrency(new CurrencyModel
                            {
                                CurrencyId = model.CurrencyId,
                                CurrencyName = model.CurrencyName,
                                IsActive = model.IsActive,
                                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("Currency");
                            }


                        }
                    }

                    else
                    {
                        int result = new CurrencyBLL { }.AddEditCurrency(new CurrencyModel
                        {
                            CurrencyId = model.CurrencyId,
                            CurrencyName = model.CurrencyName,
                            IsActive = model.IsActive,
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("Currency");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("Currency");
                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult ViewCurrency(int id = 0)
        {
            CustomMethods.ValidateRoles("Currency");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CurrencyWebModel currency = new CurrencyWebModel();
            if (id != 0)
            {
                var objcurrency = new CurrencyBLL { }.GetCurrencyById(id);
                if (objcurrency != null)
                {
                    currency.CurrencyName = objcurrency.CurrencyName;
                    currency.CurrencyId = objcurrency.CurrencyId == null ? 0 : Convert.ToInt32(objcurrency.CurrencyId);
                    currency.IsActive = Convert.ToBoolean(objcurrency.IsActive);
                }
            }
            return View(currency);
        }

        public JsonResult ChangeCurrencyStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new CurrencyBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }
        #endregion

        #region AccessPlan
        public ActionResult AccessPlan(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            AccessPlanWebModel model = new AccessPlanWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<AccessPlanWebModel> AccessPlans = new List<AccessPlanWebModel>();
            CustomMethods.ValidateRoles("AccessPlan");
            var AccessPlanlist = new AccessPlanBLL { }.GetAllAccessPlan(skip, take);


            model.AccessPlan = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new AccessPlanBLL { }.GetAllAccessPlan(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.AccessPlans = sortedlist.Select(x => new AccessPlanWebModel
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
                    AccessPlanImage = x.AccessPlanImage,
                    CurrencyId = x.CurrencyId,
                    Validity = x.Validity,
                    CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (AccessPlanlist != null)
                {
                    double count = Convert.ToDouble(new AccessPlanBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.AccessPlans = AccessPlanlist.Select(x => new AccessPlanWebModel
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
                        AccessPlanImage = x.AccessPlanImage,
                        CurrencyId = x.CurrencyId,
                        Validity = x.Validity,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult AddEditAccessPlan(int id = 0)
        {
            CustomMethods.ValidateRoles("AccessPlan");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            AccessPlanWebModel AccessPlanModel = new AccessPlanWebModel();
            if (id != 0)
            {
                var objacplan = new AccessPlanBLL { }.GetAccessPlanById(id);
                if (objacplan != null)
                {
                    AccessPlanModel.AccessPlanId = objacplan.AccessPlanId;
                    AccessPlanModel.AccessPlanName = objacplan.AccessPlanName;
                    AccessPlanModel.Description = objacplan.Description;
                    AccessPlanModel.MaxNoDeals = objacplan.MaxNoDeals;
                    AccessPlanModel.ProdPerMonth = objacplan.ProdPerMonth;
                    AccessPlanModel.UpdatesPerMonth = objacplan.UpdatesPerMonth;
                    AccessPlanModel.AddPostCharge = objacplan.AddPostCharge;
                    AccessPlanModel.NewsCharge = objacplan.NewsCharge;
                    AccessPlanModel.AddUpdCharge = objacplan.AddUpdCharge;
                    AccessPlanModel.MembershipFee = objacplan.MembershipFee;
                    AccessPlanModel.Validity = objacplan.Validity;
                    AccessPlanModel.AccessPlanImage = objacplan.AccessPlanImage;
                    AccessPlanModel.CurrencyId = objacplan.CurrencyId;
                    AccessPlanModel.IsActive = Convert.ToBoolean(objacplan.IsActive);
                }
            }
            CustomMethods.BindCurrencyForMembership(AccessPlanModel);
            return View(AccessPlanModel);
        }

        [HttpPost]
        public ActionResult AddEditAccessPlan(AccessPlanWebModel model, HttpPostedFileBase file)
        {
            try
            {
                if (model.AccessPlanId == 0)
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
                if (ModelState.IsValid)
                {
                    if (model.AccessPlanId == 0)
                    {
                        var checkDuplicate = objdb.AccessPlans.Where(x => x.AccessPlanName == model.AccessPlanName).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            if (file != null)
                            {
                                strFileName = "AccessPlanImage_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                                path = Server.MapPath("~/Areas/Admin/ProjectImages/AccessPlanImage/" + strFileName);
                                res = new AccessPlanBLL { }.AddEditAccessPlan(new AccessPlanModel
                                {
                                    AccessPlanId = model.AccessPlanId,
                                    AccessPlanName = model.AccessPlanName,
                                    Description = model.Description,
                                    MaxNoDeals = model.MaxNoDeals,
                                    ProdPerMonth = model.ProdPerMonth,
                                    UpdatesPerMonth = model.UpdatesPerMonth,
                                    AddPostCharge = model.AddPostCharge,
                                    NewsCharge = model.NewsCharge,
                                    AddUpdCharge = model.AddUpdCharge,
                                    MembershipFee = model.MembershipFee,
                                    Validity = model.Validity,
                                    IsActive = model.IsActive,
                                    AccessPlanImage = file == null ? model.AccessPlanImage : strFileName,
                                    CurrencyId = model.CurrencyId,
                                    CreatedDate = model.CreatedDate,
                                    //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                                });
                                file.SaveAs(path);
                                if (res != 0)
                                {
                                    Session["Success"] = "Successfully Added The Record";
                                    return RedirectToAction("AccessPlan");
                                }
                            }
                            else
                            {
                                res = new AccessPlanBLL { }.AddEditAccessPlan(new AccessPlanModel
                                {
                                    AccessPlanId = model.AccessPlanId,
                                    AccessPlanName = model.AccessPlanName,
                                    Description = model.Description,
                                    MaxNoDeals = model.MaxNoDeals,
                                    ProdPerMonth = model.ProdPerMonth,
                                    UpdatesPerMonth = model.UpdatesPerMonth,
                                    AddPostCharge = model.AddPostCharge,
                                    NewsCharge = model.NewsCharge,
                                    AddUpdCharge = model.AddUpdCharge,
                                    MembershipFee = model.MembershipFee,
                                    Validity = model.Validity,
                                    AccessPlanImage = file == null ? model.AccessPlanImage : strFileName,
                                    CurrencyId = model.CurrencyId,
                                    IsActive = model.IsActive,
                                    CreatedDate = model.CreatedDate,
                                });
                            }
                            //Session["Error"] = "Record Already Exists";
                            //return RedirectToAction("AccessPlan");
                        }

                    }
                    else
                    {
                        if (file != null)
                        {
                            strFileName = "AccessPlanImage_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                            path = Server.MapPath("~/Areas/Admin/ProjectImages/AccessPlanImage/" + strFileName);
                            res = new AccessPlanBLL { }.AddEditAccessPlan(new AccessPlanModel
                            {
                                AccessPlanId = model.AccessPlanId,
                                AccessPlanName = model.AccessPlanName,
                                Description = model.Description,
                                MaxNoDeals = model.MaxNoDeals,
                                ProdPerMonth = model.ProdPerMonth,
                                UpdatesPerMonth = model.UpdatesPerMonth,
                                AddPostCharge = model.AddPostCharge,
                                NewsCharge = model.NewsCharge,
                                AddUpdCharge = model.AddUpdCharge,
                                MembershipFee = model.MembershipFee,
                                Validity = model.Validity,
                                IsActive = model.IsActive,
                                AccessPlanImage = file == null ? model.AccessPlanImage : strFileName,
                                CurrencyId = model.CurrencyId,
                                CreatedDate = model.CreatedDate,
                                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                            });
                            file.SaveAs(path);
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("AccessPlan");
                            }
                        }
                        else
                        {
                            res = new AccessPlanBLL { }.AddEditAccessPlan(new AccessPlanModel
                            {
                                AccessPlanId = model.AccessPlanId,
                                AccessPlanName = model.AccessPlanName,
                                Description = model.Description,
                                MaxNoDeals = model.MaxNoDeals,
                                ProdPerMonth = model.ProdPerMonth,
                                UpdatesPerMonth = model.UpdatesPerMonth,
                                AddPostCharge = model.AddPostCharge,
                                NewsCharge = model.NewsCharge,
                                AddUpdCharge = model.AddUpdCharge,
                                MembershipFee = model.MembershipFee,
                                Validity = model.Validity,
                                AccessPlanImage = file == null ? model.AccessPlanImage : strFileName,
                                CurrencyId = model.CurrencyId,
                                IsActive = model.IsActive,
                                CreatedDate = model.CreatedDate,
                            });
                        }
                    }
                    if (res != 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("AccessPlan");
                    }
                }
                Session["Error"] = "An Error has occured";
                CustomMethods.BindCurrencyForMembership(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                CustomMethods.BindCurrencyForMembership(model);
                return View(model);
                throw;
            }
        }

        public ActionResult ViewAccessPlan(int id = 0)
        {
            CustomMethods.ValidateRoles("AccessPlan");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            AccessPlanWebModel acplan = new AccessPlanWebModel();
            if (id != 0)
            {
                var objacplan = new AccessPlanBLL { }.GetAccessPlanById(id);
                if (objacplan != null)
                {
                    acplan.AccessPlanName = objacplan.AccessPlanName;
                    acplan.AccessPlanId = objacplan.AccessPlanId;
                    //acplan.AccessPlanId = objacplan.AccessPlanId == null ? 0 : Convert.ToInt32(objacplan.AccessPlanId);
                    acplan.Description = objacplan.Description;
                    acplan.MaxNoDeals = objacplan.MaxNoDeals;
                    acplan.ProdPerMonth = objacplan.ProdPerMonth;
                    acplan.UpdatesPerMonth = objacplan.UpdatesPerMonth;
                    acplan.AddPostCharge = objacplan.AddPostCharge;
                    acplan.NewsCharge = objacplan.NewsCharge;
                    acplan.AddUpdCharge = objacplan.AddUpdCharge;
                    acplan.MembershipFee = objacplan.MembershipFee;
                    acplan.Validity = objacplan.Validity;
                    acplan.AccessPlanImage = objacplan.AccessPlanImage;
                    acplan.CurrencyId = objacplan.CurrencyId;
                    acplan.CurrencyName = objacplan.CurrencyName;
                    acplan.IsActive = Convert.ToBoolean(objacplan.IsActive);
                }
            }
            CustomMethods.BindCurrencyForMembership(acplan);
            return View(acplan);
        }

        public JsonResult ChangeAccessPlanStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new AccessPlanBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }
        #endregion

        #region Membership
        public ActionResult Membership(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            MembershipWebModel model = new MembershipWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<MembershipWebModel> emailtemps = new List<MembershipWebModel>();
            CustomMethods.ValidateRoles("Membership");
            var Memlist = new MembershipBLL { }.GetAllMembership(skip, take);


            model.Membership = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new MembershipBLL { }.GetAllMembership(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.Memberships = sortedlist.Select(x => new MembershipWebModel
                {
                    BusinessName = x.BusinessName,
                    AccessPlanName = x.AccessPlanName,
                    MembershipID = x.MembershipID,
                    ActivatedOn = x.ActivatedOn,
                    ExpiresOn = x.ExpiresOn,
                    AccessPlanId = x.AccessPlanId,
                    UserId = x.UserId,
                    UpdatedDate = x.UpdatedDate,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (Memlist != null)
                {
                    double count = Convert.ToDouble(new MembershipBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.Memberships = Memlist.Select(x => new MembershipWebModel
                    {
                        BusinessName = x.BusinessName,
                        AccessPlanName = x.AccessPlanName,
                        MembershipID = x.MembershipID,
                        ActivatedOn = x.ActivatedOn,
                        ExpiresOn = x.ExpiresOn,
                        AccessPlanId = x.AccessPlanId,
                        UserId = x.UserId,
                        UpdatedDate = x.UpdatedDate,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult AddEditMembership(int id = 0)
        {
            CustomMethods.ValidateRoles("Membership");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            MembershipWebModel model = new MembershipWebModel();
            if (id != 0)
            {
                var objmodel = new MembershipBLL { }.GetMembershipById(id);
                if (objmodel != null)
                {
                    model.MembershipID = objmodel.MembershipID;
                    model.ActivatedOn = objmodel.ActivatedOn;
                    model.ExpiresOn = objmodel.ExpiresOn;
                    model.AccessPlanId = objmodel.AccessPlanId;
                    model.UserId = objmodel.UserId;
                    model.IsActive = Convert.ToBoolean(objmodel.IsActive);
                    model.AccessPlanName = objmodel.AccessPlanName;
                    model.AccessPlanId = objmodel.AccessPlanId;
                    model.BusinessName = objmodel.BusinessName;

                }
            }
            CustomMethods.BindAccessPlanForMembership(model);
            CustomMethods.BindBusinessUser(model);
            return View(model);
        }

        [HttpPost]
        public ActionResult AddEditMembership(MembershipWebModel model)
        {
            try
            {
                if (model.MembershipID == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }
                if (ModelState.IsValid)
                {
                    if (model.MembershipID == 0)
                    {

                        int res = new MembershipBLL { }.AddEditMembership(new MembershipModel
                        {
                            MembershipID = model.MembershipID,
                            ActivatedOn = model.ActivatedOn,
                            ExpiresOn = model.ExpiresOn,
                            AccessPlanId = model.AccessPlanId,
                            UserId = model.UserId,
                            IsActive = model.IsActive,
                            CreatedDate = model.CreatedDate,
                            //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                        });
                        if (res != 0)
                        {
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("Membership");
                        }


                    }

                    else
                    {
                        int result = new MembershipBLL { }.AddEditMembership(new MembershipModel
                        {
                            MembershipID = model.MembershipID,
                            ActivatedOn = model.ActivatedOn,
                            ExpiresOn = model.ExpiresOn,
                            AccessPlanId = model.AccessPlanId,
                            UserId = model.UserId,
                            IsActive = model.IsActive,
                            CreatedDate = model.CreatedDate,
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("Membership");
                        }
                    }
                }

                Session["Error"] = "An error has occured";
                CustomMethods.BindAccessPlanForMembership(model);
                CustomMethods.BindBusinessUser(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An error has occured";
                CustomMethods.BindAccessPlanForMembership(model);
                CustomMethods.BindBusinessUser(model);
                return RedirectToAction("Membership");
                throw;
            }
        }

        public ActionResult ViewMembership(int id = 0)
        {
            CustomMethods.ValidateRoles("Membership");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            MembershipWebModel model = new MembershipWebModel();
            if (id != 0)
            {
                var objmodel = new MembershipBLL { }.GetMembershipById(id);
                if (objmodel != null)
                {
                    model.AccessPlanName = objmodel.AccessPlanName;
                    model.BusinessName = objmodel.BusinessName;
                    model.MembershipID = objmodel.MembershipID;
                    model.ActivatedOn = objmodel.ActivatedOn;
                    model.ExpiresOn = objmodel.ExpiresOn;
                    model.AccessPlanId = objmodel.AccessPlanId;
                    model.UserId = objmodel.UserId;
                    model.IsActive = Convert.ToBoolean(objmodel.IsActive);
                }
            }
            CustomMethods.BindAccessPlanForMembership(model);
            CustomMethods.BindBusinessUser(model);
            return View(model);
        }

        public JsonResult ChangeMembershipStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new MembershipBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult SearchMember(string searchValue)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            SearchwordsModel searchObj = new SearcbyKeywordBLL { }.SearchByAccessPlan(searchValue);
            ViewBag.SearchKey = searchValue;
            return View(searchObj);
        }

        #endregion

        #region Roles

        public ActionResult Roles(int pid = 0)
        {
            int take = 10;
            int skip = take * pid;

            RoleWebModel model = new RoleWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<RoleWebModel> roles = new List<RoleWebModel>();
            CustomMethods.ValidateRoles("Role");
            var roleslist = new RolesBLL { }.GetAllRoles(skip, take);

            double count = Convert.ToDouble(new RolesBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);

            if (roleslist != null)
            {
                model.Roles = roleslist.Select(x => new RoleWebModel
                {
                    RoleID = x.RoleId,
                    RoleName = x.RoleName,
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            return View(model);
        }

        public ActionResult AddEditRoles(int id = 0)
        {
            try
            {
                CustomMethods.ValidateRoles("Role");
                if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                    return View("ErrorPage", "Error");
                RoleWebModel model = new RoleWebModel();

                string str = Server.MapPath("~/Areas/Admin/RoleManagement.xml");
                var xml = XDocument.Load(str);
                var xmlpages = xml.Root.Elements("Page").Select(x => new
                {
                    id = x.Attribute("Id").Value,
                    name = x.Attribute("Name").Value,

                }).ToList();
                model.Pagecount = xmlpages.Count;

                if (id != 0)
                {
                    var res = new RolesBLL { }.GetRoleById(id);
                    if (res != null)
                    {
                        model.RoleID = res.RoleId;
                        model.RoleName = res.RoleName;
                        model.IsActive = Convert.ToBoolean(res.IsActive);

                        model.Rolemanages = res.Rolemanages.Select(x => new RolemanagementWebModel
                        {
                            Add = Convert.ToBoolean(x.Add),
                            Edit = Convert.ToBoolean(x.Edit),
                            Delete = Convert.ToBoolean(x.Delete),
                            View = Convert.ToBoolean(x.View),
                            RoleID = Convert.ToInt32(x.RoleID),
                            PageID = Convert.ToInt32(x.PageID),
                            PageName = xmlpages.Where(z => Convert.ToInt32(z.id) == x.PageID).Select(z => z.name).SingleOrDefault()
                        }).ToList();
                        if (model.Rolemanages.Count == 0)
                        {
                            model.Rolemanages = xmlpages.Select(x => new RolemanagementWebModel
                            {
                                Add = false,
                                Delete = false,
                                View = false,
                                Edit = false,
                                PageID = Convert.ToInt32(x.id),
                                PageName = x.name
                            }).ToList();
                        }

                    }



                }
                else
                {
                    model.Rolemanages = xmlpages.Select(x => new RolemanagementWebModel
                    {
                        Add = false,
                        Delete = false,
                        View = false,
                        Edit = false,
                        PageID = Convert.ToInt32(x.id),
                        PageName = x.name
                    }).ToList();
                }



                return View(model);
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddEditRoles(RoleWebModel model, FormCollection Pages)
        {
            try
            {
                if (model.RoleID == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                RoleModel objrole = new RoleModel();

                for (int i = 1; i <= model.Pagecount; i++)
                {
                    RolemanagementModel obj = new RolemanagementModel();

                    var add = Pages["page_Add" + i];
                    var edit = Pages["page_edit" + i];
                    var delete = Pages["page_delete" + i];
                    var read = Pages["page_read" + i];

                    obj.Add = add == null ? false : true;
                    obj.Edit = edit == null ? false : true;
                    obj.Delete = delete == null ? false : true;
                    obj.View = read == null ? false : true;
                    obj.PageID = Convert.ToInt32(Pages["hd_page" + i]);
                    objrole.Rolemanages.Add(obj);
                }

                if (ModelState.IsValid)
                {
                    int res = new RolesBLL { }.AddEditRole(new RoleModel
                    {
                        RoleId = model.RoleID,
                        RoleName = model.RoleName,
                        IsActive = model.IsActive,
                        Rolemanages = objrole.Rolemanages
                    });
                    if (res != 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("Roles");
                    }

                    else
                    {
                        int result = new RolesBLL { }.AddEditRole(new RoleModel
                        {
                            RoleId = model.RoleID,
                            RoleName = model.RoleName,
                            IsActive = model.IsActive,
                            Rolemanages = objrole.Rolemanages
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("Roles");
                        }
                    }
                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult ViewRoles(int id = 0)
        {
            try
            {
                CustomMethods.ValidateRoles("Role");
                if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                    return View("ErrorPage", "Error");
                RoleWebModel model = new RoleWebModel();

                string str = Server.MapPath("~/Areas/Admin/RoleManagement.xml");
                var xml = XDocument.Load(str);
                var xmlpages = xml.Root.Elements("Page").Select(x => new
                {
                    id = x.Attribute("Id").Value,
                    name = x.Attribute("Name").Value,

                }).ToList();
                model.Pagecount = xmlpages.Count;

                if (id != 0)
                {
                    var res = new RolesBLL { }.GetRoleById(id);
                    if (res != null)
                    {
                        model.RoleID = res.RoleId;
                        model.RoleName = res.RoleName;
                        model.IsActive = Convert.ToBoolean(res.IsActive);

                        model.Rolemanages = res.Rolemanages.Select(x => new RolemanagementWebModel
                        {
                            Add = Convert.ToBoolean(x.Add),
                            Edit = Convert.ToBoolean(x.Edit),
                            Delete = Convert.ToBoolean(x.Delete),
                            View = Convert.ToBoolean(x.View),
                            RoleID = Convert.ToInt32(x.RoleID),
                            PageID = Convert.ToInt32(x.PageID),
                            PageName = xmlpages.Where(z => Convert.ToInt32(z.id) == x.PageID).Select(z => z.name).SingleOrDefault()
                        }).ToList();
                        if (model.Rolemanages.Count == 0)
                        {
                            model.Rolemanages = xmlpages.Select(x => new RolemanagementWebModel
                            {
                                Add = false,
                                Delete = false,
                                View = false,
                                Edit = false,
                                PageID = Convert.ToInt32(x.id),
                                PageName = x.name
                            }).ToList();
                        }

                    }



                }
                else
                {
                    model.Rolemanages = xmlpages.Select(x => new RolemanagementWebModel
                    {
                        Add = false,
                        Delete = false,
                        View = false,
                        Edit = false,
                        PageID = Convert.ToInt32(x.id),
                        PageName = x.name
                    }).ToList();
                }



                return View(model);
            }
            catch (Exception)
            {
                return View();
                throw;
            }
        }

        public JsonResult ChangeRolesStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new RolesBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        #endregion

        #region Administrator
        AdministratorBLL objadministratorbll = new AdministratorBLL();
        public ActionResult AllAdministrators(int pid = 0)
        {

            int take = 10;
            int skip = take * pid;

            AdministratorWebModel model = new AdministratorWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<AdministratorWebModel> administrators = new List<AdministratorWebModel>();
            CustomMethods.RoleManagement(CustomMethods.GetPageIDByPageName("Instructor"));
            var resultlist = objadministratorbll.GetAllAdministrators(skip, take);
            double count = Convert.ToDouble(new AdministratorBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (resultlist != null)
            {
                model.Administrator = resultlist.Select(x => new AdministratorWebModel
                {
                    AdminId = x.AdminId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Email = x.Email,
                    Password = x.Password,
                    Role = x.Role,
                    UserName = x.UserName,
                    IsActive = Convert.ToBoolean(x.IsActive),
                }).ToList();
            }

            return View(model);
        }

        public ActionResult AddEditAdministrator(int id = 0)
        {
            CustomMethods.ValidateRoles("Administrator");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            AdministratorWebModel model = new AdministratorWebModel();
            if (id != 0)
            {
                var objmodel = objadministratorbll.GetAdministratorById(id);
                if (objmodel != null)
                {
                    model.AdminId = objmodel.AdminId;
                    model.FirstName = objmodel.FirstName;
                    model.LastName = objmodel.LastName;
                    model.UserName = objmodel.UserName;
                    model.Password = DataEncryption.Encrypt(objmodel.Password.Trim(), "passKey");
                    model.Email = objmodel.Email;
                    model.RoleID = objmodel.RoleId;
                    //model.SubjectID = Convert.ToInt32(objmodel.SubjectID);
                    model.IsActive = Convert.ToBoolean(objmodel.IsActive);
                }
            }
            CustomMethods.BindRoles(model);
            //CustomMethods.BindMembershipsForInstructor(model);
            //CustomMethods.BindSubjects(model);
            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddEditAdministrator(AdministratorWebModel model, HttpPostedFileBase file)
        {
            try
            {
                int res = 0;
                //string strFileName = "";
                //string path = "";
                Random rnd = new Random();
                if (model.AdminId != 0)
                {
                    // code to remove password validation error in edit mode
                    ModelState.Values.ToList()[11].Errors.RemoveAt(0);
                    ModelState.Values.ToList()[12].Errors.RemoveAt(0);

                    res = objadministratorbll.AddUpdateAdministrator(new AdministratorModel
                    {
                        AdminId = model.AdminId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email,
                        RoleId = model.RoleID,
                        IsActive = model.IsActive,
                        Password = DataEncryption.Encrypt(model.Password.Trim(), "passKey"),
                        //SubjectID = model.SubjectID,
                    });
                    if (res > 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("AllAdministrators");
                    }
                }

                if (ModelState.IsValid)
                {
                    res = objadministratorbll.AddUpdateAdministrator(new AdministratorModel
                    {
                        AdminId = model.AdminId,
                        FirstName = model.FirstName,
                        LastName = model.LastName,
                        UserName = model.UserName,
                        Email = model.Email,
                        RoleId = model.RoleID,
                        IsActive = model.IsActive,
                        Password = DataEncryption.Encrypt(model.Password.Trim(), "passKey"),
                        //SubjectID = model.SubjectID,
                    });

                    if (res > 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("AllAdministrators");
                    }
                }
                Session["Error"] = "An error has occured";
                CustomMethods.BindRoles(model);
                //CustomMethods.BindMembershipsForInstructor(model);
                //CustomMethods.BindSubjects(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An error has occured";
                CustomMethods.BindRoles(model);
                //CustomMethods.BindMembershipsForInstructor(model);
                //CustomMethods.BindSubjects(model);
                return RedirectToAction("AllAdministrators");
                throw;
            }
        }

        public JsonResult InstructorStatus(int id)
        {
            bool Result = false;
            bool changestatus = new AdministratorBLL { }.ChangeStatus(id);
            if (changestatus)
            {
                Result = true;

            }
            return Json(Result);
        }

        public ActionResult ViewAdministrator(int id = 0)
        {
            AdministratorWebModel model = new AdministratorWebModel();
            if (id != 0)
            {
                var objmodel = objadministratorbll.GetAdministratorById(id);
                if (objmodel != null)
                {
                    model.Role = objmodel.Role;
                    model.AdminId = objmodel.AdminId;
                    model.FirstName = objmodel.LastName;
                    model.LastName = objmodel.LastName;
                    model.UserName = objmodel.UserName;
                    model.Email = objmodel.Email;
                    model.RoleID = objmodel.RoleId;
                    model.IsActive = Convert.ToBoolean(objmodel.IsActive);
                }
            }
            CustomMethods.BindRoles(model);
            //CustomMethods.BindMembershipsForInstructor(model);
            //CustomMethods.BindSubjects(model);
            return View(model);
        }

        public JsonResult ChangeAdminStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new AdministratorBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        #endregion

        #region CMS
        public ActionResult CMS(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            CmsWebModel model = new CmsWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<CmsWebModel> cms = new List<CmsWebModel>();
            CustomMethods.ValidateRoles("CMS");
            var cmslist = new CMSBLL { }.GetAllCms();
            if (cid != null)
            {
                var sortedlist = new CMSBLL { }.GetAllCms(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.CMS = cmslist.Select(x => new CmsWebModel
                {
                    CMSId = x.CMSId,
                    CMSTitle = x.CMSTitle,
                    Description = x.Description,
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            return View(model);
        }

        public ActionResult AddEditCms(int id = 0)
        {
            CustomMethods.ValidateRoles("CMS");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CmsWebModel cms = new CmsWebModel();
            if (id != 0)
            {
                CMSModel cmsmodel = new CMSBLL { }.GetCmsById(id);
                if (cmsmodel != null)
                {
                    cms.CMSId = cmsmodel.CMSId;
                    cms.CMSTitle = cmsmodel.CMSTitle;
                    cms.Description = cmsmodel.Description;
                    cms.IsActive = Convert.ToBoolean(cmsmodel.IsActive);
                }
            }
            return View(cms);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddEditCms(CmsWebModel model)
        {
            try
            {
                if (model.CMSId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }
                if (ModelState.IsValid)
                {
                    if (model.CMSId == 0)
                    {
                        var checkDuplicate = objdb.CMS.Where(x => x.CMSTitle == model.CMSTitle).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new CMSBLL { }.AddEditCms(new CMSModel
                            {
                                CMSId = model.CMSId,
                                CMSTitle = model.CMSTitle,
                                Description = model.Description,
                                IsActive = model.IsActive,
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("CMS");
                            }
                        }
                        Session["Error"] = "Record Already Exists";
                        return RedirectToAction("CMS");
                    }
                    else
                    {
                        int result = new CMSBLL { }.AddEditCms(new CMSModel
                        {
                            CMSId = model.CMSId,
                            CMSTitle = model.CMSTitle,
                            Description = model.Description,
                            IsActive = model.IsActive,
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("CMS");
                        }
                    }


                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult ViewCms(int id = 0)
        {
            CustomMethods.ValidateRoles("CMS");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CmsWebModel cms = new CmsWebModel();
            if (id != 0)
            {
                CMSModel cmsmodel = new CMSBLL { }.GetCmsById(id);
                if (cmsmodel != null)
                {
                    cms.CMSId = cmsmodel.CMSId;
                    cms.CMSTitle = cmsmodel.CMSTitle;
                    cms.Description = cmsmodel.Description;
                    cms.IsActive = Convert.ToBoolean(cmsmodel.IsActive);
                }
            }
            return View(cms);
        }

        public JsonResult ChangeStatuscms(int id)
        {
            bool Result = false;
            bool ChangeStatus = new CMSBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        #endregion

        #region News

        public ActionResult News(int pid = 0)
        {
            int take = 10;
            int skip = take * pid;
            NewsWebModel model = new NewsWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<NewsWebModel> news = new List<NewsWebModel>();
            CustomMethods.ValidateRoles("News");
            var newsList = new NewsBLL { }.GetAllNews(skip, take);
            double count = Convert.ToDouble(new NewsBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (newsList != null)
            {
                model.News = newsList.Select(x => new NewsWebModel
                {
                    NewsId = x.NewsId,
                    NewsTitle = x.NewsTitle,
                    NewsDescription = x.NewsDesc,
                    //NewsImage=x.NewsImage,
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            return View(model);
        }

        public ActionResult AddEditNews(int id = 0)
        {
            CustomMethods.ValidateRoles("News");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            NewsWebModel news = new NewsWebModel();
            if (id != 0)
            {
                NewsModel newsmodel = new NewsBLL { }.GetNewsById(id);
                if (newsmodel != null)
                {
                    news.NewsId = newsmodel.NewsId;
                    news.NewsTitle = newsmodel.NewsTitle;
                    news.NewsDescription = newsmodel.NewsDesc;
                    news.NewsImage = newsmodel.NewsImage;
                    news.IsActive = Convert.ToBoolean(newsmodel.IsActive);
                }
            }
            return View(news);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddEditNews(NewsWebModel model, HttpPostedFileBase file)
        {
            try
            {
                if (model.NewsId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }
                string strFileName = "";
                string path = "";
                Random rnd = new Random();
                int res = 0;
                if (ModelState.IsValid)
                {
                    if (model.NewsId == 0)
                    {
                        var checkDuplicate = objdb.News.Where(x => x.NewsTitle == model.NewsTitle).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            if (file!=null)
                            {
                                strFileName = "HomePageNewsImage_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                                path = Server.MapPath("~/Areas/Admin/ProjectImages/HomepageNews/" + strFileName);
                                res = new NewsBLL { }.AddEditNews(new NewsModel
                                {
                                    NewsId = model.NewsId,
                                    NewsTitle = model.NewsTitle,
                                    NewsDesc = model.NewsDescription,
                                    IsActive = model.IsActive,
                                    NewsImage =file==null? model.NewsImage:strFileName,
                                });
                                file.SaveAs(path);
                                if (res != 0)
                                {
                                    Session["Success"] = "Successfully Added The Record";
                                    return RedirectToAction("News");
                                }
                            }
                            else
                            {
                                res = new NewsBLL { }.AddEditNews(new NewsModel
                                {
                                    NewsId = model.NewsId,
                                    NewsTitle = model.NewsTitle,
                                    NewsDesc = model.NewsDescription,
                                    IsActive = model.IsActive,
                                    NewsImage = file == null ? model.NewsImage : strFileName,
                                });
                                if (res != 0)
                                {
                                    Session["Success"] = "Successfully Added The Record";
                                    return RedirectToAction("News");
                                }
                            }
                        }
                    }
                    else
                    {
                        if (file!=null)
                        {
                            strFileName = "HomePageNewsImage_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                            path = Server.MapPath("~/Areas/Admin/ProjectImages/HomepageNews/" + strFileName);
                            int result = new NewsBLL { }.AddEditNews(new NewsModel
                            {
                                NewsId = model.NewsId,
                                NewsTitle = model.NewsTitle,
                                NewsDesc = model.NewsDescription,
                                IsActive = model.IsActive,
                                NewsImage = file == null ? model.NewsImage : strFileName,
                            });
                            file.SaveAs(path);
                            if (result != 0)
                            {
                                Session["Success"] = "Successfully Updated The Record";
                                return RedirectToAction("News");
                            }
                        }
                        else
                        {
                            res = new NewsBLL { }.AddEditNews(new NewsModel
                            {
                                NewsId = model.NewsId,
                                NewsTitle = model.NewsTitle,
                                NewsDesc = model.NewsDescription,
                                IsActive = model.IsActive,
                                NewsImage = model.NewsImage
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("News");
                            }
                        }
                        
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("News");
                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult ViewNews(int id = 0)
        {
            CustomMethods.ValidateRoles("CMS");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            NewsWebModel news = new NewsWebModel();
            if (id != 0)
            {
                NewsModel newsmodel = new NewsBLL { }.GetNewsById(id);
                if (newsmodel != null)
                {
                    news.NewsId = newsmodel.NewsId;
                    news.NewsTitle = newsmodel.NewsTitle;
                    news.NewsDescription = newsmodel.NewsDesc;
                    news.NewsImage = newsmodel.NewsImage;
                    news.IsActive = Convert.ToBoolean(newsmodel.IsActive);
                }
            }
            return View(news);
        }

        public JsonResult ChangeNewsStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new NewsBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        #endregion

        #region NewsLetters
        string guid = Guid.NewGuid().ToString();
        public ActionResult NewsLetter(int pid = 0)
        {
            int take = 10;
            int skip = take * pid;
            NewsLetterWebModel model = new NewsLetterWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<NewsLetterWebModel> newsletter = new List<NewsLetterWebModel>();
            CustomMethods.ValidateRoles("NewsLetter");
            var newsletterList = new NewsLetterBLL { }.GetAllNewsLetterDetails(skip, take);
            double count = Convert.ToDouble(new NewsLetterBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (newsletterList != null)
            {
                model.NewsLetter = newsletterList.Select(x => new NewsLetterWebModel
                {
                    NewsLetterID = x.NewsLetterID,
                    Title = x.Title,
                    Description = x.Description,
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }

            return View(model);
        }

        public Task CallingAsynNewsLetters(string emailTo)
        {
            return Task.Run(() => SendAsyncEmails(emailTo));
        }

        public void SendAsyncEmails(string emailTo)
        {
            string emailId = string.Empty;
            MailMessage msg;
            string encrpEmail = string.Empty;
            string ActivationUrl = string.Empty;

            #region Send Email
            msg = new MailMessage();
            SmtpClient smtp = new SmtpClient();
            emailId = emailTo;
            //string password = (from BusinessUser in objdb.BussinessUsers where BusinessUser.EmailId == emailId select BusinessUser.Password).FirstOrDefault();
            //uPwd = DataEncryption.Decrypt(password, "passKey");
            //Password = DataEncryption.Encrypt(model.Password.Trim(), "passKey"),
            encrpEmail = DataEncryption.Encrypt(emailId.Trim(), "passkey");
            //sender email address
            msg.From = new MailAddress("businessbranding.in@gmail.com");
            //Receiver email address
            msg.To.Add(emailId);
            msg.Subject = "Confirmation email for account activation";
            encrpEmail = Server.HtmlEncode(" http://www.businessbranding.in/Admin/MasterData/Unsubscribe?id=" + encrpEmail);//@ production time change the current code to  www.currnetDomainname.com/Request.Url.AbsolutePath
            msg.Body = "Hi " + emailId + "!\n" +
             "Thanks for showing interest and registering in <a href='http://businessbranding.in/'> businessbranding.in <a> " +
             " Please <a href='" + ActivationUrl + "'>click here to activate</a>  your account and enjoy our services. \nThanks! <br>your emailid: " + emailId + "<br><a href='" + encrpEmail + "'>click here to Unsubscribe</a>";
            msg.IsBodyHtml = true;
            smtp.Credentials = new NetworkCredential("businessbranding.in@gmail.com", "amirpasha");
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.Send(msg);
            #endregion
        }

        public async Task<ActionResult> SendNewsLetter()
        {
            int counter = 0;
            List<SubscribeModel> subslist = new List<SubscribeModel>();
            List<SubscribeModel> usersendinglist = new List<SubscribeModel>();
            var subsuser = (from users in objdb.Subscribes
                            where users.IsActive == true
                            select new SubscribeModel
                            {
                                SubscribeId = users.SubscribeId,
                                EmailId = users.EmailId
                            }).ToList();

            foreach (SubscribeModel item in subsuser)
            {
                await CallingAsynNewsLetters(item.EmailId);
                counter++;
            }
            if (counter > 0)
            {
                Session["Success"] = "Successfully NewsLetters Send to subscribed users";
                //return Json(new
                //{
                //    redirectUrl=Url.Action("NewsLetter","MasterData"),
                //    isredirect=true
                //});
                //return Json(counter);
                return RedirectToAction("NewsLetter");
            }
            //return Json(counter);
            return RedirectToAction("NewsLetter");


        }

        public ActionResult Unsubscribe(string id)
        {
            try
            {
                if (id != null)
                {
                    //userid = Convert.ToInt32(Request.QueryString["UserID"]);
                    Guid temp = Guid.Parse(guid);
                    string dcrptemail = string.Empty;
                    dcrptemail = DataEncryption.Decrypt(id, "passKey");
                    var update = objdb.Subscribes.Where(x => x.EmailId == dcrptemail).SingleOrDefault();

                    if (update != null)
                    {
                        update.IsActive = false;
                        objdb.SaveChanges();
                        return RedirectToAction("Login", "Home");
                    }

                    return RedirectToAction("Login", "Home");
                }
            }
            catch (Exception)
            {
                // ScriptManager.RegisterStartupScript(this, this.GetType(), "Message", "alert('Error occured : " + ex.Message.ToString() + "');", true);
            }
            return View();
        }

        public JsonResult ChangeNewsLetterStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new NewsLetterBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult AddEditNewsLetter(int id = 0)
        {
            CustomMethods.ValidateRoles("NewsLetter");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            NewsLetterWebModel newsletter = new NewsLetterWebModel();
            if (id != 0)
            {
                NewsLetterModel newslettermodel = new NewsLetterBLL { }.GetNewsLetterById(id);
                if (newslettermodel != null)
                {
                    newsletter.NewsLetterID = newslettermodel.NewsLetterID;
                    newsletter.Title = newslettermodel.Title;
                    newsletter.Description = newslettermodel.Description;
                    newsletter.IsActive = Convert.ToBoolean(newslettermodel.IsActive);
                }
            }
            return View(newsletter);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddEditNewsLetter(NewsLetterWebModel model)
        {
            try
            {
                if (model.NewsLetterID == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }
                if (ModelState.IsValid)
                {
                    if (model.NewsLetterID == 0)
                    {
                        var checkDuplicate = objdb.NewsLetters.Where(x => x.Title == model.Title).FirstOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new NewsLetterBLL { }.AddEditNewsLetter(new NewsLetterModel
                            {
                                NewsLetterID = model.NewsLetterID,
                                Title = model.Title,
                                Description = model.Description,
                                IsActive = model.IsActive,
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("NewsLetter");
                            }


                        }
                        Session["Error"] = "Record Already Exists";
                        return RedirectToAction("NewsLetter");
                    }

                    else
                    {
                        int result = new NewsLetterBLL { }.AddEditNewsLetter(new NewsLetterModel
                        {
                            NewsLetterID = model.NewsLetterID,
                            Title = model.Title,
                            Description = model.Description,
                            IsActive = model.IsActive,
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("NewsLetter");
                        }
                    }

                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }


        #endregion

        #region BusinessUserNews

        public ActionResult BusinessUserNews(int pid = 0)
        {
            int take = 10;
            int skip = take * pid;
            BusinessNewsModel model = new BusinessNewsModel();
            model.PageID = pid;
            model.Current = pid + 1;
            //IEnumerable<NewsWebModel> news = new List<NewsWebModel>();
            CustomMethods.ValidateRoles("BusinessUserNews");
            var businessusernewsList = new BusinessNewsBLL { }.GetAllBusinessNews(skip, take);
            double count = Convert.ToDouble(new BusinessNewsBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (businessusernewsList != null)
            {
                model.BusinessNewsList = businessusernewsList.Select(x => new BusinessNewsModel
                {
                    BusinessNewsId = x.BusinessNewsId,
                    NewsTitle = x.NewsTitle,
                    Description = x.Description,
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            return View(model);
        }

        public ActionResult AddEditBusinessUserNews(int id = 0)
        {
            CustomMethods.ValidateRoles("BusinessUserNews");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            BusinessNewsModel bizznews = new BusinessNewsModel();
            if (id != 0)
            {
                BusinessNewsModel businessnewsmodel = new BusinessNewsBLL { }.GetBusinessNewsById(id);
                if (businessnewsmodel != null)
                {
                    bizznews.BusinessNewsId = businessnewsmodel.BusinessNewsId;
                    bizznews.NewsTitle = businessnewsmodel.NewsTitle;
                    bizznews.Description = businessnewsmodel.Description;
                    bizznews.IsActive = Convert.ToBoolean(businessnewsmodel.IsActive);
                }
            }
            return View(bizznews);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddEditBusinessUserNews(BusinessNewsModel model)
        {
            try
            {
                if (model.BusinessNewsId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }
                if (ModelState.IsValid)
                {
                    if (model.BusinessNewsId == 0)
                    {
                        var checkDuplicate = objdb.BusinessNews.Where(x => x.NewsTitle == model.NewsTitle).SingleOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new BusinessNewsBLL { }.AddEditBusinessNews(new BusinessNewsModel
                            {
                                BusinessNewsId = model.BusinessNewsId,
                                NewsTitle = model.NewsTitle,
                                Description = model.Description,
                                IsActive = model.IsActive,
                            });
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("BusinessUserNews");
                            }

                        }
                    }
                    else
                    {
                        int result = new BusinessNewsBLL { }.AddEditBusinessNews(new BusinessNewsModel
                        {
                            BusinessNewsId = model.BusinessNewsId,
                            NewsTitle = model.NewsTitle,
                            Description = model.Description,
                            IsActive = model.IsActive,
                        });
                        if (result != 0)
                        {
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("BusinessUserNews");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("BusinessUserNews");
                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult ViewBusinessUserNews(int id = 0)
        {
            CustomMethods.ValidateRoles("BusinessUserNews");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            BusinessNewsModel businessnews = new BusinessNewsModel();
            if (id != 0)
            {
                BusinessNewsModel bizznewsmodel = new BusinessNewsBLL { }.GetBusinessNewsById(id);
                if (bizznewsmodel != null)
                {
                    businessnews.BusinessNewsId = bizznewsmodel.BusinessNewsId;
                    businessnews.NewsTitle = bizznewsmodel.NewsTitle;
                    businessnews.Description = bizznewsmodel.Description;
                    businessnews.IsActive = Convert.ToBoolean(bizznewsmodel.IsActive);
                }
            }
            return View(businessnews);
        }

        public JsonResult ChangeBusinessNewsStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new BusinessNewsBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        #endregion

        #region PaymentTransaction

        public ActionResult PaymentTransactionList(int pid = 0)
        {
            int take = 10;
            int skip = take * pid;
            PaymentTransactionModel model = new PaymentTransactionModel();
            model.PageID = pid;
            model.Current = pid + 1;
            CustomMethods.ValidateRoles("PaymentTransactionDetails");
            var Transaclist = new PaymentTransactionBLL { }.TransactionDetails();
            var Transactionlist = new PaymentTransactionBLL { }.GetAllTransactionDetails(skip, take);
            double count = Convert.ToDouble(new PaymentTransactionBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (Transactionlist != null)
            {
                model.PaymentList = Transactionlist.Select(x => new PaymentTransactionModel
                {
                    TransationId = x.TransationId,
                    Amount = x.Amount,
                    PaymentReturnId = x.PaymentReturnId,
                    TransationStatus = x.TransationStatus,
                    MessageFromGateway = x.MessageFromGateway,
                    CreatedDate = x.CreatedDate
                }).ToList();
            }
            return View(model);
        }
        #endregion

        #region CoBrandedUsers
        public ActionResult CoBranded(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            CoBrandModel model = new CoBrandModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<CoBrandModel> Cobrand = new List<CoBrandModel>();
            CustomMethods.ValidateRoles("CoBranded");
            //var Memlist = new CoBrandBLL { }.GetAllCoBrandUsers(skip, take);
            var UserList = new CoBrandBLL { }.GetAllCoBrandUsers(skip, take);


            // model.CoBrandList = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new CoBrandBLL { }.GetAllCoBrandUsers(skip, take);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.CoBrandList = sortedlist.Select(x => new CoBrandModel
                {
                    BusinessNameA = x.BusinessNameA,
                    PartnerA = x.PartnerA,
                    CompanyLogo = x.CompanyLogo,
                    PartnerB = x.PartnerB,
                    IsApproved = x.IsApproved,
                    IsActive = x.IsActive,
                    CoBrandId = x.CoBrandId,
                    CoBrandedName = x.CoBrandedName,
                    ActivatedOn = x.ActivatedOn,
                    ExpiresOn = Convert.ToDateTime(x.No_Month),
                    No_Month = x.No_Month,
                    // UpdatedDate = x.UpdatedDate,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    CreatedOn = Convert.ToDateTime(x.CreatedOn),
                    //IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (UserList != null)
                {
                    double count = Convert.ToDouble(new CoBrandBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.CoBrandList = UserList.Select(x => new CoBrandModel
                    {
                        BusinessNameA = x.BusinessNameA,
                        BusinessNameB = x.BusinessNameB,
                        PartnerA = x.PartnerA,
                        PartnerB = x.PartnerB,
                        IsApproved = x.IsApproved,
                        IsActive = x.IsActive,
                        CoBrandId = x.CoBrandId,
                        CoBrandedName = x.CoBrandedName,
                        ActivatedOn = x.ActivatedOn,
                        ExpiresOn = x.ExpiresOn,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        CreatedOn = Convert.ToDateTime(x.CreatedOn),
                        //IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult AddEditCoBrand(int id = 0)
        {
            CustomMethods.ValidateRoles("CoBranded");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CoBrandModel CoBModel = new CoBrandModel();
            if (id != 0)
            {
                var objcob = new CoBrandBLL { }.GetCoBrandUsersById(id);
                if (objcob != null)
                {
                    CoBModel.CoBrandId = objcob.CoBrandId;
                    CoBModel.PartnerA = objcob.PartnerA;
                    CoBModel.PartnerB = objcob.PartnerB;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    CoBModel.IsApproved = objcob.IsApproved;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    CoBModel.IsActive = Convert.ToBoolean(objcob.IsActive);
                    CoBModel.CoBrandedName = objcob.CoBrandedName;
                    CoBModel.ActivatedOn = objcob.ActivatedOn;
                    CoBModel.ExpiresOn = objcob.ExpiresOn;
                    CoBModel.CoProductImages = objcob.CoProductImages;
                    CoBModel.ProductDetails = objcob.ProductDetails;
                    CoBModel.No_Month = objcob.No_Month;

                }
            }
            CustomMethods.BindPartnerA(CoBModel);
            CustomMethods.BindPartnerB(CoBModel);
            CustomMethods.BindCoBrandedUsers(CoBModel);
            return View(CoBModel);

        }

        [HttpPost]
        public ActionResult AddEditCoBrand(CoBrandModel model, IEnumerable<HttpPostedFileBase> file)
        {
            Random rnd = new Random();
            string strImgName = "";
            string path = "";
            try
            {
                if (model.CoBrandId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.CoBrandId == 0)
                    {
                        #region newrecord cannot be created
                        //var checkduplicate = objdb.CoBrandings.Where(x => x.PartnerA == model.PartnerA || x.PartnerA == model.PartnerB).SingleOrDefault();
                        //if (checkduplicate == null)
                        //{
                        //    var checkduplicatein = objdb.CoBrandings.Where(x => x.PartnerB == model.PartnerA || x.PartnerB == model.PartnerB).SingleOrDefault();
                        //    if (checkduplicatein == null)
                        //    {
                        //        if (file != null)
                        //        {

                        //            int res = new CoBrandBLL { }.AddEditCoBrandUsers(new CoBrandModel
                        //            {
                        //                CoBrandId = model.CoBrandId,
                        //                PartnerA = model.PartnerA,
                        //                PartnerB = model.PartnerB,
                        //                IsApproved = model.IsApproved,
                        //                IsActive = model.IsActive,
                        //                ProductDetails = model.ProductDetails,
                        //                CoBrandedName = model.CoBrandedName,
                        //                ActivatedOn = DateTime.Now,
                        //                ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(model.No_Month)),
                        //                No_Month = model.No_Month,
                        //                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                        //            });
                        //            if (res != 0)
                        //            {
                        //                foreach (var item in file)
                        //                {
                        //                    if (item != null)
                        //                    {
                        //                        string sname = item.FileName;
                        //                        strImgName = "Co-BrandProImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                        //                        path = Server.MapPath("~/Images/Co-BrandProductImages/" + strImgName);
                        //                        model.CoProductImages = strImgName;
                        //                        item.SaveAs(path);
                        //                    }
                        //                }
                        //            }
                        //            Session["Success"] = "Successfully Added The Record";
                        //            return RedirectToAction("CoBranded");

                        //        }

                        //    }
                        //}
                        #endregion
                    }

                    else
                    {
                        int result = new CoBrandBLL { }.AddEditCoBrandUsers(new CoBrandModel
                        {
                            CoBrandId = model.CoBrandId,
                            PartnerA = model.PartnerA,
                            PartnerB = model.PartnerB,
                            IsApproved = model.IsApproved,
                            IsActive = model.IsActive,
                            ProductDetails = model.ProductDetails,
                            CoBrandedName = model.CoBrandedName,
                            ActivatedOn = DateTime.Now,
                            ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(model.No_Month)),
                            No_Month = model.No_Month
                        });
                        if (result != 0)
                        {
                            foreach (var item in file)
                            {
                                if (item != null)
                                {
                                    CoBrandingProImgModel objCoBraProductImage = new CoBrandingProImgModel();
                                    string sname = item.FileName;
                                    strImgName = "Co-BrandProImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                                    path = Server.MapPath("~/Images/Co-BrandProductImages/" + strImgName);
                                    objCoBraProductImage.CoBrandProdImage = strImgName;
                                    objCoBraProductImage.CreatedOn = DateTime.Now;
                                    objCoBraProductImage.IsActive = true;
                                    objCoBraProductImage.CoBrandingId = model.CoBrandId;
                                    int res = new CoBrandingProImgBLL { }.AddEditCoBrandProductImages(objCoBraProductImage);
                                    item.SaveAs(path);
                                }
                            }
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("CoBranded");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("CoBranded");
                }
                Session["Error"] = "An Error has occured";
                CustomMethods.BindPartnerA(model);
                CustomMethods.BindPartnerB(model);

                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                CustomMethods.BindPartnerA(model);
                CustomMethods.BindPartnerB(model);

                return View(model);
                throw;
            }
        }

        public ActionResult ViewCoBrand(int id)
        {
            CustomMethods.ValidateRoles("CoBranded");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CoBrandModel cob = new CoBrandModel();
            if (id != 0)
            {
                var objcob = new CoBrandBLL { }.GetCoBrandUsersById(id);
                if (objcob != null)
                {
                    cob.PartnerA = objcob.PartnerA;
                    cob.PartnerB = objcob.PartnerB;
                    cob.CoBrandId = objcob.CoBrandId;  //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    cob.IsApproved = objcob.IsApproved;
                    cob.BusinessNameA = objcob.BusinessNameA;
                    cob.BusinessNameB = objcob.BusinessNameB;
                    cob.CoBrandedName = objcob.CoBrandedName;
                    cob.ProductDetails = objcob.ProductDetails;
                    cob.IsActive = Convert.ToBoolean(objcob.IsActive);
                    cob.No_Month = objcob.No_Month;
                    cob.ExpiresOn = objcob.ExpiresOn;
                    cob.ActivatedOn = objcob.ActivatedOn;
                    cob.Phone = objcob.Phone;
                    cob.Address = objcob.Address;
                    cob.TradeEmailId = objcob.TradeEmailId;
                    cob.EmailId = objcob.EmailId;
                    cob.IsApproved = Convert.ToBoolean(objcob.IsApproved);

                    var objCoBraProd = new CoBrandingProImgBLL { }.GetCoBrandProductImageById(id);
                    double count = Convert.ToDouble(objCoBraProd.Count);
                    cob.CoBrandProductImages = objCoBraProd.Select(x => new CoBrandingProImgModel
                    {
                        CoBrandingId = x.CoBrandingId,
                        CoBrandProdImage = x.CoBrandProdImage,
                        Id = x.Id,
                        CreatedOn = Convert.ToDateTime(x.CreatedOn),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(cob);
        }

        public JsonResult ChangeCoBrandStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new CoBrandBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public JsonResult ChangeCoBrandApprovalStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new CoBrandBLL { }.ChangeApprovalStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult RemoveCoBrand(int id)
        {
            var User = new CoBrandBLL { }.Remove(id);
            if (User != 0)
            {
                return RedirectToAction("CoBranded");
            }
            return RedirectToAction("CoBranded");
        }

        #endregion

        #region EmpBranding

        public ActionResult EmpBranding(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            EmpBrandingModel model = new EmpBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<EmpBrandingModel> Cobrand = new List<EmpBrandingModel>();
            CustomMethods.ValidateRoles("EmpBranding");
            //var Memlist = new CoBrandBLL { }.GetAllCoBrandUsers(skip, take);
            var UserList = new EmpBrandingBLL { }.GetAllEmployeeBranding(skip, take);


            // model.CoBrandList = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new EmpBrandingBLL { }.GetAllEmployeeBranding(skip, take);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.EmpBrandingList = sortedlist.Select(x => new EmpBrandingModel
                {
                    EmployerName = x.EmployerName,
                    EmpName = x.EmpName,
                    Message = x.Message,
                    EmpBrandingId = x.EmpBrandingId,
                    EmployerId = x.EmployerId,
                    CreateOn = x.CreateOn,
                    IsActive = x.IsActive,
                    CreatedBy = x.CreatedBy,
                    Description = x.Description,
                    //IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (UserList != null)
                {
                    double count = Convert.ToDouble(new CoBrandBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.EmpBrandingList = UserList.Select(x => new EmpBrandingModel
                    {
                        EmployerName = x.EmployerName,
                        EmpName = x.EmpName,
                        Message = x.Message,
                        EmpBrandingId = x.EmpBrandingId,
                        EmployerId = x.EmployerId,
                        CreateOn = x.CreateOn,
                        IsActive = x.IsActive,
                        CreatedBy = x.CreatedBy,
                        Description = x.Description
                        //IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult AddEditEmpBranding(int id = 0)
        {
            CustomMethods.ValidateRoles("EmpBranding");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            EmpBrandingModel CoBModel = new EmpBrandingModel();
            if (id != 0)
            {
                var objcob = new EmpBrandingBLL { }.GetEmpBrandingById(id);
                if (objcob != null)
                {
                    CoBModel.EmpBrandingId = objcob.EmpBrandingId;
                    CoBModel.EmployerName = objcob.EmployerName;
                    CoBModel.EmpName = objcob.EmpName;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    CoBModel.EmployerId = objcob.EmployerId;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    CoBModel.IsActive = Convert.ToBoolean(objcob.IsActive);
                    CoBModel.EmpImage = objcob.EmpImage;
                    CoBModel.CreateOn = objcob.CreateOn;
                    CoBModel.CreatedBy = objcob.CreatedBy;
                    CoBModel.UpdateBy = objcob.UpdateBy;
                    CoBModel.UpdatedOn = objcob.UpdatedOn;
                    CoBModel.Message = objcob.Message;

                }
            }
            //CustomMethods.BindPartnerA(CoBModel);
            //CustomMethods.BindPartnerB(CoBModel);
            //CustomMethods.BindCoBrandedUsers(CoBModel);
            return View(CoBModel);

        }

        [HttpPost]
        public ActionResult AddEditEmpBranding(EmpBrandingModel model, IEnumerable<HttpPostedFileBase> file)
        {
            Random rnd = new Random();
            string strImgName = "";
            string path = "";
            try
            {
                if (model.EmpBrandingId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.EmpBrandingId == 0)
                    {
                        #region newrecord cannot be created
                        //var checkduplicate = objdb.CoBrandings.Where(x => x.PartnerA == model.PartnerA || x.PartnerA == model.PartnerB).SingleOrDefault();
                        //if (checkduplicate == null)
                        //{
                        //    var checkduplicatein = objdb.CoBrandings.Where(x => x.PartnerB == model.PartnerA || x.PartnerB == model.PartnerB).SingleOrDefault();
                        //    if (checkduplicatein == null)
                        //    {
                        //        if (file != null)
                        //        {

                        //            int res = new CoBrandBLL { }.AddEditCoBrandUsers(new CoBrandModel
                        //            {
                        //                CoBrandId = model.CoBrandId,
                        //                PartnerA = model.PartnerA,
                        //                PartnerB = model.PartnerB,
                        //                IsApproved = model.IsApproved,
                        //                IsActive = model.IsActive,
                        //                ProductDetails = model.ProductDetails,
                        //                CoBrandedName = model.CoBrandedName,
                        //                ActivatedOn = DateTime.Now,
                        //                ExpiresOn = DateTime.Now.AddMonths(Convert.ToInt32(model.No_Month)),
                        //                No_Month = model.No_Month,
                        //                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                        //            });
                        //            if (res != 0)
                        //            {
                        //                foreach (var item in file)
                        //                {
                        //                    if (item != null)
                        //                    {
                        //                        string sname = item.FileName;
                        //                        strImgName = "Co-BrandProImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                        //                        path = Server.MapPath("~/Images/Co-BrandProductImages/" + strImgName);
                        //                        model.CoProductImages = strImgName;
                        //                        item.SaveAs(path);
                        //                    }
                        //                }
                        //            }
                        //            Session["Success"] = "Successfully Added The Record";
                        //            return RedirectToAction("CoBranded");

                        //        }

                        //    }
                        //}
                        #endregion
                    }

                    else
                    {
                        if (file != null)
                        {

                        }
                        int result = new EmpBrandingBLL { }.AddEditEmpBranding(new EmpBrandingModel
                        {
                            EmpBrandingId = model.EmpBrandingId,
                            EmployerId = model.EmployerId,
                            EmployerName = model.EmployerName,
                            EmpImage = model.EmpImage,
                            IsActive = model.IsActive,
                            EmpName = model.EmpName,
                            Message = model.Message,
                            UpdatedOn = DateTime.Now,
                            UpdateBy = model.EmployerId,
                        });
                        if (result != 0)
                        {
                            foreach (var item in file)
                            {
                                if (item != null)
                                {
                                    CoBrandingProImgModel objCoBraProductImage = new CoBrandingProImgModel();
                                    string sname = item.FileName;
                                    strImgName = "Co-BrandProImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                                    path = Server.MapPath("~/Images/Co-BrandProductImages/" + strImgName);
                                    objCoBraProductImage.CoBrandProdImage = strImgName;
                                    objCoBraProductImage.CreatedOn = DateTime.Now;
                                    objCoBraProductImage.IsActive = true;
                                    objCoBraProductImage.CoBrandingId = model.EmpBrandingId;
                                    int res = new CoBrandingProImgBLL { }.AddEditCoBrandProductImages(objCoBraProductImage);
                                    item.SaveAs(path);
                                }
                            }
                            Session["Success"] = "Successfully Updated The Record";
                            return RedirectToAction("EmpBranding");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("EmpBranding");
                }
                Session["Error"] = "An Error has occured";
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                return View(model);
                throw;
            }
        }

        public ActionResult ViewEmpBranding(int id)
        {
            CustomMethods.ValidateRoles("EmpBranding");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            EmpBrandingModel cob = new EmpBrandingModel();
            if (id != 0)
            {
                var objcob = new EmpBrandingBLL { }.GetEmpBrandingById(id);
                if (objcob != null)
                {
                    cob.EmployerId = objcob.EmployerId;
                    cob.EmpBrandingId = objcob.EmpBrandingId;
                    cob.EmployerName = objcob.EmployerName;  //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    cob.EmpName = objcob.EmpName;
                    cob.EmpImage = objcob.EmpImage;
                    cob.Message = objcob.Message;
                    cob.CreatedBy = objcob.EmployerId;
                    cob.CreateOn = objcob.CreateOn;
                    cob.IsActive = Convert.ToBoolean(objcob.IsActive);
                    cob.No_Month = objcob.No_Month;
                    //var objCoBraProd = new CoBrandingProImgBLL { }.GetCoBrandProductImageById(id);
                    //double count = Convert.ToDouble(objCoBraProd.Count);
                    //cob.CoBrandProductImages = objCoBraProd.Select(x => new CoBrandingProImgModel
                    //{
                    //    CoBrandingId = x.CoBrandingId,
                    //    CoBrandProdImage = x.CoBrandProdImage,
                    //    Id = x.Id,
                    //    CreatedOn = Convert.ToDateTime(x.CreatedOn),
                    //    IsActive = Convert.ToBoolean(x.IsActive)
                    //}).ToList();
                }
            }
            return View(cob);
        }

        public JsonResult ChangeEmpBrandingStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new EmpBrandingBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public JsonResult ChangeEmpBrandApprovalStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new EmpBrandingBLL { }.ChangeApprovalStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult RemoveEmpBranding(int id)
        {
            var User = new EmpBrandingBLL { }.Remove(id);
            if (User != 0)
            {
                return RedirectToAction("EmpBranding");
            }
            return RedirectToAction("EmpBranding");
        }
        #endregion

        #region Franchise

        public ActionResult FranchiseeList(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            FranchiseModel model = new FranchiseModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<FranchiseModel> Cobrand = new List<FranchiseModel>();
            CustomMethods.ValidateRoles("Franchise");
            var UserList = new FranchiseBLL { }.GetAllFranchiseeList(skip, take);


            // model.CoBrandList = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new FranchiseBLL { }.GetAllFranchiseeList(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.FranchiseeList = sortedlist.Select(x => new FranchiseModel
                {
                    Admin = x.Admin,
                    BusinessName = x.BusinessName,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    CreatedOn = x.CreatedOn,
                    Details = x.Details,
                    IsActive = x.IsActive,
                    BusinessId = x.BusinessId,
                    FranchiseId = x.FranchiseId,
                    // UpdatedDate = x.UpdatedDate,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (UserList != null)
                {
                    double count = Convert.ToDouble(new FranchiseBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.FranchiseeList = UserList.Select(x => new FranchiseModel
                    {
                        Admin = x.Admin,
                        ApprovedBy = x.ApprovedBy,
                        BusinessName = x.BusinessName,
                        ApprovedOn = x.ApprovedOn,
                        CreatedOn = x.CreatedOn,
                        Details = x.Details,
                        IsActive = x.IsActive,
                        BusinessId = x.BusinessId,
                        FranchiseId = x.FranchiseId,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult ViewFranchiserDetails(int id)
        {
            CustomMethods.ValidateRoles("Franchise");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            FranchiseModel ObjFranchise = new FranchiseModel();
            if (id != 0)
            {
                var obj = new FranchiseBLL { }.GetFranchiserDetailsById(id);
                if (obj != null)
                {
                    ObjFranchise.FranchiseId = obj.FranchiseId;
                    ObjFranchise.BusinessId = obj.BusinessId;
                    ObjFranchise.Details = obj.Details;
                    ObjFranchise.BusinessName = obj.BusinessName;
                    ObjFranchise.Admin = obj.Admin;
                    ObjFranchise.ApprovedBy = obj.ApprovedBy;
                    ObjFranchise.ApprovedOn = obj.ApprovedOn;
                    ObjFranchise.CreatedOn = obj.CreatedOn;
                    ObjFranchise.CompanyLogo = obj.CompanyLogo;
                    ObjFranchise.ExpiresOn = obj.ExpiresOn;
                    ObjFranchise.IsActive = Convert.ToBoolean(obj.IsActive);
                    ObjFranchise.No_Month = obj.No_Month;
                    ObjFranchise.TradeEmailId = obj.TradeEmailId;
                    ObjFranchise.Phone = obj.Phone;
                    ObjFranchise.Address = obj.Address;
                    ObjFranchise.EmailId = obj.EmailId;

                    //var objCoBraProd = new CoBrandingProImgBLL { }.GetCoBrandProductImageById(id);
                    //double count = Convert.ToDouble(objCoBraProd.Count);
                    //ObjFranchise.CoBrandProductImages = objCoBraProd.Select(x => new CoBrandingProImgModel
                    //{
                    //    CoBrandingId = x.CoBrandingId,
                    //    CoBrandProdImage = x.CoBrandProdImage,
                    //    Id = x.Id,
                    //    CreatedOn = Convert.ToDateTime(x.CreatedOn),
                    //    IsActive = Convert.ToBoolean(x.IsActive)
                    //}).ToList();
                }
            }
            return View(ObjFranchise);
        }

        public ActionResult AddEditFranchise(int id = 0)
        {
            CustomMethods.ValidateRoles("Franchise");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            FranchiseModel Franchisemodel = new FranchiseModel();
            if (id != 0)
            {
                var objmodel = new FranchiseBLL { }.GetFranchiserDetailsById(id);
                if (objmodel != null)
                {
                    Franchisemodel.BusinessId = objmodel.BusinessId;
                    Franchisemodel.FranchiseId = objmodel.FranchiseId;
                    Franchisemodel.Details = objmodel.Details;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    Franchisemodel.BusinessName = objmodel.BusinessName;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    Franchisemodel.IsActive = Convert.ToBoolean(objmodel.IsActive);
                    Franchisemodel.ApprovedBy = objmodel.ApprovedBy;
                    Franchisemodel.ApprovedOn = objmodel.ApprovedOn;
                    Franchisemodel.ExpiresOn = objmodel.ExpiresOn;
                    Franchisemodel.CreatedOn = objmodel.CreatedOn;
                    Franchisemodel.Admin = objmodel.Admin;
                    Franchisemodel.CompanyLogo = objmodel.CompanyLogo;
                    Franchisemodel.No_Month = objmodel.No_Month;

                }
            }
            return View(Franchisemodel);

        }

        [HttpPost]
        public ActionResult AddEditFranchise(FranchiseModel model)//, HttpPostedFileBase file
        {
            try
            {
                if (model.FranchiseId == 0)
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
                //string strFileName = "";
                //string path = "";
                //Random rnd = new Random();
                if (ModelState.IsValid)
                {
                    var checkDuplicate = objdb.Franchises.Where(x => x.BusinessId == model.BusinessId).FirstOrDefault();
                    if (checkDuplicate == null)
                    {
                        //if (file != null)
                        // {
                        //strFileName = "AccessPlanImage_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                        //path = Server.MapPath("~/Areas/Admin/ProjectImages/AccessPlanImage/" + strFileName);
                        res = new FranchiseBLL { }.AddEditFranchise(new FranchiseModel
                        {
                            BusinessId = model.BusinessId,
                            FranchiseId = model.FranchiseId,
                            BusinessName = model.BusinessName,
                            Admin = model.Admin,
                            Details = model.Details,
                            ApprovedBy = model.ApprovedBy,
                            ApprovedOn = model.ApprovedOn,
                            CreatedOn = model.CreatedOn,
                            No_Month = model.No_Month,
                            ExpiresOn = model.ExpiresOn,
                            IsActive = model.IsActive,

                        });
                        // file.SaveAs(path);
                        if (res != 0)
                        {
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("FranchiseeList");
                        }
                        //}
                        Session["Error"] = "Record Already Exists";
                        return RedirectToAction("FranchiseeList");
                    }
                    else
                    {
                        res = new FranchiseBLL { }.AddEditFranchise(new FranchiseModel
                        {
                            BusinessId = model.BusinessId,
                            FranchiseId = model.FranchiseId,
                            BusinessName = model.BusinessName,
                            Admin = model.Admin,
                            Details = model.Details,
                            ApprovedBy = model.ApprovedBy,
                            ApprovedOn = model.ApprovedOn,
                            CreatedOn = model.CreatedOn,
                            No_Month = model.No_Month,
                            ExpiresOn = model.ExpiresOn,
                            IsActive = model.IsActive,
                        });
                    }

                    if (res != 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("FranchiseeList");
                    }
                }
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindCurrencyForMembership(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindCurrencyForMembership(model);
                return View(model);
                throw;
            }
        }

        public JsonResult ChangeFranchiseStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new FranchiseBLL { }.ChangeFranchiseStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public JsonResult ChangeFranchiseApprovalStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new FranchiseBLL { }.ChangeFranchiseApprovalStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult RemoveFranchise(int id)
        {
            var User = new FranchiseBLL { }.Remove(id);
            if (User != 0)
            {
                return RedirectToAction("FranchiseeList");
            }
            return RedirectToAction("FranchiseeList");
        }

        #endregion

        #region InvestorPartnering

        public ActionResult InvestorPartnerList(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            InvestorPartneringModel model = new InvestorPartneringModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<InvestorPartneringModel> ObjInvestor = new List<InvestorPartneringModel>();
            CustomMethods.ValidateRoles("InvestorPartnering");
            var UserList = new InvestorPartnerBLL { }.GetAllInvestorPartnerList(skip, take);


            // model.CoBrandList = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new InvestorPartnerBLL { }.GetAllInvestorPartnerList(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.InvestorPartneringList = sortedlist.Select(x => new InvestorPartneringModel
                {
                    Admin = x.Admin,
                    BusinessName = x.BusinessName,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    CreatedOn = x.CreatedOn,
                    Details = x.Details,
                    IsActive = x.IsActive,
                    BusinessUserId = x.BusinessUserId,
                    InvestorPartnerId = x.InvestorPartnerId,
                    CompanyLogo = x.CompanyLogo,
                    // UpdatedDate = x.UpdatedDate,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (UserList != null)
                {
                    double count = Convert.ToDouble(new InvestorPartnerBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.InvestorPartneringList = UserList.Select(x => new InvestorPartneringModel
                    {
                        Admin = x.Admin,
                        ApprovedBy = x.ApprovedBy,
                        BusinessName = x.BusinessName,
                        ApprovedOn = x.ApprovedOn,
                        CreatedOn = x.CreatedOn,
                        Details = x.Details,
                        IsActive = x.IsActive,
                        BusinessUserId = x.BusinessUserId,
                        InvestorPartnerId = x.InvestorPartnerId,
                        CompanyLogo = x.CompanyLogo
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult ViewInvestorPartnerDetails(int id)
        {
            CustomMethods.ValidateRoles("InvestorPartnering");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            InvestorPartneringModel ObjInvestor = new InvestorPartneringModel();
            if (id != 0)
            {
                var obj = new InvestorPartnerBLL { }.GetInvestorPartnerDetailsById(id);
                if (obj != null)
                {
                    ObjInvestor.InvestorPartnerId = obj.InvestorPartnerId;
                    ObjInvestor.BusinessUserId = obj.BusinessUserId;
                    ObjInvestor.Details = obj.Details;
                    ObjInvestor.BusinessName = obj.BusinessName;
                    ObjInvestor.Admin = obj.Admin;
                    ObjInvestor.ApprovedBy = obj.ApprovedBy;
                    ObjInvestor.ApprovedOn = obj.ApprovedOn;
                    ObjInvestor.CreatedOn = obj.CreatedOn;
                    ObjInvestor.CompanyLogo = obj.CompanyLogo;
                    ObjInvestor.ExpiresOn = obj.ExpiresOn;
                    ObjInvestor.IsActive = Convert.ToBoolean(obj.IsActive);
                    ObjInvestor.No_Month = obj.No_Month;
                    ObjInvestor.Phone = obj.Phone;
                    ObjInvestor.EmailId = obj.EmailId;
                    ObjInvestor.TradeEmailId = obj.TradeEmailId;
                    ObjInvestor.Address = obj.Address;

                    //var objCoBraProd = new CoBrandingProImgBLL { }.GetCoBrandProductImageById(id);
                    //double count = Convert.ToDouble(objCoBraProd.Count);
                    //ObjFranchise.CoBrandProductImages = objCoBraProd.Select(x => new CoBrandingProImgModel
                    //{
                    //    CoBrandingId = x.CoBrandingId,
                    //    CoBrandProdImage = x.CoBrandProdImage,
                    //    Id = x.Id,
                    //    CreatedOn = Convert.ToDateTime(x.CreatedOn),
                    //    IsActive = Convert.ToBoolean(x.IsActive)
                    //}).ToList();
                }
            }
            return View(ObjInvestor);
        }

        public ActionResult AddEditInvestorPartner(int id = 0)
        {
            CustomMethods.ValidateRoles("InvestorPartnering");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            InvestorPartneringModel Franchisemodel = new InvestorPartneringModel();
            if (id != 0)
            {
                var objmodel = new InvestorPartnerBLL { }.GetInvestorPartnerDetailsById(id);
                if (objmodel != null)
                {
                    Franchisemodel.BusinessUserId = objmodel.BusinessUserId;
                    Franchisemodel.InvestorPartnerId = objmodel.InvestorPartnerId;
                    Franchisemodel.Details = objmodel.Details;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    Franchisemodel.BusinessName = objmodel.BusinessName;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    Franchisemodel.IsActive = Convert.ToBoolean(objmodel.IsActive);
                    Franchisemodel.ApprovedBy = objmodel.ApprovedBy;
                    Franchisemodel.ApprovedOn = objmodel.ApprovedOn;
                    Franchisemodel.ExpiresOn = objmodel.ExpiresOn;
                    Franchisemodel.CreatedOn = objmodel.CreatedOn;
                    Franchisemodel.Admin = objmodel.Admin;
                    Franchisemodel.CompanyLogo = objmodel.CompanyLogo;
                    Franchisemodel.No_Month = objmodel.No_Month;

                }
            }
            return View(Franchisemodel);

        }

        [HttpPost]
        public ActionResult AddEditInvestorPartner(InvestorPartneringModel model)//, HttpPostedFileBase file
        {
            try
            {
                if (model.InvestorPartnerId == 0)
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
                //string strFileName = "";
                //string path = "";
                //Random rnd = new Random();
                if (ModelState.IsValid)
                {
                    var checkDuplicate = objdb.InvestorPartnerings.Where(x => x.BusinessUserId == model.BusinessUserId).FirstOrDefault();
                    if (checkDuplicate == null)
                    {
                        //if (file != null)
                        // {
                        //strFileName = "AccessPlanImage_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                        //path = Server.MapPath("~/Areas/Admin/ProjectImages/AccessPlanImage/" + strFileName);
                        res = new InvestorPartnerBLL { }.AddEditInvestorPartnering(new InvestorPartneringModel
                        {
                            BusinessUserId = model.BusinessUserId,
                            InvestorPartnerId = model.InvestorPartnerId,
                            BusinessName = model.BusinessName,
                            Admin = model.Admin,
                            Details = model.Details,
                            ApprovedBy = model.ApprovedBy,
                            ApprovedOn = model.ApprovedOn,
                            CreatedOn = model.CreatedOn,
                            No_Month = model.No_Month,
                            ExpiresOn = model.ExpiresOn,
                            IsActive = model.IsActive,

                        });
                        // file.SaveAs(path);
                        if (res != 0)
                        {
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("InvestorPartnerList");
                        }
                        //}
                        Session["Error"] = "Record Already Exists";
                        return RedirectToAction("InvestorPartnerList");
                    }
                    else
                    {
                        res = new InvestorPartnerBLL { }.AddEditInvestorPartnering(new InvestorPartneringModel
                        {
                            BusinessUserId = model.BusinessUserId,
                            InvestorPartnerId = model.InvestorPartnerId,
                            BusinessName = model.BusinessName,
                            Admin = model.Admin,
                            Details = model.Details,
                            ApprovedBy = model.ApprovedBy,
                            ApprovedOn = model.ApprovedOn,
                            CreatedOn = model.CreatedOn,
                            No_Month = model.No_Month,
                            ExpiresOn = model.ExpiresOn,
                            IsActive = model.IsActive,
                        });
                    }

                    if (res != 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("InvestorPartnerList");
                    }
                }
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindCurrencyForMembership(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindCurrencyForMembership(model);
                return View(model);
                throw;
            }
        }

        public JsonResult ChangeInvestorPartnerStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new InvestorPartnerBLL { }.ChangeInvestorPartnerStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public JsonResult ChangeInvestorPartnerApprovalStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new InvestorPartnerBLL { }.ChangeInvestorPartnerApprovalStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult RemoveInvestorPartner(int id)
        {
            var User = new InvestorPartnerBLL { }.Remove(id);
            if (User != 0)
            {
                return RedirectToAction("InvestorPartnerList");
            }
            return RedirectToAction("InvestorPartnerList");
        }


        #endregion

        #region CustomerEnquiries

        public ActionResult CustomerEnquiry(int pid = 0)
        {
            int take = 10;
            int skip = take * pid;
            CustomerEnquiriesModel model = new CustomerEnquiriesModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<CustomerEnquiriesModel> news = new List<CustomerEnquiriesModel>();
            CustomMethods.ValidateRoles("CustomerEnquiry");
            var List = new CustomerEnquiryBLL { }.GetAllCustomerEnquiry(skip, take);
            double count = Convert.ToDouble(new CustomerEnquiryBLL { }.GetPageCount());
            var res = count / take;
            model.Pagecount = (int)Math.Ceiling(res);
            if (List != null)
            {
                model.CustomerEnquiryList = List.Select(x => new CustomerEnquiriesModel
                {
                    ContactId = x.ContactId,
                    CustomerName = x.CustomerName,
                    LoggedInUserId = x.LoggedInUserId,
                    CustSubject = x.CustSubject,
                    CustomerPhone = x.CustomerPhone,
                    CustEmailId = x.CustEmailId,
                    CustEnquiry = x.CustEnquiry
                }).ToList();
            }
            return View(model);
        }

        //public ActionResult AddEditCustomerEnquiry(int id = 0)
        //{
        //    CustomMethods.ValidateRoles("CustomerEnquiry");
        //    if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
        //        return View("ErrorPage", "Error");
        //    CustomerEnquiriesModel obj = new CustomerEnquiriesModel();
        //    if (id != 0)
        //    {
        //        CustomerEnquiriesModel newsmodel = new CustomerEnquiryBLL { }.GetCustomerEnquiryById(id);
        //        if (newsmodel != null)
        //        {
        //            obj.ContactId = newsmodel.ContactId;
        //            obj.Name = newsmodel.Name;
        //            obj.Phone = newsmodel.Phone;
        //            obj.Subject = newsmodel.Subject;
        //            obj.Enquiry = newsmodel.Enquiry;
        //           //obj.IsActive = Convert.ToBoolean(newsmodel.IsActive);
        //        }
        //    }
        //    return View(obj);
        //}

        //[HttpPost, ValidateInput(false)]
        //public ActionResult AddEditCustomerEnquiry(CustomerEnquiriesModel model)
        //{
        //    try
        //    {
        //        if (model.ContactId == 0)
        //        {
        //            if (!Convert.ToBoolean(Session["Add"]))
        //                return View("ErrorPage");
        //        }
        //        else
        //        {
        //            if (!Convert.ToBoolean(Session["Edit"]))
        //                return View("ErrorPage");
        //        }
        //        if (ModelState.IsValid)
        //        {
        //            if (model.ContactId == 0)
        //            {
        //                var checkDuplicate = objdb.CustomerEnquiries.Where(x => x.EmailId == model.EmailId).FirstOrDefault();
        //                if (checkDuplicate == null)
        //                {
        //                    int res = new CustomerEnquiryBLL { }.AddEditCustomerEnquiry(new CustomerEnquiriesModel
        //                    {
        //                        ContactId = model.ContactId,
        //                        Name = model.Name,
        //                        Enquiry = model.Enquiry,
        //                        Subject = model.Subject,
        //                        Phone = model.Phone,
        //                        EmailId = model.EmailId,
        //                    });
        //                    if (res != 0)
        //                    {
        //                        Session["Success"] = "Successfully Added The Record";
        //                        return RedirectToAction("CustomerEnquiry");
        //                    }


        //                }
        //            }

        //            else
        //            {
        //                int result = new CustomerEnquiryBLL { }.AddEditCustomerEnquiry(new CustomerEnquiriesModel
        //                {
        //                    NewsId = model.NewsId,
        //                    NewsTitle = model.NewsTitle,
        //                    NewsDesc = model.NewsDescription,
        //                    IsActive = model.IsActive,
        //                });
        //                if (result != 0)
        //                {
        //                    Session["Success"] = "Successfully Updated The Record";
        //                    return RedirectToAction("News");
        //                }
        //            }
        //            Session["Error"] = "Record Already Exists";
        //            return RedirectToAction("News");
        //        }
        //        Session["Error"] = "An Error has occured";
        //        return View(model);
        //    }
        //    catch (Exception)
        //    {
        //        Session["Error"] = "An Error has occured";
        //        return View(model);
        //        throw;
        //    }
        //}

        public ActionResult ViewCustomerEnquiry(int id = 0)
        {
            CustomMethods.ValidateRoles("CustomerEnquiry");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CustomerEnquiriesModel model = new CustomerEnquiriesModel();
            if (id != 0)
            {
                CustomerEnquiriesModel obj = new CustomerEnquiryBLL { }.GetCustomerEnquiryById(id);
                if (obj != null)
                {
                    model.ContactId = obj.ContactId;
                    model.LoggedInUserId = obj.LoggedInUserId;
                    model.CustomerName = obj.CustomerName;
                    model.CustomerPhone = obj.CustomerPhone;
                    model.CustSubject = obj.CustSubject;
                    model.CustEmailId = obj.CustEmailId;
                    model.CustEnquiry = obj.CustEnquiry;
                    // model.IsActive = Convert.ToBoolean(obj.IsActive);
                }
            }
            return View(model);
        }

        public ActionResult RemoveCustomerEnquiry(int id)
        {
            var User = new CustomerEnquiryBLL { }.Remove(id);
            if (User != 0)
            {
                return RedirectToAction("CustomerEnquiry");
            }
            return RedirectToAction("CustomerEnquiry");
        }

        public ActionResult SearchCustomerEnquiry(string date, string date1)
        {
            DateTime dt1 = Convert.ToDateTime(date);
            DateTime dt2 = Convert.ToDateTime(date1);
            var omodel = objdb.CustomerEnquiries.Where(x => x.CreatedDate >= dt1 && x.CreatedDate <= dt2).Select(x => new CustomerEnquiriesModel
            {
                ContactId = x.ContactId,
                CustomerName = x.Name,
                LoggedInUserId = x.LoginUserId,
                CustSubject = x.Subject,
                CustomerPhone = x.Phone,
                CustEmailId = x.EmailId,
                CustEnquiry = x.Enquiry,
                CreatedDate = x.CreatedDate

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
            //
            //}).OrderBy(x => x.CreatedDate).ToList();
        }

        //public JsonResult ChangeCustomerEnquiryStatus(int id)
        //{
        //    bool Result = false;
        //    bool ChangeStatus = new CustomerEnquiryBLL { }.ChangeStatus(id);
        //    if (ChangeStatus)
        //    {
        //        Result = true;
        //    }
        //    return Json(Result);
        //}

        #endregion

        #region CorporateBranding

        public ActionResult CorporateBranding(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            CorporateBrandingModel model = new CorporateBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<CorporateBrandingModel> ObjInvestor = new List<CorporateBrandingModel>();
            CustomMethods.ValidateRoles("CorporateBranding");
            var UserList = new CorporateBrandingBLL { }.GetAllCorporateBrandingList(skip, take);


            // model.CoBrandList = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new CorporateBrandingBLL { }.GetAllCorporateBrandingList(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.CorporateBrandingList = sortedlist.Select(x => new CorporateBrandingModel
                {
                    Admin = x.Admin,
                    BusinessName = x.BusinessName,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    CreatedOn = x.CreatedOn,
                    CorporateBrandDetails = x.CorporateBrandDetails,
                    IsActive = x.IsActive,
                    BusinessUserId = x.BusinessUserId,
                    CorporateBrandingId = x.CorporateBrandingId,
                    CompanyLogo = x.CompanyLogo,
                    // UpdatedDate = x.UpdatedDate,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (UserList != null)
                {
                    double count = Convert.ToDouble(new CorporateBrandingBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.CorporateBrandingList = UserList.Select(x => new CorporateBrandingModel
                    {
                        Admin = x.Admin,
                        ApprovedBy = x.ApprovedBy,
                        BusinessName = x.BusinessName,
                        ApprovedOn = x.ApprovedOn,
                        CreatedOn = x.CreatedOn,
                        CorporateBrandDetails = x.CorporateBrandDetails,
                        IsActive = x.IsActive,
                        BusinessUserId = x.BusinessUserId,
                        CorporateBrandingId = x.CorporateBrandingId,
                        CompanyLogo = x.CompanyLogo
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult ViewCorporateBrandingDetails(int id)
        {
            CustomMethods.ValidateRoles("CorporateBranding");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CorporateBrandingModel Objm = new CorporateBrandingModel();
            if (id != 0)
            {
                var obj = new CorporateBrandingBLL { }.GetCorporateBrandingDetailsById(id);
                if (obj != null)
                {
                    Objm.CorporateBrandingId = obj.CorporateBrandingId;
                    Objm.BusinessUserId = obj.BusinessUserId;
                    Objm.CorporateBrandDetails = obj.CorporateBrandDetails;
                    Objm.BusinessName = obj.BusinessName;
                    Objm.Admin = obj.Admin;
                    Objm.ApprovedBy = obj.ApprovedBy;
                    Objm.ApprovedOn = obj.ApprovedOn;
                    Objm.CreatedOn = obj.CreatedOn;
                    Objm.CompanyLogo = obj.CompanyLogo;
                    Objm.ExpiresOn = obj.ExpiresOn;
                    Objm.IsActive = Convert.ToBoolean(obj.IsActive);
                    Objm.No_Month = obj.No_Month;
                    Objm.EmailId = obj.EmailId;
                    Objm.Phone = obj.Phone;
                    Objm.TradeEmailId = obj.TradeEmailId;
                    Objm.Address = obj.Address;



                    //var objCoBraProd = new CoBrandingProImgBLL { }.GetCoBrandProductImageById(id);
                    //double count = Convert.ToDouble(objCoBraProd.Count);
                    //ObjFranchise.CoBrandProductImages = objCoBraProd.Select(x => new CoBrandingProImgModel
                    //{
                    //    CoBrandingId = x.CoBrandingId,
                    //    CoBrandProdImage = x.CoBrandProdImage,
                    //    Id = x.Id,
                    //    CreatedOn = Convert.ToDateTime(x.CreatedOn),
                    //    IsActive = Convert.ToBoolean(x.IsActive)
                    //}).ToList();
                }
            }
            return View(Objm);
        }

        public ActionResult AddEditCorporateBranding(int id = 0)
        {
            CustomMethods.ValidateRoles("CorporateBranding");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            CorporateBrandingModel Objmodel = new CorporateBrandingModel();
            if (id != 0)
            {
                var obj = new CorporateBrandingBLL { }.GetCorporateBrandingDetailsById(id);
                if (obj != null)
                {
                    Objmodel.BusinessUserId = obj.BusinessUserId;
                    Objmodel.CorporateBrandingId = obj.CorporateBrandingId;
                    Objmodel.CorporateBrandDetails = obj.CorporateBrandDetails;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    Objmodel.BusinessName = obj.BusinessName;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    Objmodel.IsActive = Convert.ToBoolean(obj.IsActive);
                    Objmodel.ApprovedBy = obj.ApprovedBy;
                    Objmodel.ApprovedOn = obj.ApprovedOn;
                    Objmodel.ExpiresOn = obj.ExpiresOn;
                    Objmodel.CreatedOn = obj.CreatedOn;
                    Objmodel.Admin = obj.Admin;
                    Objmodel.CompanyLogo = obj.CompanyLogo;
                    Objmodel.No_Month = obj.No_Month;

                }
            }
            return View(Objmodel);

        }

        [HttpPost]
        public ActionResult AddEditCorporateBranding(CorporateBrandingModel model)//, HttpPostedFileBase file
        {
            try
            {
                if (model.CorporateBrandingId == 0)
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
                //string strFileName = "";
                //string path = "";
                //Random rnd = new Random();
                if (ModelState.IsValid)
                {
                    var checkDuplicate = objdb.CorporateBrandings.Where(x => x.BusinessUserId == model.BusinessUserId).FirstOrDefault();
                    if (checkDuplicate == null)
                    {
                        //res = new InvestorPartnerBLL { }.AddEditInvestorPartnering(new InvestorPartneringModel
                        //{
                        //    BusinessUserId = model.BusinessUserId,
                        //    InvestorPartnerId = model.CorporateBrandingId,
                        //    BusinessName = model.BusinessName,
                        //    Admin = model.Admin,
                        //    Details = model.CorporateBrandDetails,
                        //    ApprovedBy = model.ApprovedBy,
                        //    ApprovedOn = model.ApprovedOn,
                        //    CreatedOn = model.CreatedOn,
                        //    No_Month = model.No_Month,
                        //    ExpiresOn = model.ExpiresOn,
                        //    IsActive = model.IsActive,

                        //});
                        res = new CorporateBrandingBLL { }.AddEditCorporateBranding(new CorporateBrandingModel
                        {
                            BusinessUserId = model.BusinessUserId,
                            CorporateBrandingId = model.CorporateBrandingId,
                            BusinessName = model.BusinessName,
                            Admin = model.Admin,
                            CorporateBrandDetails = model.CorporateBrandDetails,
                            ApprovedBy = model.ApprovedBy,
                            ApprovedOn = model.ApprovedOn,
                            CreatedOn = model.CreatedOn,
                            No_Month = model.No_Month,
                            ExpiresOn = model.ExpiresOn,
                            IsActive = model.IsActive,

                        });
                        if (res != 0)
                        {
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("CorporateBranding");
                        }
                        //}
                        Session["Error"] = "Record Already Exists";
                        return RedirectToAction("CorporateBranding");
                    }
                    else
                    {
                        res = new CorporateBrandingBLL { }.AddEditCorporateBranding(new CorporateBrandingModel
                        {
                            BusinessUserId = model.BusinessUserId,
                            CorporateBrandingId = model.CorporateBrandingId,
                            BusinessName = model.BusinessName,
                            Admin = model.Admin,
                            CorporateBrandDetails = model.CorporateBrandDetails,
                            ApprovedBy = model.ApprovedBy,
                            ApprovedOn = model.ApprovedOn,
                            CreatedOn = model.CreatedOn,
                            No_Month = model.No_Month,
                            ExpiresOn = model.ExpiresOn,
                            IsActive = model.IsActive,
                        });
                    }

                    if (res != 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("CorporateBranding");
                    }
                }
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindCurrencyForMembership(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindCurrencyForMembership(model);
                return View(model);
                throw;
            }
        }

        public JsonResult ChangeCorporateBrandingStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new CorporateBrandingBLL { }.ChangeCorporateBrandingStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public JsonResult ChangeCorporateBrandingApprovalStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new CorporateBrandingBLL { }.ChangeCorporateBrandingApprovalStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult RemoveCorporateBranding(int id)
        {
            var User = new CorporateBrandingBLL { }.Remove(id);
            if (User != 0)
            {
                return RedirectToAction("CorporateBranding");
            }
            return RedirectToAction("CorporateBranding");
        }

        #endregion

        #region GlobalBranding

        public ActionResult GlobalBranding(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            GlobalBrandingModel model = new GlobalBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<GlobalBrandingModel> ObjInvestor = new List<GlobalBrandingModel>();
            CustomMethods.ValidateRoles("GlobalBranding");
            var UserList = new GlobalBrandingBLL { }.GetAllGlobalBrandingList(skip, take);


            // model.CoBrandList = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new GlobalBrandingBLL { }.GetAllGlobalBrandingList(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.GlobalBrandingList = sortedlist.Select(x => new GlobalBrandingModel
                {
                    Admin = x.Admin,
                    BusinessName = x.BusinessName,
                    ApprovedOn = x.ApprovedOn,
                    ApprovedBy = x.ApprovedBy,
                    CreatedOn = x.CreatedOn,
                    GlobalBrandDetails = x.GlobalBrandDetails,
                    IsActive = x.IsActive,
                    BusinessUserId = x.BusinessUserId,
                    GlobalBrandingId = x.GlobalBrandingId,
                    CompanyLogo = x.CompanyLogo,
                    // UpdatedDate = x.UpdatedDate,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (UserList != null)
                {
                    double count = Convert.ToDouble(new GlobalBrandingBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.GlobalBrandingList = UserList.Select(x => new GlobalBrandingModel
                    {
                        Admin = x.Admin,
                        ApprovedBy = x.ApprovedBy,
                        BusinessName = x.BusinessName,
                        ApprovedOn = x.ApprovedOn,
                        CreatedOn = x.CreatedOn,
                        GlobalBrandDetails = x.GlobalBrandDetails,
                        IsActive = x.IsActive,
                        BusinessUserId = x.BusinessUserId,
                        GlobalBrandingId = x.GlobalBrandingId,
                        CompanyLogo = x.CompanyLogo
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        //IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult ViewGlobalBrandingDetails(int id)
        {
            CustomMethods.ValidateRoles("GlobalBranding");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            GlobalBrandingModel ObjGlobal = new GlobalBrandingModel();
            if (id != 0)
            {
                var obj = new GlobalBrandingBLL { }.GetGlobalBrandingDetailsById(id);
                if (obj != null)
                {
                    ObjGlobal.GlobalBrandingId = obj.GlobalBrandingId;
                    ObjGlobal.BusinessUserId = obj.BusinessUserId;
                    ObjGlobal.GlobalBrandDetails = obj.GlobalBrandDetails;
                    ObjGlobal.BusinessName = obj.BusinessName;
                    ObjGlobal.Admin = obj.Admin;
                    ObjGlobal.ApprovedBy = obj.ApprovedBy;
                    ObjGlobal.ApprovedOn = obj.ApprovedOn;
                    ObjGlobal.CreatedOn = obj.CreatedOn;
                    ObjGlobal.CompanyLogo = obj.CompanyLogo;
                    ObjGlobal.ExpiresOn = obj.ExpiresOn;
                    ObjGlobal.IsActive = Convert.ToBoolean(obj.IsActive);
                    ObjGlobal.No_Month = obj.No_Month;
                    ObjGlobal.EmailId = obj.EmailId;
                    ObjGlobal.Phone = obj.Phone;
                    ObjGlobal.TradeEmailId = obj.TradeEmailId;
                    ObjGlobal.Address = obj.Address;


                    //var objCoBraProd = new CoBrandingProImgBLL { }.GetCoBrandProductImageById(id);
                    //double count = Convert.ToDouble(objCoBraProd.Count);
                    //ObjFranchise.CoBrandProductImages = objCoBraProd.Select(x => new CoBrandingProImgModel
                    //{
                    //    CoBrandingId = x.CoBrandingId,
                    //    CoBrandProdImage = x.CoBrandProdImage,
                    //    Id = x.Id,
                    //    CreatedOn = Convert.ToDateTime(x.CreatedOn),
                    //    IsActive = Convert.ToBoolean(x.IsActive)
                    //}).ToList();
                }
            }
            return View(ObjGlobal);
        }

        public ActionResult AddEditGlobalBranding(int id = 0)
        {
            CustomMethods.ValidateRoles("GlobalBranding");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            GlobalBrandingModel Globalmodel = new GlobalBrandingModel();
            if (id != 0)
            {
                //var objmodel = new InvestorPartnerBLL { }.GetInvestorPartnerDetailsById(id);
                var objmodel = new GlobalBrandingBLL { }.GetGlobalBrandingDetailsById(id);
                if (objmodel != null)
                {
                    Globalmodel.BusinessUserId = objmodel.BusinessUserId;
                    //Globalmodel.GlobalBrandingId = objmodel.InvestorPartnerId;
                    //Globalmodel.GlobalBrandDetails = objmodel.Details;   
                    Globalmodel.GlobalBrandingId = objmodel.GlobalBrandingId;
                    Globalmodel.GlobalBrandDetails = objmodel.GlobalBrandDetails; 
                    Globalmodel.BusinessName = objmodel.BusinessName;         
                    Globalmodel.IsActive = Convert.ToBoolean(objmodel.IsActive);
                    Globalmodel.ApprovedBy = objmodel.ApprovedBy;
                    Globalmodel.ApprovedOn = objmodel.ApprovedOn;
                    Globalmodel.ExpiresOn = objmodel.ExpiresOn;
                    Globalmodel.CreatedOn = objmodel.CreatedOn;
                    Globalmodel.Admin = objmodel.Admin;
                    Globalmodel.CompanyLogo = objmodel.CompanyLogo;
                    Globalmodel.No_Month = objmodel.No_Month;

                }
            }
            return View(Globalmodel);

        }

        [HttpPost]
        public ActionResult AddEditGlobalBranding(GlobalBrandingModel model)//, HttpPostedFileBase file
        {
            try
            {
                if (model.GlobalBrandingId == 0)
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
                //string strFileName = "";
                //string path = "";
                //Random rnd = new Random();
                if (ModelState.IsValid)
                {
                    var checkDuplicate = objdb.GlobalBrandings.Where(x => x.BusinessUserId == model.BusinessUserId).FirstOrDefault();
                    if (checkDuplicate == null)
                    {

                        res = new GlobalBrandingBLL { }.AddEditGlobalBranding(new GlobalBrandingModel
                        {
                            BusinessUserId = model.BusinessUserId,
                            GlobalBrandingId = model.GlobalBrandingId,
                            BusinessName = model.BusinessName,
                            Admin = model.Admin,
                            GlobalBrandDetails = model.GlobalBrandDetails,
                            ApprovedBy = model.ApprovedBy,
                            ApprovedOn = model.ApprovedOn,
                            CreatedOn = model.CreatedOn,
                            No_Month = model.No_Month,
                            ExpiresOn = model.ExpiresOn,
                            IsActive = model.IsActive,

                        });
                        // file.SaveAs(path);
                        if (res != 0)
                        {
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("GlobalBranding");
                        }
                        //}
                        Session["Error"] = "Record Already Exists";
                        return RedirectToAction("GlobalBranding");
                    }
                    else
                    {
                        res = new GlobalBrandingBLL { }.AddEditGlobalBranding(new GlobalBrandingModel
                        {
                            BusinessUserId = model.BusinessUserId,
                            GlobalBrandingId = model.GlobalBrandingId,
                            BusinessName = model.BusinessName,
                            Admin = model.Admin,
                            GlobalBrandDetails = model.GlobalBrandDetails,
                            ApprovedBy = model.ApprovedBy,
                            ApprovedOn = model.ApprovedOn,
                            CreatedOn = model.CreatedOn,
                            No_Month = model.No_Month,
                            ExpiresOn = model.ExpiresOn,
                            IsActive = model.IsActive,
                        });
                    }

                    if (res != 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("GlobalBranding");
                    }
                }
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindCurrencyForMembership(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                //CustomMethods.BindCurrencyForMembership(model);
                return View(model);
                throw;
            }
        }

        public JsonResult ChangeGlobalBrandingStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new GlobalBrandingBLL { }.ChangeGlobalBrandingStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public JsonResult ChangeGlobalBrandingApprovalStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new GlobalBrandingBLL { }.ChangeGlobalBrandingApprovalStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult RemoveGlobalBranding(int id)
        {
            var User = new GlobalBrandingBLL { }.Remove(id);
            if (User != 0)
            {
                return RedirectToAction("GlobalBranding");
            }
            return RedirectToAction("GlobalBranding");
        }

        #endregion

        #region BigHoarding
        public ActionResult BigHoarding(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            HomeBannerModel model = new HomeBannerModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<HomeBannerModel> Products = new List<HomeBannerModel>();
            CustomMethods.ValidateRoles("BigHoardingBanner");
            var BigHoardingList = new HomeBannerBLL { }.GetAllBannerImages(skip, take);
            if (cid != 0)
            {
                var sortedlist = new HomeBannerBLL { }.GetBannerImageByIUserId(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.HomeBannerImgList = sortedlist.Select(x => new HomeBannerModel
                {
                    UserId = x.UserId,
                    BannerImage = x.BannerImage.Single().ToString(),
                    BusinessName = x.BusinessName.Single().ToString(),
                    IsActive = x.IsActive,
                    //// UpdatedDate = x.UpdatedDate,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (BigHoardingList.GroupBy(x => x.UserId).Distinct() != null)
                {
                    double count = Convert.ToDouble(new HomeBannerBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.HomeBannerImgList = BigHoardingList.Select(x => new HomeBannerModel
                     {
                         HomeBannerID = x.HomeBannerID,
                         UserId = x.UserId,
                         BannerImage = x.BannerImage,
                         BusinessName = x.BusinessName,
                         IsActive = x.IsActive,
                         //CreatedBy = Convert.ToInt32(x.CreatedBy),
                         //IsActive = Convert.ToBoolean(x.IsActive)
                     }).ToList();
                }
            }
            return View(model);
        }

        public ActionResult AddEditBigHoarding(int id = 0)
        {
            CustomMethods.ValidateRoles("BigHoardingBanner");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            HomeBannerModel Obj = new HomeBannerModel();
            if (id != 0)
            {
                //var userdata = new BussinessUserBLL { }.GetUserById(id);
                var data1 = new HomeBannerBLL { }.GetBannerImageById(id);
                if (data1 != null)
                {
                    //Obj.HomeBannerID = data.HomeBannerID;
                    Obj.UserId = data1.UserId;
                    //Obj.BannerImage = data.BannerImage;
                    //Obj.IsActive = data.IsActive;
                    Obj.Phone = data1.Phone;
                    Obj.CoAddress = data1.CoAddress;
                    Obj.TradeEmaiIId = data1.TradeEmaiIId;
                    Obj.Fax = data1.Fax;
                    Obj.EmailId = data1.EmailId;
                    Obj.BusinessName = data1.BusinessName;
                    Obj.BannerImage = data1.BannerImage;
                    Obj.CompanyURL = data1.CompanyURL;
                    Obj.HomeBannerID = data1.HomeBannerID;
                    Obj.IsActive = data1.IsActive;

                    //var data = new HomeBannerBLL { }.GetBannerImageByIUserId(id);
                    //Obj.HomeBannerImgList = data.Select(x => new HomeBannerModel
                    //{
                    //    HomeBannerID = x.HomeBannerID,
                    //    BannerImage = x.BannerImage,
                    //    IsActive = Convert.ToBoolean(x.IsActive),
                    //    UserId = x.UserId
                    //}).ToList();
                }
            }

            return View(Obj);
        }

        [HttpPost]
        public ActionResult AddEditBigHoarding(HomeBannerModel model)
        {
            //int res = 0;
            //res = new HomeBannerBLL { }.AddEditBanner(new HomeBannerModel
            //{
            //    UserId = model.UserId,
            //    IsActive = model.IsActive,
            //    //BannerImage = strFileName,
            //    HomeBannerID = model.HomeBannerID,
            //    CompanyURL = model.CompanyURL
            //    //CompanyURL = strURL
            //});
            Session["Success"] = "We can only Update Status from Big Hoarding";
            return RedirectToAction("BigHoarding");
        }

        public ActionResult ViewBigHoardingUser(int id)
        {
            CustomMethods.ValidateRoles("BigHoardingBanner");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            HomeBannerModel Obj = new HomeBannerModel();
            if (id != 0)
            {
                var userdata = new BussinessUserBLL { }.GetUserById(id);

                if (userdata != null)
                {
                    //Obj.HomeBannerID = data.HomeBannerID;
                    Obj.UserId = userdata.UserId;
                    //Obj.BannerImage = data.BannerImage;
                    //Obj.IsActive = data.IsActive;
                    Obj.Phone = userdata.Phone;
                    Obj.CoAddress = userdata.CoAddress;
                    Obj.TradeEmaiIId = userdata.TradeEmaiIId;
                    Obj.Fax = userdata.Fax;
                    Obj.EmailId = userdata.EmailId;
                    Obj.BusinessName = userdata.BusinessName;

                    var data = new HomeBannerBLL { }.GetBannerImageById(id);
                    if (data!=null)
                    {
                        Obj.BannerImage = data.BannerImage;
                    }
                    //Obj.HomeBannerImgList = data.Select(x => new HomeBannerModel
                    //{
                    //    HomeBannerID = x.HomeBannerID,
                    //    BannerImage = x.BannerImage,
                    //    IsActive = Convert.ToBoolean(x.IsActive),
                    //    UserId = x.UserId
                    //}).ToList();
                }
            }

            return View(Obj);
        }

        public JsonResult ChangeBigHoardingStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new HomeBannerBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult UploadAsync()
        {
            var file = Request.Files;
            string saveto = string.Empty;
            string name = string.Empty;
            if (Request.Files.Count != 0)
            {
                var File = Request.Files[0];
                if (File != null)
                {
                    string ext = Path.GetExtension(File.FileName);
                    name = Path.GetFileName(File.FileName);
                    //name = name.Substring(0, name.LastIndexOf("."));
                    string newfilename = Filelist("~/Scripts/jquery.bxslider/images/", name) + ext;
                    saveto = Path.Combine(Server.MapPath("~/Scripts/jquery.bxslider/images"), name);
                    File.SaveAs(saveto);
                }
            }

            return Json(name, JsonRequestBehavior.AllowGet);
        }

        private string Filelist(string filepath, string filename)
        {
            string newfilename = filename;
            FileInfo fi = new FileInfo(Server.MapPath(filepath));
            DirectoryInfo di = fi.Directory;
            FileSystemInfo[] fsi = di.GetFiles();
            var orderredfile = fsi.OrderBy(f => f.CreationTime);

            foreach (FileSystemInfo info in orderredfile)
            {
                if (info.FullName.Contains(filename))
                {
                    string[] arr = info.Name.Split(new char[] { '_' });
                    string id = "0";
                    if (arr.Length > 2)
                        id = arr[1];
                    //string id = info.Name.Substring(info.Name.LastIndexOf('_') + 1, (info.Name.LastIndexOf('.') - info.Name.LastIndexOf('_')) - 1);
                    newfilename = filename + "_" + Convert.ToInt32(Convert.ToInt32(id) + 1).ToString();


                }
            }
            if (newfilename == filename)
            {
                newfilename = newfilename + "_1";
            }
            return newfilename;
        }

        #endregion

        #region TargetBranding

        public ActionResult TargetBranding(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            TargetBrandingModel model = new TargetBrandingModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<TargetBrandingModel> Target = new List<TargetBrandingModel>();
            CustomMethods.ValidateRoles("TargetBranding");
            var UserList = new TargerBrandBLL { }.GetAllTargetedBrandUsers(skip, take);


            // //model.CoBrandList = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new TargerBrandBLL { }.GetAllTargetedBrandUsers(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.TargetBrandingList = sortedlist.Select(x => new TargetBrandingModel
                {
                    BusinessName = x.BusinessName,
                    BusinessUserId = x.BusinessUserId,
                    CityId = x.CityId,
                    CityName = x.CityName,
                    IndustryId = x.IndustryId,
                    IndustryName = x.IndustryName,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                    Image = x.Image,
                    URL = x.URL,
                    TargetBrandingId = x.TargetBrandingId,
                    No_Month = x.No_Month,
                    ExpiresOn = x.ExpiresOn,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    //IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
                //model.Timespanlist = sortedlist.Select(x => new TargetBrandingModel
                //{
                //    BusinessUserId=x.BusinessUserId,
                //    stdt=x.CreatedDate,
                //    enddt=x.ExpiresOn,
                //    DaysCounter=
                //}).ToList();
            }
            else
            {
                if (UserList != null)
                {
                    double count = Convert.ToDouble(new TargerBrandBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.TargetBrandingList = UserList.Select(x => new TargetBrandingModel
                    {
                        BusinessName = x.BusinessName,
                        BusinessUserId = x.BusinessUserId,
                        CityId = x.CityId,
                        CityName = x.CityName,
                        IndustryId = x.IndustryId,
                        IndustryName = x.IndustryName,
                        CreatedDate = x.CreatedDate,
                        IsActive = x.IsActive,
                        Image = x.Image,
                        URL = x.URL,
                        TargetBrandingId = x.TargetBrandingId,
                        No_Month = x.No_Month,
                        ExpiresOn = x.ExpiresOn,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                    }).ToList();
                    //DateTime stdate=Convert.ToDateTime(model.CreatedDate);
                    //DateTime enddate=Convert.ToDateTime(model.ExpiresOn);
                    //TimeSpan diff = enddate.Subtract(stdate);
                    //model.DaysCounter = diff;
                }
            }
            return View(model);
        }

        

        public ActionResult AddEditTargetBranding(int id = 0)
        {
            CustomMethods.ValidateRoles("TargetBranding");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            TargetBrandingModel Objmodel = new TargetBrandingModel();
            DateTime dt = new PaymentTransactionBLL { }.TargerBrandAdImageBLL();
            if (dt != Convert.ToDateTime("1980-01-01"))
            {
                if (dt.Date <= DateTime.Now.Date)
                {
                    ViewBag.Date = DateTime.Now;

                }
                else
                {
                    ViewBag.Date = dt.AddDays(1);
                }
            }
            if (id != 0)
            {

                var obj = new TargerBrandBLL { }.GetTargetBrandByImageId(id);
                if (obj != null)
                {
                    Objmodel.BusinessUserId = obj.BusinessUserId;
                    Objmodel.BusinessName = obj.BusinessName;
                    Objmodel.IndustryId = obj.IndustryId;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    Objmodel.IndustryName = obj.IndustryName;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    Objmodel.IsActive = Convert.ToBoolean(obj.IsActive);
                    Objmodel.CityId = obj.CityId;
                    Objmodel.CityName = obj.CityName;
                    Objmodel.ExpiresOn = obj.ExpiresOn;
                    Objmodel.No_Month = obj.No_Month;
                    Objmodel.TargetBrandingId = obj.TargetBrandingId;
                    Objmodel.URL = obj.URL;

                }
            }
            CustomMethods.BindBusinessUser(Objmodel);
            CustomMethods.BindIndustry(Objmodel);
            CustomMethods.BindCityForUser(Objmodel);
            return View(Objmodel);
        }

        [HttpPost]
        public ActionResult AddEditTargetBranding(TargetBrandingModel model, IEnumerable<HttpPostedFileBase> file)
        {
            Random rnd = new Random();
            string strImgName = "";
            string path = "";
            try
            {
                if (model.TargetBrandingId == 0)
                {
                    if (!Convert.ToBoolean(Session["Add"]))
                        return View("ErrorPage");
                }
                else
                {
                    if (!Convert.ToBoolean(Session["Edit"]))
                        return View("ErrorPage");
                }

                if (ModelState.IsValid)
                {
                    if (model.TargetBrandingId == 0)
                    {
                        //var checkDuplicate = objdb.TargetBrandings.Where(x => x.ProductName == model.ProductName && x.CategoryId == model.CategoryId && x.SubCategoryId == model.SubCategoryId).SingleOrDefault();
                        //if (checkDuplicate == null)
                        //{
                        int res = new TargerBrandBLL { }.AddEditTargetBranding(new TargetBrandingModel
                        {
                            TargetBrandingId = model.TargetBrandingId,
                            IndustryId = model.IndustryId,
                            CityId = model.CityId,
                            CreatedDate = model.CreatedDate,
                            ExpiresOn = model.ExpiresOn,
                            IsActive = model.IsActive,
                            BusinessUserId = model.BusinessUserId,
                            BusinessName = model.BusinessName,
                            URL = model.URL
                        });
                        if (res != 0)
                        {
                            foreach (var item in file)
                            {
                                if (item != null)
                                {
                                    TargetImageModel objImage = new TargetImageModel();
                                    string sname = item.FileName;
                                    strImgName = "TargetAdImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                                    path = Server.MapPath("~/Images/TargetBrandingImages/" + strImgName);
                                    objImage.Image = strImgName;
                                    objImage.CreatedDate = model.CreatedDate;
                                    objImage.IsActive = true;
                                    objImage.TargetBrandingId = res;
                                    int result = new TargetImageBLL { }.InsertTargetAdImagesBLL(objImage);
                                    item.SaveAs(path);
                                }
                            }
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("TargetBranding");
                        }
                        Session["Error"] = "An Error has occured";
                        CustomMethods.BindBusinessUser(model);
                        CustomMethods.BindIndustry(model);
                        CustomMethods.BindCityForUser(model);
                        return View(model);
                        //}
                    }
                    else
                    {
                        int data = new TargerBrandBLL { }.AddEditTargetBranding(new TargetBrandingModel
                        {
                            TargetBrandingId = model.TargetBrandingId,
                            CityId = model.CityId,
                            IndustryId = model.IndustryId,
                            BusinessName = model.BusinessName,
                            URL = model.URL,
                            IsActive = model.IsActive,
                            CreatedDate = model.CreatedDate,
                            ExpiresOn = model.ExpiresOn,
                            UpdateDate = DateTime.Now,
                            BusinessUserId = model.BusinessUserId,
                        });
                        if (data != 0)
                        {
                            foreach (var item in file)
                            {
                                if (item != null)
                                {
                                    TargetImageModel objImage = new TargetImageModel();
                                    string sname = item.FileName;
                                    strImgName = "TargetAdImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                                    path = Server.MapPath("~/Images/TargetBrandingImages/" + strImgName);
                                    objImage.Image = strImgName;
                                    objImage.CreatedDate = model.CreatedDate;
                                    objImage.IsActive = true;
                                    objImage.TargetBrandingId = data;
                                    int result = new TargetImageBLL { }.InsertTargetAdImagesBLL(objImage);
                                    item.SaveAs(path);
                                }
                                else
                                {
                                    Session["Success"] = "Successfully Updated The Record";
                                    return RedirectToAction("TargetBranding");
                                }
                            }
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("TargetBranding");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("AddEditTargetBranding");
                }
                Session["Error"] = "An Error has occured";
                CustomMethods.BindBusinessUser(model);
                CustomMethods.BindIndustry(model);
                CustomMethods.BindCityForUser(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                CustomMethods.BindBusinessUser(model);
                CustomMethods.BindIndustry(model);
                CustomMethods.BindCityForUser(model);
                return View(model);
                throw;
            }
        }

        public JsonResult ChangeTargetBrandStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new TargerBrandBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult RemoveTargetBranding(int id)
        {
            var User = new TargerBrandBLL { }.Remove(id);
            if (User != 0)
            {
                return RedirectToAction("TargetBranding");
            }
            return RedirectToAction("TargetBranding");
        }

        public ActionResult ViewTargetBrand(int id = 0)
        {
            CustomMethods.ValidateRoles("TargetBranding");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            TargetBrandingModel objmodel = new TargetBrandingModel();
            if (id != 0)
            {
                var obj = new TargerBrandBLL { }.GetTargetBrandByImageId(id);
                if (obj != null)
                {
                    objmodel.TargetBrandingId = obj.TargetBrandingId;
                    objmodel.CityId = obj.CityId;
                    objmodel.IndustryId = obj.IndustryId;
                    objmodel.CityName = obj.CityName;
                    objmodel.IndustryName = obj.IndustryName;
                    objmodel.BusinessName = obj.BusinessName;
                    objmodel.TradeEmaiIId = obj.TradeEmaiIId;
                    objmodel.Phone = obj.Phone;
                    objmodel.CoAddress = obj.CoAddress;
                    objmodel.EmailId = obj.EmailId;
                    objmodel.Fax = obj.Fax;
                    objmodel.ExpiresOn = obj.ExpiresOn;
                    objmodel.CreatedDate = obj.CreatedDate;
                    objmodel.URL = obj.URL;
                    objmodel.IsActive = Convert.ToBoolean(obj.IsActive);

                    // ProductImageWebModel prodimage = new ProductImageWebModel();
                    //if (id != 0)
                    //{
                    var objacplan = new TargetImageBLL { }.GetTargetBrandAdImagesById(id);
                    double count = Convert.ToDouble(objacplan.Count);
                    objmodel.TargetImageList = objacplan.Select(x => new TargetImageModel
                    {
                        TargetBrandingId = x.TargetBrandingId,
                        TargetImageId = x.TargetImageId,
                        Image = x.Image,
                        CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                    //}
                }
            }
            //CustomMethods.BindProduct(prod);
            //CustomMethods.BindSubategory(prod);
            //CustomMethods.BindSubCategory(prod);
            return View(objmodel);
        }

        #endregion

        public ActionResult Searching(string searchValue)
        {
            //int userid = Convert.ToInt32(Session["UserId"]);
            SearchwordsModel searchObj = new SearcbyKeywordBLL { }.Search(searchValue);
            ViewBag.SearchKey = searchValue;
            return View(searchObj);
        }
    }
}
