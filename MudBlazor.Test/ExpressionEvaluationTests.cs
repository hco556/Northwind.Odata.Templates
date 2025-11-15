using System;
using System.Linq.Expressions;
using Xunit;

namespace Northwind.Test
{
    public class ExpressionEvaluationTests
    {
        [Fact]
        public void DayOfWeekExpression_EvaluatesWeekdaysTrueWeekendsFalse()
        {
            // Build expression equivalent to: (DateTime dt) => ((int)dt.DayOfWeek > 0 && (int)dt.DayOfWeek < 6)
            var dtParam = Expression.Parameter(typeof(DateTime), "dt");
            var dayOfWeekMember = Expression.Property(dtParam, nameof(DateTime.DayOfWeek));
            var dayOfWeekAsInt = Expression.Convert(dayOfWeekMember, typeof(int));

            var greaterThanZero = Expression.GreaterThan(dayOfWeekAsInt, Expression.Constant(0));
            var lessThanSix = Expression.LessThan(dayOfWeekAsInt, Expression.Constant(6));
            var body = Expression.AndAlso(greaterThanZero, lessThanSix);

            var lambda = Expression.Lambda<Func<DateTime, bool>>(body, dtParam);

            // Compile and invoke
            var predicate = lambda.Compile();

            // Weekday examples (should be true)
            var monday = new DateTime(2025, 11, 10);   // Monday
            var wednesday = new DateTime(2025, 11, 12); // Wednesday
            var friday = new DateTime(2025, 11, 14);   // Friday

            Assert.True(predicate(monday));
            Assert.True(predicate(wednesday));
            Assert.True(predicate(friday));

            // Weekend examples (should be false)
            var saturday = new DateTime(2025, 11, 15); // Saturday
            var sunday = new DateTime(2025, 11, 16);   // Sunday

            Assert.False(predicate(saturday));
            Assert.False(predicate(sunday));

            // Also demonstrate dynamic invocation via Delegate.DynamicInvoke
            var del = (Delegate)predicate;
            var result = (bool)del.DynamicInvoke(monday)!;
            Assert.True(result);
        }
    }
}
