using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BizzBranding.Areas.Admin.Models
{
    public class RoleWebModel
    {
        public RoleWebModel()
        {
            Rolemanages = new List<RolemanagementWebModel>();
            Roles = new List<RoleWebModel>();
        }

        public int RoleID { get; set; }
        [Required(ErrorMessage = "Please enter RoleName")] 
        public string RoleName { get; set; }
         //[Required(ErrorMessage = "Please enter Description")] 
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public int Pagecount { get; set; }
        public List<RoleWebModel> Roles { get; set; }
        public List<RolemanagementWebModel> Rolemanages { get; set; }

        public int PageID { get; set; }
        public int Total { get; set; }
        public int Current { get; set; }
    }

    public class RolemanagementWebModel
    {
        public int RolemanagementID { get; set; }
        public int RoleID { get; set; }
        public int PageID { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }

        public string PageName { get; set; }

    }
}