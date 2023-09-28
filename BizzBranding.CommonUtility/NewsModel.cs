using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
   public class NewsModel
    {
      // public List<NewsModel> AdminHomeNews { get; set; }
        public int NewsId { get; set; }
        public string NewsTitle { get; set; }
        public string NewsDesc { get; set; }

        public string NewsDate { get; set; }
        public bool IsActive { get; set; }
        public int? CreatedByUserId { get; set; }
        public int? CreatedByAdminId { get; set; }

        public Nullable<System.DateTime> CreateOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public int? UpdateBy { get; set; }


        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public string NewsImage { get; set; }
        public List<NewsModel> NewsList { get; set; }
    }
}
