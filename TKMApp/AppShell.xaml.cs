using TKMApp.Views;

namespace TKMApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            RegisterRoutes();

        }

        private void RegisterRoutes()
        {
            // Unregister routes individually
            UnregisterRouteIfExists("patients");
            UnregisterRouteIfExists("add-patient");
            UnregisterRouteIfExists("patient-details");
            UnregisterRouteIfExists("prescription");
            UnregisterRouteIfExists("prescription/create");
            UnregisterRouteIfExists("patient-details/view");

            // Register main page routes with unique paths
            Routing.RegisterRoute("patients", typeof(PatientsPage));
            Routing.RegisterRoute("add-patient", typeof(AddPatientPage));
            Routing.RegisterRoute("patient-details", typeof(PatientDetailsPage));
            Routing.RegisterRoute("prescription", typeof(PrescriptionPage));

            // Register parameterized routes with distinct paths
            Routing.RegisterRoute("prescription/create", typeof(PrescriptionPage));
            Routing.RegisterRoute("patient-details/view", typeof(PatientDetailsPage));
        }

        private void UnregisterRouteIfExists(string route)
        {
            try
            {
                Routing.UnRegisterRoute(route);
            }
            catch
            {
                // Route didn't exist, ignore the error
            }
        }
    }
}