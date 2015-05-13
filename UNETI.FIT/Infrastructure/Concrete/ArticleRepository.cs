using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Infrastructure.Abstract;

namespace UNETI.FIT.Infrastructure.Concrete
{
    public class ArticleRepository : RepositoryBase<Article>, IArticleRepository
    {
    }

    public interface IArticleRepository : IRepository<Article> { }
}