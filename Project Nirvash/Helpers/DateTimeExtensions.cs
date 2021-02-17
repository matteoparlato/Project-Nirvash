using System;

namespace Project_Nirvash.Helpers
{
    /// <summary>
    /// DateTimeExtensions class
    /// </summary>
    internal static class DateTimeExtensions
    {
        public static DateTime StartingSchoolDate
        {
            get
            {
                if (DateTime.Today.Month >= 9 && DateTime.Today.Month <= 12)
                {
                    return new DateTime(DateTime.Today.Year, 9, 1);
                }
                else
                {
                    return new DateTime(DateTime.Today.AddYears(-1).Year, 9, 1);
                }
            }
        }

        public static DateTime EndingSchoolDate
        {
            get
            {
                if (DateTime.Today.Month >= 9 && DateTime.Today.Month <= 12)
                {
                    return new DateTime(DateTime.Today.AddYears(1).Year, 6, 30);
                }
                else
                {
                    return new DateTime(DateTime.Today.Year, 6, 30);
                }
            }
        }
    }
}
