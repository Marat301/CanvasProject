using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.CompilerServices;


namespace MAUI.LearningManagement.ViewModels {
    class CourseDetailViewModel : INotifyPropertyChanged {
        public CourseDetailViewModel(string prefix = "") {
            if (prefix != "") {
                LoadByPrefix(prefix);
            }
        }

        public string Name {
            get; set;
        }
        public string Description {
            get; set;
        }
        // null exception when I try to do a normal get set
        public string Prefix {
            get => course?.Prefix ?? string.Empty;
            set { if (course != null) course.Prefix = value; }
        }
        public int Id { get; set; }

        public int CreditHours { get; set; }

        public List<Person> Roster { get; set; }

        public List<AssignmentGroup> AssignmentGroups { get; set; }

        public List<Submission> Submissions { get; set; }

        public List<Library.LearningManagement.Models.Module> Modules { get; set; }

        public List<Announcement> Announcements { get; set; }

        public void LoadByPrefix(string prefix) {
            if (prefix == "") {
                
                return; 
            }
            var courseObj = CourseService.Current.GetByPrefix(prefix) as Course;
            if (courseObj != null) {
                Name = courseObj.Name;
                Prefix = courseObj.Prefix;
                Description = courseObj.Description;
                Roster = courseObj.Roster;
                AssignmentGroups = courseObj.AssignmentGroups;
                Submissions = courseObj.Submissions;
                Modules = courseObj.Modules;
                Announcements = courseObj.Announcements;
                Id = courseObj.ID;
                CreditHours = courseObj.CreditHours;
            }
            // editing
            NotifyPropertyChanged(nameof(Name));
            NotifyPropertyChanged(nameof(Prefix));
            NotifyPropertyChanged(nameof(Description));
            NotifyPropertyChanged(nameof(Roster));
            NotifyPropertyChanged(nameof(AssignmentGroups));
            NotifyPropertyChanged(nameof(Submissions));
            NotifyPropertyChanged(nameof(Modules));
            NotifyPropertyChanged(nameof(Announcements));
            NotifyPropertyChanged(nameof(Id));
            NotifyPropertyChanged(nameof(CreditHours));
        }


        public string CourseCode {
            get => course?.Code ?? string.Empty;
        }

        private Course course;

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public void AddCourse(Shell s) {
            if (Prefix == "") {
                CourseService.Current.Add(new Course { Name = Name, Prefix = Prefix, Description = Description });
            } else {
                var refToUpdate = CourseService.Current.GetByPrefix(Prefix) as Course;
                refToUpdate.Name = Name;
                refToUpdate.Prefix = Prefix;
                refToUpdate.Description = Description;
                refToUpdate.Roster = Roster;
                refToUpdate.AssignmentGroups = AssignmentGroups;
                refToUpdate.Submissions = Submissions;
                refToUpdate.Modules = Modules;
                refToUpdate.Announcements = Announcements;
                refToUpdate.CreditHours = CreditHours;
            }
            s.GoToAsync($"//Instructor");
        }

        public void AddModuleClick(Shell s) {
            s.GoToAsync($"//ModuleDetail");
        }

        public void AddAssignmentClick(Shell s) {
            s.GoToAsync($"//AssignmentDetail");
        }

    }
}