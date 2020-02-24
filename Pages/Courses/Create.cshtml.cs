using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using authorizationRoles.Data;
using authorizationRoles.Models;

namespace authorizationRoles.Pages.Courses
{
    public class CreateModel : DepartmentNamePageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopulateDepartmentsDropDownList(_context);
            return Page();
        }

        [BindProperty]
        public Course Course { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {

            var emptyCourse = new Course();

            if( await TryUpdateModelAsync<Course>(  // to prevent overposting - change just desired fields
                emptyCourse,
                "course",
                s => s.CourseID, s => s.DepartmentID, s => s.Title, s => s.Credits))
            {
                _context.Courses.Add(emptyCourse);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateDepartmentsDropDownList(_context, emptyCourse.DepartmentID);
            return Page();
        }
    }
}
