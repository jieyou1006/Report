using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public IActionResult DoToken([FromHeader] string Authorization)
        {
            //反解析Token的值
            var token = JwtHelper.SerializeJwt(Authorization.Replace("Bearer ",""));
            return Ok(token);
        }
    }
}
