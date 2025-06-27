using SQLite;
using TKMApp.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace TKMApp.Services
{
    public class DatabaseService : IDatabaseService
    {
        private readonly SQLiteAsyncConnection _database;

        
        public DatabaseService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "DoctorPatientDB.db");
            _database = new SQLiteAsyncConnection(dbPath);
            InitializeDatabase();
        }


        //private async Task InitializeDatabase()
        //{
        //    try
        //    {
        //        // Debug: Delete existing DB to ensure fresh start (remove after testing)
        //        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "DoctorPatientDB.db");


        //        await _database.ExecuteAsync("PRAGMA foreign_keys = ON;");

        //        // Create tables with verification
        //        var patientResult = await _database.CreateTableAsync<Patient>();
        //        var prescriptionResult = await _database.CreateTableAsync<Prescription>();

        //        Debug.WriteLine($"Patient table created: {patientResult}");
        //        Debug.WriteLine($"Prescription table created: {prescriptionResult}");

        //        // Verify tables exist
        //        var tableInfo = await _database.GetTableInfoAsync("Prescriptions");
        //        if (!tableInfo.Any())
        //        {
        //            throw new Exception("Prescriptions table not created!");
        //        }
        //        else
        //        {
        //            Debug.WriteLine("Prescriptions table verified");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Database init error: {ex}");
        //        throw;
        //    }
        //}

        private async Task InitializeDatabase()
        {
            try
            {
                await _database.ExecuteAsync("PRAGMA foreign_keys = ON;");

                // Create tables with explicit table names
                var patientResult = await _database.CreateTableAsync<Patient>();
                var prescriptionResult = await _database.CreateTableAsync<Prescription>();

                // Verify BOTH tables exist
                var patientTableInfo = await _database.GetTableInfoAsync("Patient");
                var prescriptionTableInfo = await _database.GetTableInfoAsync("Prescriptions");

                if (!patientTableInfo.Any() || !prescriptionTableInfo.Any())
                {
                    throw new Exception("Tables not created properly!");
                }

                Debug.WriteLine("Database initialized successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Database init error: {ex}");
                // Consider recreating the database file if this fails
                File.Delete(Path.Combine(FileSystem.AppDataDirectory, "DoctorPatientDB.db"));
                throw;
            }
        }

        public async Task<int> AddPatient(Patient patient)
        {
            try
            {
                return await _database.InsertAsync(patient);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding patient: {ex.Message}");
                throw;
            }
        }

        
        public async Task AddPrescriptions(int patientId, List<Prescription> prescriptions)
        {
            try
            {
                // First verify table exists
                var tableCheck = await _database.GetTableInfoAsync("Prescriptions");
                if (!tableCheck.Any())
                {
                    throw new Exception("Prescriptions table missing!");
                }

                Debug.WriteLine($"Attempting to save {prescriptions.Count} prescriptions for PatientId: {patientId}");


                await _database.RunInTransactionAsync(async tx =>
                {
                    foreach (var prescription in prescriptions)
                    {
                        Debug.WriteLine($"Saving prescription for PatientId: {patientId}, " +
                              $"Medicine: {prescription.MedicineName}");
                        prescription.PatientId = patientId;
                        await _database.InsertAsync(prescription);
                        
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving prescriptions: {ex}");
                throw;
            }
        }


        public async Task<List<Patient>> GetAllPatients()
        {
            try
            {
                var patients = await _database.Table<Patient>()
                    .OrderByDescending(p => p.VisitDate)
                    .ToListAsync();

                foreach (var patient in patients)
                {
                    patient.Prescriptions = await GetPrescriptionsForPatient(patient.Id);
                }

                return patients;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting patients: {ex.Message}");
                throw;
            }
        }

        public async Task<Patient> GetPatientById(int id)
        {
            try
            {
                var patient = await _database.Table<Patient>()
                    .Where(p => p.Id == id)
                    .FirstOrDefaultAsync();

                if (patient != null)
                {
                    patient.Prescriptions = await GetPrescriptionsForPatient(id);
                }

                return patient;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting patient by ID: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Patient>> SearchPatients(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return await GetAllPatients();

                var patients = await _database.Table<Patient>()
                    .Where(p => p.Name.Contains(searchTerm) ||
                               (p.Notes != null && p.Notes.Contains(searchTerm)))
                    .OrderBy(p => p.Name)
                    .ToListAsync();

                foreach (var patient in patients)
                {
                    patient.Prescriptions = await GetPrescriptionsForPatient(patient.Id);
                }

                return patients;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error searching patients: {ex.Message}");
                throw;
            }
        }

        public async Task<List<Prescription>> GetPrescriptionsForPatient(int patientId)
        {
            try
            {
                return await _database.Table<Prescription>()
                    .Where(p => p.PatientId == patientId)
                    .OrderBy(p => p.MedicineName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting prescriptions: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> TestConnection()
        {
            try
            {
                var result = await _database.ExecuteScalarAsync<int>("SELECT 1");
                return result == 1;
            }
            catch
            {
                return false;
            }
        }
    }
}