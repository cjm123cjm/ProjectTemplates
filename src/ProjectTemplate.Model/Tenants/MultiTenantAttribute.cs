namespace ProjectTemplate.Model.Tenants
{
    [AttributeUsage(AttributeTargets.Class)]
    public class MultiTenantAttribute : Attribute
    {
        public TenantTypeEnum TenantTypeEnum { get; set; }

        public MultiTenantAttribute(TenantTypeEnum tenantTypeEnum)
        {
            TenantTypeEnum = tenantTypeEnum;
        }
        public MultiTenantAttribute()
        {
            
        }
    }
}
