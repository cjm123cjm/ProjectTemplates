using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectTemplate.IService.Base;
using ProjectTemplate.Model.Tenants;
using ProjectTemplate.Model.Tenants.Dtos;

namespace ProjectTemplate.Api.Controllers
{
    /// <summary>
    /// 多租户查询数据
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BusinessTableController : ControllerBase
    {
        private readonly IBaseService<BusinessTable, BusinessTableDto> _baseService;

        public BusinessTableController(IBaseService<BusinessTable, BusinessTableDto> baseService)
        {
            _baseService = baseService;
        }
        public async Task<IActionResult> Get()
        {
            return Ok(await _baseService.Query());
        }
    }
}
