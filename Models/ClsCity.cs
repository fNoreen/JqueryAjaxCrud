﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JqueryAjaxCrud.Models
{
    public class ClsCity
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CountryId { get; set; }
        public string CountryName { get; set; }
    }
}