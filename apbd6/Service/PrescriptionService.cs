using apbd6.Context;
using apbd6.DTOModel;
using apbd6.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace apbd6.Service;

public interface IPrescriptionsService
{
    IActionResult AddPrescription(PrescriptionDTO prescription);
}

public class PrescriptionsService : IPrescriptionsService
{
    private readonly ApbdContext _contextEf;

    public PrescriptionsService(ApbdContext contextEf)
    {
        _contextEf = contextEf;
    }

    public IActionResult AddPrescription(PrescriptionDTO newPrescription)
    {
        if (IsInvalidPrescriptionDate(newPrescription))
        {
            return new BadRequestObjectResult("Wrong Date");
        }

        if (HasTooManyMedicaments(newPrescription))
        {
            return new BadRequestObjectResult("Too many medicaments");
        }

        if (ContainsInvalidMedicaments(newPrescription, out var missingMedicamentIds))
        {
            return new BadRequestObjectResult($"Medicament doesn't exist: {string.Join(", ", missingMedicamentIds)}");
        }

        EnsureEntityExists(newPrescription.Patient);
        EnsureEntityExists(newPrescription.Doctor);

        var prescriptionEntity = AddNewPrescription(newPrescription);
        AddPrescriptionMedicaments(newPrescription, prescriptionEntity.IdPrescription);

        return new OkObjectResult("Added");
    }

    private bool IsInvalidPrescriptionDate(PrescriptionDTO prescription)
    {
        return prescription.DueDate <= prescription.Date;
    }

    private bool HasTooManyMedicaments(PrescriptionDTO prescription)
    {
        return prescription.Medicament != null && prescription.Medicament.Count() >= 10;
    }

    private bool ContainsInvalidMedicaments(PrescriptionDTO prescription, out List<int> missingMedicamentIds)
    {
        var medicamentIds = prescription.Medicament?.Select(m => m.IdMedicament).ToList() ?? new List<int>();
        var existingMedicamentIds = _contextEf.Medicaments.Select(m => m.IdMedicament).ToList();
        missingMedicamentIds = medicamentIds.Except(existingMedicamentIds).ToList();
        return missingMedicamentIds.Any();
    }

    private void EnsureEntityExists<T>(T entityDto) where T : class
    {
        switch (entityDto)
        {
            case PatientDTO patientDto:
                EnsurePatientExists(patientDto);
                break;
            case DoctorDTO doctorDto:
                EnsureDoctorExists(doctorDto);
                break;
        }
    }

    private void EnsurePatientExists(PatientDTO patientDto)
    {
        if (!_contextEf.Patients.Any(p => p.IdPatient == patientDto.IdPatient))
        {
            _contextEf.Patients.Add(new Patient
            {
                IdPatient = patientDto.IdPatient,
                FirstName = patientDto.FirstName,
                LastName = patientDto.LastName,
                Birthdate = patientDto.Birthdate,
            });
            _contextEf.SaveChanges();
        }
    }

    private void EnsureDoctorExists(DoctorDTO doctorDto)
    {
        if (!_contextEf.Doctors.Any(d => d.IdDoctor == doctorDto.IdDoctor))
        {
            _contextEf.Doctors.Add(new Doctor
            {
                IdDoctor = doctorDto.IdDoctor,
                FirstName = doctorDto.FirstName,
                LastName = doctorDto.LastName,
                Email = doctorDto.Email,
            });
            _contextEf.SaveChanges();
        }
    }

    private Prescription AddNewPrescription(PrescriptionDTO prescriptionDto)
    {
        var newPrescription = new Prescription
        {
            Date = prescriptionDto.Date,
            DueDate = prescriptionDto.DueDate,
            IdPatient = prescriptionDto.Patient.IdPatient,
            IdDoctor = prescriptionDto.Doctor.IdDoctor,
        };

        var addedEntity = _contextEf.Prescriptions.Add(newPrescription);
        _contextEf.SaveChanges();
        return addedEntity.Entity;
    }

    private void AddPrescriptionMedicaments(PrescriptionDTO prescriptionDto, int prescriptionId)
    {
        var prescriptionMedicaments = prescriptionDto.Medicament.Select(m => new Prescription_Medicament()
        {
            IdMedicament = m.IdMedicament,
            IdPrescription = prescriptionId,
            Dose = m.Dose,
            Details = m.Details,
        }).ToList();

        _contextEf.PrescriptionMedicaments.AddRange(prescriptionMedicaments);
        _contextEf.SaveChanges();
    }
}