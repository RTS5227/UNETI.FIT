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
    [Table("DeTai")]
    public class Subject
    {
        private string thumb = "thumbnail.png";

        [HiddenInput(DisplayValue = false)]
        public int ID { get; set; }

        //Ảnh đại diện
        [LocalizedDisplayName("Thumbnail")]
        [UIHint("Thumbnail")]
        public string Thumbnail { get { return thumb; } set { thumb = value; } }

        //Tiêu đề
        [LocalizedDisplayName("Title")]
        public string Title { get; set; }

        //Nội dung
        [AllowHtml]
        [UIHint("Content")]
        [LocalizedDisplayName("Content")]
        public string Content { get; set; }

        //Mục tiêu
        [LocalizedDisplayName("Goal")]
        [UIHint("MultilineText")]
        public string Goal { get; set; }

        //Kết quả cần đạt
        [LocalizedDisplayName("Result")]
        [UIHint("MultilineText")]
        public string Result { get; set; }

        //Ghi chú
        [LocalizedDisplayName("Note")]
        [UIHint("MultilineText")]
        public string Note { get; set; }

        //Ngày nhận đồ án
        [DataType(DataType.Date)]
        [LocalizedDisplayName("StartAt")]
        public DateTime StartAt { get; set; }

        //Ngày nộp đồ án
        [LocalizedDisplayName("EndAt")]
        [DataType(DataType.Date)]
        public DateTime EndAt { get; set; }

        //Học phần
        [LocalizedDisplayName("Module")]
        [UIHint("DDLFModule")]
        public int ModuleID { get; set; }
        public virtual Module Module { get; set; }

        //Tổng số tín chỉ
        [LocalizedDisplayName("TotalModule")]
        public int TotalModules { get; set; }

        //Tệp đính kèm
        [LocalizedDisplayName("Attachment")]
        [UIHint("Attachment")]
        public string Attachment { get; set; }

        //Giáo viên hướng dẫn
        [LocalizedDisplayName("Teacher")]
        [UIHint("DDLFTeacher")]
        public string TeacherID { get; set; }
        public virtual Teacher Teacher { get; set; }

        //Trạng thái
        [LocalizedDisplayName("Status")]
        [UIHint("DDLFStatus")]
        public SubjectStatus Status { get; set; }

        //Thời gian tạo
        [LocalizedDisplayName("CreateAt")]
        [ScaffoldColumn(false)]
        public DateTime? CreateAt { get; set; }

        //Số lượt xem
        [LocalizedDisplayName("View")]
        [HiddenInput(DisplayValue = false)]
        public int View { get; set; }

        //Thông tin xác nhân
        [LocalizedDisplayName("RegisteredStudent")]
        public virtual ICollection<Confirm> Confirms { get; set; }

        //Tiến độ thực hiện
        [LocalizedDisplayName("Progress")]
        public virtual ICollection<Report> Reports { get; set; }

        public Subject()
        {
            Confirms = new List<Confirm>();
            Reports = new List<Report>();
        }
    }

    public enum SubjectStatus
    {
        Public,
        Private
    }
}