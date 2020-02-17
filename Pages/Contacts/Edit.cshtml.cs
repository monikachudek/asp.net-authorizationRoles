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

namespace authorizationRoles.Pages.Contacts
{
    public class EditModel : DI_BasePageModel
    {
        public EditModel(ApplicationDbContext context,
                         IAuthorizationService authorizationService)
            : base(context, authorizationService)
        {
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Contact = await Context.Contact.FirstOrDefaultAsync(c => c.ContactId == id);

            if (Contact == null) return BadRequest();

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User,
                                                                         Contact,
                                                                         ContactOperations.Update);

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

            var contact = await Context.Contact.AsNoTracking().FirstOrDefaultAsync(c => c.ContactId == id);

            if (contact == null) return NotFound();

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User,
                                                                         contact,
                                                                         ContactOperations.Update);

            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Contact.OwnerID = contact.OwnerID;
            Context.Attach(Contact).State = EntityState.Modified;

            if(Contact.Status == ContactStatus.Approved)
            {
                var canApproved = await AuthorizationService.AuthorizeAsync(User, Contact, ContactOperations.Approve);
                if (!canApproved.Succeeded)
                {
                    Contact.Status = ContactStatus.Submittet;
                }
            }

            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
