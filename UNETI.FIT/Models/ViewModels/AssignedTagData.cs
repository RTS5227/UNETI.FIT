using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace UNETI.FIT.Models.ViewModels
{
    public class AssignedTagData
    {
        [Key]
        public int TagID { get; set; }
        public string TagDescription { get; set; }
        public bool Assigned { get; set; }
    }
}