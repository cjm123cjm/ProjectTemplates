using SqlSugar;

namespace ProjectTemplate.Model.LogEntity
{
    public class BaseLog
    {
        [SugarColumn(IsPrimaryKey = true, IsNullable = false)]
        public long Id { get; set; }

        //根据这个字段来分表
        [SplitField]
        public DateTime? DateTime { get; set; }

        [SugarColumn(IsNullable = true)]
        public string Level { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "lobgtext,text,clob")]
        public string Message { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "lobgtext,text,clob")]
        public string MessageTemplate { get; set; }

        [SugarColumn(IsNullable = true, ColumnDataType = "lobgtext,text,clob")]
        public string Properties { get; set; }
    }
}
