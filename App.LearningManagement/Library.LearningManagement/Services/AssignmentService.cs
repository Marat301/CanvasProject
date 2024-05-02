using Library.LearningManagement.Database;
using Library.LearningManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.LearningManagement.Services {
    public class AssignmentService {

        private static AssignmentService? _instance;

        // singleton to make sure we only have one assignment service
        public static AssignmentService Current {
            get {
                if (_instance == null) {
                    _instance = new AssignmentService();
                }

                return _instance;
            }
        }

        // initializes AssignmentService
        public AssignmentService() { 
        
        }

        // adds an assignment to the list
        public void Add(Assignment assignment) {
            FakeDatabase.Assignments.Add(assignment);
        }

        // removes an assignment from the list
        public void Remove(Assignment assignment) {
            FakeDatabase.Assignments.Remove(assignment);
        }

        // returns the list of assignments
        public List<Assignment> Assignments {
            get {
                return FakeDatabase.Assignments;
            }
        }

        // return all assignments which contain query in name
        public IEnumerable<Assignment> Search(string query) {
            return Assignments.Where(assignment => assignment.Name.ToUpper().Contains(query.ToUpper()));
        }
    }
}
