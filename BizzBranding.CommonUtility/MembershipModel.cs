using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class MembershipModel
    {
        public int MembershipID { get; set; }
        public Nullable<int> AccessPlanId { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<System.DateTime> ActivatedOn { get; set; }
        public Nullable<System.DateTime> ExpiresOn { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> UpdatedDate { get; set; }

        public string AccessPlanName { get; set; }
        public string BusinessName { get; set; }
        public List<MembershipModel> Membership { get; set; }
    }
}
