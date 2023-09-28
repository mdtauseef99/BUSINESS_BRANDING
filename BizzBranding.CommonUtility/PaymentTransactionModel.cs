using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
   public class PaymentTransactionModel
    {
        public int PaymentTransationId { get; set; }
        public string TransationId { get; set; }
        public decimal? Amount { get; set; }
        public string PaymentReturnId { get; set; }
        public bool? TransationStatus { get; set; }
        public int? UserId { get; set; }
        public string MessageFromGateway { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public List<PaymentTransactionModel> PaymentList { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }

    }
}
