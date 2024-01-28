using Library.LearningManagement.Models;

namespace Library.LearningManagement.Services {
    public class CourseService {
        private List<Course> courseList;
        private static CourseService _instance;

        // initializes CourseService
        private CourseService() {
            courseList = new List<Course>();
        }

        // singleton to make sure we only have one course service
        public static CourseService Current {
            get {
                if (_instance == null) {
                    _instance = new CourseService();
                }

                return _instance;
            }
        }

        // adds a course to the list
        public void Add(Course course) {
            courseList.Add(course);
        }

        // returns the list of courses
        public List<Course> Courses {
            get {
                return courseList;
            }
        }

        // return all courses which contain query in name, description, or code
        public IEnumerable<Course> Search(string query) {
            return Courses.Where(course => course.Name.ToUpper().Contains(query.ToUpper()) ||
                course.Description.ToUpper().Contains(query.ToUpper()) ||
                course.Code.ToUpper().Contains(query.ToUpper()));
        }
    }
}
