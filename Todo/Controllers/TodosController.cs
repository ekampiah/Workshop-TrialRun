using Microsoft.AspNetCore.Mvc;

namespace Todo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase{
    [HttpGet]
    public string TestMethod(){
        return "Testicles";
    }
}