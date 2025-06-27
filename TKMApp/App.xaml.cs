using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using TKMApp.Views;

namespace TKMApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Set the main page to your AppShell
            MainPage = new AppShell();

            // Register all navigation routes
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            Routing.RegisterRoute(nameof(PatientsPage), typeof(PatientsPage));
            Routing.RegisterRoute(nameof(AddPatientPage), typeof(AddPatientPage));
            Routing.RegisterRoute(nameof(PatientDetailsPage), typeof(PatientDetailsPage));
            Routing.RegisterRoute(nameof(PrescriptionPage), typeof(PrescriptionPage));
        }

        protected override Window CreateWindow(IActivationState activationState)
        {
            var window = base.CreateWindow(activationState);

            // Set window sizing (optional)
            window.Width = 1200;
            window.Height = 800;
            window.Title = "Doctor Patient Notes";

            return window;
        }
    }
}