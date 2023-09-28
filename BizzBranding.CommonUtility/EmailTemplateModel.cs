using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class EmailTemplateModel
    {
        public int EmailTempId { get; set; }
        public string EmailTempTitle { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public List<EmailTemplateModel> EmailTemplate { get; set; }
    } 
}
