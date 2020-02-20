using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using authorizationRoles.Data;
using authorizationRoles.Models;
using Microsoft.AspNetCore.Authorization;
using authorizationRoles.Authorization;
using Microsoft.AspNet.Identity;

namespace authorizationRoles.Pages.Students
{
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(ApplicationDbContext context,
                           IAuthorizationService authorizationService)
            : base(context, authorizationService)
        {
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Student = await Context.Students.
                FirstOrDefaultAsync(c => c.StudentId == id);

            if (Student == null) return NotFound();

            var isAuthorized = await AuthorizationService.
                AuthorizeAsync(User, Student, StudentOperations.Delete);

            if (!isAuthorized.Succeeded) return Forbid();

            return Page();


        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var contact = await Context.Students.AsNoTracking().
                 FirstOrDefaultAsync(c => c.StudentId == id);

            if (contact == null) return NotFound();

            var isAuthorized = await AuthorizationService.
                AuthorizeAsync(User, contact, StudentOperations.Delete);

            if (!isAuthorized.Succeeded) return Forbid();

            Context.Students.Remove(contact);

            await Context.SaveChangesAsync();
            
            return RedirectToPage("./Index");
        }
    }
}
