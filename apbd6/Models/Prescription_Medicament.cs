using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace apbd6.Models;

public class Prescription_Medicament
{
    [Required]
    public int IdMedicament { get; set; }
    
    [Key]
    public int IdPrescription { get; set; }
    
    public int Dose { get; set; }
    
    [MaxLength(100)]
    [Required]
    public string Details { get; set; }
    
    [ForeignKey(nameof(IdPrescription))]
    public Prescription Prescription { get; set; }
    
    [ForeignKey(nameof(IdMedicament))]
    public Medicament Medicament { get; set; }
}