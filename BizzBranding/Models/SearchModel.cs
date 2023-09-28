using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BizzBranding.Models
{
    public class SearchModel
    {
        public DateTime? CreatedOn { get; set; }
        public bool? IsReadTo { get; set; }
        public string MessageFrom { get; set; }
        public string MessageTo { get; set; }
        public string Message { get; set; }
        public string MessageTitle { get; set; }
        public int MessageId { get; set; }
        public string Username { get; set; }
        public string Messagefile { get; set; }
    }
}