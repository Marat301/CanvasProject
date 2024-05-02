using Library.LearningManagement.Models;
using Library.LearningManagement.Services;
using MAUI.LearningManagement.ViewModels;

namespace MAUI.LearningManagement.Views;

[QueryProperty(nameof(coursePrefix), "coursePrefix")]

public partial class CourseDetailView : ContentPage {
    public CourseDetailView() {
        InitializeComponent();
        BindingContext = new CourseDetailViewModel();
    }

    public string coursePrefix {
        set; get;
    }

    private void OnLeaving(object sender, NavigatedFromEventArgs e) {
        BindingContext = null;
    }

    private void OnArriving(object sender, NavigatedToEventArgs e) {
        BindingContext = new CourseDetailViewModel();
    }

    private void CancelClicked(object sender, EventArgs e) {
        Shell.Current.GoToAsync("//Instructor");
    }

    private void AddModuleClick(object sender, EventArgs e) {
        (BindingContext as CourseDetailViewModel).AddModuleClick(Shell.Current);
    }

    private void AddAssignmentClick(object sender, EventArgs e) {
        (BindingContext as CourseDetailViewModel).AddAssignmentClick(Shell.Current);
    }

    private void OkClicked(object sender, EventArgs e) {
        
        (BindingContext as CourseDetailViewModel).AddCourse(Shell.Current);
    }

    
}