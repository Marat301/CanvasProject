using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.LearningManagement.ViewModels {
    class AssignmentDetailViewModel {
        public AssignmentDetailViewModel() {
            Assignment assignment = new Assignment();
        }
        public string Name {
            get => assignment?.Name ?? string.Empty;
            set { if (assignment != null) assignment.Name = value; }
        }
        public string Description {
            get => assignment?.Description ?? string.Empty;
            set { if (assignment != null) assignment.Description = value; }
        }

        private Assignment assignment;

        public void AddAssignment(Shell s) {
            AssignmentService.Current.Add(assignment);
            s.GoToAsync("//CourseDetail");
        }
    }
}
