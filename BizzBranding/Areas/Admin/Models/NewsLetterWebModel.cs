using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Entity;

namespace BizzBranding.Areas.Admin.Models
{
    public class NewsLetterWebModel
    {
        public NewsLetterWebModel()
        {
            NewsLetter = new List<NewsLetterWebModel>();
        }
        public int NewsLetterID { get; set; }
         [Required(ErrorMessage = "Please enter NewsTitle")] 
        public string Title { get; set; }

        [Required(ErrorMessage = "Please enter Description")] 
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public List<NewsLetterWebModel> NewsLetter { get; set; }
        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
    }
}