using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;

namespace UNETI.FIT.Areas.SubjectManager.Controllers
{
    public class TeacherController : Controller
    {
        private ITeacherRepository teacherRepository;
        public TeacherController(ITeacherRepository repo)
        {
            teacherRepository = repo;
        }

        //
        // GET: /giangvien/1
        [HttpGet]
        [Authorize(Roles="teacher,admin")]
        public ActionResult Index(string id)
        {
            var result = teacherRepository.GetByID(id);
            if (result == null) return HttpNotFound();
            return View(result);
        }

    }
}
