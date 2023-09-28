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
    
    public partial class CoBranding
    {
        public CoBranding()
        {
            this.CoBrandingImages = new HashSet<CoBrandingImage>();
        }
    
        public int CoBrandingId { get; set; }
        public Nullable<int> PartnerA { get; set; }
        public Nullable<int> PartnerB { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public Nullable<int> UpdatedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsAprroved { get; set; }
        public string ProductsLogo { get; set; }
        public string ProductsDetails { get; set; }
        public string CoBrandedName { get; set; }
        public Nullable<System.DateTime> ActivatedOn { get; set; }
        public Nullable<System.DateTime> ExpiresOn { get; set; }
        public Nullable<int> No_Months { get; set; }
    
        public virtual Administrator Administrator { get; set; }
        public virtual BussinessUser BussinessUser { get; set; }
        public virtual BussinessUser BussinessUser1 { get; set; }
        public virtual ICollection<CoBrandingImage> CoBrandingImages { get; set; }
    }
}
