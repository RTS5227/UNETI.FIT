using UNETI.FIT.Areas.SubjectManager.Models;
using UNETI.FIT.Areas.SubjectManager.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using UNETI.FIT.Models.ViewModels;

namespace UNETI.FIT.Areas.SubjectManager.Infrastructure.Helpers
{
    public static class ViewModelHelper
    {
        public static IEnumerable<Student> Filter(this IQueryable<Student> entities, FilterModel filter)
        {
            //Sort
            switch (filter.SortOrder)
            {
                case "Name":
                    entities = entities.OrderBy(s => s.FullName);
                    break;
                case "Name_desc":
                    entities = entities.OrderByDescending(s => s.FullName);
                    break;
                case "MSGV":
                    entities = entities.OrderBy(s => s.ID);
                    break;
                case "MSGV_desc":
                    entities = entities.OrderByDescending(s => s.ID);
                    break;
                default:
                    entities = entities.OrderBy(s => s.ID);
                    break;
            }
            //Pagination
            filter.TotalItems = entities.Count();
            filter.TotalPages = (int)Math.Ceiling((double)filter.TotalItems / filter.PageSize);

            entities = entities
                .Skip(filter.PageSize * (filter.PageIndex - 1))
                .Take(filter.PageSize);

            return entities.ToList();
        }

        public static IEnumerable<Module> Filter(this IQueryable<Module> entities, FilterModel filter)
        {
            //Sort
            switch (filter.SortOrder)
            {
                case "Name":
                    entities = entities.OrderBy(s => s.Name);
                    break;
                case "Name_desc":
                    entities = entities.OrderByDescending(s => s.Name);
                    break;
                default:
                    entities = entities.OrderBy(s => s.Name);
                    break;
            }

            //Pagination
            filter.TotalItems = entities.Count();
            filter.TotalPages = (int)Math.Ceiling((double)filter.TotalItems / filter.PageSize);

            entities = entities
                .Skip(filter.PageSize * (filter.PageIndex - 1))
                .Take(filter.PageSize);

            return entities.ToList();
        }

        public static IEnumerable<Teacher> Filter(this IQueryable<Teacher> entities, FilterModel filter)
        {
            //Sort
            switch (filter.SortOrder)
            {
                case "Name":
                    entities = entities.OrderBy(s => s.FullName);
                    break;
                case "Name_desc":
                    entities = entities.OrderByDescending(s => s.FullName);
                    break;
                case "MSGV":
                    entities = entities.OrderBy(s => s.ID);
                    break;
                case "MSGV_desc":
                    entities = entities.OrderByDescending(s => s.ID);
                    break;
                default:
                    entities = entities.OrderBy(s => s.FullName);
                    break;
            }

            //Pagination
            filter.TotalItems = entities.Count();
            filter.TotalPages = (int)Math.Ceiling((double)filter.TotalItems / filter.PageSize);

            entities = entities
                .Skip(filter.PageSize * (filter.PageIndex - 1))
                .Take(filter.PageSize);

            return entities.ToList();
        }

        public static IEnumerable<Subject> Filter(this IQueryable<Subject> entities, FilterModel filter)
        {
            //Sort
            switch (filter.SortOrder.ToLower())
            {
                case "name":
                    entities = entities.OrderBy(s => s.Title);
                    break;
                case "view":
                    entities = entities.OrderByDescending(s => s.View);
                    break;
                case "register":
                    entities = entities.OrderByDescending(s => s.Confirms.Count);
                    break;
                case "date":
                    entities = entities.OrderByDescending(s => s.CreateAt);
                    break;
                default:
                    entities = entities.OrderByDescending(s => s.CreateAt);
                    break;
            }

            //Pagination
            filter.TotalItems = entities.Count();
            filter.TotalPages = (int)Math.Ceiling((double)filter.TotalItems / filter.PageSize);

            entities = entities
                .Skip(filter.PageSize * (filter.PageIndex - 1))
                .Take(filter.PageSize);

            return entities.ToList();
        }
    }
}