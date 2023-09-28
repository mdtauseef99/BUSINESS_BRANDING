using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
   public class CustomerEnquiryDAL
    {
        BizzBrandingEntities objdb = new BizzBrandingEntities();

        public int AddEditCustomerEnquiry(CustomerEnquiriesModel model)
        {
            try
            {
                if (model.ContactId == 0 && model.LoggedInUserId==0)
                {
                    CustomerEnquiry obj = new CustomerEnquiry
                    {
                        Name = model.CustomerName,
                        EmailId = model.CustEmailId,
                        Subject = model.CustSubject,
                        Enquiry = model.CustEnquiry,
                        Phone=model.CustomerPhone,
                    };
                    objdb.CustomerEnquiries.Add(obj);
                    objdb.SaveChanges();
                    return obj.ContactId;
                }
                else if (model.LoggedInUserId>0)
                {
                    CustomerEnquiry obj = new CustomerEnquiry
                    {
                        Name = model.CustomerName,
                        EmailId = model.CustEmailId,
                        Subject = model.CustSubject,
                        Enquiry = model.CustEnquiry,
                        Phone = model.CustomerPhone,
                        LoginUserId=model.LoggedInUserId
                    };
                    objdb.CustomerEnquiries.Add(obj);
                    objdb.SaveChanges();
                    return obj.ContactId;
                }
                else
                {
                    int id = model.ContactId;
                    CustomerEnquiry obj = objdb.CustomerEnquiries.Find(id);
                    if (obj != null)
                    {
                        obj.Name = model.CustomerName;
                        obj.EmailId = model.CustEmailId;
                        obj.Enquiry = model.CustEnquiry;
                        obj.Subject = model.CustSubject;
                        obj.Phone = model.CustomerPhone;
                        obj.LoginUserId = model.LoggedInUserId;

                        objdb.SaveChanges();
                        return obj.ContactId;
                    }
                    return 0;
                }
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public List<CustomerEnquiriesModel> GetAllCustomerEnquiry()
        {
            try
            {
                return objdb.CustomerEnquiries.Select(x => new CustomerEnquiriesModel
                {
                    CustomerName = x.Name,
                    CustEmailId = x.EmailId,
                    CustSubject = x.Subject,
                    LoggedInUserId = x.LoginUserId,
                    CustEnquiry=x.Enquiry,
                    CustomerPhone = x.Phone,
                }).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public List<CustomerEnquiriesModel> GetAllCustomerEnquiry(int skip, int take)
        {
            try
            {
                return objdb.CustomerEnquiries.Select(x => new CustomerEnquiriesModel
                {
                    ContactId=x.ContactId,
                    CustomerName = x.Name,
                    CustEmailId = x.EmailId,
                    CustSubject = x.Subject,
                    LoggedInUserId = x.LoginUserId,
                    CustEnquiry=x.Enquiry,
                    CustomerPhone = x.Phone,
                }).OrderByDescending(x =>x.ContactId).Skip(skip).Take(take).ToList();
            }
            catch (Exception)
            {
                return null;
                throw;
            }
        }

        public CustomerEnquiriesModel GetCustomerEnquiryById(int id)
        {
            try
            {
                return objdb.CustomerEnquiries.Where(x => x.ContactId == id).Select(x => new CustomerEnquiriesModel
                {
                    CustomerName = x.Name,
                    CustEmailId = x.EmailId,
                    CustSubject = x.Subject,
                    LoggedInUserId = x.LoginUserId,
                    CustEnquiry=x.Enquiry,
                    CustomerPhone = x.Phone,
                    ContactId=x.ContactId,
                }).SingleOrDefault();
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
                return objdb.CustomerEnquiries.Select(x => x.ContactId).Count();
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        public int Remove(int id)
        {
            try
            {
                var obj = objdb.CustomerEnquiries.Find(id);
                if (obj != null)
                {
                    objdb.CustomerEnquiries.Remove(obj);
                    objdb.SaveChanges();
                }
                return obj.ContactId;
            }
            catch (Exception)
            {
                return 0;
                throw;
            }
        }

        //public bool ChangeStatus(int id)
        //{
        //    try
        //    {
        //        var obj = objdb.CustomerEnquiries.Find(id);
        //        if (obj != null && obj.IsActive == true)
        //        {
        //            obj.IsActive = false;
        //            objdb.SaveChanges();
        //            return false;
        //        }
        //        else if (obj != null)
        //        {
        //            obj.IsActive = true;
        //            objdb.SaveChanges();
        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }

        //}
    }
}
