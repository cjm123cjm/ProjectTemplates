using SqlSugar;

namespace ProjectTemplate.Model.Tenants
{
    public interface ITenantEntity
    {
        [SugarColumn(DefaultValue = "0")]
        public long TenantId { get; set; }
    }
}
