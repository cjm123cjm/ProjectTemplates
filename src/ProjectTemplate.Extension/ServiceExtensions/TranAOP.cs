using Castle.DynamicProxy;
using Microsoft.Extensions.Logging;
using ProjectTemplate.Common.CustomizeAttributes;
using ProjectTemplate.Repository.UnitOfWorks;
using System.Reflection;

namespace ProjectTemplate.Extension.ServiceExtensions
{
    public class TranAOP : IInterceptor
    {
        private readonly ILogger<TranAOP> _logger;
        private readonly IUnitOfWorkManage _unitOfWorkManage;

        public TranAOP(ILogger<TranAOP> logger, IUnitOfWorkManage unitOfWorkManage)
        {
            _logger = logger;
            _unitOfWorkManage = unitOfWorkManage;
        }

        public void Intercept(IInvocation invocation)
        {
            var method = invocation.MethodInvocationTarget ?? invocation.Method;
            if (method.GetCustomAttribute<UseTranAttribute>(true) is { } uta)
            {
                try
                {
                    Before(method, uta.Propagation);

                    invocation.Proceed();

                    if (IsAsyncMethod(method))
                    {
                        var result = invocation.ReturnValue;
                        if (result is Task)
                        {
                            Task.WaitAll(result as Task);
                        }
                    }

                    After(method);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.ToString());
                    AfterException(method);
                    throw;
                }
            }
            else
            {
                invocation.Proceed();
            }
        }
        public void Before(MethodInfo method, Propagation propagation)
        {
            switch (propagation)
            {
                case Propagation.Required:
                    if (_unitOfWorkManage.TranCount <= 0)
                    {
                        _logger.LogDebug($"Begin Transaction");
                        Console.WriteLine($"Begin Transaction");
                        _unitOfWorkManage.BeginTran(method);
                    }
                    break;
                case Propagation.Mandatory:
                    if (_unitOfWorkManage.TranCount <= 0)
                    {
                        throw new Exception("事务传播机制为:[Mandatory],当前不存在事务");
                    }
                    break;
                case Propagation.Nested:
                    _logger.LogDebug($"Begin Transaction");
                    Console.WriteLine($"Begin Transaction");
                    _unitOfWorkManage.BeginTran(method);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(propagation), propagation, null);
            }
        }
        private void After(MethodInfo method)
        {
            _unitOfWorkManage.CommitTran(method);
        }
        private void AfterException(MethodInfo method)
        {
            _unitOfWorkManage.RollbackTran(method);
        }
        public static bool IsAsyncMethod(MethodInfo method)
        {
            return
                method.ReturnType == typeof(Task) ||
                method.ReturnType.IsGenericType && method.ReturnType.GetGenericTypeDefinition() == typeof(Task<>);
        }
    }
}
