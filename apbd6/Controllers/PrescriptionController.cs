using apbd6.DTOModel;
using apbd6.Service;
using Microsoft.AspNetCore.Mvc;

namespace apbd6.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PrescriptionsController : ControllerBase
{
    private readonly IPrescriptionsService _prescriptionsService;

    public PrescriptionsController(IPrescriptionsService prescriptionsService)
    {
        _prescriptionsService = prescriptionsService;
    }

    [HttpPost]
    public IActionResult AddPrescription([FromBody] PrescriptionDTO prescription)
    {
        var result = _prescriptionsService.AddPrescription(prescription);
        return Ok(result);
    }
}