using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace UNETI.FIT.Models.Entity
{
    public class Attachment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public int Length { get; set; }
    }
}