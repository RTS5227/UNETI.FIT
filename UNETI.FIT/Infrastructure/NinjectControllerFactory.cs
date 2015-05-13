using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using UNETI.FIT.Infrastructure.Concrete;
using UNETI.FIT.Areas.SubjectManager.Infrastructure.Concrete;

namespace UNETI.FIT.Infrastructure
{
    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel kernel;

        public NinjectControllerFactory()
        {
            kernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            return controllerType == null ? null : (IController)kernel.Get(controllerType);
        }

        private void AddBindings()
        {
            kernel.Bind<IArticleRepository>().To<ArticleRepository>();
            kernel.Bind<ITagRepository>().To<TagRepository>();
            kernel.Bind<ICategoryRepository>().To<CategoryRepository>();

            kernel.Bind<IContactProcessor>().To<EmailContactProcessor>();

            kernel.Bind<IMessageRepository>().To<MessageRepository>();

            kernel.Bind<IModuleRepository>().To<ModuleRepository>();
            kernel.Bind<IStudentRepository>().To<StudentRepository>();
            kernel.Bind<ITeacherRepository>().To<TeacherRepository>();
            kernel.Bind<ISubjectRepository>().To<SubjectRepository>();
            kernel.Bind<IConfirmRepository>().To<ConfirmRepository>();
            kernel.Bind<IReportRepository>().To<ReportRepository>();
            kernel.Bind<ICommentRepository>().To<CommentRepository>();
        }
    }
}