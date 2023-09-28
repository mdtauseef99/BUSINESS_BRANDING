using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
namespace BizzBranding.DAL
{
    public class AdministratorDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public AdministratorModel GetAdministratorById(int id)
        {
            try
            {
                return objdb.Administrators.Where(x => x.AdminId == id).Select(x => new AdministratorModel
                {
                    AdminId = x.AdminId,
                    FirstName = x.FirstName,
                    UserName = x.UserName,
                    LastName = x.LastName,
                    Password = x.Password,
                    Email = x.Email,
                    RoleId = x.RoleId,
                    IsActive = x.IsActive,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public AdministratorModel AdminLogin(AdministratorModel objmodel)
        {
            try
            {
                return objdb.Administrators.Where(x => x.UserName == objmodel.UserName && x.Password == objmodel.Password)
                            .Select(x => new AdministratorModel
                            {
                                AdminId = x.AdminId,
                                UserName = x.UserName,
                                Password = x.Password,
                                RoleId = x.RoleId
                            }).SingleOrDefault();
            }
            catch (Exception)
            {
                //return null;
                throw;
            }
        }

        public int AddUpdateAdministrator(AdministratorModel objmodel)
        {
            try
            {
                if (objmodel.AdminId != 0)
                {
                    var administrator = objdb.Administrators.Find(objmodel.AdminId);
                    administrator.FirstName = objmodel.FirstName;
                    administrator.LastName = objmodel.LastName;
                    administrator.UserName = objmodel.UserName;
                    administrator.Email = objmodel.Email;
                    administrator.RoleId = objmodel.RoleId;
                    administrator.Password = objmodel.Password;
                    administrator.IsActive = objmodel.IsActive;
                    objdb.SaveChanges();
                    return objmodel.AdminId;
                }
                else
                {
                    Administrator objadministrator = new Administrator
                    {
                        FirstName = objmodel.FirstName,
                        LastName = objmodel.LastName,
                        UserName = objmodel.UserName,
                        Email = objmodel.Email,
                        Password = objmodel.Password,
                        RoleId = objmodel.RoleId,
                        IsActive = objmodel.IsActive,
                    };

                    objdb.Administrators.Add(objadministrator);
                    objdb.SaveChanges();
                    return objadministrator.AdminId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        //public List<AdministratorModel> GetAllInstructorsWithImages()
        //{
        //    try
        //    {
        //        return objdb.Administrators.Where(x => x.InstructorImage != null).Select(x => new AdministratorModel
        //        {
        //            InstructorID = x.InstructorID,
        //            InstructorName = x.InstructorName,
        //            Description = x.Description,
        //            UserName = x.UserName,
        //            InstructorImage = x.InstructorImage,
        //            Email = x.Email,
        //            SubjectID = x.SubjectID,
        //            Subject = x.Subject.SubjectName,
        //            MembershipID = x.MembershipID,
        //        }).OrderByDescending(x => x.InstructorID).Take(5).ToList();
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //        throw;
        //    }
        //}

        public List<AdministratorModel> GetAllAdministrators()
        {
            try
            {
                return objdb.Administrators.Select(x => new AdministratorModel
                {
                    AdminId = x.AdminId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Password = x.Password,
                    Email = x.Email,
                    Role = x.Role.RoleName,
                }).OrderByDescending(x => x.AdminId).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<AdministratorModel> GetAllAdministrators(int skip, int take)
        {
            try
            {
                return objdb.Administrators.Select(x => new AdministratorModel
                {
                    AdminId = x.AdminId,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    UserName = x.UserName,
                    Password = x.Password,
                    Email = x.Email,
                    Role = x.Role.RoleName,
                    IsActive = x.IsActive,
                }).OrderByDescending(x => x.AdminId).Skip(skip).Take(take).ToList();
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
                return objdb.Administrators.Select(x => x.AdminId).Count();
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
                var obj = objdb.Administrators.Find(id);
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
