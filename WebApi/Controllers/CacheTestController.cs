using Common;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CacheTestController : ControllerBase
    {
        MemoryCacheHelper m = new MemoryCacheHelper();

        [HttpPost]
        public string SetCache(string key,string value)
        {
            if (m.Set(key, value, TimeSpan.FromSeconds(20), TimeSpan.FromMinutes(60)))
                return "OK";
            else
                return "NG";
        }

        [HttpGet]
        public string GetCache(string key)
        {
            if (m.Exists(key))
                return m.Get(key).ToString();
            else
                return string.Empty;
        }
    }
}
