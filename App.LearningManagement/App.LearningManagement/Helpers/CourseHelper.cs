using Library.LearningManagement.Models;
using Library.LearningManagement.Services;

namespace App.LearningManagement.Helpers {
    public class CourseHelper {
        private CourseService courseService;
        private StudentService studentService;

        // use the current services (singleton implementation)
        public CourseHelper() {
            studentService = StudentService.Current;
            courseService = CourseService.Current;
        }

        // creates a course
        public void CreateCourseRecord(Course? selectedCourse = null) {
            // get the name of the course
            Console.WriteLine("What is the name for the course?");
            var name = Console.ReadLine() ?? string.Empty;

            // get the code of the course
            Console.WriteLine("What is the code for the course?");
            var code = Console.ReadLine() ?? string.Empty;

            // get the description of the course
            Console.WriteLine("What is the description of the course?");
            var description = Console.ReadLine() ?? string.Empty;

            // add students to the course
            Console.WriteLine("Which students (ID) should be enrolled in this course? ('Q' to Quit)");
            var roster = new List<Person>();
            bool continueAdding = true;
            while (continueAdding) {
                // find and print all students who aren't enrolled yet
                studentService.Students.Where(
                    student => !roster.Any(enrolledStudent => enrolledStudent.ID == student.ID)
                ).ToList().ForEach(Console.WriteLine);

                var selection = "Q";
                // let the user pick another student if there are still students unenrolled in the course
                if (studentService.Students.Any(student => !roster.Any(enrolledStudent => enrolledStudent.ID == student.ID))) {
                    selection = Console.ReadLine() ?? string.Empty;
                }

                if (selection.Equals("Q", StringComparison.InvariantCultureIgnoreCase)) {
                    // if no more students or user enters 'Q', quit
                    continueAdding = false;
                } else {
                    var selectedID = int.Parse(selection);
                    // find the student with the given ID
                    var selectedStudent = studentService.Students.FirstOrDefault(student => student.ID == selectedID);

                    // add the student to the course
                    if (selectedStudent != null) {
                        roster.Add(selectedStudent);
                    }
                }
            }

            // add assignments to the course
            Console.WriteLine("Would you like to add assignments? (Y/N)");
            var assignmentResponse = Console.ReadLine() ?? "N";
            var assignments = new List<Assignment>();
            if (assignmentResponse.Equals("Y", StringComparison.InvariantCultureIgnoreCase)) {
                continueAdding = true;
                while (continueAdding) {
                    // get name of assignment
                    Console.WriteLine("Name: ");
                    var assignmentName = Console.ReadLine() ?? string.Empty;

                    // get description of assignment
                    Console.WriteLine("Description: ");
                    var assignmentDescription = Console.ReadLine() ?? string.Empty;

                    // get total points of assignment
                    Console.WriteLine("Total Points: ");
                    var assignmentTotalPoints = decimal.Parse(Console.ReadLine() ?? "100");

                    // get due date of assignment
                    Console.WriteLine("Due Date: ");
                    var assignmentDueDate = DateTime.Parse(Console.ReadLine() ?? "01/01/1900");

                    // add assignment
                    assignments.Add(new Assignment {
                        Name = assignmentName,
                        Description = assignmentDescription,
                        TotalAvailablePoints = assignmentTotalPoints,
                        DueDate = assignmentDueDate
                    });

                    // quit if not adding any more assignments
                    Console.WriteLine("Add more assignments? (Y/N)");
                    assignmentResponse = Console.ReadLine() ?? "N";
                    if (assignmentResponse.Equals("N", StringComparison.InvariantCultureIgnoreCase)) {
                        continueAdding = false;
                    }
                }
            }

            // check if we are updating or creating a course
            bool isNewCourse = selectedCourse == null;
            if (isNewCourse) {
                selectedCourse = new Course();
            }

            // set the course data
            selectedCourse.Code = code;
            selectedCourse.Name = name;
            selectedCourse.Description = description;
            selectedCourse.Roster = new List<Person>();
            selectedCourse.Roster.AddRange(roster);
            selectedCourse.Assignments = new List<Assignment>();
            selectedCourse.Assignments.AddRange(assignments);

            // add the course
            if (isNewCourse) {
                courseService.Add(selectedCourse);
            }
        }

        // updates a course
        public void UpdateCourseRecord() {
            Console.WriteLine("Enter the course code to update");
            SearchCourses();

            var selection = Console.ReadLine();

            // get course whose ID matches given ID
            var selectedCourse = courseService.Courses.FirstOrDefault(
                course => course.Code.Equals(selection, StringComparison.InvariantCultureIgnoreCase)
            );

            // reset all the course's data
            if (selectedCourse != null) {
                CreateCourseRecord(selectedCourse);
            }
        }

        // search for course and print all their data
        public void SearchCourses(string? query = null) {
            if (string.IsNullOrEmpty(query)) {
                courseService.Courses.ForEach(Console.WriteLine);
            } else {
                // find and print the course
                courseService.Search(query).ToList().ForEach(Console.WriteLine);
            }

            Console.WriteLine("Select a course:");
            var code = Console.ReadLine() ?? string.Empty;

            var selectedCourse = courseService.Courses.FirstOrDefault(c => c.Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));

            if (selectedCourse != null) {
                Console.WriteLine(selectedCourse.DetailDisplay);
            }

        }
    }
}
