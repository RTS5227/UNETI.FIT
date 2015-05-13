using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using UNETI.FIT.Areas.SubjectManager.Models.Entity;
using UNETI.FIT.Areas.SubjectManager.Models;
using UNETI.FIT.Areas.SubjectManager.Filters;
using UNETI.FIT.Areas.SubjectManager.Infrastructure;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Helpers;

using UNETI.FIT.Models.ViewModels;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Infrastructure.Helper;

using UNETI.FIT.Infrastructure.Concrete;
using System.IO;
using System.Configuration;

namespace UNETI.FIT.Areas.SubjectManager.Controllers
{
    public class SubjectController : Controller
    {
        private ISubjectRepository subjectRepository;
        private IConfirmRepository confirmRepository;
        private ICommentRepository commentRepository;
        private ITeacherRepository teacherRepository;
        private IStudentRepository studentRepository;
        private IModuleRepository ModuleRepository;
        private IReportRepository reportRepository;
        private IMessageRepository messageRepository;
        private IContactProcessor contactProcessor;

        public SubjectController(
            IContactProcessor pro1,
            ISubjectRepository isr,
            IConfirmRepository icr,
            ICommentRepository ipr,
            IStudentRepository repo1,
            ITeacherRepository repo2,
            IModuleRepository repo3,
            IReportRepository repo4,
            IMessageRepository repo5)
        {
            contactProcessor = pro1;
            subjectRepository = isr;
            confirmRepository = icr;
            commentRepository = ipr;
            studentRepository = repo1;
            teacherRepository = repo2;
            ModuleRepository = repo3;
            reportRepository = repo4;
            messageRepository = repo5;
        }

        //
        // GET: /Subject/

        public ActionResult Index(FilterModel filter)
        {
            var result = new ListViewModelBase<Subject>();

            var entities = subjectRepository
                .GetMany(s => s.Title.Contains(filter.SearchString)); ;

            if (filter.Category >= 1)
            {
                entities = entities
                    .Where(s => s.ModuleID == filter.Category);
            }

            result.List = entities.Filter(filter).ToList();
            result.Filter = filter;
            return View(result);
        }

        //Render danh sach cac mon do an
        public ActionResult Category()
        {
            var result = new List<Module>();
            result = ModuleRepository.GetAll().ToList();
            return PartialView(result);
        }

        //Render danh sach cac do an nhieu luot xem nhat
        public ActionResult Top()
        {
            var result = new List<Subject>();
            result = subjectRepository
                .GetAll()
                .OrderByDescending(s => s.View)
                .Take(5)
                .ToList();
            return PartialView(result);
        }

        // Thay doi ngon ngu hien thi
        // GET: /ChangeLanguage/

        public ActionResult ChangeLanguage(string culture, string returnUrl)
        {
            if (!string.IsNullOrEmpty(culture))
            {
                var httpCookie = Request.Cookies["language"];
                if (httpCookie != null)
                {
                    var cookie = Response.Cookies["language"];
                    if (cookie != null) cookie.Value = culture;
                }
            }
            if (!string.IsNullOrEmpty(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToIndex();
        }

        #region Message
        //Render danh sach tin nhan cho sv
        [Authorize(Roles = "student")]
        public ActionResult Message()
        {
            var result = new List<Message>();
            int limit = 10;
            result = messageRepository
                .GetMany(m => m.StudentID == User.Identity.Name)
                .OrderBy(m => m.IsReaded)
                .Take(limit)
                .ToList();
            result.ForEach(a => a.Teacher = teacherRepository.GetByID(a.TeacherID));
            result = result
                .OrderByDescending(m => m.CreateAt)
                .ToList();
            return PartialView(result);
        }

        //Sv danh dau da doc het tin nhan
        [Authorize(Roles = "student")]
        public bool MarkAllReaded(string uid)
        {
            var list = messageRepository
                .GetMany(a => a.StudentID == uid && a.IsReaded == false)
                .ToList();
            if (list.Count == 0) return true;
            list.ForEach(a => a.IsReaded = true);
            messageRepository.UpdateRange(list);
            return true;
        }
        #endregion

        #region Report
        //Sv gui bao cao len he thong
        [Authorize(Roles = "student")]
        public ActionResult BaoCao(Report model)
        {
            if (ModelState.IsValid)
            {
                Subject entry = subjectRepository.GetByID(model.SubjectID);
                if (entry == null) return HttpNotFound();
                var confirm = confirmRepository.Get(a => a.StudentID == User.Identity.Name);
                if (confirm == null || confirm.Content != ConfirmEnum.Accept)
                {
                    TempData["message"] = "Bạn không được cấp quyền truy cập nội dung này.";
                    return RedirectToIndex();
                }
                model.StudentID = User.Identity.Name;
                model.CreateAt = DateTime.Now;
                reportRepository.Add(model);
            }
            return RedirectToChiTiet(model.SubjectID);
        }

        //Render danh sach cac bao cao cua SV ve do an X
        public ActionResult DSBC(int id)
        {
            var result = new List<Report>();
            result = reportRepository
                .GetMany(a => a.SubjectID == id)
                .OrderByDescending(a => a.CreateAt)
                .ToList();
            result.ForEach(a => a.Student = studentRepository.GetByID(a.StudentID));
            return PartialView(result);
        }

        //GV danh gia tien do cua moi bao cao
        [HttpGet]
        [Authorize(Roles = "teacher")]
        public ActionResult DanhGiaTienDo(int id, ProgressEnum progress)
        {
            int reportID = id;
            var entry = reportRepository.GetByID(reportID);
            if (entry == null) return HttpNotFound();
            entry.Progress = progress;
            reportRepository.Update(entry);
            return RedirectToChiTiet(entry.SubjectID);
        }

        //GV nhan xet cho moi bao cao
        [HttpPost]
        [Authorize(Roles = "teacher")]
        public ActionResult NhanXet(int id, string content)
        {
            var subjectID = id;
            if (subjectRepository.GetByID(subjectID) == null) return HttpNotFound();
            commentRepository.Add(new Comment
            {
                Content = content,
                CreateAt = DateTime.Now
            });
            return RedirectToChiTiet(subjectID);
        }

        #endregion


        #region Confirm
        //SV gui yeu cau dang ky, cho xac nhan cua gv
        [Authorize(Roles = "student")]
        public ActionResult DangKy(int id)
        {
            var subjectID = id;
            Subject model = subjectRepository.GetByID(subjectID);
            if (model == null) return HttpNotFound();

            confirmRepository.Add(new Confirm
            {
                CreateAt = DateTime.Now,
                StudentID = User.Identity.Name,
                SubjectID = subjectID,
                Content = ConfirmEnum.Wait
            });

            TempData["message"] = string.Format("Bạn đã đăng ký thành công đồ án {0}. Vui lòng chờ xác nhận từ giáo viên hướng dẫn.", model.Title);
            return RedirectToChiTiet(subjectID);
        }

        //Hien thi cac task de gv xac nhan cho sv tham gia do an
        [Authorize(Roles = "teacher")]
        public ActionResult Xacnhan()
        {
            var result = new List<Confirm>();
            result = confirmRepository
                .GetMany(c => c.Subject.TeacherID == User.Identity.Name && c.Content == ConfirmEnum.Wait)
                .ToList();
            return PartialView(result);
        }

        //GV chap nhan cho sv tham gia do an X
        public ActionResult Accept(int id)
        {
            Confirm entry = confirmRepository.GetByID(id);
            if (entry == null || entry.Subject.TeacherID != User.Identity.Name)
            {
                return HttpNotFound();
            }
            entry.Content = ConfirmEnum.Accept;
            confirmRepository.Update(entry);
            string body =
                String.Format("Bạn đã đăng ký tham gia thành công đồ án {0} của khoa CNTT trường ĐHKTKTCN."
                    , entry.Subject.Title);
            SendMailAndMessager(body, entry.Student);
            return RedirectToChiTiet(entry.SubjectID);
        }

        //GV tu choi cho sv tham gia do an X
        public ActionResult Deny(int id)
        {
            Confirm entry = confirmRepository.GetByID(id);
            if (entry == null || entry.Subject.TeacherID != User.Identity.Name)
            {
                return HttpNotFound();
            }
            entry.Content = ConfirmEnum.Deny;
            confirmRepository.Update(entry);
            string body =
                String.Format("Bạn chưa đủ điều kiện tham gia đồ án {0} của khoa CNTT trường ĐHKTKTCN."
                    , entry.Subject.Title);
            SendMailAndMessager(body, entry.Student);
            return RedirectToChiTiet(entry.SubjectID);
        }

        //GV gui email va tin nhan cho sv
        private void SendMailAndMessager(string body, Student student)
        {
            messageRepository.Add(new Message
            {
                Body = body,
                CreateAt = DateTime.Now,
                IsReaded = false,
                StudentID = student.ID,
                TeacherID = User.Identity.Name
            });
            try
            {
                contactProcessor.ProcessContact(new Email
                {
                    Message = body,
                    Subject = Resources.Resource.UNETI,
                    To = student.Email
                });
                TempData["messager"] = String.Format("Một email và tin nhắn đã được gửi tới sinh viên {0}.", student.FullName);
            }
            catch (Exception e)
            {
                TempData["messager"] = "Không thể gửi tin nhắn đến email: " + student.Email;
            }
        }

        #endregion


        #region CURD

        [HttpGet]
        [SubjectAccessable]
        public ActionResult ChiTiet(int id)
        {
            if (ModelState.IsValid)
            {
                Subject entry = ViewData.Model as Subject;
                entry.View++;
                subjectRepository.Update(entry);
                return View(entry);
            }
            else
            {
                TempData["message"] = "Bạn không được cấp quyền truy cập nội dung này.";
                return RedirectToIndex();
            }
        }

        [HttpGet]
        [Authorize(Roles = "teacher")]
        public ActionResult Them()
        {
            return View(new Subject());
        }

        [HttpPost]
        [Authorize(Roles = "teacher")]
        public ActionResult Them(Subject model)
        {
            if (ModelState.IsValid)
            {
                model.TeacherID = User.Identity.Name;
                model.CreateAt = DateTime.Now;
                subjectRepository.Add(model);
                TempData["message"] = string.Format("Đồ án \"{0}\" đã được thêm thành công.", model.Title);
                return RedirectToIndex();
            }
            return View(model);
        }

        [HttpGet]
        [MySubject]
        public ActionResult Sua(int id)
        {
            Subject model = subjectRepository.GetByID(id);
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        [HttpPost]
        [MySubject]
        public ActionResult Sua(Subject model)
        {
            if (ModelState.IsValid)
            {
                subjectRepository.Update(model);
                TempData["message"] = string.Format("Đồ án \"{0}\" đã được thêm thành công.", model.Title);
                return RedirectToIndex();
            }
            return View(model);
        }

        [HttpGet]
        [MySubject]
        public ActionResult Xoa(int id)
        {
            Subject model = subjectRepository.GetByID(id);
            if (model != null)
            {
                confirmRepository.DeleteMany(a => a.SubjectID == id);
                subjectRepository.Delete(model);
                TempData["message"] = string.Format("Đồ án \"{0}\" đã được xóa thành công.", model.Title);
            }
            return RedirectToIndex();
        }

        #endregion

        [Authorize(Roles = "student")]
        public ActionResult StudentCP()
        {
            var result = new StudentCPModel();
            int limit = 10;
            result.Student = studentRepository.GetByID(User.Identity.Name);
            result.Messages = messageRepository
                .GetMany(m => m.StudentID == User.Identity.Name)
                .OrderBy(m => m.IsReaded)
                .Take(limit)
                .OrderByDescending(m => m.CreateAt)
                .ToList();
            result.Messages.ForEach(a => a.Teacher = teacherRepository.GetByID(a.TeacherID));
            return PartialView(result);
        }

        [Authorize(Roles = "teacher")]
        public ActionResult TeacherCP()
        {
            var result = new TeacherCPModel();
            int limit = 10;
            result.Teacher = teacherRepository.GetByID(User.Identity.Name);
            result.Confirms = confirmRepository
                .GetMany(c => c.Subject.TeacherID == User.Identity.Name && c.Content == ConfirmEnum.Wait)
                .OrderByDescending(m => m.CreateAt)
                .Take(limit)
                .ToList();
            return PartialView(result);
        }

        private ActionResult RedirectToChiTiet(int id)
        {
            return RedirectToAction("ChiTiet", new { id = id });
        }

        private ActionResult RedirectToIndex()
        {
            return RedirectToAction("Index");
        }
    }
}
