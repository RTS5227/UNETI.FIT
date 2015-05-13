using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using UNETI.FIT.Models.Entity;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;

namespace UNETI.FIT.Infrastructure
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext() : base("DefaultConnection") { }
        public DbSet<Article> Articles { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<Message> Messages { get; set; }

        //public DbSet<Attachment> Attachments { get; set; }


        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Confirm> Confirms { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<ClassOfStudy> ClassesOfStudy { get; set; }
        public DbSet<LevelOfTraining> LevelsOfTraining { get; set; }
    }
}