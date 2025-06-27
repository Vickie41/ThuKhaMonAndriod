using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TKMApp.ViewModels
{
    public class MedicineItem : BaseViewModel
    {
        private string _medicineName;
        public string MedicineName
        {
            get => _medicineName;
            set => SetProperty(ref _medicineName, value);
        }

        // Original property style
        private string _dose;
        public string Dose
        {
            get => _dose;
            set => SetProperty(ref _dose, value);
        }

        // New property style (for Picker)
        private string _selectedDose;
        public string SelectedDose
        {
            get => _selectedDose;
            set
            {
                SetProperty(ref _selectedDose, value);
                Dose = value; // Keep both properties synchronized
            }
        }

        // Original property style
        private string _duration;
        public string Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        // New property style (for Picker)
        private string _selectedDuration;
        public string SelectedDuration
        {
            get => _selectedDuration;
            set
            {
                SetProperty(ref _selectedDuration, value);
                Duration = value; // Keep both properties synchronized
            }
        }
    }
}