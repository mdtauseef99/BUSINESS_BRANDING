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
    
    public partial class VisitorCount
    {
        public int VisitorId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string IpAddress { get; set; }
    
        public virtual BussinessUser BussinessUser { get; set; }
    }
}
