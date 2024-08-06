namespace ProjectTemplate.Common.CustomizeAttributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class UseTranAttribute : Attribute
    {
        public Propagation Propagation { get; set; } = Propagation.Required;
    }
}
