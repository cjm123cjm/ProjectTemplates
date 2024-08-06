using Microsoft.Extensions.Logging;
using ProjectTemplate.Common;
using SqlSugar;
using System.Collections.Concurrent;
using System.Reflection;

namespace ProjectTemplate.Repository.UnitOfWorks
{
    public class UnitOfWorkManage : IUnitOfWorkManage
    {
        private readonly ISqlSugarClient _sqlSugarClient;
        private readonly ILogger<UnitOfWorkManage> _logger;

        private int _tranCount { get; set; }
        public int TranCount => _tranCount;
        private readonly ConcurrentStack<string> TranStack = new();

        public UnitOfWorkManage(ISqlSugarClient sqlSugarClient, ILogger<UnitOfWorkManage> logger)
        {
            _sqlSugarClient = sqlSugarClient;
            _logger = logger;
            _tranCount = 0;
        }

        public SqlSugarScope GetDbClient()
        {
            return _sqlSugarClient as SqlSugarScope;
        }

        public void BeginTran()
        {
            lock (this)
            {
                _tranCount++;
                GetDbClient().BeginTran();
            }
        }

        public void BeginTran(MethodInfo methodInfo)
        {
            lock (this)
            {
                GetDbClient().BeginTran();
                TranStack.Push(methodInfo.Name);
                _tranCount = TranStack.Count;
            }
        }

        public void CommitTran()
        {
            lock (this)
            {
                _tranCount--;
                if (_tranCount == 0)
                {
                    try
                    {
                        GetDbClient().CommitTran();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex.Message);
                        GetDbClient().RollbackTran();
                    }
                }
            }
        }

        public void CommitTran(MethodInfo methodInfo)
        {
            lock (this)
            {
                string result = "";
                while (!TranStack.IsEmpty && !TranStack.TryPeek(out result))
                {
                    Thread.Sleep(1);
                }

                if (result == methodInfo.GetFullName())
                {
                    try
                    {
                        GetDbClient().CommitTran();
                        _logger.LogInformation("CommitTran {0}", methodInfo.Name);
                    }
                    catch (Exception ex)
                    {
                        GetDbClient().RollbackTran();
                        _logger.LogError(ex.Message);
                    }
                    finally
                    {
                        while (!TranStack.TryPop(out _))
                        {
                            Thread.Sleep(1);
                        }

                        _tranCount = TranStack.Count;
                    }
                }
            }
        }

        public void RollbackTran()
        {
            lock (this)
            {
                _tranCount--;
                GetDbClient().RollbackTran();
            }
        }
        public void RollbackTran(MethodInfo methodInfo)
        {
            lock (this)
            {
                string result = "";
                while (!TranStack.IsEmpty && !TranStack.TryPeek(out result))
                {
                    Thread.Sleep(1);
                }

                if (result == methodInfo.GetFullName())
                {
                    GetDbClient().RollbackTran();

                    _logger.LogError("RollbackTran {0}", methodInfo.Name);

                    while (!TranStack.TryPop(out _))
                    {
                        Thread.Sleep(1);
                    }

                    _tranCount = TranStack.Count;
                }
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
