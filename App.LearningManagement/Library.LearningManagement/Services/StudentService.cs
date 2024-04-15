using Library.LearningManagement.Database;
using Library.LearningManagement.Models;

namespace Library.LearningManagement.Services {
    public class StudentService {
        private static StudentService _instance;

        public IEnumerable<Student> Students {
            get {
                return FakeDatabase.People.Where
                    (p => p is Student).Select(p => p as Student);
            }
        }


        // initializes StudentService
        private StudentService() {

        }

        // singleton to make sure we only have one StudentService
        public static StudentService Current {
            get {
                if (_instance == null) {
                    _instance = new StudentService();
                }

                return _instance;
            }
        }

        // adds a student to the list
        public void Add(Student student) {
            FakeDatabase.People.Add(student);
        }

        // removes a student from the list
        public void Remove(Person student) {
            FakeDatabase.People.Remove(student);
        }

        // finds the student by ID
        public Person? GetById(int ID) {
            return FakeDatabase.People.FirstOrDefault(p => p.ID == ID);
        }

        // returns all students which contain query in name

        public IEnumerable<Student?> Search(string query) {
            return Students.Where
                (s => (s != null) && s.Name.ToUpper().Contains(query.ToUpper()));
        }

        // return the GPA of the selected student
        public decimal GetGPA(int studentID) {
            var courseSvc = CourseService.Current;
            var courses = courseSvc.Courses.Where(c => c.Roster.Select(s => s.ID).Contains(studentID));

            var totalGradePoints = courses.Select(c => courseSvc.GetGradePoints(c.ID, studentID) * c.CreditHours).Sum();
            var totalCreditHours = courses.Select(c => c.CreditHours).Sum();

            return totalGradePoints / (totalCreditHours > 0 ? totalCreditHours : -1);
        }
    }
}
