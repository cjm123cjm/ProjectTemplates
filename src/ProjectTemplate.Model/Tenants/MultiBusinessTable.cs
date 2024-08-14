namespace ProjectTemplate.Model.Tenants
{
    /// <summary>
    /// 多租户-多表方案 业务表
    /// </summary>
    [MultiTenant(TenantTypeEnum = TenantTypeEnum.Table)]
    public class MultiBusinessTable : RootEntityTkey<long>
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
