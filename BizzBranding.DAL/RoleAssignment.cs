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
    
    public partial class RoleAssignment
    {
        public int RoleAssignmentId { get; set; }
        public int RoleId { get; set; }
        public int PageId { get; set; }
        public bool AddRec { get; set; }
        public bool EditRec { get; set; }
        public bool DeleteRec { get; set; }
        public bool ViewRec { get; set; }
        public bool IsActive { get; set; }
    
        public virtual Role Role { get; set; }
    }
}
