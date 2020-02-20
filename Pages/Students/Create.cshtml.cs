using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using authorizationRoles.Data;
using authorizationRoles.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNet.Identity.EntityFramework;
using authorizationRoles.Authorization;

namespace authorizationRoles.Pages.Students
{
    public class CreateModel : DI_BasePageModel

    {
        public CreateModel(
            ApplicationDbContext context,
            IAuthorizationService authorizationService)
            : base(context, authorizationService)
        {
        }

        public IActionResult OnGet()
        {
            Student = new Student
            {
                FirstName = "Edward",
                LastName = "Norton",
                Address = "123 N 456 S",
                City = "GF",
                State = "MT",
                Zip = "59405",
                Email = "rick@example.com"
            };
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Student.OwnerID = User.Identity.GetUserId();

            // requires using StudentManager.Authorization;
            var isAuthorized = await AuthorizationService.AuthorizeAsync(
                                                        User, Student,
                                                        StudentOperations.Create);
            if (!isAuthorized.Succeeded)
            {
                return Forbid();
            }

            Context.Students.Add(Student);
            await Context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}