namespace ProjectTemplate.Common.Caches
{
    public class RedisOption
    {
        public bool Enable { get; set; }
        public string ConnectionString { get; set; } = string.Empty;
        public string InstanceName { get; set; } = string.Empty;
    }
}
