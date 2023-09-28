using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizzBranding.Areas.Admin.Models
{
    public class NewsWebModel
    {
        public NewsWebModel()
        {
            News = new List<NewsWebModel>();
        }
        public int NewsId { get; set; }
        [Required(ErrorMessage = "Please enter NewsTitle")] 
        public string NewsTitle { get; set; }
        public string NewsImage { get; set; }
         [Required(ErrorMessage = "Please enter Description")] 
        public string NewsDescription { get; set; }
        public bool IsActive { get; set; }

        public List<NewsWebModel> News { get; set; }
        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }

    }
}