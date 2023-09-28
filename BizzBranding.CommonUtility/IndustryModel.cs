using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class IndustryModel
    {
        public int IndustryId { get; set; }
        public string IndustryName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public List<IndustryModel> Industry { get; set; }

    }
}
