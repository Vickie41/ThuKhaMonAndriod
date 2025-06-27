using TKMApp.ViewModels;
namespace TKMApp.Views;

public partial class PatientsPage : ContentPage
{
    public PatientsPage(PatientsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}