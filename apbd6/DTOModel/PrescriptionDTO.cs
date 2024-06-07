namespace apbd6.DTOModel;

public class PrescriptionDTO
{
    public PatientDTO Patient { get; set; }

    public DoctorDTO Doctor { get; set; }   

    public IEnumerable<MedicamentDTO> Medicament { get; set; }

    public DateTime Date { get; set; }  
    public DateTime DueDate { get; set; } 
}