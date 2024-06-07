using apbd6.Service;
using Microsoft.AspNetCore.Mvc;

namespace apbd6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientsService _patientsService;

    public PatientsController(IPatientsService patientsService)
    {
        _patientsService = patientsService;
    }

    [HttpGet("{patientId}")]
    public IActionResult GetPatientData(int patientId)
    {
        var result = _patientsService.GetPatientData(patientId);
        return Ok(result);
    }
}