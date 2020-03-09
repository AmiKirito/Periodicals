using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Client.ViewModels
{
    public class SubscriptionCreateViewModel
    {
        public string UserId { get; set; }
        public int UserBalance { get; set; }
        public Publisher Publisher { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SubscriptionPeriod { get; set; }
        public int TotalPrice { get; set; }
    }
}