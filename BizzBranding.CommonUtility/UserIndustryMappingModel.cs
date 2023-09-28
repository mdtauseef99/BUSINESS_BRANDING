using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BizzBranding.CommonUtility;

namespace BizzBranding.CommonUtility
{
    public class UserIndustryMappingModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int IndustryId { get; set; }

        public IEnumerable<string> SelectItems { set; get; }

        public List<SelectListItem> Industries { set; get; }
    }
}
