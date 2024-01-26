using App.LearningManagement.Helpers;
using Library.LearningManagement.Models;

namespace CanvasProject {
    internal class Program {
        static void Main(string[] args) {
            var studentHelper = new StudentHelper();

            bool cont = true;
            while (cont) {
                Console.WriteLine("Choose an action: ");
                Console.WriteLine("0. Exit");
                Console.WriteLine("1. Add a student");
                Console.WriteLine("2. List all students");
                var input = Console.ReadLine();
                if (int.TryParse(input, out int result)) {
                    if (result == 0) {
                        cont = false;
                    } else if (result == 1) {                       
                        studentHelper.CreateStudentRecord();
                    } else if (result == 2) {
                        studentHelper.ListStudents();
                    }                  
                }
            }
        }
    }
}