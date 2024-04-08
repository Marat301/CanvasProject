using Library.LearningManagement.Models;

namespace Library.LearningManagement.Services {
    public class StudentService {
        private List<Student> studentList;
        private static StudentService _instance;

        // initializes StudentService
        private StudentService() {
            studentList = new List<Student>();
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
            studentList.Add(student);
        }

        // returns the list of students
        public List<Student> Students {
            get {
                return studentList;
            }
        }

        // returns all students which contain query in name
        public IEnumerable<Student> Search(string query) {
            return studentList.Where(student => student.Name.ToUpper().Contains(query.ToUpper()));
        }
    }
}
