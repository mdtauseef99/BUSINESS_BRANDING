using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizzBranding.CommonUtility
{
    public class CityModel
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public Nullable<int> CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }

        public List<CityModel> City { get; set; }
        public List<CountryModel> Country { get; set; }
    }
}
