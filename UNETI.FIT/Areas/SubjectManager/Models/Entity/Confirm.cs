using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;

namespace UNETI.FIT.Areas.SubjectManager.Models.Entity
{
    public class Confirm
    {
        [Key]
        public int ID { get; set; }

        public ConfirmEnum Content { get; set; }

        public DateTime CreateAt { get; set; }

        public string StudentID { get; set; }
        public virtual Student Student { get; set; }

        public int SubjectID { get; set; }
        public virtual Subject Subject { get; set; }
    }

    public enum ConfirmEnum
    {
        Wait,
        Accept,
        Deny
    }
}