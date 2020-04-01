using BLL.Models;
using System.Collections.Generic;

namespace Client.ViewModels
{
    /// <summary>
    /// Class that represents cabinet ViewModel for business logic and presentation layers
    /// </summary>
    public class CabinetViewModel
    {
        public string Username { get; set; }
        public int Balance { get; set; }
        public List<Subscription> Subscriptions { get; set; }
    }
}