using SqlSugar;

namespace ProjectTemplate.Model.Tenants.Dtos
{
    public class SysTenantDto
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 租户类型
        /// </summary>
        public TenantTypeEnum TenantType { get; set; }

        /// <summary>
        /// 数据库/租户标识 不可重复<br/>
        /// 使用Id方案,可无需配置
        /// </summary>
        public string ConfigId { get; set; }

        /// <summary>
        /// 主机<br/>
        /// 使用Id方案,可无需配置
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 数据库类型<br/>
        /// 使用Id方案,可无需配置
        /// </summary>
        public DbType? DbType { get; set; }

        /// <summary>
        /// 数据库连接<br/>
        /// 使用Id方案,可无需配置
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
