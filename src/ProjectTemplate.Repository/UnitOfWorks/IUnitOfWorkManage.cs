using SqlSugar;
using System.Reflection;

namespace ProjectTemplate.Repository.UnitOfWorks
{
    public interface IUnitOfWorkManage
    {
        SqlSugarScope GetDbClient();
        int TranCount { get; }
        UnitOfWork CreateUnitOfWork();
        void BeginTran();
        void BeginTran(MethodInfo methodInfo);
        void CommitTran();
        void CommitTran(MethodInfo methodInfo);
        void RollbackTran();
        void RollbackTran(MethodInfo methodInfo);
    }
}
