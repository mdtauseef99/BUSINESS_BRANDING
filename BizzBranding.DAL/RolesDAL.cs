using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class RolesDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<RoleModel> GetAllRoles()
        {
            try
            {
                return objdb.Roles.Where(x => x.IsActive == true).Select(x => new RoleModel
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName,
                    Description = x.Description,
                    IsActive = x.IsActive,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }


        public List<RoleModel> GetAllRoles(int skip, int take)
        {
            try
            {
                return objdb.Roles.Select(x => new RoleModel
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName,
                    Description = x.Description,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.RoleId).Skip(skip).Take(take).ToList();
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
                return objdb.Roles.Where(x => x.IsActive == true)
                            .Select(x => x.RoleId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int AddEditRole(RoleModel objmodel)
        {
            try
            {

                if (objmodel.RoleId != 0)
                {
                    var objrole = objdb.Roles.Find(objmodel.RoleId);
                    objrole.RoleName = objmodel.RoleName;
                    objrole.IsActive = true;
                    objdb.SaveChanges();

                    foreach (var item in objmodel.Rolemanages)
                    {
                        var obj = objdb.RoleAssignments.Where(x => x.RoleId == objrole.RoleId && x.PageId == item.PageID).SingleOrDefault();   //&& x.PageID == pageid
                        obj.AddRec = item.Add;
                        obj.EditRec = item.Edit;
                        obj.DeleteRec = item.Delete;
                        obj.ViewRec = item.View;
                        objdb.SaveChanges();
                    }

                    return objmodel.RoleId;
                }
                else
                {
                    Role objrole = new Role
                    {
                        IsActive = objmodel.IsActive,
                        RoleName = objmodel.RoleName,
                    };
                    objdb.Roles.Add(objrole);
                    objdb.SaveChanges();


                    foreach (var item in objmodel.Rolemanages)
                    {
                        RoleAssignment obj = new RoleAssignment
                        {
                            AddRec = item.Add,
                            EditRec = item.Edit,
                            DeleteRec = item.Delete,
                            PageId = item.PageID,

                            RoleId = objrole.RoleId,

                        };

                        objdb.RoleAssignments.Add(obj);
                        objdb.SaveChanges();
                    }
                    return objrole.RoleId;

                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public RoleModel GetRoleById(int id)
        {
            try
            {
                var res = objdb.Roles.Where(x => x.RoleId == id).Select(x => new RoleModel
                {
                    RoleId = x.RoleId,
                    RoleName = x.RoleName,
                    IsActive = x.IsActive,
                }).SingleOrDefault();

                res.Rolemanages = objdb.RoleAssignments.Where(x => x.RoleId == id).Select(x => new RolemanagementModel
                {
                    RoleID = x.RoleId,
                    Add = x.AddRec,
                    Edit = x.EditRec,
                    Delete = x.DeleteRec,
                    View = x.ViewRec,
                    PageID = x.PageId
                }).ToList();
                return res;
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public RolemanagementModel GetRoleManagement(int pageid, int roleid)
        {
            try
            {
                return objdb.RoleAssignments.Where(x => x.RoleId == roleid && x.PageId == pageid)
                    .Select(x => new RolemanagementModel
                    {
                        Add = x.AddRec,
                        Edit = x.EditRec,
                        Delete = x.DeleteRec,
                        View = x.ViewRec,
                        RoleID = x.RoleId,
                        PageID = x.PageId
                    }).SingleOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ChangeStatus(int id)
        {
            try
            {
                var obj = objdb.Roles.Find(id);
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
    }

}
