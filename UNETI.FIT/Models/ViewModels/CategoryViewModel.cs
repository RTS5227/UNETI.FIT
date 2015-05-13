using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UNETI.FIT.Models.Entity;

namespace UNETI.FIT.Models.ViewModels
{
    public class CategoryViewModel
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public IEnumerable<ArticleViewModel> Articles { get; set; }

        public CategoryViewModel()
        {
            Articles = new List<ArticleViewModel>();
        }
    }
}