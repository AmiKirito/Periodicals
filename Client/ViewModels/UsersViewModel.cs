using Client.Models;
using System;
using System.Collections.Generic;

namespace Client.ViewModels
{
    /// <summary>
    /// Class that represents user ViewModel for business logic and presentation layers
    /// </summary>
    public class UsersViewModel
    {
        public IEnumerable<Tuple<string, string, string, bool, bool, string>> Items = new List<Tuple<string, string, string, bool, bool, string>>();
        public PageInfo PageInfo { get; set; }
    }
}