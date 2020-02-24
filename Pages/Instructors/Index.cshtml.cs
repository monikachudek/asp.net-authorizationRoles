using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using authorizationRoles.Data;
using authorizationRoles.Models;
using authorizationRoles.Models.SchoolModels;

namespace authorizationRoles.Pages.Instructors
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public InstructorIndexData InstructorData { get; set; }
        public int InstructorId { get; set; }
        public int CourseId{ get; set; }

        public async Task OnGetAsync(int? id, int? courseId)
        {
            InstructorData = new InstructorIndexData();
            InstructorData.Instructors = await _context.Instructors
                                                   .Include(i => i.OfficeAssignment)
                                                   .Include(i => i.CourseAssignments)
                                                        .ThenInclude(i => i.Course)
                                                            .ThenInclude(i => i.Department)
                                                   //.Include(i => i.CourseAssignments)
                                                   //     .ThenInclude(i => i.Course)
                                                   //         .ThenInclude(i => i.Enrollments)
                                                   //             .ThenInclude(i => i.Student)
                                                   //.AsNoTracking()
                                                   .OrderBy(i => i.LastName)
                                                   .ToListAsync();
            if(id != null)
            {
                InstructorId = id.Value;
                Instructor instructor = InstructorData.Instructors
                                            .Single(i => i.InstructorId == id.Value);
                InstructorData.Courses = instructor.CourseAssignments.Select(s => s.Course);
            }

            if(courseId != null)
            {
                CourseId = courseId.Value;
                var selectedCourse = InstructorData.Courses
                    .Single(c => c.CourseID == courseId.Value);

                await _context.Entry(selectedCourse).Collection(x => x.Enrollments).LoadAsync();
                foreach(Enrollment enrollment in selectedCourse.Enrollments)
                {
                    await _context.Entry(enrollment).Reference(x => x.Student).LoadAsync();
                }
                InstructorData.Enrollments = selectedCourse.Enrollments;
            }

        }
    }
}
