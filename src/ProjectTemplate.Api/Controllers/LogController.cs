using Microsoft.AspNetCore.Mvc;
using ProjectTemplate.IService;

namespace ProjectTemplate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly IAuditSqlLogService _sqlLogService;
        public LogController(IAuditSqlLogService sqlLogService)
        {
            _sqlLogService = sqlLogService;
        }
        public async Task<ActionResult> Get()
        {
            var logList = await _sqlLogService.Query();
            return Ok(logList);
        }
    }
}
