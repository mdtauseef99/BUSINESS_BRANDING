using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
   public class SubscribeModel
    {
        public int SubscribeId { get; set; }
        public string EmailId { get; set; }
        public bool IsActive { get; set; }
       
       public List<SubscribeModel> subscribeList { get; set; }
    }
}
