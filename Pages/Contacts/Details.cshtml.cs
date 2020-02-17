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

namespace authorizationRoles.Pages.Contacts
{
    public class DetailsModel : DI_BasePageModel
    {
        public DetailsModel(ApplicationDbContext context,
                            IAuthorizationService authorizationService)
            : base(context, authorizationService)
        {
        }

        public Contact Contact { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Contact = await Context.Contact.FirstOrDefaultAsync(c => c.ContactId == id);

            if (Contact == null) return NotFound();

            var isAuthorized = (Contact.Status == ContactStatus.Approved ||
                                User.IsInRole(Constants.ContactAdministratorsRole) ||
                                User.IsInRole(Constants.ContactManagersRole));

            if (!isAuthorized) return Forbid();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id, ContactStatus status)
        {
            var contact = await Context.Contact.FirstOrDefaultAsync(c => c.ContactId == id);

            if (contact == null) return BadRequest();

            var contactOperation = (status == ContactStatus.Approved)
                                           ? ContactOperations.Approve
                                           : ContactOperations.Reject;

            var isAuthorized = await AuthorizationService.AuthorizeAsync(User, contact, contactOperation);

            if (!isAuthorized.Succeeded) return Forbid();

            contact.Status = status;
            Context.Contact.Update(contact);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
