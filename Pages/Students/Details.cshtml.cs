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

namespace authorizationRoles.Pages.Students
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(ApplicationDbContext context,
                            IAuthorizationService authorizationService)
            : base(context, authorizationService)
        {
        }

        public Student Student { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Student = await Context.Students.FirstOrDefaultAsync(c => c.StudentId == id);

            if (Student == null) return NotFound();

            var isAuthorized = (Student.Status == StudentStatus.Approved ||
                                User.IsInRole(Constants.StudentAdministratorsRole) ||
                                User.IsInRole(Constants.StudentManagersRole));

            if (!isAuthorized) return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, StudentStatus status)
        {
            var contact = await Context.Students.FirstOrDefaultAsync(c => c.StudentId == id);

            if (contact == null) return BadRequest();

            var contactOperation = (status == StudentStatus.Approved)
                                           ? StudentOperations.Approve
                                           : StudentOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, contact, contactOperation);

            if (!isAuthorized.Succeeded) return Forbid();

            contact.Status = status;
            Context.Students.Update(contact);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
