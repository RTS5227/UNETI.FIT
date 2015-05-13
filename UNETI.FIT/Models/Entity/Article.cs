using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace UNETI.FIT.Models.Entity
{
    public class Article
    {
        public Article()
        {
            Tags = new List<Tag>();
        }

        [Key]
        public int ID { get; set; }
        public string Title { get; set; }

        public string Thumbnail { get; set; }

        [AllowHtml]
        public string Content { get; set; }

        public DateTime CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }

        public int View { get; set; }
        public bool Pin { get; set; }

        public ArticleStatus Status { get; set; }

        public int CategoryID { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<Tag> Tags { get; set; }
    }

    public enum ArticleStatus
    {
        Public = 0,
        Private = 1
    }

    //public enum CategoryEnum
    //{
    //    Home, Introduction, News, Training, Research, OLP, Admissions, 
    //    Students, Contact, Notice, Practise, TradeUnion, Youth, Gallery
    //}
}