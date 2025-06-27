using SQLite;

namespace TKMApp.Models
{
    [Table("Prescriptions")] // Explicit table name
    public class Prescription
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int PatientId { get; set; }
        public string MedicineName { get; set; }
        public string Dose { get; set; }
        public string Duration { get; set; }

        [Ignore]
        public Patient Patient { get; set; }
    }
}