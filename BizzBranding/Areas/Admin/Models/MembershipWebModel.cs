using BizzBranding.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BizzBranding.Areas.Admin.Models
{
    public class MembershipWebModel
    {
        public int MembershipID { get; set; }
         [Required(ErrorMessage = "Please select AccessPlan")] 
        public Nullable<int> AccessPlanId { get; set; }
         [Required(ErrorMessage = "Please select Business Type")] 
        public Nullable<int> UserId { get; set; }
         [Required(ErrorMessage = "Please enter date")] 
        public Nullable<System.DateTime> ActivatedOn { get; set; }
         [Required(ErrorMessage = "Please enter date")] 
        public Nullable<System.DateTime> ExpiresOn { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }
        
        public string AccessPlanName { get; set; }
        public string BusinessName { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public virtual AccessPlan AccessPlan { get; set; }
        public virtual BussinessUser BussinessUser { get; set; }

        public List<MembershipWebModel> Memberships{ get; set; }
        public IEnumerable<SelectListItem> Membership { get; set; }
        public IEnumerable<SelectListItem> AccessPlanNames { get; set; }
        public IEnumerable<SelectListItem> BusinessNames { get; set; }
    }
}