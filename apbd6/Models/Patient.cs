using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices.JavaScript;

namespace apbd6.Models;

public class Patient
{
    [Key]
    public int IdPatient { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string FirstName { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string LastName { get; set; }
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime Birthdate { get; set; }
    
    public ICollection<Prescription_Medicament> PrescriptionMedicaments { get; set; }
}