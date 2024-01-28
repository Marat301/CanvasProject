using Library.LearningManagement.Models;
using Library.LearningManagement.Services;

namespace App.LearningManagement.Helpers {
    internal class StudentHelper {
        private StudentService studentService;
        private CourseService courseService;

        // use the current services (singleton implementation)
        public StudentHelper() {
            studentService = StudentService.Current;
            courseService = CourseService.Current;
        }

        // creates a student
        public void CreateStudentRecord(Person? selectedStudent = null) {
            // get the name of the student
            Console.WriteLine("What is the name of the student?");
            var name = Console.ReadLine();

            // get the ID of the student
            Console.WriteLine("What is the ID of the student?");
            var id = Console.ReadLine();

            // get the classification of the student
            Console.WriteLine("What is the classification of the student? [(F)reshman, S(O)phomore, (J)unior, (S)enior]");
            var classification = Console.ReadLine() ?? string.Empty;

            // convert student classification to enum
            PersonClassification classEnum;
            if (classification.Equals("O", StringComparison.InvariantCultureIgnoreCase)) {
                classEnum = PersonClassification.Sophomore;
            } else if (classification.Equals("J", StringComparison.InvariantCultureIgnoreCase)) {
                classEnum = PersonClassification.Junior;
            } else if (classification.Equals("S", StringComparison.InvariantCultureIgnoreCase)) {
                classEnum = PersonClassification.Senior;
            } else {
                classEnum = PersonClassification.Freshman;
            }

            // check if we are updating or creating a student
            bool isCreate = selectedStudent == null;
            if (isCreate) {
                selectedStudent = new Person();
            }

            // set the student data
            selectedStudent.ID = int.Parse(id ?? "0");
            selectedStudent.Name = name ?? string.Empty;
            selectedStudent.Classification = classEnum;

            // add the student
            if (isCreate) {
                studentService.Add(selectedStudent);
            }
        }

        // updates a student
        public void UpdateStudentRecord() {
            Console.WriteLine("Select the student to update");
            ListStudents();

            var selectionStr = Console.ReadLine();

            if (int.TryParse(selectionStr, out int selectionInt)) {
                // get student whose ID matches given ID
                var selectedStudent = studentService.Students.FirstOrDefault(student => student.ID == selectionInt);

                // reset all the student's data
                if (selectedStudent != null) {
                    CreateStudentRecord(selectedStudent);
                }
            }
        }

        // prints list of all students, and lists all the courses of one student
        public void ListStudents() {
            // print the list of all students
            studentService.Students.ForEach(Console.WriteLine);

            // get the ID of the student whose courses we want to print
            Console.WriteLine("Which student would you like to select?");
            var selectionStr = Console.ReadLine();
            var selectionInt = int.Parse(selectionStr ?? "0");

            // get and print all the courses in which that student is enrolled
            Console.WriteLine("Student Course List");
            courseService.Courses.Where(
                course => course.Roster.Any(student => student.ID == selectionInt)
            ).ToList().ForEach(Console.WriteLine);
        }

        // search for student and print all their data
        public void SearchStudents() {
            // get the name of the student
            Console.WriteLine("Enter the student's name: ");
            var query = Console.ReadLine() ?? string.Empty;

            // find and print the student's data
            studentService.Search(query).ToList().ForEach(Console.WriteLine);

            // find and print the student's courses
            Console.WriteLine("Student Course List:");
            courseService.Courses.Where(
                course => course.Roster.Any(student => student.Name == query)
            ).ToList().ForEach(Console.WriteLine);
        }
    }
}
