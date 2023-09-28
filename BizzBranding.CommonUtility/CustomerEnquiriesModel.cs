using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
  public  class CustomerEnquiriesModel
    {

        public int ContactId { get; set; }

      [Required(ErrorMessage="Enter Name")]
        public string CustomerName { get; set; }

      [Required(ErrorMessage = "Email is required")]
      [RegularExpression(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*", ErrorMessage = "Email is not valid")]
      public string CustEmailId { get; set; }

       [Required(ErrorMessage = "Enter Phone")]
      public string CustomerPhone { get; set; }

       [Required(ErrorMessage = "Enter Subject")]
       public string CustSubject { get; set; }

       [Required(ErrorMessage = "Enter Your Enquiry")]
       public string CustEnquiry { get; set; }

       public int? LoggedInUserId { get; set; }

       public int Pagecount { get; set; }
       public int PageID { get; set; }
       public int Current { get; set; }

       public Nullable<System.DateTime> CreatedDate { get; set; }
       public Nullable<System.DateTime> UpdatedDate { get; set; }

        public List<CustomerEnquiriesModel> CustomerEnquiryList { get; set; }

    }
}
