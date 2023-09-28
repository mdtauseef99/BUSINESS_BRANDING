using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BizzBranding.CommonUtility
{
   public class BusinessJobModel
    {
       public int BusinessJobId { get; set; }
       public int? BusinessUserId { get; set; }
       public string CompanyLogo { get; set; }

       [Required (ErrorMessage="Enter Job Title")]
       public string JobTitle { get; set; }

       [Required(ErrorMessage = "Enter Job Description")]
       public string Description { get; set; }
       public bool IsActive { get; set; }
       public int Pagecount { get; set; }
       public int PageID { get; set; }
       public int Current { get; set; }

       public IList<BusinessJobModel> BusinessJobList { get; set; }
    }
}
