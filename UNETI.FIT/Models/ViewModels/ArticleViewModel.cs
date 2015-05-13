using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Filters;
using System.ComponentModel.DataAnnotations;

namespace UNETI.FIT.Models.ViewModels
{
    public class ArticleViewModel
    {
        private string thumb = "thumbnail.png";

        [UIHint("Thumbnail")]
        [LocalizedDisplayName("Thumbnail")]
        public string Thumbnail { get { return thumb; } set { thumb = value; } }

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [LocalizedDisplayName("Title")]
        public string Title { get; set; }

        [AllowHtml]
        [LocalizedDisplayName("Content")]
        [UIHint("Content")]
        public string Content { get; set; }

        public bool Pin { get; set; }

        [UIHint("DDLFArticleStatus")]
        [LocalizedDisplayName("Status")]
        public ArticleStatus Status { get; set; }

        [LocalizedDisplayName("Category")]
        [UIHint("DDLFCategory")]
        public int CategoryID { get; set; }
        public Category Category { get; set; }

        [ScaffoldColumn(false)]
        [LocalizedDisplayName("CreateAt")]
        public DateTime? CreateAt { get; set; }

        [ScaffoldColumn(false)]
        [LocalizedDisplayName("UpdateAt")]
        public DateTime? UpdateAt { get; set; }

        [ScaffoldColumn(false)]
        [LocalizedDisplayName("View")]
        public int View { get; set; }

        [UIHint("AssignedTag")]
        [LocalizedDisplayName("Tag")]
        public virtual ICollection<AssignedTagData> Tags { get; set; }

        public ArticleViewModel()
        {
            Tags = new List<AssignedTagData>();
        }
    }
}