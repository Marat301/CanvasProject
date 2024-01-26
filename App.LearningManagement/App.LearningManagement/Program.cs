using App.LearningManagement.Helpers;
using Library.LearningManagement.Models;

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
                Console.WriteLine("4. Add a new course");
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
                    } else if (result == 3)
                    {
                        studentHelper.SearchStudents();
                    } else if (result == 4)
                    {
                        courseHelper.CreateCourseRecord();
                    }
                }
            }
        }
    }
}