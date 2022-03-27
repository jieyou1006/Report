using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MemoryController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        public MemoryController(IMemoryCache cache)
        {
            _cache = cache;
        }

        [HttpPost]
        public string Set(string Name)
        {
            var key = Guid.NewGuid().ToString();
            _cache.Set(key, Name, TimeSpan.FromSeconds(100));
            return key;
        }

        [HttpGet]
        public string Get(string key)
        {
            return _cache.Get(key)?.ToString();
        }

        [HttpDelete]
        public bool Del(string key)
        {
            try
            {
                _cache.Remove(key);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
