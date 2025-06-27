using TKMApp.Models;
using TKMApp.Services;
using TKMApp.Views;
using System.Windows.Input;

namespace TKMApp.ViewModels
{
    public class AddPatientViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;

        public AddPatientViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            SavePatientCommand = new Command(async () => await SavePatient());
            CancelCommand = new Command(async () => await Cancel());
            VisitDate = DateTime.Today;

            Genders = new List<string> { "Male", "Female", "Other" };
        }

        public ICommand SavePatientCommand { get; }
        public ICommand CancelCommand { get; }
        //public ICommand GenderSelectedCommand => new Command<string>((gender) => Gender = gender);

        public List<string> Genders { get; }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private int? _age;
        public int? Age
        {
            get => _age;
            set => SetProperty(ref _age, value);
        }

        private string _gender;
        public string Gender
        {
            get => _gender;
            set => SetProperty(ref _gender, value);
        }

        private DateTime _visitDate;
        public DateTime VisitDate
        {
            get => _visitDate;
            set => SetProperty(ref _visitDate, value);
        }

        private int? _systolicBP;
        public int? SystolicBP
        {
            get => _systolicBP;
            set => SetProperty(ref _systolicBP, value);
        }

        private int? _diastolicBP;
        public int? DiastolicBP
        {
            get => _diastolicBP;
            set => SetProperty(ref _diastolicBP, value);
        }

        private int? _pulseRate;
        public int? PulseRate
        {
            get => _pulseRate;
            set => SetProperty(ref _pulseRate, value);
        }

        private decimal? _temperature;
        public decimal? Temperature
        {
            get => _temperature;
            set => SetProperty(ref _temperature, value);
        }

        private int? _rbs;
        public int? RBS
        {
            get => _rbs;
            set => SetProperty(ref _rbs, value);
        }

        private string _notes;
        public string Notes
        {
            get => _notes;
            set => SetProperty(ref _notes, value);
        }

        
        private async Task SavePatient()
        {
            if (!ValidatePatient())
                return;

            try
            {
                IsBusy = true;
                var patient = new Patient
                {
                    
                    Name = Name,
                    Age = Age,
                    Gender = Gender,
                    VisitDate = VisitDate,
                    SystolicBP = SystolicBP,
                    DiastolicBP = DiastolicBP,
                    PulseRate = PulseRate,
                    Temperature = Temperature,
                    RBS = RBS,
                    Notes = Notes
                };

                var patientId = await _databaseService.AddPatient(patient);
                //await Shell.Current.GoToAsync($"prescription/create?PatientId={patientId}");
                //await Shell.Current.GoToAsync(nameof(PrescriptionPage), new Dictionary<string, object>
                //{
                //    ["PatientId"] = patientId // Pass the patient ID
                //});
                // This should be how you navigate to the prescription page
                // When navigating from AddPatient or PatientDetails:
                await Shell.Current.GoToAsync($"{nameof(PrescriptionPage)}?PatientId={patient.Id}");
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

        private async Task<bool> ValidatePatientAsync()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                await Shell.Current.DisplayAlert("Error", "Patient name is required", "OK");
                return false;
            }

            if (!Age.HasValue)
            {
                await Shell.Current.DisplayAlert("Error", "Age is required", "OK");
                return false;
            }

            return true;
        }

        private bool ValidatePatient()
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                Shell.Current.DisplayAlert("Error", "Patient name is required", "OK").Wait();
                return false;
            }

            if (!Age.HasValue)
            {
                Shell.Current.DisplayAlert("Error", "Age is required", "OK").Wait();
                return false;
            }

            if (!SystolicBP.HasValue || !DiastolicBP.HasValue)
            {
                Shell.Current.DisplayAlert("Error", "Blood pressure is required", "OK").Wait();
                return false;
            }

            return true;
        }

        private async Task Cancel()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}