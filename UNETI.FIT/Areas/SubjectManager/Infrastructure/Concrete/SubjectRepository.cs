using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;
using UNETI.FIT.Infrastructure.Abstract;

namespace UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete
{
    public class SubjectRepository : RepositoryBase<Subject>, ISubjectRepository
    {
        public SubjectRepository()
        {
        }
    }

    public interface ISubjectRepository : IRepository<Subject>
    {
    }
}