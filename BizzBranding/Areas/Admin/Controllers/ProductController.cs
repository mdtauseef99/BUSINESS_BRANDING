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

namespace BizzBranding.Areas.Admin.Controllers
{
    public class ProductController : Controller
    {
        //
        //// GET: /Admin/Product/
        BizzBrandingEntities objdb = new BizzBrandingEntities();
        public ActionResult Index()
        {
            return View();
        }

        #region Products
        public ActionResult Products(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            ProductWebModel model = new ProductWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<ProductWebModel> Products = new List<ProductWebModel>();
            CustomMethods.ValidateRoles("Product");
            var Prolist = new ProductBLL { }.GetAllProduct(skip, take);


            model.Product = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new ProductBLL { }.GetAllProduct(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.Products = sortedlist.Select(x => new ProductWebModel
                {
                    CategoryId = x.CategoryId,
                    SubCategoryId = x.SubCategoryId,
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    ProdDetails = x.ProdDetails,
                    //CreatedDate = Convert.ToInt32(x.CreatedDate),
                    CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (Prolist != null)
                {
                    double count = Convert.ToDouble(new ProductBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.Products = Prolist.Select(x => new ProductWebModel
                    {
                        CategoryId = x.CategoryId,
                        SubCategoryId = x.SubCategoryId,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        ProdDetails = x.ProdDetails,
                        //CreatedDate = Convert.ToInt32(x.CreatedDate),
                        CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            CustomMethods.BindParentCategory(model);
            CustomMethods.BindSubCategory(model);
            return View(model);
        }

        public ActionResult AddEditProduct(int id = 0)
        {
            CustomMethods.ValidateRoles("Product");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            ProductWebModel ProdModel = new ProductWebModel();
            if (id != 0)
            {
                var objpro = new ProductBLL { }.GetProductById(id);
                if (objpro != null)
                {
                    ProdModel.SubCategoryId = objpro.SubCategoryId;
                    ProdModel.ProductName = objpro.ProductName;
                    ProdModel.CategoryId = objpro.CategoryId;          //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    ProdModel.ProductId = objpro.ProductId;
                    ProdModel.ProdDetails = objpro.ProdDetails;
                    ProdModel.IsActive = Convert.ToBoolean(objpro.IsActive);
                }
            }
            CustomMethods.BindParentCategory(ProdModel);
            CustomMethods.BindSubCategory(ProdModel);
            return View(ProdModel);
        }

        [HttpPost]
        public ActionResult AddEditProduct(ProductWebModel model, IEnumerable<HttpPostedFileBase> file)
        {
            Random rnd = new Random();
            string strImgName = "";
            string path = "";
            try
            {
                if (model.ProductId == 0)
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
                    if (model.ProductId == 0)
                    {
                        var checkDuplicate = objdb.Products.Where(x => x.ProductName == model.ProductName && x.CategoryId == model.CategoryId && x.SubCategoryId == model.SubCategoryId).SingleOrDefault();
                        if (checkDuplicate == null)
                        {
                            int res = new ProductBLL { }.AddEditProduct(new ProductModel
                            {
                                SubCategoryId = model.SubCategoryId,
                                ProductName = model.ProductName,
                                ProdDetails = model.ProdDetails,
                                CategoryId = model.CategoryId,
                                ProductId = model.ProductId,
                                IsActive = model.IsActive,
                                CreatedDate = model.CreatedDate,
                            });
                            if (res != 0)
                            {
                                foreach (var item in file)
                                {
                                    if (item != null)
                                    {
                                        ProductImageModel objProductImage = new ProductImageModel();
                                        string sname = item.FileName;
                                        strImgName = "ProImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                                        path = Server.MapPath("~/Images/ProductImages/" + strImgName);
                                        objProductImage.ProdImage = strImgName;
                                        objProductImage.CreatedDate = DateTime.Now;
                                        objProductImage.IsActive = true;
                                        objProductImage.ProductId = res;
                                        int result = new ProductBLL { }.InsertProductImagesBLL(objProductImage);
                                        item.SaveAs(path);
                                    }
                                }
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("Products");
                            }
                            Session["Error"] = "An Error has occured";
                            CustomMethods.BindParentCategory(model);
                            CustomMethods.BindSubCategory(model);
                            return View(model);
                        }
                    }
                    else
                    {
                        int data = new ProductBLL { }.AddEditProduct(new ProductModel
                        {
                            SubCategoryId = model.SubCategoryId,
                            ProductName = model.ProductName,
                            ProdDetails = model.ProdDetails,
                            CategoryId = model.CategoryId,
                            ProductId = model.ProductId,
                            IsActive = model.IsActive,
                            CreatedDate = model.CreatedDate,
                        });
                        if (data != 0)
                        {
                            foreach (var item in file)
                            {
                                if (item != null)
                                {
                                    ProductImageModel objProductImage = new ProductImageModel();
                                    string sname = item.FileName;
                                    strImgName = "ProImg" + rnd.Next(10000, 10000000) + "." + "jpg";
                                    path = Server.MapPath("~/Areas/Admin/ProjectImages/ProductImage/" + strImgName);
                                    objProductImage.ProdImage = strImgName;
                                    objProductImage.CreatedDate = DateTime.Now;
                                    objProductImage.IsActive = true;
                                    objProductImage.ProductId = data;
                                    int result = new ProductBLL { }.InsertProductImagesBLL(objProductImage);
                                    item.SaveAs(path);
                                }
                                else
                                {
                                    Session["Success"] = "Successfully Updated The Record";
                                    return RedirectToAction("Products");
                                }
                            }
                            Session["Success"] = "Successfully Added The Record";
                            return RedirectToAction("Products");
                        }
                    }
                    Session["Error"] = "Record Already Exists";
                    return RedirectToAction("AddEditProduct");
                }
                Session["Error"] = "An Error has occured";
                CustomMethods.BindParentCategory(model);
                CustomMethods.BindSubCategory(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                CustomMethods.BindParentCategory(model);
                CustomMethods.BindSubCategory(model);
                return View(model);
                throw;
            }
        }

        public ActionResult ViewProduct(int id = 0)
        {
            CustomMethods.ValidateRoles("Product");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            ProductWebModel prod = new ProductWebModel();
            if (id != 0)
            {
                var objprod = new ProductBLL { }.GetProductById(id);
                if (objprod != null)
                {
                    prod.SubCategoryId = objprod.SubCategoryId;
                    prod.SubCategoryName = objprod.SubCategoryName;
                    prod.CategoryId = objprod.CategoryId;  //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    prod.CategoryName = objprod.CategoryName;
                    prod.ProductId = objprod.ProductId;  //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    prod.ProductName = objprod.ProductName;  //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    prod.ProdDetails = objprod.ProdDetails;  //== null ? 0 : Convert.ToInt32(objcourse.CountryId);
                    prod.IsActive = Convert.ToBoolean(objprod.IsActive);

                    // ProductImageWebModel prodimage = new ProductImageWebModel();
                     //if (id != 0)
                     //{
                         var objacplan = new ProductImageBLL { }.GetProductImageById(id);
                         double count = Convert.ToDouble(objacplan.Count);
                         prod.ProductImages = objacplan.Select(x => new ProductImageWebModel
                         {
                             ProductId = x.ProductId,
                             ProdImage = x.ProdImage,
                             ProductImgId = x.ProductImageId,
                             CreatedDate = Convert.ToDateTime(x.CreatedDate),
                             IsActive = Convert.ToBoolean(x.IsActive)
                         }).ToList();
                     //}
                }
            }
            //CustomMethods.BindProduct(prod);
            //CustomMethods.BindSubategory(prod);
            //CustomMethods.BindSubCategory(prod);
            return View(prod);
        }

        public ActionResult GetSubCategory(int id)
        {
            try
            {
                var List = new SubCategoryBLL { }.GetAllSubCategory().Where(x => x.ParentCategoryId == id).Select(x => new SubCategoryModel
                {
                    SubCategoryId = x.SubCategoryId,
                    SubCatgoryName = x.SubCatgoryName,
                }).ToList();
                return Json(List);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public JsonResult ChangeProductStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new ProductBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }

        public ActionResult SearchMember(string searchValue)
        {
            int userid = Convert.ToInt32(Session["UserId"]);
            SearchwordsModel searchObj = new SearcbyKeywordBLL { }.SearchMemberByProduct(searchValue);
            return View(searchObj);
        }

        #endregion


        #region ProductImages
        public ActionResult ProductImages(int pid = 0, int cid = 0)
        {
            int take = 10;
            int skip = take * pid;
            ProductImageWebModel model = new ProductImageWebModel();
            model.PageID = pid;
            model.Current = pid + 1;
            IEnumerable<ProductImageWebModel> prodimages = new List<ProductImageWebModel>();
            CustomMethods.ValidateRoles("ProductImages");
            var ProdImagelist = new ProductImageBLL { }.GetAllProductImages(skip, take);


            model.ProductImage = new List<SelectListItem> { new SelectListItem { Text = "Select", Value = "0" } };
            if (cid != 0)
            {
                var sortedlist = new ProductImageBLL { }.GetAllProductImages(skip, take, cid);
                double count = Convert.ToDouble(sortedlist.Count);
                var res = count / take;
                model.Pagecount = (int)Math.Ceiling(res);
                model.ProductImages = sortedlist.Select(x => new ProductImageWebModel
                {
                    ProductId = x.ProductId,
                    ProdImage = x.ProdImage,
                    ProductImgId = x.ProductImageId,
                    CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();
            }
            else
            {
                if (ProdImagelist != null)
                {
                    double count = Convert.ToDouble(new ProductImageBLL { }.GetPageCount());
                    var res = count / take;
                    model.Pagecount = (int)Math.Ceiling(res);
                    model.ProductImages = ProdImagelist.Select(x => new ProductImageWebModel
                    {
                        ProductId = x.ProductId,
                        ProdImage = x.ProdImage,
                        ProductImgId = x.ProductImageId,
                        //CreatedBy = Convert.ToInt32(x.CreatedBy),
                        CreatedDate = Convert.ToDateTime(x.CreatedDate),
                        IsActive = Convert.ToBoolean(x.IsActive)
                    }).ToList();
                }
            }
            CustomMethods.BindProduct(model);
            return View(model);
        }

        //public ActionResult AddEditProductImages(int id = 0)
        //{
        //    CustomMethods.ValidateRoles("ProductImages");
        //    if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
        //        return View("ErrorPage", "Error");
        //    ProductImageWebModel ProdImageModel = new ProductImageWebModel();
        //    if (id != 0)
        //    {
        //        var objprodimage = new ProductImageBLL { }.GetProductImageById(id);
        //        if (objprodimage != null)
        //        {
        //            ProdImageModel.ProductImgId = objprodimage.ProductImageId;
        //            ProdImageModel.ProdImage = objprodimage.ProdImage;
        //            ProdImageModel.ProductId = objprodimage.ProductId;
        //            ProdImageModel.IsActive = Convert.ToBoolean(objprodimage.IsActive);
        //        }
        //    }
        //    CustomMethods.BindProduct(ProdImageModel);
        //    return View(ProdImageModel);
        //}

        [HttpPost]
        public ActionResult AddEditProductImages(ProductImageWebModel model, HttpPostedFileBase file)
        {
            try
            {
                if (model.ProductId == 0)
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
                    if (model.ProductImgId == 0)
                    {
                        if (file != null)
                        {
                            strFileName = "ProductImage_" + rnd.Next(100, 100000000) + "." + file.FileName.Split('.')[1].ToString();
                            path = Server.MapPath("~/Areas/Admin/ProjectImages/ProductImage/" + strFileName);
                            res = new ProductImageBLL { }.AddEditProductImages(new ProductImageModel
                            {
                                ProductImageId = model.ProductImgId,
                                ProductId = model.ProductId,
                                IsActive = model.IsActive,
                                ProdImage = file == null ? model.ProdImage : strFileName,
                                CreatedDate = model.CreatedDate,
                                //CreatedBy = Convert.ToInt32(Session["AdminUserId"]),
                            });
                            file.SaveAs(path);
                            if (res != 0)
                            {
                                Session["Success"] = "Successfully Added The Record";
                                return RedirectToAction("ProductImages");
                            }
                        }
                    }
                    else
                    {
                        res = new ProductImageBLL { }.AddEditProductImages(new ProductImageModel
                        {
                            ProductImageId = model.ProductImgId,
                            ProductId = model.ProductId,
                            IsActive = model.IsActive,
                            ProdImage = file == null ? model.ProdImage : strFileName,
                            CreatedDate = model.CreatedDate,
                        });
                    }

                    if (res != 0)
                    {
                        Session["Success"] = "Successfully Added The Record";
                        return RedirectToAction("ProductImages");
                    }
                }
                Session["Error"] = "An Error has occured";
                CustomMethods.BindProduct(model);
                return View(model);
            }
            catch (Exception)
            {
                Session["Error"] = "An Error has occured";
                CustomMethods.BindProduct(model);
                return View(model);
                throw;
            }
        }

        public ActionResult ViewProductImages(int id = 0)
        {
            CustomMethods.ValidateRoles("ProductImages");
            if (!Convert.ToBoolean(Session["Add"]) && !Convert.ToBoolean(Session["Edit"]))
                return View("ErrorPage", "Error");
            ProductImageWebModel prodimage = new ProductImageWebModel();
            if (id != 0)
            {
                var objacplan = new ProductImageBLL { }.GetProductImageById(id);
                double count = Convert.ToDouble(objacplan.Count);
                prodimage.ProductImages = objacplan.Select(x => new ProductImageWebModel
                {
                    ProductId = x.ProductId,
                    ProdImage = x.ProdImage,
                    ProductImgId = x.ProductImageId,
                    CreatedDate = Convert.ToDateTime(x.CreatedDate),
                    IsActive = Convert.ToBoolean(x.IsActive)
                }).ToList();

                //if (objacplan != null)
                //{
                //    prodimage.ProductImgId = objacplan.ProductImageId;
                //    prodimage.ProductId = objacplan.ProductId;
                //    prodimage.ProdImage = objacplan.ProdImage;
                //    prodimage.IsActive = Convert.ToBoolean(objacplan.IsActive);
                //}
            }
            CustomMethods.BindProduct(prodimage);
            return View(prodimage);
        }

        public JsonResult ChangeProductImageStatus(int id)
        {
            bool Result = false;
            bool ChangeStatus = new ProductImageBLL { }.ChangeStatus(id);
            if (ChangeStatus)
            {
                Result = true;
            }
            return Json(Result);
        }
        #endregion


    }
}
