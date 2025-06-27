using TKMApp.ViewModels;

namespace TKMApp.Views;

public partial class PatientDetailsPage : ContentPage
{
    public PatientDetailsPage(PatientDetailsViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is PatientDetailsViewModel viewModel)
        {
            // Get the patientId from the ViewModel which was set via QueryProperty
            await viewModel.LoadPatientDetails(viewModel.PatientId);
        }
    }
}