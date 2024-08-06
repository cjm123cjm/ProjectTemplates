using Microsoft.AspNetCore.Mvc;
using ProjectTemplate.Common;
using ProjectTemplate.IService.Base;
using ProjectTemplate.Model.Entity;
using ProjectTemplate.Repository.UnitOfWorks;

namespace ProjectTemplate.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUnitOfWorkManage _unitOfWorkManage;
        private readonly IBaseService<SysUserInfo, SysUserInfo> _sysUserInfoService;

        public ValuesController(IUnitOfWorkManage unitOfWorkManage, IBaseService<SysUserInfo, SysUserInfo> sysUserInfoService)
        {
            _unitOfWorkManage = unitOfWorkManage;
            _sysUserInfoService = sysUserInfoService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //_unitOfWorkManage.BeginTran();

                using var tran = _unitOfWorkManage.CreateUnitOfWork(); //开启事务

                var query1 = await _sysUserInfoService.Query();
                Console.WriteLine($"count={query1.Count}");

                TimeSpan timeSpan = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                long id = await _sysUserInfoService.Add(new SysUserInfo
                {
                    Id = timeSpan.TotalSeconds.ObjToLong(),
                    LoginName = "chen",
                    LoginPWD = "chen123"
                });
                var query = await _sysUserInfoService.Query();
                Console.WriteLine($"count={query.Count}");

                //throw new Exception("测试异常");

                //_unitOfWorkManage.CommitTran();
                tran.Commit();

                return Ok();
            }
            catch (Exception ex)
            {
                _unitOfWorkManage.RollbackTran();
                var query = await _sysUserInfoService.Query();
                Console.WriteLine($"count={query.Count}");
                return BadRequest();
            }
        }

        /// <summary>
        /// 事务AOP形式
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult AopTran()
        {
            //调用service的方法,每个方法上面使用事务特性,[UseTran(Propagation = Propagation.Required)]
            return Ok();
        }
    }
}
