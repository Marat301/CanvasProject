using App.LearningManagement.Helpers;
using Library.LearningManagement.Models;

namespace CanvasProject {
    internal class Program {
        static void Main(string[] args) {

            var studentHelper = new StudentHelper();

            Console.WriteLine("Choose an action: ");
            Console.WriteLine("0. Exit");
            Console.WriteLine("1. Add a student");
            var input = Console.ReadLine();

            if (int.TryParse(input, out int result)) { 
                while (result != 0) {
                    if (result == 1) {
                        studentHelper.CreateStudentRecord();
                        Console.WriteLine("Choose an action: ");
                    }

                    input = Console.ReadLine();
                    int.TryParse(input, out result);
                }
                
            }

        }
    }
}