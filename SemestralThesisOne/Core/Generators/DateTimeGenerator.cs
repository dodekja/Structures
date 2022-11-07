using System;

namespace SemestralThesisOne.Core.Generators
{
    internal static class DateTimeGenerator
    {
        public static DateTime GenerateRandomDateTime(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            DateTime dt = start.AddDays(Random.Shared.Next(range));
            dt = dt.AddSeconds(Random.Shared.Next(60));
            dt = dt.AddMinutes(Random.Shared.Next(60));
            dt = dt.AddHours(Random.Shared.Next(60));
            return dt;
        }

        public static DateTime AddRandomTime(DateTime dateTime)
        {
            dateTime = dateTime.AddSeconds(Random.Shared.Next(60));
            dateTime = dateTime.AddMinutes(Random.Shared.Next(60));
            dateTime = dateTime.AddHours(Random.Shared.Next(60));
            return dateTime;
        }
    }
}
