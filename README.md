# ProjectTemplates
ProjectTemplate.Extension：扩展层，Automapper、Autofact...写在这一层

事务

```c#
[UseTran(Propagation = Propagation.Required)]
```



自定义授权策略

```markdown
先获取角色，再获取角色所能访问的菜单url，自定一个授权策略，所访问的url和当前访问的url做对比，有就可以访问，没有就返回403
```



多租户单表：根据用户表的TenantId和业务表的TenantId多匹配查询

多租户多表：根据TenantId生成多表，比如用户1的TenantId为10001，用户表2的TeanantId为10203，那么就会生成两张表：MultiBusinessTable_10001、MultiBusinessTable_10203

多租户多库：
