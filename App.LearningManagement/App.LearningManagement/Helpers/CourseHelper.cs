using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace App.LearningManagement.Helpers
{
    public class CourseHelper
    {
        private CourseService courseService;
        private StudentService studentService;

        public CourseHelper()
        {
            studentService = StudentService.Current;
            courseService = CourseService.Current;
        }
        public void CreateCourseRecord(Course? selectedCourse = null)
        {
            Console.WriteLine("What is the name for the course?");
            var name = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the code for the course?");
            var code = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("What is the description of the course?");
            var description = Console.ReadLine() ?? string.Empty;

            Console.WriteLine("Which students (ID) should be enrolled in this course? ('Q' to Quit)");
            var roster = new List<Person>();
            bool continueAdding = true;
            while (continueAdding)
            {
                studentService.Students.Where(s => !roster.Any(s2 => s2.ID == s.ID)).ToList().ForEach(Console.WriteLine);
                var selection = "Q";
                if (studentService.Students.Any(s => !roster.Any(s2 => s2.ID == s.ID)))
                {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if (selection.Equals("Q", StringComparison.InvariantCultureIgnoreCase))
                {
                    continueAdding = false;
                } else
                {
                    var selectedID = int.Parse(selection);
                    var selectedStudent = studentService.Students.FirstOrDefault(s => s.ID == selectedID);

                    if (selectedStudent != null)
                    {
                        roster.Add(selectedStudent);
                    }
                }
            }

            Console.WriteLine("Would you like to add assignments? (Y/N)");
            var assignmentResponse = Console.ReadLine() ?? "N";
            var assignments = new List<Assignment>();
            if (assignmentResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) {
                continueAdding = true;
                while (continueAdding)
                {
                    // Name
                    Console.WriteLine("Name: ");
                    var assignmentName = Console.ReadLine() ?? string.Empty;

                    // Description
                    Console.WriteLine("Description: ");
                    var assignmentDescription = Console.ReadLine() ?? string.Empty;

                    // Total points
                    Console.WriteLine("Total Points: ");
                    var assignmentTotalPoints = decimal.Parse(Console.ReadLine() ?? "100");

                    // Due Date
                    Console.WriteLine("Due Date: ");
                    var assignmentDueDate = DateTime.Parse(Console.ReadLine() ?? "01/01/1900");

                    assignments.Add(new Assignment
                    {
                        Name = assignmentName, Description = assignmentDescription,
                        TotalAvailablePoints = assignmentTotalPoints, DueDate = assignmentDueDate
                    });

                    Console.WriteLine("Add more courses? (Y/N)");
                    assignmentResponse = Console.ReadLine() ?? "N";
                    if (assignmentResponse.Equals("N", StringComparison.InvariantCultureIgnoreCase))
                    {
                        continueAdding = false;
                    }
                }
            }

            bool isNewCourse = false;
            if (selectedCourse == null)
            {
                isNewCourse = true;
                selectedCourse = new Course();
            }

            selectedCourse.Code = code;
            selectedCourse.Name = name;
            selectedCourse.Description = description;
            selectedCourse.Roster = new List<Person>();
            selectedCourse.Roster.AddRange(roster);
            selectedCourse.Assignments = new List<Assignment>();
            selectedCourse.Assignments.AddRange(assignments);

            if (isNewCourse)
            {
                courseService.Add(selectedCourse);
            }
        }

        public void UpdateCourseRecord()
        {
            Console.WriteLine("Enter the course code to update");
            ListCourses();

            var selection = Console.ReadLine();

            var selectedCourse = courseService.Courses.FirstOrDefault(s => s.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase));
            if (selectedCourse != null)
            {
                CreateCourseRecord(selectedCourse);
            }
        }
        public void ListCourses()
        {
            courseService.Courses.ForEach(Console.WriteLine);
        }

        public void SearchCourses()
        {
            Console.WriteLine("Enter the course: ");
            var query = Console.ReadLine() ?? string.Empty;

            courseService.Search(query).ToList().ForEach(Console.WriteLine);
        }
    }
}
