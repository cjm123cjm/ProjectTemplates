using SqlSugar;

namespace ProjectTemplate.Repository.UnitOfWorks
{
    public interface IUnitOfWorkManage
    {
        SqlSugarScope GetDbClient();
        UnitOfWork CreateUnitOfWork();
        void BeginTran();
        void CommitTran();
        void RollbackTran();
    }
}
