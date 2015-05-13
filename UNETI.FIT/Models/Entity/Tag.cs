using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UNETI.FIT.Models.Entity
{
    public class Tag
    {
        public int TagID { get; set; }
        public string TagDescription { get; set; }

        public virtual ICollection<Article> Articles { get; set; }

        public Tag()
        {
            Articles = new List<Article>();
        }
    }
}