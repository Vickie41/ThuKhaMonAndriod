using TKMApp.ViewModels;

namespace TKMApp.Views
{
    public partial class PrescriptionPage : ContentPage
    {
        public PrescriptionPage(PrescriptionViewModel viewModel)
        {
            InitializeComponent();
             BindingContext = viewModel;
            

        }
    }
}