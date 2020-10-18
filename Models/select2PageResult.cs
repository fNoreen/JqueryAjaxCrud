using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JqueryAjaxCrud.Models
{
    public class select2PageResult
    {
        public int Total { get; set; }
        public List<clsList> Results { get; set; }
    }
}