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

namespace authorizationRoles.Pages.Contacts
{
    public class IndexModel : DI_BasePageModel
    {
        public IndexModel(ApplicationDbContext context,
                          IAuthorizationService authorizationService)
        : base(context, authorizationService)
        {
        }

        public IList<Contact> Contacts { get;set; }

        public string NameSort { get; set; }
        public string StatusSort { get; set; }
        public string CurrentFilter { get; set; }

        public async Task OnGetAsync(string sortOrder, string searchString)
        {
            var contacts = from c in Context.Contact
                           select c;

            var isAuthorized = User.IsInRole(Authorization.Constants.ContactManagersRole) ||
                               User.IsInRole(Authorization.Constants.ContactAdministratorsRole);

            var currentUserId = User.Identity.GetUserId();

            // Only approved contacts are shown UNLESS you're authorized to see them
            // or you are the owner.
            if (!isAuthorized)
            {
                contacts = contacts.Where(c => c.Status == ContactStatus.Approved
                                            || c.OwnerID == currentUserId);
            }

            // sorting
            NameSort = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            StatusSort = sortOrder == "Status" ? "status_desc" : "Status";

            // filter
            CurrentFilter = searchString;

            if (!String.IsNullOrEmpty(searchString))
            {
                contacts = contacts.Where(c => c.FirstName.Contains(searchString) ||
                            c.LastName.Contains(searchString));
            }
            
            switch (sortOrder)
            {
                case "name_desc":
                    contacts = contacts.OrderByDescending(c => c.LastName);
                    break;
                case "Status":
                    contacts = contacts.OrderBy(c => c.Status);
                    break;
                case "status_desc":
                    contacts = contacts.OrderByDescending(c => c.Status);
                    break;
                default:
                    contacts = contacts.OrderBy(c => c.LastName);
                    break;
            }


            Contacts = await contacts.ToListAsync();
        }
    }
}
