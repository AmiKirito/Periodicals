using Client.Models;
using System;
using System.Collections.Generic;

namespace Client.ViewModels
{
    public class UsersViewModel
    {
        public IEnumerable<Tuple<string, string, string, bool, bool>> Items = new List<Tuple<string, string, string, bool, bool>>();
        public PageInfo PageInfo { get; set; }
    }
}