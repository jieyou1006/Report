using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using Services.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CheckController : ControllerBase
    {
        [HttpPost("User")]
        public ValidationResult GetList([FromBody] User user)
        {
            UserValidator va = new UserValidator();
            ValidationResult result = va.Validate(user);

            return result;
        }
    }
}
