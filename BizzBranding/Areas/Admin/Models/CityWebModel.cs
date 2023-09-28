using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class CityWebModel
    {
        public int CityId { get; set; }
        [Required(ErrorMessage = "Please enter Cityname")] 
        public string CityName { get; set; }
        [Required(ErrorMessage = "Please select country")] 
        public Nullable<int> CountryId { get; set; }
        public string CountryName { get; set; }
        public bool IsActive { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public List<CityWebModel> Cities { get; set; }
        public IEnumerable<SelectListItem> City { get; set; }
        public IEnumerable<SelectListItem> CountryNames { get; set; }

    }
}