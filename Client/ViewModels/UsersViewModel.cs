using Client.Models;
using System;
using System.Collections.Generic;

namespace Client.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<Tuple<string, string, string, bool, bool, string>> Items = new List<Tuple<string, string, string, bool, bool, string>>();
        public PageInfo PageInfo { get; set; }
    }
}