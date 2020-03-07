using BLL.Models;
using System.Collections.Generic;

namespace Client.ViewModels
{
    public class CabinetViewModel
    {
        public string Username { get; set; }
        public int Balance { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}