using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using UNETI.FIT.Filters;
using UNETI.FIT.Models.Entity;

namespace UNETI.FIT.Areas.SubjectManager.Models.Entity
{
    public class Student
    {
        private string ava = "avatar.png";

        //Ảnh đại diện
        [LocalizedDisplayName("Avatar")]
        [UIHint("Avatar")]
        public string Avatar { get { return ava; } set { ava = value; } }

        [Key]
        //MSSV
        [StringLength(50, MinimumLength = 5)]
        [LocalizedDisplayName("StudentID")]
        public string ID { get; set; }

        //Họ Tên
        [StringLength(50, MinimumLength = 1)]
        [LocalizedDisplayName("FullName")]
        public string FullName { get; set; }

        //Email
        [StringLength(100, MinimumLength = 1)]
        public string Email { get; set; }

        //Giới tính
        [LocalizedDisplayName("Gender")]
        [UIHint("DDLFGender")]
        public Gender Gender { get; set; }

        //SĐT
        [LocalizedDisplayName("PhoneNumber")]
        public string PhoneNumber { get; set; }

        //Khóa đào tạo
        [LocalizedDisplayName("LevelOfTraining")]
        [UIHint("DDLFLevel")]
        public int LevelOfTrainingID { get; set; }
        public virtual LevelOfTraining LevelOfTraining { get; set; }

        //Khoa
        [LocalizedDisplayName("Faculty")]
        [UIHint("DDLFFaculty")]
        public int FacultyID { get; set; }
        public virtual Faculty Faculty { get; set; }

        //Lớp
        [LocalizedDisplayName("ClassOfStudy")]
        [UIHint("DDLFClass")]
        public int ClassOfStudyID { get; set; }
        public virtual ClassOfStudy ClassOfStudy { get; set; }
    }

    public class Faculty
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class ClassOfStudy
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class LevelOfTraining
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }

    public class Module
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        [LocalizedDisplayName("Module")]
        public string Name { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }

    public class StudentCPModel
    {
        public Student Student { get; set; }
        public List<Message> Messages { get; set; }

        public StudentCPModel()
        {
            Messages = new List<Message>();
        }
    }

}