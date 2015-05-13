using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Filters;
using System.ComponentModel.DataAnnotations;

namespace UNETI.FIT.Areas.SubjectManager.Models.Entity
{
    public class Report
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [UIHint("MultilineText")]
        public string Content { get; set; }

        [UIHint("Attachment")]
        public string Attachment { get; set; }

        [HiddenInput(DisplayValue = false)]
        public DateTime CreateAt { get; set; }

        [HiddenInput(DisplayValue = false)]
        public ProgressEnum? Progress { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string StudentID { get; set; }
        public Student Student { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? CommentID { get; set; }
        public Comment Comment { get; set; }
    }

    public class ReportModelX
    {
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [UIHint("MultilineText")]
        public string Content { get; set; }

        [UIHint("Attachment")]
        public string Attachment { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int SubjectID { get; set; }
        public Subject Subject { get; set; }

        public string StudentID { get; set; }
        public Student Student { get; set; }
    }

    public class Comment
    {
        public int ID { get; set; }
        public string Content { get; set; }
        public DateTime CreateAt { get; set; }
    }

    public enum ProgressEnum
    {
        Poor,
        Fair,
        Average,
        Good,
        Excellent,
    }
}