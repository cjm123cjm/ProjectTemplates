using Microsoft.Extensions.Logging;
using SqlSugar;

namespace ProjectTemplate.Repository.UnitOfWorks
{
    public class UnitOfWorkManage : IUnitOfWorkManage
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly ILogger<UnitOfWorkManage> _logger;

        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWorkManage> logger)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
        }

        public SqlSugarScope GetDbClient()
        {
            return _sqlSugarClient as SqlSugarScope;
        }

        public void BeginTran()
        {
            lock (this)
            {
                GetDbClient().BeginTran();
            }
        }

        public void CommitTran()
        {
            lock (this)
            {
                GetDbClient().CommitTran();
            }
        }

        public void RollbackTran()
        {
            lock (this)
            {
                GetDbClient().RollbackTran();
            }
        }

        public UnitOfWork CreateUnitOfWork()
        {
            UnitOfWork uow = new UnitOfWork();
            uow.Logger = _logger;
            uow.Db = _sqlSugarClient;
            uow.Tenant = (ITenant)_sqlSugarClient;
            uow.IsTran = true;

            uow.Db.Open();
            uow.Tenant.BeginTran();
            _logger.LogDebug("UnitOfWork Begin");
            return uow;
        }
    }
}
