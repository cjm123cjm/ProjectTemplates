using SqlSugar;

namespace ProjectTemplate.Model
{
    public class RootEntityTkey<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// ID
        /// 泛型主键Tkey
        /// </summary>
        [SugarColumn(IsNullable = false, IsPrimaryKey = true)]
        public TKey Id { get; set; }
    }
}
