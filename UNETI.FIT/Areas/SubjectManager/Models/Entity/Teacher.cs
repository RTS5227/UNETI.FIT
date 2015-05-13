using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Filters;

namespace UNETI.FIT.Areas.SubjectManager.Models.Entity
{
    public class Teacher
    {
        private string ava = "avatar.png";

        //Ảnh đại diện
        [LocalizedDisplayName("Avatar")]
        [UIHint("Avatar")]
        public string Avatar { get { return ava; } set { ava = value; } }

        [Key]
        //MSGV
        [StringLength(50, MinimumLength = 5)]
        [LocalizedDisplayName("TeacherID")]
        public string ID { get; set; }

        //Họ tên
        [LocalizedDisplayName("FullName")]
        [StringLength(50, MinimumLength = 1)]
        public string FullName { get; set; }

        //Bằng cấp
        [LocalizedDisplayName("Degree")]
        public string Degree { get; set; }

        //Địa chỉ
        [LocalizedDisplayName("Address")]
        public string Address { get; set; }

        //SĐT
        [LocalizedDisplayName("PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }

        //Chuyên ngành
        [LocalizedDisplayName("Specialization")]
        public string Specialization { get; set; }

        //Đồ án quản lý
        [LocalizedDisplayName("Subject")]
        public virtual ICollection<Subject> Subjects { get; set; }

        public Teacher()
        {
            Subjects = new List<Subject>();
        }
    }

    public class TeacherCPModel
    {
        public Teacher Teacher { get; set; }
        public List<Confirm> Confirms { get; set; }
        public TeacherCPModel()
        {
            Confirms = new List<Confirm>();
        }
    }
}