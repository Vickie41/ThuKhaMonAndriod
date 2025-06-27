using TKMApp.Models;
using TKMApp.Services;
using TKMApp.Views;
using System.Diagnostics;
using System.Windows.Input;

namespace TKMApp.ViewModels
{
    [QueryProperty(nameof(PatientId), "PatientId")]
    public class PatientDetailsViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;

        public PatientDetailsViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            AddPrescriptionCommand = new Command(async () => await AddPrescription());
            BackCommand = new Command(async () => await Back());
        }

        public ICommand AddPrescriptionCommand { get; }
        public ICommand BackCommand { get; }

        private int _patientId;
        public int PatientId
        {
            get => _patientId;
            set
            {
                if (SetProperty(ref _patientId, value))
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        await LoadPatientDetails(value);
                    });
                }
            }
        }

        private Patient _patient;
        public Patient Patient
        {
            get => _patient;
            set => SetProperty(ref _patient, value);
        }

        private List<Prescription> _prescriptions;
        public List<Prescription> Prescriptions
        {
            get => _prescriptions;
            set => SetProperty(ref _prescriptions, value);
        }

        //public async Task LoadPatientDetails(int patientId)
        //{
        //    if (patientId <= 0) return;

        //    try
        //    {
        //        IsBusy = true;

        //        // Load patient with their prescriptions
        //        Patient = await _databaseService.GetPatientById(patientId);

        //        if (Patient != null)
        //        {
        //            Debug.WriteLine($"Loaded patient: {Patient.Name} with {Patient.Prescriptions?.Count} prescriptions");
        //            Prescriptions = Patient.Prescriptions ?? new List<Prescription>();
        //        }
        //        else
        //        {
        //            await Shell.Current.DisplayAlert("Error", "Patient not found", "OK");
        //            await Shell.Current.GoToAsync("..");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Error loading patient: {ex}");
        //        await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}
        public async Task LoadPatientDetails(int patientId)
        {
            if (patientId <= 0) return;

            try
            {
                IsBusy = true;

                // Load patient first
                Patient = await _databaseService.GetPatientById(patientId);

                if (Patient == null)
                {
                    await Shell.Current.DisplayAlert("Error", "Patient not found", "OK");
                    await Shell.Current.GoToAsync("..");
                    return;
                }

                // Then load prescriptions specifically for this patient
                Prescriptions = await _databaseService.GetPrescriptionsForPatient(patientId);
                //await Shell.Current.GoToAsync("patients");
                Debug.WriteLine($"Loaded {Prescriptions.Count} prescriptions for patient {PatientId}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading patient details: {ex}");
                await Shell.Current.DisplayAlert("Error", "Failed to load patient details", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }


        private async Task AddPrescription()
        {
            if (PatientId <= 0) return;
            await Shell.Current.GoToAsync($"prescription/create?PatientId={PatientId}");
        }

        private async Task Back()
        {
            await Shell.Current.GoToAsync("patients");

        }
    }
}