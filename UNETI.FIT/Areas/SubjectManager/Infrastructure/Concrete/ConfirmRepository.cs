using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;
using UNETI.FIT.Infrastructure.Abstract;

namespace UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete
{
    public class ConfirmRepository : RepositoryBase<Confirm>, IConfirmRepository
    {
        public void Confirm(int subjectID, string studentID)
        {
            
        }
    }

    public interface IConfirmRepository : IRepository<Confirm> { }
}