# ProjectTemplates
ProjectTemplate.Extension：扩展层，Automapper、Autofact...写在这一层

## 事务

```c#
[UseTran(Propagation = Propagation.Required)]
```



## 自定义授权策略

```markdown
先获取角色，再获取角色所能访问的菜单url，自定一个授权策略，所访问的url和当前访问的url做对比，有就可以访问，没有就返回403
```



## 多租户

新建一个SysTenant表

| Id         | long         | 主键                                                         |
| ---------- | ------------ | ------------------------------------------------------------ |
| Name       | varchar(100) | 名称                                                         |
| TenantType | int          | 多租户类型(1:单表字段,2:分库,3:分表)                         |
| ConfigId   | varchar(50)  | 查询哪个数据库                                               |
| Host       | varchar(50)  | 主机IP                                                       |
| DbType     | int          | 数据库类型(<br/>      MySql = 0,<br/>      SqlServer = 1,<br/>      Sqlite = 2,<br/>      Oracle = 3,<br/>      PostgreSQL = 4,<br/>      Dm = 5,//达梦<br/>      Kdbndp = 6,//人大金仓) |
| Connection | varchar(100) | 连接字符串                                                   |
| Status     | int          | 是否启用                                                     |
| Rematk     | varchar(50)  | 备注                                                         |

多租户单表：根据用户表的TenantId和业务表的TenantId匹配查询。

多租户多表：根据TenantId生成多表，比如用户1的TenantId为10001，用户表2的TeanantId为10203，那么就会生成两张表：MultiBusinessTable_10001、MultiBusinessTable_10203。

多租户多库：根据用户的TenantId到SysTenant表里查询数据(TenantId=SysTenant.id)，拿到Connection，根据Connection切换库操作。



## SqlSugar日志输出与自动缓存
