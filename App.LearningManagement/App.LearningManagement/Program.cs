using App.LearningManagement.Helpers;

namespace CanvasProject {
    internal class Program {
        static void Main(string[] args) {
            var studentHelper = new StudentHelper();
            var courseHelper = new CourseHelper();

            bool cont = true;
            while (cont) {
                Console.WriteLine("Choose an action: ");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Add a student");
                Console.WriteLine("2. List all students");
                Console.WriteLine("3. Search for a student");
                Console.WriteLine("4. Update a student");
                Console.WriteLine("5. Add a new course");
                Console.WriteLine("6. List all courses");
                Console.WriteLine("7. Search for a course");
                Console.WriteLine("8. Update a course");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int result)) {
                    if (result == 0) {
                        cont = false;
                    } else if (result == 1) {                       
                        studentHelper.CreateStudentRecord();
                    } else if (result == 2) {
                        studentHelper.ListStudents();
                    } else if (result == 3) {
                        studentHelper.SearchStudents();
                    } else if (result == 4) {
                        studentHelper.UpdateStudentRecord();
                    } else if (result == 5) {
                        courseHelper.CreateCourseRecord();
                    } else if (result == 6) {
                        courseHelper.SearchCourses();
                    } else if (result == 7) {
                        // get the name, description, or course of the code
                        Console.WriteLine("Enter the course name, description, or code: ");
                        var query = Console.ReadLine() ?? string.Empty;
                        courseHelper.SearchCourses(query);
                    } else if (result == 8) {
                        courseHelper.UpdateCourseRecord();
                    }
                }
            }
        }
    }
}