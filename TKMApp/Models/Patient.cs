using SQLite;
using System.Collections.Generic;

namespace TKMApp.Models
{
    [Table("Patient")] // Explicit table name
    public class Patient
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Name { get; set; }
        public int? Age { get; set; }
        public string Gender { get; set; }
        public DateTime VisitDate { get; set; }

        public int? SystolicBP { get; set; }
        public int? DiastolicBP { get; set; }
        public int? PulseRate { get; set; }
        public decimal? Temperature { get; set; }
        public int? RBS { get; set; }
        public string Notes { get; set; }

        [Ignore]
        public List<Prescription> Prescriptions { get; set; }
    }
}
