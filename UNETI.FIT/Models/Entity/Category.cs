using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace UNETI.FIT.Models.Entity
{
    public class Category
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }
        public string Name { get; set; }//unique
        public int Order { get; set; }
        [UIHint("DDLFCategoryType")]
        public CategoryTypeEnum Type { get; set; }
        public IEnumerable<Article> Articles { get; set; }

        public Category()
        {
            Articles = new List<Article>();
        }
    }

    public enum CategoryTypeEnum
    {
        Article,
        Announcement,
        Navigator
    }
}