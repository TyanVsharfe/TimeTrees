using System;
using System.IO;
using System.Globalization;

namespace timetrees
{
    public class ValidationCheck
    {
        public static DateTime GetTrueDateTime()
        {
            DateTime timeTrue = DateTime.MinValue;
            do
            {
                string date = Console.ReadLine();
                if ((date == "0") | (date == "")) return DateTime.MinValue;
                if (ParseDate(date) == DateTime.MinValue) Console.WriteLine("Дата введена некорректно, попробуйте снова");
                else
                {
                    timeTrue = ParseDate(date);
                }
            }
            while (timeTrue == DateTime.MinValue);
            return timeTrue;
        }

        public static DateTime ParseDate(string value)
        {
            DateTime date;
            if (!DateTime.TryParseExact(value, "yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
            {
                if (!DateTime.TryParseExact(value, "mm-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    if (!DateTime.TryParseExact(value, "dd-mm-yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                    {
                        return DateTime.MinValue;
                    }
                }
            }
            return date;
        }
    }
}
