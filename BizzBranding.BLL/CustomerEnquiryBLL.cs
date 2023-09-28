using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
   public class CustomerEnquiryBLL
    {
       CustomerEnquiryDAL objdal = new CustomerEnquiryDAL();

       public int AddEditCustomerEnquiry(CustomerEnquiriesModel model)
        {
            try
            {
                return objdal.AddEditCustomerEnquiry(model);
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
                return objdal.GetAllCustomerEnquiry();
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
                return objdal.GetAllCustomerEnquiry(skip, take);
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
                return objdal.GetCustomerEnquiryById(id);
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
                return objdal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int Remove(int id)
        {
            try
            {
                return objdal.Remove(id);
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
        //        return objdal.ChangeStatus(id);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //        throw;
        //    }
        //}
    }
}
