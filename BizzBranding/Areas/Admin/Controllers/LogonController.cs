using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data.Entity;
using BizzBranding.Areas.Admin.Models;
using BizzBranding.CommonUtility;
using BizzBranding.BLL;
using BizzBranding.Areas.Admin.Filters;
using BizzBranding.Filters;


namespace BizzBranding.Areas.Admin.Controllers
{
     [CustumAuthorize.AllowAnonymous]
    public class LogonController : Controller
    {
        //
        // GET: /Admin/Logon/

        public ActionResult Index(string returnurl = "")
        {
            LogonModel model = new LogonModel
            {
                Returnurl = returnurl,
            };

            return View(model);
        }

        public ActionResult Login(LogonModel logonmodel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AdministratorBLL objadministratorbll = new AdministratorBLL();
                    AdministratorModel objadministrator = objadministratorbll.AdminLogin(new AdministratorModel
                    {
                        UserName = logonmodel.UserName,
                        Password = DataEncryption.Encrypt(logonmodel.Password.Trim(), "passKey")
                    });
                    if (objadministrator != null)
                    {
                        Session["AdminUserId"] = objadministrator.AdminId;
                        Session["UserName"] = objadministrator.UserName;
                        Session["RoleId"] = objadministrator.RoleId;
                        //Session.Timeout = 60;
                        if (logonmodel.Returnurl != null)
                        {
                            return Redirect(logonmodel.Returnurl);
                        }
                        return RedirectToAction("Category", "MasterData");
                    }
                }
                Session["Error"] = "Invalid User name or Password.";
                return View("Index", logonmodel);
            }
            catch (Exception)
            {
                Session["Error"] = "Invalid User name or Password.";
                return View("Index", logonmodel);
                throw;
            }
        }

        public ActionResult Logout()
        {
            try
            {
                Session["AdminUserId"] = string.Empty;
                Session.Abandon();

                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                return RedirectToAction("Index");
                throw;
            }
        }

        public ActionResult Header()
        {

            List<RolemanagementWebModel> objrole = new List<RolemanagementWebModel>();

            var rolelist = new RolesBLL { }.GetRoleById(Convert.ToInt32(Session["RoleId"]));

            if (rolelist != null)
            {
                foreach (var item in rolelist.Rolemanages)
                {
                    var role = new RolemanagementWebModel();
                    role.Add = Convert.ToBoolean(item.Add);
                    role.Edit = Convert.ToBoolean(item.Edit);
                    role.Delete = Convert.ToBoolean(item.Delete);
                    role.View = Convert.ToBoolean(item.View);
                    role.PageID = Convert.ToInt32(item.PageID);
                    role.RoleID = Convert.ToInt32(item.RoleID);
                    objrole.Add(role);
                }
            }

            return PartialView("_Header", objrole);
        }

    }
}
