using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using authorizationRoles.Data;
using authorizationRoles.Models;
using Microsoft.AspNetCore.Authorization;
using authorizationRoles.Authorization;
using Microsoft.AspNet.Identity;

namespace authorizationRoles.Pages.Students
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(ApplicationDbContext context,
                         IAuthorizationService authorizationService)
            : base(context, authorizationService)
        {
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Student = await Context.Students.FirstOrDefaultAsync(c => c.StudentId == id);

            if (Student == null) return BadRequest();

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User,
                                                                         Student,
                                                                         StudentOperations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            var contact = await Context.Students.AsNoTracking().FirstOrDefaultAsync(c => c.StudentId == id);

            if (contact == null) return NotFound();

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User,
                                                                         contact,
                                                                         StudentOperations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Student.OwnerID = contact.OwnerID;
            Context.Attach(Student).State = EntityState.Modified;

            if(Student.Status == StudentStatus.Approved)
            {
                var canApproved = await AuthorizationService.AuthorizeAsync(User, Student, StudentOperations.Approve);
                if (!canApproved.Succeeded)
                {
                    Student.Status = StudentStatus.Submittet;
                }
            }

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
