using System;

namespace CodacyCSharp.Metrics.Seed
{
    /// <summary>
    ///     TimeSpan helper.
    ///     This parse a given string to a TimeSpan.
    /// </summary>
    public static class TimeSpanHelper
    {
        /// <summary>
        ///     Parse a TimeSpan from a string that contains a integer
        ///     with seconds
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        /// <exception cref="FormatException"></exception>
        public static TimeSpan Parse(string value)
        {
            return TimeSpan.FromSeconds(int.Parse(value));
        }
    }
}
