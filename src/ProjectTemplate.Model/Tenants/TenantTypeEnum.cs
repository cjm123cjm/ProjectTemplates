using System.ComponentModel;

namespace ProjectTemplate.Model.Tenants
{
    /// <summary>
    /// 多租户阻隔方案
    /// </summary>
    public enum TenantTypeEnum
    {
        None,

        /// <summary>
        /// Id隔离
        /// </summary>
        [Description("Id隔离")]
        Id = 1,

        /// <summary>
        /// 表隔离
        /// </summary>
        [Description("表隔离")]
        Table = 3,

        /// <summary>
        /// 库隔离
        /// </summary>
        [Description("库隔离")]
        Db = 2
    }
}
