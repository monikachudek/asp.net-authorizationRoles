using authorizationRoles.Data;
using authorizationRoles.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

// dotnet aspnet-codegenerator razorpage -m Student -dc ApplicationDbContext -udl -outDir Pages\Students --referenceScriptLibraries

namespace authorizationRoles.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider, string testUserPw)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // For sample purposes seed both with the same password.
                // Password is set with the following:
                // dotnet user-secrets set SeedUserPW <pw>
                // The admin user can do anything

                var adminID = await EnsureUser(serviceProvider, testUserPw, "admin@contoso.com");
                await EnsureRole(serviceProvider, adminID, "StudentAdministrators");

                // allowed user can create and edit students that they create
                var managerID = await EnsureUser(serviceProvider, testUserPw, "manager@contoso.com");
                await EnsureRole(serviceProvider, managerID, "StudentManagers");

                SeedDB(context, adminID);
            }
        }

        private static async Task<string> EnsureUser(IServiceProvider serviceProvider,
                                            string testUserPw, string UserName)
        {
            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
            {
                user = new IdentityUser
                {
                    UserName = UserName,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(user, testUserPw);
            }

            if (user == null)
            {
                throw new Exception("The password is probably not strong enough!");
            }

            return user.Id;
        }

        private static async Task<IdentityResult> EnsureRole(IServiceProvider serviceProvider,
                                                              string uid, string role)
        {
            IdentityResult IR = null;
            var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            if (roleManager == null)
            {
                throw new Exception("roleManager null");
            }

            if (!await roleManager.RoleExistsAsync(role))
            {
                IR = await roleManager.CreateAsync(new IdentityRole(role));
            }

            var userManager = serviceProvider.GetService<UserManager<IdentityUser>>();

            var user = await userManager.FindByIdAsync(uid);

            if (user == null)
            {
                throw new Exception("The testUserPw password was probably not strong enough!");
            }

            IR = await userManager.AddToRoleAsync(user, role);

            return IR;
        }

        public static void SeedDB(ApplicationDbContext context, string adminID)
        {
            if (!context.Students.Any())
            {
                context.Students.AddRange(
                    new Student
                    {
                        FirstName= "Hermiona",
                        LastName = "Granger",
                        Address = "1234 Main St",
                        City = "London",
                        State = "WA",
                        Zip = "10999",
                        Email = "hermiona@example.com",
                        Status = StudentStatus.Approved,
                        OwnerID = adminID,
                        EnrollmentDate = DateTime.Parse("2019-09-01")
                    },
                    new Student
                    {
                        FirstName = "Harry",
                        LastName = "Potter",
                        Address = "5678 1st Ave W",
                        City = "London",
                        State = "WA",
                        Zip = "10999",
                        Email = "potterh@example.com",
                        Status = StudentStatus.Rejected,
                        OwnerID = adminID,
                        EnrollmentDate = DateTime.Parse("2017-09-01")
                    },
                 new Student
                 {
                     FirstName = "Ronald",
                     LastName = "Weasley",
                     Address = "9012 State st",
                     City = "Cambridge",
                     State = "WA",
                     Zip = "10999",
                     Email = "weasleyr@example.com",
                     Status = StudentStatus.Submittet,
                     OwnerID = adminID,
                     EnrollmentDate = DateTime.Parse("2018-09-01")
                 },
                 new Student
                 {
                     FirstName = "Neville",
                     LastName = "Longbottom",
                     Address = "3456 Maple St",
                     City = "Birmingham",
                     State = "WA",
                     Zip = "10999",
                     Email = "nevil@example.com",
                     Status = StudentStatus.Approved,
                     OwnerID = adminID,
                     EnrollmentDate = DateTime.Parse("2019-09-10")
                 },
                 new Student
                 {
                     FirstName = "Draco",
                     LastName = "Malfoy",
                     Address = "7890 2nd Ave E",
                     City = "Bournemouth",
                     State = "WA",
                     Zip = "10999",
                     Email = "malfoy@example.com",
                     Status = StudentStatus.Approved,
                     OwnerID = adminID,
                     EnrollmentDate = DateTime.Parse("2016-09-01")
                 }
                 );
                context.SaveChanges();
            }

            var instructors = new Instructor[]
            {
                new Instructor
                    {
                        FirstName = " Filius",
                        LastName = "Flitwick",
                        HireDate = DateTime.Parse("1995-03-11")
                    },
                new Instructor
                {
                    FirstName = "Severus",
                    LastName = "Snape",
                    HireDate = DateTime.Parse("2002-07-06")
                },
                new Instructor
                {
                    FirstName = "Rolanda",
                    LastName = "Hooch",
                    HireDate = DateTime.Parse("1998-07-01")
                },
                new Instructor
                {
                    FirstName = "Pomona",
                    LastName = "Sprout",
                    HireDate = DateTime.Parse("2001-01-15")
                },
                new Instructor
                {
                    FirstName = "Minerva",
                    LastName = "McGonagall",
                    HireDate = DateTime.Parse("2004-02-12")
                }
            };

            if (!context.Instructors.Any())
            {
                context.Instructors.AddRange(
                    instructors);
                context.SaveChanges();
            }


                var departments = new Department[]
                {
                    new Department
                    {
                        Name = "A",
                        Budget = 350000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID = instructors.Single(i => i.LastName == "McGonagall").InstructorId
                    },
                    new Department
                    {
                        Name = "B",
                        Budget = 100000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID = instructors.Single(i => i.LastName == "Snape").InstructorId
                    },
                    new Department
                    {
                        Name = "C",
                        Budget = 350000,
                        StartDate = DateTime.Parse("2007-09-01"),
                        InstructorID = instructors.Single(i => i.LastName == "Flitwick").InstructorId
                    }
                };

            if (!context.Departments.Any())
            {

                context.Departments.AddRange(departments) ;
                context.SaveChanges();
            }

            var courses = new Course[]{
                new Course { CourseID = 1050, Title = "Transfiguration", Credits = 3, DepartmentID = departments.Single(d => d.Name == "A").DepartmentId },
                new Course { CourseID = 4022, Title = "Charms", Credits = 3, DepartmentID = departments.Single(d => d.Name == "C").DepartmentId },
                new Course { CourseID = 4041, Title = "Defence Against the Dark Arts", Credits = 3, DepartmentID = departments.Single(d => d.Name == "B").DepartmentId },
                new Course { CourseID = 1045, Title = "Flying", Credits = 4, DepartmentID = departments.Single(d => d.Name == "A").DepartmentId },
                new Course { CourseID = 3141, Title = "History of Magic", Credits = 4, DepartmentID = departments.Single(d => d.Name == "C").DepartmentId },
                new Course { CourseID = 2021, Title = "Herbology", Credits = 3, DepartmentID = departments.Single(d => d.Name == "A").DepartmentId },
                new Course { CourseID = 2042, Title = "Potions", Credits = 4, DepartmentID = departments.Single(d => d.Name == "B").DepartmentId }
            };

            if (!context.Courses.Any())
            {
                context.Courses.AddRange(
                    courses);
                context.SaveChanges();
            }

            if (!context.OfficeAssignments.Any())
            {
                context.OfficeAssignments.AddRange(
                   new OfficeAssignment
                   {
                       InstructorId = instructors.Single(i => i.LastName == "McGonagall").InstructorId,
                       Location = "Smith 17"
                   },
                new OfficeAssignment
                {
                    InstructorId = instructors.Single(i => i.LastName == "Snape").InstructorId,
                    Location = "Gowan 27"
                },
                new OfficeAssignment
                {
                    InstructorId = instructors.Single(i => i.LastName == "Flitwick").InstructorId,
                    Location = "Thompson 304"
                });
                context.SaveChanges();
            }


            if (!context.CourseAssignments.Any())
            {
                context.CourseAssignments.AddRange(
                   new CourseAssignment
                   {
                       CourseID = courses.Single(c => c.Title == "Transfiguration").CourseID,
                       InstructorID = instructors.Single(i => i.LastName == "McGonagall").InstructorId
                   },
               new CourseAssignment
               {
                   CourseID = courses.Single(c => c.Title == "Charms").CourseID,
                   InstructorID = instructors.Single(i => i.LastName == "Flitwick").InstructorId
               },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Defence Against the Dark Arts").CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Snape").InstructorId
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Flying").CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Hooch").InstructorId
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "History of Magic").CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Flitwick").InstructorId
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Herbology").CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Sprout").InstructorId
                },
                new CourseAssignment
                {
                    CourseID = courses.Single(c => c.Title == "Potions").CourseID,
                    InstructorID = instructors.Single(i => i.LastName == "Snape").InstructorId
                });
                context.SaveChanges();
            }



            if (!context.Enrollments.Any())
            {
                context.Enrollments.AddRange(
                new Enrollment { StudentID = 1, CourseID = 1050, Grade = Grade.A },
                new Enrollment { StudentID = 1, CourseID = 4022, Grade = Grade.C },
                new Enrollment { StudentID = 1, CourseID = 4041, Grade = Grade.B },
                new Enrollment { StudentID = 2, CourseID = 1045, Grade = Grade.B },
                new Enrollment { StudentID = 2, CourseID = 3141, Grade = Grade.F },
                new Enrollment { StudentID = 2, CourseID = 2021, Grade = Grade.F },
                new Enrollment { StudentID = 3, CourseID = 1050 },
                new Enrollment { StudentID = 4, CourseID = 1050 },
                new Enrollment { StudentID = 4, CourseID = 4022, Grade = Grade.F },
                new Enrollment { StudentID = 5, CourseID = 4041, Grade = Grade.C },
                new Enrollment { StudentID = 2, CourseID = 1045 }
                );

              context.SaveChanges();
            }

        }

    }
}