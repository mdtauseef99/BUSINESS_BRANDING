using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;
using BizzBranding.DAL;

namespace BizzBranding.BLL
{
    public class BussinessUserBLL
    {
        BussinessUserDAL objuserdal = new BussinessUserDAL();

        public List<BussinessUserModel> GetAllUsers(int skip, int take)
        {
            try
            {
                return objuserdal.GetAllUsers(skip, take);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<BussinessUserModel> GetAllUsers()
        {
            try
            {
                return objuserdal.GetAllUsers();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

       public List<TargetBranding> GetAllAdsHome()
        {
            return objuserdal.GetAllAdsHome();
        }

       public List<TargetImageModel> GetAllAdsHomebyUserId(int id)
        {
            return objuserdal.GetAllAdsHomebyUserId(id);
        } 

        public int AddEditUser(BussinessUserModel model)
        {
            try
            {
                return objuserdal.AddEditUser(model);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int AddUserIndutryMapping(UserIndustryMappingModel model)
        {
            try
            {
                return objuserdal.AddUserindustryMapping(model);
            }
            catch (Exception)
            {

                return 0;
                throw;
            }
        }

        public int EditUser(BussinessUserModel model)
        {
            try
            {
                return objuserdal.EditUser(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int DeleteUser(int id)
        {
            try
            {
                return objuserdal.DeleteUser(id);
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int ChangePassword(BussinessUserModel model)
        {
            try
            {
                return objuserdal.ChangePassword(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int ChangeEmail(BussinessUserModel model)
        {
            try
            {
                return objuserdal.ChangeEmail(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int UpdateUserByFB(BussinessUserModel model)
        {
            try
            {
                return objuserdal.UpdateUserByFB(model);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BussinessUserModel GetUserById(int id)
        {
            try
            {
                return objuserdal.GetUserById(id);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }



        public BussinessUserModel GetUserByEmailId(string emailid)
        {
            try
            {
                return objuserdal.GetUserByEmailId(emailid);
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
                return objuserdal.ChangeStatus(id);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public BussinessUserModel UserLogin(string email, string pass)
        {
            try
            {
                return objuserdal.UserLogin(email, pass);
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
                return objuserdal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BussinessUserModel IsEmailExists(string email)
        {
            try
            {
                return objuserdal.IsEmailExists(email);
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public int VisitcountBLL(int UserId, string Ip)
        {
            return objuserdal.Visitcount(UserId, Ip);
        }

        public BussinessUser GetBusinessUserDetailsBLL(int UserID)
        {
            return objuserdal.GetBusinessUserDetailsDAL(UserID);
        }

        public bool CheckDuplicate(string emailId)
        {
            try
            {
                return objuserdal.CheckDuplicate(emailId);
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }




















        //public UserModel GetUserBySocialID(string type, string id)
        //{
        //    try
        //    {
        //        return objuserdal.GetUserBySocialID(type, id);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //        throw;
        //    }
        //}

        //public int AddSocialUser(UserModel model, string type)
        //{
        //    try
        //    {
        //        return objuserdal.AddSocialUser(model, type);
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //        throw;
        //    }
        //}

    }
}
