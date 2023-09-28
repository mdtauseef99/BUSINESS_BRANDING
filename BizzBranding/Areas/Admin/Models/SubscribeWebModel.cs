using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizzBranding.Areas.Admin.Models
{
    public class SubscribeWebModel
    {
        public SubscribeWebModel()
        {
            Subscribe = new List<SubscribeWebModel>();
        }
        public int SubscribeId { get; set; }
       [Required(ErrorMessage = "Please EmailID")]
        public string EmailId { get; set; }

        public bool IsActive { get; set; }

        public List<SubscribeWebModel> Subscribe { get; set; }
        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
    }
}