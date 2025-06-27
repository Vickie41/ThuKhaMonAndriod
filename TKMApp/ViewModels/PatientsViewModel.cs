using TKMApp.Models;
using TKMApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace TKMApp.ViewModels
{
    public class PatientsViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;

        public PatientsViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            Patients = new ObservableCollection<Patient>();

            // Commands
            LoadPatientsCommand = new Command(async () => await LoadPatients());
            SearchCommand = new Command(async () => await SearchPatients());
            AddPatientCommand = new Command(async () => await AddPatient());
            PatientTappedCommand = new Command<Patient>(async (patient) => await ShowPatientDetails(patient));

            // Initial load
            Task.Run(async () => await LoadPatients());
        }

        public ICommand LoadPatientsCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand AddPatientCommand { get; }
        public ICommand PatientTappedCommand { get; }

        private ObservableCollection<Patient> _patients;
        public ObservableCollection<Patient> Patients
        {
            get => _patients;
            set => SetProperty(ref _patients, value);
        }

        private Patient _selectedPatient;
        public Patient SelectedPatient
        {
            get => _selectedPatient;
            set
            {
                SetProperty(ref _selectedPatient, value);
                if (value != null)
                    Task.Run(async () => await ShowPatientDetails(value));
            }
        }

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                if (SetProperty(ref _searchTerm, value))
                {
                    Task.Run(async () => await SearchPatients());
                }
            }
        }

        private async Task LoadPatients()
        {
            try
            {
                IsBusy = true;
                var patients = await _databaseService.GetAllPatients();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Patients.Clear();
                    foreach (var patient in patients)
                    {
                        Patients.Add(patient);
                    }
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task SearchPatients()
        {
            try
            {
                IsBusy = true;
                var allPatients = await _databaseService.GetAllPatients();
                var filteredPatients = string.IsNullOrWhiteSpace(SearchTerm)
                    ? allPatients
                    : allPatients.Where(p => p.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase)).ToList();

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    Patients.Clear();
                    foreach (var patient in filteredPatients)
                    {
                        Patients.Add(patient);
                    }
                });
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task AddPatient()
        {
            await Shell.Current.GoToAsync("add-patient");
        }

        private async Task ShowPatientDetails(Patient patient)
        {
            if (patient == null) return;

            try
            {
                IsBusy = true;
                await Shell.Current.GoToAsync($"patient-details/view?PatientId={patient.Id}");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            finally
            {
                IsBusy = false;
                SelectedPatient = null;
            }
        }
    }
}