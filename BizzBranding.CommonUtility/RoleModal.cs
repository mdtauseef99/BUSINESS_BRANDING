using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class RoleModel
    {
        public RoleModel()
        {
            Rolemanages = new List<RolemanagementModel>();
        }

        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public List<RolemanagementModel> Rolemanages { get; set; }
    }

    public class RolemanagementModel
    {
        public int RolemanagementID { get; set; }
        public int RoleID { get; set; }
        public int PageID { get; set; }
        public bool Add { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
        public bool View { get; set; }

    }
}
