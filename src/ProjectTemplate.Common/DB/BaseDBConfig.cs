using SqlSugar;

namespace ProjectTemplate.Common.DB
{
    public class BaseDBConfig
    {
        /// <summary>
        /// 所有数据库配置
        /// </summary>
        public static readonly List<ConnectionConfig> AllConfig = new List<ConnectionConfig>();

        /// <summary>
        /// 有效的库连接(除去Log库)
        /// </summary>
        public static readonly List<ConnectionConfig> ValidConfig = new List<ConnectionConfig>();

        public static ConnectionConfig? MainConfig = null;
        public static ConnectionConfig? LogConfig = null;


        /* 之前的单库操作已经删除，如果想要之前的代码，可以查看我的GitHub的历史记录
         * 目前是多库操作，默认加载的是appsettings.json设置为true的第一个db连接。
         *
         * 优化配置连接
         * 老的配置方式,再多库和从库中有些冲突
         * 直接在单个配置中可以配置从库
         *
         * 新增故障转移方案
         * 增加主库备用连接,配置方式为ConfigId为主库的ConfigId+随便数字 只要不重复就好
         *
         * 主库在无法连接后会自动切换到备用链接
         */
        public static (List<MutiDBOperate> allDbs, List<MutiDBOperate> slaveDbs) MutiConnectionString;
    }
    public enum DataBaseType
    {
        MySql = 0,
        SqlServer = 1,
        Sqlite = 2,
        Oracle = 3,
        PostgreSQL = 4,
        Dm = 5,
        Kdbndp = 6,
    }
    public class MutiDBOperate
    {
        /// <summary>
        /// 连接启用开关
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 连接ID
        /// </summary>
        public string ConnId { get; set; }

        /// <summary>
        /// 从库执行级别，越大越先执行
        /// </summary>
        public int HitRate { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }

        /// <summary>
        /// 数据库类型
        /// </summary>
        public DataBaseType DbType { get; set; }

        /// <summary>
        /// 从库
        /// </summary>
        public List<MutiDBOperate> Slaves { get; set; }
    }
}
