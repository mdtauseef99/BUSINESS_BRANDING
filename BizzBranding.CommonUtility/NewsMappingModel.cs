using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
   public class NewsMappingModel
    {
        public int NewsMappingId { get; set; }
        public int? AccessPlanId { get; set; }
        public int? UserId { get; set; }
        public DateTime? ValidityStart { get; set; }
        public DateTime? ValidityEnd { get; set; }
        public bool IsActive { get; set; }
    }
}
