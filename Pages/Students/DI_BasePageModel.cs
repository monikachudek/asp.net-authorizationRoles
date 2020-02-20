using authorizationRoles.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace authorizationRoles.Pages.Students
{
    
        public class DI_BasePageModel : PageModel
        {
            protected ApplicationDbContext Context { get; }
            protected IAuthorizationService AuthorizationService { get; }

            public DI_BasePageModel(
                ApplicationDbContext context,
                IAuthorizationService authorizationService) : base()
            {
                Context = context;
                AuthorizationService = authorizationService;
            }
        }
    
}
