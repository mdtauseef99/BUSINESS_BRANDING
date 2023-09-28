using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class CurrencyModel
    {
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public bool IsActive { get; set; }

        public List<CurrencyModel> Currency { get; set; }
    }
}
