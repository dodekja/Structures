using System;

namespace SemestralThesisOne.Core.Generators
{
    internal static class DateTimeGenerator
    {
        public static DateTime GenerateRandomDateTime(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            return start.AddDays(Random.Shared.Next(range));
        }
    }
}
