using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UNETI.FIT.Infrastructure.Abstract;
using UNETI.FIT.Models.Entity;

namespace UNETI.FIT.Infrastructure.Concrete
{
    public class AttachmentRepository : RepositoryBase<Attachment>, IAttachmentRepository
    {
    }

    public interface IAttachmentRepository : IRepository<Attachment> { }
}