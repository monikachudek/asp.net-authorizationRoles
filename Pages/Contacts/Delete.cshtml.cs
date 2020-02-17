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

namespace authorizationRoles.Pages.Contacts
{
    public class DeleteModel : DI_BasePageModel
    {
        public DeleteModel(ApplicationDbContext context,
                           IAuthorizationService authorizationService)
            : base(context, authorizationService)
        {
        }

        [BindProperty]
        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Contact = await Context.Contact.
                FirstOrDefaultAsync(c => c.ContactId == id);

            if (Contact == null) return NotFound();

            var isAuthorized = await AuthorizationService.
                AuthorizeAsync(User, Contact, ContactOperations.Delete);

            if (!isAuthorized.Succeeded) return Forbid();

            return Page();


        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            var contact = await Context.Contact.AsNoTracking().
                 FirstOrDefaultAsync(c => c.ContactId == id);

            if (contact == null) return NotFound();

            var isAuthorized = await AuthorizationService.
                AuthorizeAsync(User, contact, ContactOperations.Delete);

            if (!isAuthorized.Succeeded) return Forbid();

            Context.Contact.Remove(contact);

            await Context.SaveChangesAsync();
            
            return RedirectToPage("./Index");
        }
    }
}
