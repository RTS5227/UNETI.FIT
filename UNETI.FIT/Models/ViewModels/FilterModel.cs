using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UNETI.FIT.Models.ViewModels
{
    public class FilterModel
    {
        public int? page { get; set; }
        public int PageIndex
        {
            get
            {
                return page.HasValue && page.Value > 0 ? page.Value : 1;
            }
        }
        public int PageSize { get; set; }//Total items per page
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }

        public string sort { get; set; }
        public string SortOrder { get { return this.sort ?? ""; } }

        public string kw { get; set; }
        public string SearchString { get { return this.kw ?? ""; } }

        public int? us { get; set; }
        public int Category
        {
            get
            {
                return us.HasValue && us.Value >= 0 ? us.Value : 0;
            }
        }

        public string CurrentFileter { get; set; }

        public FilterModel()
        {
            PageSize = 10;
        }
    }
}