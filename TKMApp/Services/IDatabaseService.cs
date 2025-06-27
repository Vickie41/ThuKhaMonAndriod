using TKMApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TKMApp.Services
{
    public interface IDatabaseService
    {
        Task<int> AddPatient(Patient patient);
        Task AddPrescriptions(int patientId, List<Prescription> prescriptions);
        Task<List<Patient>> GetAllPatients();
        Task<Patient> GetPatientById(int id);
        Task<List<Patient>> SearchPatients(string searchTerm);
        Task<List<Prescription>> GetPrescriptionsForPatient(int patientId);
        Task<bool> TestConnection();
    }
}