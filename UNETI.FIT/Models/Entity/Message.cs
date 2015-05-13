using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;

namespace UNETI.FIT.Models.Entity
{
    public class Message
    {
        public int ID { get; set; }
        public string Body { get; set; }
        public DateTime CreateAt { get; set; }
        public bool IsReaded { get; set; }
        //FROM
        public string TeacherID { get; set; }
        public Teacher Teacher { get; set; }
        //TO
        public string StudentID { get; set; }
        public Student Student { get; set; }
    }
}