using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;

namespace UNETI.FIT.Areas.SubjectManager.Controllers
{
    public class StudentController : Controller
    {
        private IStudentRepository studentRepository;
        public StudentController(IStudentRepository repo)
        {
            studentRepository = repo;
        }

        //
        // GET: /sinhvien/1
        [HttpGet]
        [Authorize]
        public ActionResult Index(string id)
        {
            var result = studentRepository.GetByID(id);
            if (result == null) return HttpNotFound();
            return View(result);
        }
    }
}
