using apbd6.Context;
using apbd6.DTOModel;
using apbd6.Models;
using Microsoft.AspNetCore.Mvc;

namespace apbd6.Service;

 public interface IPatientsService
    {
        IActionResult GetPatientData(int patientId);
    }

    public class PatientsService : IPatientsService
    {
        private readonly ApbdContext _contextEf;

        public PatientsService(ApbdContext contextEf)
        {
            _contextEf = contextEf;
        }

        public IActionResult GetPatientData(int patientId)
        {
            if (IsInvalidPatientId(patientId))
            {
                return new BadRequestObjectResult("Invalid patient ID");
            }

            var patientData = FetchPatientData(patientId);

            if (patientData == null)
            {
                return new NotFoundObjectResult("Patient not found");
            }

            return new OkObjectResult(patientData);
        }

        private bool IsInvalidPatientId(int patientId)
        {
            return patientId <= 0;
        }

        private PrescriptionDTO FetchPatientData(int patientId)
        {
            return _contextEf.Prescriptions
                .Where(p => p.IdPatient == patientId)
                .Select(p => new PrescriptionDTO
                {
                    Patient = CreatePatientDTO(p),
                    Doctor = CreateDoctorDTO(p),
                    Medicament = CreateMedicamentDTOList(p)
                })
                .FirstOrDefault();
        }

        private PatientDTO CreatePatientDTO(Prescription p)
        {
            return new PatientDTO
            {
                IdPatient = p.Patient.IdPatient,
                FirstName = p.Patient.FirstName,
                LastName = p.Patient.LastName,
                Birthdate = p.Patient.Birthdate,
            };
        }

        private DoctorDTO CreateDoctorDTO(Prescription p)
        {
            return new DoctorDTO
            {
                IdDoctor = p.Doctor.IdDoctor,
                FirstName = p.Doctor.FirstName,
                LastName = p.Doctor.LastName,
                Email = p.Doctor.Email,
            };
        }

        private List<MedicamentDTO> CreateMedicamentDTOList(Prescription p)
        {
            return p.PrescriptionMedicaments
                .Select(pm => new MedicamentDTO
                {
                    IdMedicament = pm.Medicament.IdMedicament,
                    Name = pm.Medicament.Name,
                    Dose = pm.Dose ?? 0,
                    Description = pm.Medicament.Description,
                })
                .ToList();
        }
    }