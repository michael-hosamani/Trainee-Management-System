using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class HealthController: ControllerBase
{
    [HttpGet]
    public ActionResult Get(){
        // this is the GET /api/health route
        return Ok(new { 
            status = "running", 
            application = "Trainee Management App", 
            timestamp = DateTime.Now 
        });
    }

}