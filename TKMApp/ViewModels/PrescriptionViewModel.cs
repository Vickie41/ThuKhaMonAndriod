using TKMApp.Models;
using TKMApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using System.Diagnostics;

namespace TKMApp.ViewModels
{
    [QueryProperty(nameof(PatientId), nameof(PatientId))]

    public class PrescriptionViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;
        private int _patientId;

        public PrescriptionViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            AddMedicineCommand = new Command(AddMedicine);
            SavePrescriptionCommand = new Command(async () => await SavePrescription());
            CancelCommand = new Command(async () => await Cancel());

            MedicineItems = new ObservableCollection<MedicineItem>();
            AddMedicine(); // Add first empty medicine
        }

        public ICommand AddMedicineCommand { get; }
        public ICommand SavePrescriptionCommand { get; }
        public ICommand CancelCommand { get; }

        public ObservableCollection<MedicineItem> MedicineItems { get; }

        public List<string> DoseOptions { get; } = new List<string> { "od", "bd", "tds", "qid", "hs", "pm" };
        public List<string> DurationOptions { get; } = new List<string>
        {
            "2 days", "3 days", "5 days", "10 days", "1 week", "1 month", "As needed", "Stat"
        };

        public int PatientId
        {
            get => _patientId;
            set
            {
                _patientId = value;
                Debug.WriteLine($"PatientId set to: {value}");
                OnPropertyChanged();

                // Load any existing prescriptions
                LoadPrescriptions();
            }

        }
        private async void LoadPrescriptions()
        {
            if (PatientId > 0)
            {
                var prescriptions = await _databaseService.GetPrescriptionsForPatient(PatientId);
                MedicineItems.Clear();
                foreach (var p in prescriptions)
                {
                    MedicineItems.Add(new MedicineItem
                    {
                        MedicineName = p.MedicineName,
                        Dose = p.Dose,
                        Duration = p.Duration
                    });
                }
            }
        }

        private void AddMedicine()
        {
            MedicineItems.Add(new MedicineItem());
        }


        //private async Task SavePrescription()
        //{
        //    if (!ValidatePrescriptions())
        //        return;

        //    try
        //    {
        //        IsBusy = true;
        //        Debug.WriteLine($"Saving prescriptions for PatientId: {PatientId}"); // Debug 
        //        var prescriptions = MedicineItems
        //            .Where(m => !string.IsNullOrWhiteSpace(m.MedicineName))
        //            .Select(m => new Prescription
        //            {
        //                PatientId = this.PatientId,
        //                MedicineName = m.MedicineName?.Trim(),
        //                Dose = (m.SelectedDose ?? m.Dose ?? "Not specified")?.Trim(),
        //                Duration = (m.SelectedDuration ?? m.Duration ?? "Not specified")?.Trim()
        //            })
        //            .ToList();

        //        Debug.WriteLine($"Attempting to save {prescriptions.Count} prescriptions");

        //        await _databaseService.AddPrescriptions(PatientId, prescriptions);

        //        await Shell.Current.DisplayAlert("Success", "Prescriptions saved", "OK");
        //        await Shell.Current.GoToAsync("patients");
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Full error: {ex}");
        //        await Shell.Current.DisplayAlert("Error", $"Save failed: {ex.Message}", "OK");
        //    }
        //    finally
        //    {
        //        IsBusy = false;
        //    }
        //}

        private async Task SavePrescription()
        {
            try
            {
                if (PatientId <= 0)
                {
                    await Shell.Current.DisplayAlert("Error", "No patient selected", "OK");
                    return;
                }

                var prescriptions = MedicineItems
                    .Where(m => !string.IsNullOrWhiteSpace(m.MedicineName))
                    .Select(m => new Prescription
                    {
                        PatientId = this.PatientId, // Critical - ensure this is set
                        MedicineName = m.MedicineName?.Trim(),
                        Dose = m.Dose?.Trim(),
                        Duration = m.Duration?.Trim()
                    })
                    .ToList();

                if (prescriptions.Count == 0)
                {
                    await Shell.Current.DisplayAlert("Error", "Please add at least one medicine", "OK");
                    return;
                }

                await _databaseService.AddPrescriptions(PatientId, prescriptions);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving prescriptions: {ex}");
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
        }

        private bool ValidatePrescriptions()
        {
            if (MedicineItems.All(m => string.IsNullOrWhiteSpace(m.MedicineName)))
            {
                Shell.Current.DisplayAlert("Error", "At least one medicine required", "OK").Wait();
                return false;
            }
            return true;
        }

        private void LogPrescriptions(List<Prescription> prescriptions)
        {
            Debug.WriteLine($"Saving {prescriptions.Count} prescriptions for PatientId: {PatientId}");
            foreach (var p in prescriptions)
            {
                Debug.WriteLine($"- Medicine: {p.MedicineName}, " +
                               $"Dose: {p.Dose}, " +
                               $"Duration: {p.Duration}, " +
                               $"PatientId: {p.PatientId}");
            }
        }

        private async Task Cancel()
        {
            if (IsBusy)
            {
                await Shell.Current.DisplayAlert("Wait", "Please wait while saving data", "OK");
                return;
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}