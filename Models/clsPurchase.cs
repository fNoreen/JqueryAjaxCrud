using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace JqueryAjaxCrud.Models
{
    public class clsPurchase
    {
        public int PurchaseId { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public int RefrenceNumber { get; set; }
        public int PurchaseLineId { get; set; }
        public string ItemName { get; set; }
        public int Qty { get; set; }
        public decimal Rate { get; set; }
    }
}