using Library.LearningManagement.Models;

namespace Library.LearningManagement.Services {
    public class StudentService {
        private List<Person> studentList;
        private static StudentService _instance;

        // initializes StudentService
        private StudentService() {
            studentList = new List<Person>();
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
        public void Add(Person student) {   
            studentList.Add(student);
        }

        // returns the list of students
        public List<Person> Students {
            get {
                return studentList;
            }
        }

        // returns all students which contain query in name
        public IEnumerable<Person> Search(string query) {
            return studentList.Where(student => student.Name.ToUpper().Contains(query.ToUpper()));
        }
    }
}
