using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using authorizationRoles.Data;
using authorizationRoles.Models;

namespace authorizationRoles.Pages.Instructors
{
    public class DetailsModel : PageModel
    {
        private readonly authorizationRoles.Data.ApplicationDbContext _context;

        public DetailsModel(authorizationRoles.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Instructor Instructor { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.InstructorId == id);

            if (Instructor == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
