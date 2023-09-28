using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BizzBranding.Areas.Admin.Models
{
    public class CurrencyWebModel
    {
        public int CurrencyId { get; set; }
        [Required(ErrorMessage = "Please enter currency")] 
        public string CurrencyName { get; set; }
        public bool IsActive { get; set; }

        public int Pagecount { get; set; }
        public int PageID { get; set; }
        public int Current { get; set; }
        public List<CurrencyWebModel> Currencies { get; set; }
        public IEnumerable<SelectListItem> Currency { get; set; }
    }
}