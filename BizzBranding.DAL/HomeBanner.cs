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
    
    public partial class HomeBanner
    {
        public int HomeBannerId { get; set; }
        public Nullable<int> UserId { get; set; }
        public string BannerImage { get; set; }

        public bool IsActive { get; set; }
        public string URL { get; set; }
    
        public virtual BussinessUser BussinessUser { get; set; }
    }
}
