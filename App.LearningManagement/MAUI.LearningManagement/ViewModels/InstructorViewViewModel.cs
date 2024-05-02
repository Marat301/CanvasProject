using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MAUI.LearningManagement.ViewModels {
    public class InstructorViewViewModel : INotifyPropertyChanged {
        public InstructorViewViewModel() {
            IsEnrollmentsVisible = true;
            IsCoursesVisible = false;
        }
        public ObservableCollection<Person> People {
            get {

                var filteredList = StudentService
                    .Current
                    .Students
                    .Where(
                    s => s.Name.ToUpper().Contains(Query?.ToUpper() ?? string.Empty));
                return new ObservableCollection<Person>(filteredList);

            }
        }

        public ObservableCollection<Course> Courses {
            get {
                var filteredList = CourseService
                   .Current
                   .Courses
                   .Where(
                   s => s.Name.ToUpper().Contains(Query?.ToUpper() ?? string.Empty));
                return new ObservableCollection<Course>(filteredList);
            }
        }

        public string Title { get => "Instructor / Administrator Menu"; }

        public bool IsEnrollmentsVisible {
            get; set;
        }

        public bool IsCoursesVisible {
            get; set;
        }

        /*public bool IsModulesVisible {
            get; set;
        }*/

        public void ShowEnrollments() {
            IsEnrollmentsVisible = true;
            IsCoursesVisible = false;
            //IsModulesVisible = false;
            NotifyPropertyChanged("IsEnrollmentsVisible");
            NotifyPropertyChanged("IsCoursesVisible");
            //NotifyPropertyChanged("IsModulesVisible");
        }

        public void ShowCourses() {
            IsEnrollmentsVisible = false;
            IsCoursesVisible = true;
            //IsModulesVisible = false;
            NotifyPropertyChanged("IsEnrollmentsVisible");
            NotifyPropertyChanged("IsCoursesVisible");
            //NotifyPropertyChanged("IsModulesVisible");
        }
        /*public void ShowModules() {
            IsEnrollmentsVisible = false;
            IsCoursesVisible = false;
            IsModulesVisible = true;
            NotifyPropertyChanged("IsEnrollmentsVisible");
            NotifyPropertyChanged("IsCoursesVisible");
            NotifyPropertyChanged("IsModulesVisible");
        }*/
        public Person SelectedPerson { get; set; }
        public Course SelectedCourse { get; set; }
        public Module SelectedModule { get; set; }

        private string query;
        public string Query {
            get => query;
            set {
                query = value;
                NotifyPropertyChanged(nameof(People));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void AddEnrollmentClick(Shell s) {
            s.GoToAsync($"//PersonDetail?personId=0");
        }

        public void EditEnrollmentClick(Shell s) {
            var idParam = SelectedPerson?.ID ?? 0;
            s.GoToAsync($"//PersonDetail?personId={idParam}");
        }

        public void RemoveEnrollmentClick() {
            if (SelectedPerson == null) { return; }

            StudentService.Current.Remove(SelectedPerson);
            RefreshView();
        }

        public void AddCourseClick(Shell s) {
            s.GoToAsync($"//CourseDetail");
        }

        public void EditCourseClick(Shell s) {
            var prefixParam = SelectedCourse?.Prefix;
            s.GoToAsync($"//CourseDetail?coursePrefix={prefixParam}");
        }

        public void RemoveCourseClick() {
            if (SelectedCourse == null) { return; }

            CourseService.Current.Remove(SelectedCourse);
            RefreshView();
        }

        

        public void ResetView() {
            Query = string.Empty;
            NotifyPropertyChanged(nameof(Query));
        }

        public void RefreshView() {

            NotifyPropertyChanged(nameof(People));
            NotifyPropertyChanged(nameof(Courses));
        }

    }
}
