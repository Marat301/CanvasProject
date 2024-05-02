using MAUI.LearningManagement.ViewModels;

namespace MAUI.LearningManagement.Views;

public partial class ModuleDetailView : ContentPage
{
	public ModuleDetailView()
	{
        InitializeComponent();
        BindingContext = new ModuleDetailViewModel();
    }

    private void OnLeaving(object sender, NavigatedFromEventArgs e) {
        BindingContext = null;
    }

    private void OnArriving(object sender, NavigatedToEventArgs e) {
        BindingContext = new ModuleDetailViewModel();
    }

    private void CancelClicked(object sender, EventArgs e) {
        Shell.Current.GoToAsync("//CourseDetail");
    }

    private void OkClicked(object sender, EventArgs e) {

        (BindingContext as ModuleDetailViewModel).AddModule(Shell.Current);
    }
}