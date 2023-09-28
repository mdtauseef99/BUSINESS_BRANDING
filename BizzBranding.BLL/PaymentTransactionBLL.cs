using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.DAL;
using BizzBranding.CommonUtility;

namespace BizzBranding.BLL
{
    public class PaymentTransactionBLL
    {
        PaymentTransationDAL objpayDal = new PaymentTransationDAL();
        public bool InsertPaymentTransactionBLL(PaymentTransactionModel objmodel)
        {
            return objpayDal.InsertPaymentTransactionDAL(objmodel);
        }

        public bool UpdatePaymentTransactionByIdBLL(PaymentTransactionModel objmodel)
        {
            return objpayDal.UpdatePaymentTransactionByIdDAL(objmodel);
        }

        public List<PaymentTransactionModel> TransactionDetails()
        {
            return objpayDal.TransactionDetails();
        }

        public List<PaymentTransactionModel> GetAllTransactionDetails(int skip, int take)
        {
            try
            {
                return objpayDal.GetAllTransactionDetails(skip, take);
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
                return objpayDal.GetPageCount();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public DateTime BannerAvailabilityBLL()
        {
            return objpayDal.BannerAvailabilityDAL();
        }

        public DateTime TargerBrandAdImageBLL()
        {
            return objpayDal.TargerBrandAdImageDAL();
        }

        public DateTime NewsAvailabilityBLL()
        {
            return objpayDal.NewsAvailabilityDAL();
        }

    }
}
