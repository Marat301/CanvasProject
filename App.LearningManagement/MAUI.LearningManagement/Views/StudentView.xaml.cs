using MAUI.LearningManagement.ViewModels;

namespace MAUI.LearningManagement.Views;

public partial class StudentView : ContentPage
{
	public StudentView()
	{
		InitializeComponent();
		BindingContext = new StudentViewViewModel();
	}

    private void CancelClicked(object sender, EventArgs e) {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void CoursesClicked(object sender, EventArgs e) {
        Shell.Current.GoToAsync("//Student");
    }

    private void AssignmentsClicked(object sender, EventArgs e) {
        Shell.Current.GoToAsync("//Student");
    }

    private void GradesClicked(object sender, EventArgs e) {
        Shell.Current.GoToAsync("//Student");
    }
}