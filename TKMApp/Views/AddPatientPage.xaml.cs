namespace TKMApp.Views;
using TKMApp.ViewModels;
public partial class AddPatientPage : ContentPage
{
    public AddPatientPage(AddPatientViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}