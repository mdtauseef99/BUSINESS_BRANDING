using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BizzBranding.CommonUtility;

namespace BizzBranding.DAL
{
   public class PaymentTransationDAL
    {
       BizzBrandingEntities objdb = new BizzBrandingEntities();
       public bool InsertPaymentTransactionDAL(PaymentTransactionModel objmodel)
       {
           bool Result = false;
           try
           {
              
               PaymentTransationDetail objPayment = new PaymentTransationDetail();
               objPayment.Amount = objmodel.Amount;
               objPayment.CreatedDate = DateTime.Now;
               objPayment.TransationId = objmodel.TransationId;
               objPayment.UserId = objmodel.UserId;
               objPayment.TransationStatus = objmodel.TransationStatus;
               objdb.PaymentTransationDetails.Add(objPayment);
               objdb.SaveChanges();
               
               return Result=true;
           }
           catch (Exception)
           {
               return Result;
               throw;
           }
       }

       public bool UpdatePaymentTransactionByIdDAL(PaymentTransactionModel objmodel)
       {
           bool Result = false;
           try
           {
               var Payment = objdb.PaymentTransationDetails.Where(x => x.TransationId == objmodel.TransationId).SingleOrDefault();
               if (Payment!=null)
               {

                   Payment.TransationStatus = objmodel.TransationStatus;
                   Payment.MessageFromGateway = objmodel.MessageFromGateway;
                   Payment.PaymentReturnId = objmodel.PaymentReturnId;
                   Payment.UpdatedDate = DateTime.Now;
                   objdb.SaveChanges();
               }

               return Result = false;
           }
           catch (Exception)
           {
               return Result;
               throw;
           }
       }

       public List<PaymentTransactionModel> TransactionDetails()
       {
           return objdb.PaymentTransationDetails.Select(x => new PaymentTransactionModel
           {
               TransationId=x.TransationId,
               Amount=x.Amount,
               PaymentReturnId=x.PaymentReturnId,
               TransationStatus=x.TransationStatus,
               MessageFromGateway=x.MessageFromGateway,
               CreatedDate=x.CreatedDate
           }).ToList();

       }

       public List<PaymentTransactionModel> GetAllTransactionDetails(int skip, int take)
       {
           try
           {
               return objdb.PaymentTransationDetails.Select(x => new PaymentTransactionModel
               {
                   PaymentTransationId=x.PaymentTransationId,
                   TransationId = x.TransationId,
                   Amount = x.Amount,
                   PaymentReturnId = x.PaymentReturnId,
                   TransationStatus = x.TransationStatus,
                   MessageFromGateway = x.MessageFromGateway,
                   CreatedDate = x.CreatedDate
               }).OrderBy(x => x.PaymentTransationId).Skip(skip).Take(take).ToList();
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
               return objdb.PaymentTransationDetails.Select(x => x.PaymentTransationId).Count();
           }
           catch (Exception)
           {
               return 0;
               throw;
           }
       }

       public DateTime BannerAvailabilityDAL()
       {
           DateTime res =Convert.ToDateTime("1980-01-01");
           try
           {
               var getDate = (from d in objdb.HomeBannerMappings select d.ValidityEnd).Max();
               if (getDate == null)
               {
                   return res = DateTime.Now;
               }
               else
               {
                   return res = Convert.ToDateTime(getDate);
               }
           }
           catch (Exception)
           {
               return res;
               throw;
           }
       }

       public DateTime TargerBrandAdImageDAL()
       {
           DateTime res = Convert.ToDateTime("1980-01-01");
           try
           {
               var getDate = (from d in objdb.TargetBrandings select d.ExpiresOn).Max();
               if (getDate == null)
               {
                   return res = DateTime.Now;
               }
               else
               {
                   return res = Convert.ToDateTime(getDate);
               }
           }
           catch (Exception)
           {
               return res;
               throw;
           }
       }

       public DateTime NewsAvailabilityDAL()
       {
           DateTime res = Convert.ToDateTime("1980-01-01");
           try
           {
               var getDate = (from d in objdb.NewsMappings select d.ValidityEnd).Max();
               if (getDate==null)
               {
                   return res = DateTime.Now;
               }
               else
               {
                   return res = Convert.ToDateTime(getDate);
               }
               
           }
           catch (Exception)
           {
               return res;
               throw;
           }
       }
    }
}
