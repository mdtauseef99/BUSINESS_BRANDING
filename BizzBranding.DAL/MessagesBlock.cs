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
    
    public partial class MessagesBlock
    {
        public int BlockId { get; set; }
        public Nullable<int> ToUser { get; set; }
        public Nullable<int> FromUser { get; set; }
        public Nullable<bool> IsBlocked { get; set; }
    
        public virtual BussinessUser BussinessUser { get; set; }
        public virtual BussinessUser BussinessUser1 { get; set; }
    }
}
