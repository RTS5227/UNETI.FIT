using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UNETI.FIT.Infrastructure;

namespace UNETI.FIT.Infrastructure.Abstract
{
    public interface IDBFacetory:IDisposable
    {
        ApplicationDBContext Get();
    }
}