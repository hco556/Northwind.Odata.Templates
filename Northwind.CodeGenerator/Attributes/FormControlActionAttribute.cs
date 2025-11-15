using System;
using System.Linq.Expressions;

namespace MudBlazor.Northwind.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class FormControlActionAttribute<T> : Attribute
    {
        public FormControlActionAttribute(Expression<Func<T, object>> action)
        {
            if (action is null) throw new ArgumentNullException(nameof(action));
            _actionFunc = action.Compile();
        }
        private readonly Func<T, object> _actionFunc;

        public object? Evaluate(T model)
        {
            return _actionFunc?.Invoke(model);
        }
    }
}
