using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class CoBrandingProImgModel
    {
        public int Id { get; set; }
        public Nullable<int> CoBrandingId { get; set; }
        public string CoBrandProdImage { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public bool IsActive { get; set; }
    }
}
