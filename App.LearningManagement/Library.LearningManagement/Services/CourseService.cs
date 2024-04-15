using Library.LearningManagement.Database;
using Library.LearningManagement.Models;

namespace Library.LearningManagement.Services {
    public class CourseService {
        private static CourseService? _instance;

        // singleton to make sure we only have one course service
        public static CourseService Current {
            get {
                if (_instance == null) {
                    _instance = new CourseService();
                }

                return _instance;
            }
        }

        // initializes CourseService
        private CourseService() {

        }

        // adds a course to the list
        public void Add(Course course) {
            FakeDatabase.Courses.Add(course);
        }

        // returns the list of courses
        public List<Course> Courses {
            get {
                return FakeDatabase.Courses;
            }
        }

        // return all courses which contain query in name, description, or code
        public IEnumerable<Course> Search(string query) {
            return Courses.Where(course => course.Name.ToUpper().Contains(query.ToUpper()) ||
                course.Description.ToUpper().Contains(query.ToUpper()) ||
                course.Code.ToUpper().Contains(query.ToUpper()));
        }

        // return the weighted average
        public decimal GetWeightedGrade(int courseID, int studentID) {
            var selectedCourse = Courses.FirstOrDefault(c => c.ID == courseID);

            if (selectedCourse == null) {
                return -1M;
            }

            var weightedAverage = 0M;
            foreach (var group in selectedCourse.AssignmentGroups) {
                var submissions = selectedCourse.Submissions.Where
                    (s => s.Student.ID == studentID && group.Assignments.Select
                    (a => a.ID).Contains(s.Assignment.ID));

                if (submissions.Any()) {
                    weightedAverage += submissions.Select
                        (s => s.Grade).Average() * group.Weight;
                }
            }

            return weightedAverage;
        }

        // return the grade points
        public decimal GetGradePoints(int courseID, int studentID) {
            return GetGradePoints(GetWeightedGrade(courseID, studentID));
        }

        // returns a letter grade
        public string GetLetterGrade(decimal grade) {
            if (grade >= 93) {
                return "A";
            } else if (grade < 93 && grade >= 90) {
                return "A-";
            } else if (grade < 90 && grade >= 87) {
                return "B+";
            } else if (grade < 87 && grade >= 83) {
                return "B";
            } else if (grade < 83 && grade >= 80) {
                return "B-";
            } else if (grade < 80 && grade >= 77) {
                return "C+";
            } else if (grade < 77 && grade >= 73) {
                return "C";
            } else if (grade < 73 && grade >= 70) {
                return "C-";
            } else if (grade < 70 && grade >= 60) {
                return "D";
            } else {
                return "F";
            }

        }

        // returns grade points
        public decimal GetGradePoints(decimal grade) {
            if (grade >= 93) {
                return 4M;
            } else if (grade < 93 && grade >= 90) {
                return 3.7M;
            } else if (grade < 90 && grade >= 87) {
                return 3.3M;
            } else if (grade < 87 && grade >= 83) {
                return 3M;
            } else if (grade < 83 && grade >= 80) {
                return 2.7M;
            } else if (grade < 80 && grade >= 77) {
                return 2.3M;
            } else if (grade < 77 && grade >= 73) {
                return 2M;
            } else if (grade < 73 && grade >= 70) {
                return 1.7M;
            } else if (grade < 70 && grade >= 60) {
                return 1M;
            } else {
                return 0M;
            }
        }
    }
}
