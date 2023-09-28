using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class CountryWebModel
    {
        public int CountryId { get; set; }
         [Required(ErrorMessage = "Please enter your country")]
        public string CountryName { get; set; }
        public bool IsActive { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public List<CountryWebModel> Countries { get; set; }
        public IEnumerable<SelectListItem> Country { get; set; }
        public IEnumerable<SelectListItem> CityNames { get; set; }

    }
}