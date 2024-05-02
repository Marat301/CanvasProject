using MAUI.LearningManagement.ViewModels;

namespace MAUI.LearningManagement.Views;

public partial class AssignmentDetailView : ContentPage {
    public AssignmentDetailView() {
        InitializeComponent();
        BindingContext = new AssignmentDetailViewModel();
    }

    private void OnLeaving(object sender, NavigatedFromEventArgs e) {
        BindingContext = null;
    }

    private void OnArriving(object sender, NavigatedToEventArgs e) {
        BindingContext = new AssignmentDetailViewModel();
    }

    private void CancelClicked(object sender, EventArgs e) {
        Shell.Current.GoToAsync("//CourseDetail");
    }

    private void OkClicked(object sender, EventArgs e) {

        (BindingContext as AssignmentDetailViewModel).AddAssignment(Shell.Current);
    }
}