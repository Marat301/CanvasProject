using App.LearningManagement.Helpers;

namespace CanvasProject {
    internal class Program {
        static void Main(string[] args) {
            var studentHelper = new StudentHelper();
            var courseHelper = new CourseHelper();

            bool cont = true;
            while (cont) {
                Console.WriteLine("0. Exit");                   // sys
                Console.WriteLine("1. Maintain Students");
                Console.WriteLine("2. Maintain Courses");

                var input = Console.ReadLine();
                if (int.TryParse(input, out int result)) {
                    if (result == 0) {
                        cont = false;
                    } else if (result == 1) {
                        ShowStudentMenu(studentHelper);
                    } else if (result == 2) {
                        ShowCourseMenu(courseHelper);
                    }
                }
            }
        }

        static void ShowStudentMenu(StudentHelper studentHelper) {
            Console.WriteLine("Choose an action: ");
            Console.WriteLine("1. Add a student");          // student
            Console.WriteLine("2. List all students");      // student
            Console.WriteLine("3. Search for a student");   // student
            Console.WriteLine("4. Update a student");       // student

            var input = Console.ReadLine();
            if (int.TryParse(input, out int result)) {
                if (result == 1) {
                    studentHelper.CreateStudentRecord();
                } else if (result == 2) {
                    studentHelper.ListStudents();
                } else if (result == 3) {
                    studentHelper.SearchStudents();
                } else if (result == 4) {
                    studentHelper.UpdateStudentRecord();
                }
            }

        }

        static void ShowCourseMenu(CourseHelper courseHelper) {
            Console.WriteLine("1. Add a new course");       // course
            Console.WriteLine("2. List all courses");       // course
            Console.WriteLine("3. Search for a course");    // course
            Console.WriteLine("4. Update a course");        // course

            var input = Console.ReadLine();
            if (int.TryParse(input, out int result)) {
                if (result == 1) {
                    courseHelper.CreateCourseRecord();
                } else if (result == 2) {
                    courseHelper.SearchCourses();
                } else if (result == 3) {
                    // get the name, description, or course of the code
                    Console.WriteLine("Enter the course name, description, or code: ");
                    var query = Console.ReadLine() ?? string.Empty;
                    courseHelper.SearchCourses(query);
                } else if (result == 4) {
                    courseHelper.UpdateCourseRecord();
                }
            }
        }
    }
}