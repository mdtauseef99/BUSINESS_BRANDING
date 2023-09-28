using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
    public class BussinessUserDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public List<BussinessUserModel> GetAllUsers(int skip, int take)
        {
            try
            {

                return objdb.BussinessUsers.Select(x => new BussinessUserModel
                {
                    UserId = x.UserId,
                    CountryId = x.CountryId,
                    CityId = x.CityId,
                    GUId = x.GUId,
                    //UserName = x.UserName,
                    BusinessName = x.BusinessName,
                    EmailId = x.EmailId,
                    IsActive = x.IsActive,
                    CreatedDate = x.CreatedDate,
                }).OrderByDescending(x => x.UserId).Skip(skip).Take(take).ToList();
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
                return objdb.BussinessUsers.Select(x => x.UserId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<BussinessUserModel> GetAllUsers()
        {
            try
            {
                return objdb.BussinessUsers.Select(x => new BussinessUserModel
                {
                    UserId = x.UserId,
                    CountryId = x.CountryId,
                    CityId = x.CityId,
                    GUId = x.GUId,
                    //UserName = x.UserName,
                    BusinessName = x.BusinessName,
                    EmailId = x.EmailId,
                    //UserType = x.UserType.UserTypeName,
                    CreatedDate = x.CreatedDate,
                    IsActive = x.IsActive,
                    CompURL=x.URL
                }).OrderByDescending(x => x.UserId).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<TargetBranding> GetAllAdsHome()
        {
            var homead = objdb.TargetedImages.Where(m => m.IsActive == true).ToList();
            var totaluseradsathome = new List<TargetBranding>();
            foreach (var TargetImage in homead)
            {
                var userathome =
                    objdb.TargetedImages.Where(s => s.TargetBrandingId == TargetImage.TargetBrandingId && s.IsActive == true).ToList();
                if (userathome.Count > 0)
                {
                    foreach (var uah in userathome)
                    {
                        var userad = objdb.TargetBrandings.Where(u => u.IndustryId == uah.TargetBranding.IndustryId || u.CityId == uah.TargetBranding.CityId && u.IsActive == true).ToList();
                        totaluseradsathome.AddRange(userad);
                    }
                }
            }
            return totaluseradsathome;
        }

        public List<TargetImageModel> GetAllAdsHomebyUserId(int id)
        {
            var userdata = new BussinessUserDAL { }.GetUserById(id);
            var totaluseradsathome = new List<TargetImageModel>();
            if (userdata != null)
            {
                //var homead = objdb.TargetedImages.Where(m => m.IsActive == true).ToList();
                //foreach (var TargetImage in homead)
                //{
                totaluseradsathome = objdb.TargetedImages.Where(s => s.TargetBranding.IndustryId == userdata.IndustryId && s.TargetBranding.IsActive==true || s.TargetBranding.CityId == userdata.CityId && s.TargetBranding.IsActive== true).Select(x => new TargetImageModel { Image = x.Image,
                URL=x.TargetBranding.URL
                }).ToList();
                    //if (userathome.Count > 0)
                    //{
                    //    foreach (var uah in userathome)
                    //    {
                    //        var userad = objdb.TargetedImages.Where(u => u.TargetBranding.IndustryId == userdata.IndustryId || u.TargetBranding.CityId == userdata.CityId && u.IsActive == true).ToList();
                    //        totaluseradsathome.AddRange(userad);
                    //    }
                    //}
                //}
            }
            return totaluseradsathome;
        }

        public int AddEditUser(BussinessUserModel model)
        {
            try
            {
                if (model.UserId == 0)
                {
                    BussinessUser objuser = new BussinessUser
                    {
                        BusinessName = model.BusinessName,
                        EmailId = model.EmailId,
                        Password = model.Password,
                        CompanyLogo = model.CompanyLogo,
                        BannerImage = model.CompanyBanner,
                        CoRegNo = model.CoRegNo,
                        CoAddress = model.CoAddress,
                        Phone = model.Phone,
                        Fax = model.Fax,
                        ContactPerson = model.ContactPerson,
                        Designation = model.Designation,
                        TradeEmaiIId = model.TradeEmaiIId,
                        BusinessDetails = model.BusinessDetails,
                        IndustryId = model.IndustryId,
                        CountryId = model.CountryId,
                        CityId = model.CityId,
                        IsActive = false,
                        CreatedDate = DateTime.Now,
                        GUId = model.GUId,
                        URL = model.CompURL,
                        IsGlobal = model.IsGlobal
                    };
                    objdb.BussinessUsers.Add(objuser);
                    objdb.SaveChanges();


                    #region Add to Membership

                    var AccessPlan = objdb.AccessPlans.Where(x => x.AccessPlanId == 19).FirstOrDefault();

                    if (AccessPlan != null)
                    {
                        Membership mem = new Membership();
                        mem.AccessPlanId = AccessPlan.AccessPlanId;
                        mem.UserId = objuser.UserId;
                        mem.ActivatedOn = DateTime.Now;

                        mem.ExpiresOn = DateTime.Now.AddDays(AccessPlan.Validity);
                        mem.IsActive = true;
                        mem.CreatedDate = DateTime.Now;
                        objdb.Memberships.Add(mem);
                        objdb.SaveChanges();
                    }
                    #endregion

                    return objuser.UserId;


                }
                else
                {
                    BussinessUser objuser = objdb.BussinessUsers.Find(model.UserId);
                    objuser.BusinessName = model.BusinessName;
                    objuser.EmailId = model.EmailId;
                    objuser.Password = model.Password;
                    if (model.CompanyLogo != null)
                    {
                        objuser.CompanyLogo = model.CompanyLogo;
                    }
                    if (model.CompanyBanner != null)
                    {
                        objuser.BannerImage = model.CompanyBanner;
                    }

                    objuser.CoRegNo = model.CoRegNo;
                    objuser.CoAddress = model.CoAddress;
                    objuser.Phone = model.Phone;
                    objuser.Fax = model.Fax;
                    objuser.ContactPerson = model.ContactPerson;
                    objuser.Designation = model.Designation;
                    objuser.TradeEmaiIId = model.TradeEmaiIId;
                    objuser.BusinessDetails = model.BusinessDetails;
                    objuser.IndustryId = model.IndustryId;
                    objuser.CountryId = model.CountryId;
                    objuser.URL = model.CompURL;
                    objuser.CityId = model.CityId;
                    objuser.IsActive = true;
                    // objuser.GUId = model.GUId;
                    objuser.IsGlobal = model.IsGlobal;
                    objdb.SaveChanges();
                    return objuser.UserId;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int AddUserindustryMapping(UserIndustryMappingModel model)
        {
            int result = 0;
            try
            {
                UserIndustryMapping objIndustry = new UserIndustryMapping();
                objIndustry.UserId = model.UserId;
                objIndustry.IndustryId = model.IndustryId;
                objdb.UserIndustryMappings.Add(objIndustry);
                objdb.SaveChanges();
                result = objIndustry.Id;
                return result;
                //return result;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int EditUser(BussinessUserModel model)
        {
            try
            {

                BussinessUser objuser = objdb.BussinessUsers.Find(model.UserId);
                {
                    objuser.UserId = model.UserId;
                    //objuser.UserName = model.UserName;
                    //objuser.CityId = model.CityId;
                };
                objdb.SaveChanges();
                return objuser.UserId;

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

                BussinessUser objuser = objdb.BussinessUsers.Find(model.UserId);
                {
                    objuser.UserId = model.UserId;
                    objuser.Password = model.Password;
                };
                objdb.SaveChanges();
                return objuser.UserId;

            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int ChangeEmail(BussinessUserModel model)
        {
            try
            {
                BussinessUser objuser = objdb.BussinessUsers.Find(model.UserId);
                {
                    objuser.UserId = model.UserId;
                    //objuser.UserName = model.UserName;
                    objuser.EmailId = model.EmailId;

                };
                objdb.SaveChanges();
                return objuser.UserId;

            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int DeleteUser(int id)
        {
            try
            {
                var obj = objdb.BussinessUsers.Find(id);
                if (obj != null)
                {
                    objdb.BussinessUsers.Remove(obj);
                    objdb.SaveChanges();
                }
                return obj.UserId;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int UpdateUserByFB(BussinessUserModel model)
        {
            try
            {
                BussinessUser objuser = objdb.BussinessUsers.Find(model.UserId);
                {
                    objuser.UserId = model.UserId;
                    //objuser.UserName = model.UserName;
                    objuser.EmailId = model.EmailId;
                    objuser.Password = model.Password;

                };
                objdb.SaveChanges();
                return objuser.UserId;

            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public BussinessUserModel GetUserById(int id)
        {
            try
            {
                return objdb.BussinessUsers.Where(x => x.UserId == id).Select(x => new BussinessUserModel
                {
                    UserId = x.UserId,
                    BusinessName = x.BusinessName,
                    EmailId = x.EmailId,
                    Password = x.Password,
                    CompanyLogo = x.CompanyLogo,
                    CompanyBanner = x.BannerImage,
                    CoRegNo = x.CoRegNo,
                    CoAddress = x.CoAddress,
                    Phone = x.Phone,
                    Fax = x.Fax,
                    ContactPerson = x.ContactPerson,
                    Designation = x.Designation,
                    TradeEmaiIId = x.TradeEmaiIId,
                    BusinessDetails = x.BusinessDetails,
                    IsActive = x.IsActive,
                    IndustryId = x.IndustryId,
                    CountryName = x.Country.CountryName,
                    CityName = x.City.CityName,
                    IndustryName = x.Industry.IndustryName,
                    CountryId = x.CountryId,
                    CityId = x.CityId,
                    CompURL=x.URL

                }).SingleOrDefault();
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
                return objdb.BussinessUsers.Where(x => x.EmailId == emailid).Select(x => new BussinessUserModel
                {
                    UserId = x.UserId,
                    //BusinessName = x.BusinessName,
                    FromBusinessName=x.BusinessName,
                    EmailId = x.EmailId,
                    Password = x.Password,
                    CompanyLogo = x.CompanyLogo,
                    CompanyBanner = x.BannerImage,
                    CoRegNo = x.CoRegNo,
                    CoAddress = x.CoAddress,
                    Phone = x.Phone,
                    Fax = x.Fax,
                    ContactPerson = x.ContactPerson,
                    Designation = x.Designation,
                    TradeEmaiIId = x.TradeEmaiIId,
                    BusinessDetails = x.BusinessDetails,
                    IsActive = x.IsActive,
                    IndustryId = x.IndustryId,
                    CountryName = x.Country.CountryName,
                    CityName = x.City.CityName,
                    IndustryName = x.Industry.IndustryName,
                    CountryId = x.CountryId,
                    CityId = x.CityId,
                    CompURL=x.URL

                }).FirstOrDefault();
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
                var obj = objdb.BussinessUsers.Find(id);
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

        public BussinessUserModel UserLogin(string email, string pass)
        {
            try
            {
                return objdb.BussinessUsers.Where(x => x.EmailId == email && x.Password == pass && x.IsActive == true).Select(x => new BussinessUserModel
                {
                    UserId = x.UserId,
                    ////UserName = x.UserName,
                    EmailId = x.EmailId,
                    BusinessName = x.BusinessName,
                    MemShipId = objdb.Memberships.Where(m => m.UserId == x.UserId).Select(j => j.AccessPlanId.HasValue ? j.AccessPlanId.Value : 0).FirstOrDefault(),
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public BussinessUserModel IsEmailExists(string email)
        {
            try
            {
                return objdb.BussinessUsers.Where(x => x.EmailId == email && x.IsActive == false).Select(x => new BussinessUserModel
                {
                    EmailId = x.EmailId,
                    Password = x.Password,
                    IsActive = x.IsActive
                }).SingleOrDefault();

            }
            catch (Exception)
            {
                return null;
            }
        }


        public int Visitcount(int UserId, string Ip)
        {
            try
            {
                int res = 0;
                var ipCheck = objdb.VisitorCounts.Where(x => x.UserId == UserId && x.IpAddress == Ip).FirstOrDefault();
                if (ipCheck == null)
                {
                    VisitorCount objVisit = new VisitorCount();
                    objVisit.IpAddress = Ip;
                    objVisit.UserId = UserId;
                    objdb.VisitorCounts.Add(objVisit);
                    objdb.SaveChanges();
                }
                res = objdb.VisitorCounts.Where(s => s.UserId == UserId).Count();
                return res;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public BussinessUser GetBusinessUserDetailsDAL(int UID)
        {
            var objBUser = objdb.BussinessUsers.Where(x => x.UserId == UID).SingleOrDefault();
            return objBUser;
        }

        public bool CheckDuplicate(string emailId)
        {
            bool res = false;

            try
            {
                var obj = objdb.BussinessUsers.Where(x => x.EmailId == emailId).FirstOrDefault();

                if (obj != null)
                {
                    res = true;
                }
                return res;
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
        //        UserModel objuser = new UserModel();
        //        switch (type)
        //        {
        //            case "Facebook":
        //                objuser = objdb.Users.Where(x => x.FacebookID == id).Select(x => new UserModel
        //                {
        //                    UserID = x.UserID,
        //                    UserName = x.UserName,
        //                    Email = x.Email,
        //                    Source = x.Source,
        //                    UserTypeID = x.UserTypeID,
        //                    IsActive = x.IsActive,
        //                    MembershipID = x.MembershipID,
        //                    FacebookID = x.FacebookID
        //                }).SingleOrDefault();
        //                break;
        //            case "Twitter":
        //                decimal tid = Convert.ToDecimal(id);
        //                objuser = objdb.Users.Where(x => x.TwitterID == tid).Select(x => new UserModel
        //                {
        //                    UserID = x.UserID,
        //                    UserName = x.UserName,
        //                    Email = x.Email,
        //                    Source = x.Source,
        //                    UserTypeID = x.UserTypeID,
        //                    IsActive = x.IsActive,
        //                    MembershipID = x.MembershipID,
        //                    TwitterID = x.TwitterID
        //                }).SingleOrDefault();
        //                break;
        //            case "LinkedIn":
        //                objuser = objdb.Users.Where(x => x.LinkedID == id).Select(x => new UserModel
        //                {
        //                    UserID = x.UserID,
        //                    UserName = x.UserName,
        //                    Email = x.Email,
        //                    Source = x.Source,
        //                    UserTypeID = x.UserTypeID,
        //                    IsActive = x.IsActive,
        //                    MembershipID = x.MembershipID,
        //                    LinkedID = x.LinkedID
        //                }).SingleOrDefault();
        //                break;
        //            case "Googleplus":
        //                objuser = objdb.Users.Where(x => x.GooglePlusID == id).Select(x => new UserModel
        //                {
        //                    UserID = x.UserID,
        //                    UserName = x.UserName,
        //                    Email = x.Email,
        //                    Source = x.Source,
        //                    UserTypeID = x.UserTypeID,
        //                    IsActive = x.IsActive,
        //                    MembershipID = x.MembershipID,
        //                    GooglePlusID = x.GooglePlusID
        //                }).SingleOrDefault();
        //                break;
        //        }
        //        return objuser;
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
        //        User objuser = new User();
        //        switch (type)
        //        {
        //            case "Facebook":
        //                objuser.FacebookID = model.FacebookID;
        //                break;
        //            case "Twitter":
        //                objuser.TwitterID = model.TwitterID;
        //                break;
        //            case "LinkedIn":
        //                objuser.LinkedID = model.LinkedID;
        //                break;
        //            case "Googleplus":
        //                objuser.GooglePlusID = model.GooglePlusID;
        //                break;
        //        }
        //        objuser.UserName = model.UserName;
        //        objuser.Email = model.Email;
        //        objuser.Password = model.Password;
        //        objuser.CreatedOn = DateTime.Now;
        //        objuser.MembershipID = model.MembershipID;
        //        objuser.IsActive = model.IsActive;
        //        objuser.UserTypeID = model.UserTypeID;
        //        objdb.Users.Add(objuser);
        //        objdb.SaveChanges();
        //        return objuser.UserID;
        //    }
        //    catch (Exception)
        //    {
        //        return 0;
        //        throw;
        //    }
        //}

    }

}
