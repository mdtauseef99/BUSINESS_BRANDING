using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class MessageDataModel
    {
        public int MessageId { get; set; }
        public int MsgFrom { get; set; }
        public int MsgTo { get; set; }
        public string MsgTitle { get; set; }
        public string Message { get; set; }
        public int FromStatus { get; set; }
        public int ToStatus { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime Updateddate { get; set; }
        public bool? IsReadFrom { get; set; }
        public bool? IsReadTo { get; set; }
        public string Messagefile { get; set; }
        //public IQueryable<string> MessageToName { get; set; }
        //public IQueryable<string> MessageFromName { get; set; }
        public string MessageToName { get; set; }
        public string MessageFromName { get; set; }

    }
}
