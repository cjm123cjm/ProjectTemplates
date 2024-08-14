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
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class TenantController : ControllerBase
    {
        private readonly IBaseService<BusinessTable, BusinessTableDto> _baseService;
        private readonly IBaseService<MultiBusinessTable, MultiBusinessTableDto> _multiBusinessTableService;

        public TenantController(IBaseService<BusinessTable, BusinessTableDto> baseService, IBaseService<MultiBusinessTable, MultiBusinessTableDto> multiBusinessTableService)
        {
            _baseService = baseService;
            _multiBusinessTableService = multiBusinessTableService;
        }

        /// <summary>
        /// 多租户-单表字段
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Get1()
        {
            return Ok(await _baseService.Query());
        }

        /// <summary>
        /// 多租户-多表字段
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Get2()
        {
            return Ok(await _multiBusinessTableService.Query());
        }
    }
}
