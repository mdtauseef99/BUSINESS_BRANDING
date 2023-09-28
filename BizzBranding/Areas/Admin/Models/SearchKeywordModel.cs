using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BizzBranding.Models;

namespace BizzBranding.Areas.Admin.Models
{
    public class SearchKeywordModel
    {
        public int Id { get; set; }
         [Required(ErrorMessage = "Please enter Keyword to search")] 
        public string BusinessName { get; set; }
        public string EmailId { get; set; }
        //public string Address { get; set; }
        //public string Pincode { get; set; }
       // public Nullable<int> AreaId { get; set; }
       // public string ContactPerson { get; set; }
       // public string ContactNo { get; set; }
       // public bool IsActive { get; set; }
       // public bool IsDelete { get; set; }
       // public string AreaName { get; set; }

        //public int Pagecount { get; set; }
        //public int PageID { get; set; }
        //public int Current { get; set; }
        public List<SearchKeywordModel> searchkeyword { get; set; }
        public IEnumerable<SelectListItem> search { get; set; }
        public IEnumerable<SelectListItem> BusinessbyName { get; set; }

    }

    //public class AreaWebModel
    //{
    //    public int AreaId { get; set; }
    //    public string AreaName { get; set; }
    //    public bool IsActive { get; set; }

    //    public int Pagecount { get; set; }
    //    public int PageID { get; set; }
    //    public int Current { get; set; }
    //    public List<AreaWebModel> Areas { get; set; }
    //    public IEnumerable<SelectListItem> Area { get; set; }

    //}
}