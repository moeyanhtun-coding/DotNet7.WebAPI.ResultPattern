using DotNet7.Blog.Domain;

namespace DotNet7.Blog.WebAPI.Controllers;

public class BaseController : ControllerBase
{
    public IActionResult Execute<T>(Result<T> model)
    {
        if (model.IsValidationError)
            return BadRequest(model);
        if (model.IsSystemError)
            return StatusCode(500, model);
        return Ok(model);
    }
}