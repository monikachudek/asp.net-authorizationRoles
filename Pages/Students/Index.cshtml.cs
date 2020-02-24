using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using authorizationRoles.Data;
using authorizationRoles.Models;
using Microsoft.AspNetCore.Authorization;
using authorizationRoles.Authorization;
using Microsoft.AspNet.Identity;
using System;

namespace authorizationRoles.Pages.Students
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(ApplicationDbContext context,
                          IAuthorizationService authorizationService)
        : base(context, authorizationService)
        {
        }

        public PaginatedList<Student> Students { get;set; }

        public string NameSort { get; set; }
        public string StatusSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public async Task OnGetAsync(string sortOrder,
                                     string currentFilter,
                                     string searchString,
                                     int? pageIndex)
        {

            var students = from c in Context.Students
                           select c;

            var students_ = students.ToList();

            var isAuthorized = User.IsInRole(Authorization.Constants.StudentManagersRole) ||
                               User.IsInRole(Authorization.Constants.StudentAdministratorsRole);

            var currentUserId = User.Identity.GetUserId();

            // Only approved students are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                students = students.Where(c => c.Status == StudentStatus.Approved
                                            || c.OwnerID == currentUserId);
            }

            // sorting
            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            StatusSort = sortOrder == "Status" ? "status_desc" : "Status";

            //pagination
            if (searchString != null) pageIndex = 1;
            else searchString = currentFilter;

            // filter
            CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                students = students.Where(c => c.FirstName.Contains(searchString) ||
                            c.LastName.Contains(searchString));
            }
            
            switch (sortOrder)
            {
                case "name_desc":
                    students = students.OrderByDescending(c => c.LastName);
                    break;
                case "Status":
                    students = students.OrderBy(c => c.Status);
                    break;
                case "status_desc":
                    students = students.OrderByDescending(c => c.Status);
                    break;
                default:
                    students = students.OrderBy(c => c.LastName);
                    break;
            }

            int pageSize = 10;
            int pageIndexUse = (pageIndex == null) ? 1: (int)pageIndex;

            //var count = await students.CountAsync();
            var count = 5;

            //var items = await students.Skip(
            //   (pageIndexUse - 1) * pageSize).Take(pageSize).ToListAsync();

            var items = await students.ToListAsync();

            //Students = new  PaginatedList<Student>(items, count, pageIndexUse, pageSize);
            Students = new PaginatedList<Student>(items, count, pageIndexUse, pageSize);
        }
    }
}
