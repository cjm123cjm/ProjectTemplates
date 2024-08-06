using SqlSugar;

namespace ProjectTemplate.Model.LogEntity
{
    [Tenant("log")]
    [SplitTable(SplitType.Month)]
    [SugarTable($@"{nameof(AuditSqlLog)}_{{year}}{{month}}{{day}}")]
    public class AuditSqlLog : BaseLog
    {
    }
}
