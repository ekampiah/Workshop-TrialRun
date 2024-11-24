using Microsoft.AspNetCore.Mvc;

namespace Todo.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase{
    /// <summary>
    /// Test Get operation
    /// </summary>
    /// <returns>string response if succesful</returns>
    [HttpGet]
    [Produces("text/plain")]
    public ActionResult<string> TestMethod(){
        return Ok("Testicles");
    }

    /// <summary>
    /// Test POST operation
    /// </summary>
    /// <param name="text">The input text</param>
    /// <returns>string containing input text</returns>
    [HttpPost]
    public ActionResult TestPost([FromBody] string text){
        return Ok($"Received {text}");
    }
}