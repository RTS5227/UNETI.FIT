using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UNETI.FIT.Models.ViewModels
{
    public class ListViewModelBase<T> where T : class
    {
        public List<T> List { get; set; }
        public FilterModel Filter { get; set; }

        public ListViewModelBase()
        {
            List = new List<T>();
        }
    }
}