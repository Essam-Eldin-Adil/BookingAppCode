using Resources;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Domain
{
    public static class DateTimeHelper
    {
        public static int GetAge(System.DateTime BirthDate)
        {
            int age = System.DateTime.Today.Year - BirthDate.Year;
            return age;
        }

        public static string Duration(string startTime, string endTime)
        {
            string duration = "00:00:00";
            try
            {
                if (!string.IsNullOrEmpty(startTime) && !string.IsNullOrEmpty(endTime))
                {
                    var vStartTime = DateTime.ParseExact(startTime, "H:m", null, DateTimeStyles.None);
                    var vEndTime = DateTime.ParseExact(endTime, "H:m", null, DateTimeStyles.None);

                    TimeSpan span = vEndTime - vStartTime;
                    return span.TotalHours.ToString() + ":" + span.TotalMinutes.ToString() + ":" + span.TotalSeconds.ToString();
                }
            }
            catch
            {

            }
            return duration;
        }

        //public static System.DateTime? ConvertHijriToGregorian(System.DateTime? date)
        //{
        //    if(date != null)
        //        return System.DateTime.ParseExact(date?.ToShortDateString(), "dd/MM/yyyy", new CultureInfo("ar-SA"));
        //    return null;

        //}
        //public static System.DateTime? ConvertGregorianToHijri(System.DateTime? date)
        //{
        //    if (date != null)
        //        return System.DateTime.ParseExact(date?.ToShortDateString(), "dd/MM/yyyy", new CultureInfo("en-US"));
        //    return null;
        //}

        public static string Convert24HourTo12Hour(string time)
        {
            try
            {
                if (!string.IsNullOrEmpty(time))
                {
                    var result = DateTime.ParseExact(time, "H:m", CultureInfo.InvariantCulture);

                    time = result.ToString("hh:mm");
                    if (result.ToString("tt") == "ص")
                    {
                        time += " " + Resource.AM;
                    }
                    else
                    {
                        time += " " + Resource.PM;

                    }

                    return time;
                }
            }
            catch(Exception ex)
            {

            }
            return time;
        }
        public static string Convert12HourTo24Hour(string time)
        {
            try
            {
                if (!string.IsNullOrEmpty(time))
                {
                    var result = DateTime.ParseExact(time, "H:m", null, DateTimeStyles.None);
                    return result.TimeOfDay.ToString("HH:mm", CultureInfo.InvariantCulture);
                }
            }
            catch
            {

            }
            return time;
        }
        //public static System.DateTime? Date(System.DateTime? date)
        //{
        //    if (date?.Year < 1900)
        //    {
        //        return ConvertHijriToGregorian(date);
        //    }

        //    return date;
        //}

        public static string FormatDate(System.DateTime date, string format)
        {
            try
            {
                return date.ToString(format);
            }
            catch
            {
                return date.ToString();
            }
        }

        public static string GetShortTime(System.DateTime date, Enums.Hour mode = Enums.Hour.h12)
        {
            if (mode == Enums.Hour.h24)
            {
                return date.ToString("HH:mm");
            }

            return date.ToString("hh:mm tt");
        }
        public static string GetLongTime(System.DateTime date, Enums.Hour mode = Enums.Hour.h12)
        {
            if (mode == Enums.Hour.h24)
            {
                return date.ToString("HH:mm:ss");
            }

            return date.ToString("hh:mm:ss tt");
        }
    }
}
