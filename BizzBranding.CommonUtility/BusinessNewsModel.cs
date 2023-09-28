using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
   public class BusinessNewsModel
    {
        public int BusinessNewsId { get; set; }
       [Required(ErrorMessage = "Enter NewsTitle")]
        public string NewsTitle { get; set; }
       [Required(ErrorMessage = "Enter Description")]
        public string Description { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string NewsImage { get; set; }
        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }

        public List<BusinessNewsModel> BusinessNewsList { get; set; }
        public List<IndustryModel> IndustryList { get; set; }
        public List<CityModel> CityList { get; set; }
        //public List<BusinessNewsModel> NewsUpdates { get; set; }

       
    }
}
