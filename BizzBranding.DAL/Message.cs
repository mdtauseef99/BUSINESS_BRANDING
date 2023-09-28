//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace BizzBranding.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Message
    {
        public Message()
        {
            this.MessagesFiles = new HashSet<MessagesFile>();
        }
    
        public int MessageId { get; set; }
        public Nullable<int> MsgFrom { get; set; }
        public int MsgTo { get; set; }
        public string MsgTitle { get; set; }
        public string Message1 { get; set; }
        public Nullable<bool> FromStatus { get; set; }
        public Nullable<bool> ToStatus { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> Updateddate { get; set; }
        public Nullable<bool> IsReadFrom { get; set; }
        public Nullable<bool> IsReadTo { get; set; }
        public Nullable<bool> IsEnquiry { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual BussinessUser BussinessUser { get; set; }
        public virtual BussinessUser BussinessUser1 { get; set; }
        public virtual ICollection<MessagesFile> MessagesFiles { get; set; }
    }
}
