using CommunityToolkit.Maui;
using TKMApp.Converters;
using TKMApp.Services;
using TKMApp.ViewModels;
using TKMApp.Views;
using Microsoft.Extensions.Logging;
using SQLitePCL;


namespace TKMApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            try
            {
                Batteries.Init();

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                 .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Register Services
            builder.Services.AddSingleton<IDatabaseService, DatabaseService>();

            // Register ViewModels
            builder.Services.AddTransient<PatientsViewModel>();
            builder.Services.AddTransient<PatientDetailsViewModel>();
            builder.Services.AddTransient<AddPatientViewModel>();
            builder.Services.AddTransient<PrescriptionViewModel>();
            //builder.Services.AddSingleton<IDatabaseService, DatabaseService>();


            // Register Views
            builder.Services.AddTransient<PatientsPage>();
            builder.Services.AddTransient<PatientDetailsPage>();
            builder.Services.AddTransient<AddPatientPage>();
            builder.Services.AddTransient<PrescriptionPage>();

            builder.Services.AddSingleton<NullableIntConverter>();
            builder.Services.AddSingleton<NullableDecimalConverter>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
            }
            catch (Exception ex)
            {
                // This will help you see the error in debug output
                System.Diagnostics.Debug.WriteLine($"MAUI INIT FAILED: {ex}");
                throw; // Re-throw to see it in logcat
            }
        }
            

}
}