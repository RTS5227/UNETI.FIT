namespace UNETI.FIT.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Web.Security;
    using UNETI.FIT.Areas.SubjectManager.Models.Entity;
    using UNETI.FIT.Infrastructure;
    using UNETI.FIT.Models.Entity;
    using WebMatrix.WebData;


    internal sealed class Configuration
        : DbMigrationsConfiguration<ApplicationDBContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(ApplicationDBContext context)
        {
            if (context.UserProfiles.Count() > 0) return;
            string[] articles = "News;Training;Students;OPL;Admissions;Research".Split(';');
            string[] announcements = "Announcement;Practice;TradeUnion;Youth;Gallery".Split(';');
            string[] navigators = "About;Contact".Split(';');
            //category
            for (int i = 0; i < articles.Length; i++)
            {
                context.Categories.AddOrUpdate(new Category
                {
                    Name = articles[i],
                    Order = i,
                    Type = CategoryTypeEnum.Article
                });
            }
            for (int i = 0; i < announcements.Length; i++)
            {
                context.Categories.AddOrUpdate(new Category
                {
                    Name = announcements[i],
                    Order = i,
                    Type = CategoryTypeEnum.Announcement
                });
            }
            for (int i = 0; i < navigators.Length; i++)
            {
                context.Categories.AddOrUpdate(new Category
                {
                    Name = navigators[i],
                    Order = i,
                    Type = CategoryTypeEnum.Navigator
                });
            }

            //tags
            context.Tags.AddOrUpdate(new Tag
            {
                TagDescription = "tintuc-sukien"
            });
            context.Tags.AddOrUpdate(new Tag
            {
                TagDescription = "khoahoc"
            });
            context.Tags.AddOrUpdate(new Tag
            {
                TagDescription = "tuyensinh"
            });
            context.Tags.AddOrUpdate(new Tag
            {
                TagDescription = "sinhvien"
            });
            context.Tags.AddOrUpdate(new Tag
            {
                TagDescription = "olympic"
            });
            context.Tags.AddOrUpdate(new Tag
            {
                TagDescription = "daotao"
            });
            context.Tags.AddOrUpdate(new Tag
            {
                TagDescription = "uneti"
            });
            context.Tags.AddOrUpdate(new Tag
            {
                TagDescription = "khoacntt"
            });

            //QLDA
            context.ClassesOfStudy.AddOrUpdate(
                p => p.Name,
                new ClassOfStudy { Name = "ĐH CNTT 5A NĐ" },
                new ClassOfStudy { Name = "ĐH CNTT 5A HN" },
                new ClassOfStudy { Name = "ĐH CNTT 6A NĐ" },
                new ClassOfStudy { Name = "ĐH CNTT 6A HN" },
                new ClassOfStudy { Name = "CĐ CNTT 21 NĐ" },
                new ClassOfStudy { Name = "CĐ CNTT 21 HN" },
                new ClassOfStudy { Name = "CĐ CNTT 22 NĐ" },
                new ClassOfStudy { Name = "CĐ CNTT 22 HN" }
                );

            context.LevelsOfTraining.AddOrUpdate(
                p => p.Name,
                new LevelOfTraining { Name = "Đại học chính quy" },
                new LevelOfTraining { Name = "Cao đẳng chính quy" },
                new LevelOfTraining { Name = "Đại học liên thông" },
                new LevelOfTraining { Name = "Cao đẳng liên thông" },
                new LevelOfTraining { Name = "Trung cấp" }
                );

            context.Faculties.AddOrUpdate(
                p => p.Name,
                new Faculty { Name = "Công nghệ thông tin" }
                );

            context.Modules.AddOrUpdate(
                p => p.Name,
                new Module { Name = "Lập trình Android" },
                new Module { Name = "Ứng dụng dữ liệu WEB" },
                new Module { Name = "Truyền thông đa phương tiện" },
                new Module { Name = "Công nghệ phần mềm" },
                new Module { Name = "Phát triển mã nguồn mở" },
                new Module { Name = "Kỹ thuật Vi xử lý (CNTT)" },
                new Module { Name = "Công nghệ Java" },
                new Module { Name = "Phân tích và thiết kế hệ thống thông tin" }
                );

            SeedMembership();
        }

        private void SeedMembership()
        {
            WebSecurity.InitializeDatabaseConnection("DefaultConnection", "UserProfile", "ID", "UserName", autoCreateTables: true);
            var role = (SimpleRoleProvider)Roles.Provider;
            var membership = (SimpleMembershipProvider)Membership.Provider;

            if (!role.RoleExists("admin")) role.CreateRole("admin");
            if (!role.RoleExists("teacher")) role.CreateRole("teacher");
            if (!role.RoleExists("student")) role.CreateRole("student");

            if (membership.GetUser("admin", false) == null)
            {
                membership.CreateUserAndAccount("admin", "secret");
            }
            if (!role.GetRolesForUser("admin").Contains("admin"))
            {
                role.AddUsersToRoles(new[] { "admin" }, new[] { "admin" });
            }
        }
    }
}
