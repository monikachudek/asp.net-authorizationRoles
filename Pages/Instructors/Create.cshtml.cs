using authorizationRoles.Data;
using authorizationRoles.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace authorizationRoles.Pages.Instructors
{
    public class CreateModel : InstructorCoursePageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            PopulateAssignedCourseData(_context, instructor);
            return Page();
        }

        [BindProperty]
        public Instructor Instructor { get; set; }

        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string[] selectedCourses)
        {
            var newInstructor = new Instructor();
            if(selectedCourses != null)
            {
                newInstructor.CourseAssignments = new List<CourseAssignment>();
                foreach( var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment { CourseID = int.Parse(course) };
                    newInstructor.CourseAssignments.Add(courseToAdd);
                }
            }

            var temp = await TryUpdateModelAsync<Instructor>(
                newInstructor,
                "Instructor",
                i => i.FirstName, i => i.LastName, i => i.HireDate, i => i.OfficeAssignment);

            if (temp)
            {
                _context.Add(newInstructor);
                await _context.SaveChangesAsync();
                return RedirectToPage("./Index");
            }

            PopulateAssignedCourseData(_context, newInstructor);
            return Page();
        }
    }
}
