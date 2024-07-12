using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate.Common.Caches;

namespace ProjectTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ICaching _caching;

        public ValuesController(ICaching caching)
        {
            _caching = caching;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            await _caching.SetStringAsync("aaa", "bbbb");
            return Ok(await _caching.GetStringAsync("aaa"));
        }
    }
}
