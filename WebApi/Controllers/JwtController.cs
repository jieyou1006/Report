using Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JwtController : ControllerBase
    {
        public JwtController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }

        [HttpPost]
        public string CreateToken()
        {
            var tokenModel = Configuration.GetSection("Jwt").Get<TokenModelJwt>();
            tokenModel.UserName = "张三";
            tokenModel.UserId = 1;
            tokenModel.Role = "Admin";
            return JwtHelper.CreateJwt(tokenModel);
        }
    }
}
