using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Domain
{
    public static class DateConverter
    {

        private const int startGreg = 1900;
        private const int endGreg = 2100;
        private static string[] allFormats ={"yyyy/MM/dd","yyyy/M/d",
            "dd/MM/yyyy","d/M/yyyy",
            "dd/M/yyyy","d/MM/yyyy","yyyy-MM-dd",
            "yyyy-M-d","dd-MM-yyyy","d-M-yyyy",
            "dd-M-yyyy","d-MM-yyyy","yyyy MM dd",
            "yyyy M d","dd MM yyyy","d M yyyy",
            "dd M yyyy","d MM yyyy"};
        private static CultureInfo arCul;
        private static CultureInfo enCul;
        private static HijriCalendar h;
        private static GregorianCalendar g;

        public static System.DateTime HijriToGregorian(System.DateTime date)
        {

            if(date.Year < 1900)
            {
                string hijri = date.ToString("yyyy-MM-dd");
                System.DateTime Gregorian = System.DateTime.ParseExact(hijri, "yyyy-MM-dd", new CultureInfo("en-US"), DateTimeStyles.AllowInnerWhite);

                return Gregorian;
            }
                return date;

        }

        public static System.DateTime HijriToArabicGregorian(System.DateTime date)
        {

            string hijri = date.ToString("yyyy-MM-dd");
            System.DateTime Gregorian = System.DateTime.ParseExact(hijri, "yyyy-MM-dd", new CultureInfo("ar-EG"), DateTimeStyles.AllowInnerWhite);

            return Gregorian;
        }

        public static System.DateTime GregorianToHijri(System.DateTime date)
        {
            string Gregorian = date.ToString("yyyy-MM-dd");
            System.DateTime hijri = System.DateTime.ParseExact(Gregorian, "yyyy-MM-dd", new CultureInfo("ar-SA"), DateTimeStyles.AllowInnerWhite);

            return hijri;
        }

        public static string ConvertDateCalendar(DateTime DateConv, string Calendar, string DateLangCulture)
        {
            System.Globalization.DateTimeFormatInfo DTFormat;
            DateLangCulture = DateLangCulture.ToLower();
            /// We can't have the hijri date writen in English. We will get a runtime error - LAITH - 11/13/2005 1:01:45 PM -

            if (Calendar == "Hijri" && DateLangCulture.StartsWith("en-"))
            {
                DateLangCulture = "ar-sa";
            }

            /// Set the date time format to the given culture - LAITH - 11/13/2005 1:04:22 PM -
            DTFormat = new System.Globalization.CultureInfo(DateLangCulture, false).DateTimeFormat;

            /// Set the calendar property of the date time format to the given calendar - LAITH - 11/13/2005 1:04:52 PM -
            switch (Calendar)
            {
                case "Hijri":
                    DTFormat.Calendar = new System.Globalization.HijriCalendar();
                    break;

                case "Gregorian":
                    DTFormat.Calendar = new System.Globalization.GregorianCalendar();
                    break;

                default:
                    return "";
            }

            /// We format the date structure to whatever we want - LAITH - 11/13/2005 1:05:39 PM -
            DTFormat.ShortDatePattern = "yyyy-MM-dd";
           // return DateConv.Date.ToString("yyyy-MM-dd");
            return (DateConv.Date.ToString("f", DTFormat));
        }

        public static string HijriToGreg(string hijri)
        {
            arCul = new CultureInfo("ar-SA");
            enCul = new CultureInfo("en-US");

            h = new HijriCalendar();
            g = new GregorianCalendar(GregorianCalendarTypes.USEnglish);

            arCul.DateTimeFormat.Calendar = h;
            if (hijri.Length <= 0)
            {
               // cur.Trace.Warn("HijriToGreg :Date String is Empty");
                return "Date String is Empty";
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(hijri,
                   allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);

                return tempDate.ToString(allFormats[7], enCul.DateTimeFormat);
            }
            catch (Exception ex)
            {
               // cur.Trace.Warn("HijriToGreg :" + hijri.ToString() + "\n" + ex.Message);
                return ex.Message;
            }
        }

        public static string GregToHijri(string greg)
        {
            if (greg.Length <= 0)
            {
                //cur.Trace.Warn("GregToHijri :Date String is Empty");
                return "Date String is Empty";
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(greg, allFormats,
                    enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString("yyyy/MM/dd", arCul.DateTimeFormat);
            }
            catch (Exception ex)
            {
                //cur.Trace.Warn("GregToHijri :" + greg.ToString() + "\n" + ex.Message);
                return ex.Message.ToString();
            }
        }

        public static string FormatHijri(string date)
        {
            if (date.Length <= 0)
            {
               // cur.Trace.Warn("Format :Date String is Empty");
                return "Date String is Empty";
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(date,
                   allFormats, arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                return tempDate.ToString(allFormats[6], arCul.DateTimeFormat);
            }
            catch (Exception ex)
            {
               // cur.Trace.Warn("Date :\n" + ex.Message);
                return "";
            }
        }

        public static bool IsHijri(string hijri)
        {
            if (hijri.Length <= 0)
            {

                //cur.Trace.Warn("IsHijri Error: Date String is Empty");
                return false;
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(hijri, allFormats,
                     arCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
               // cur.Trace.Warn("IsHijri Error :" + hijri.ToString() + "\n" +
                 //                 ex.Message);
                return false;
            }
        }

        public static bool IsGreg(string greg)
        {
            if (greg.Length <= 0)
            {

                //cur.Trace.Warn("IsGreg :Date String is Empty");
                return false;
            }
            try
            {
                DateTime tempDate = DateTime.ParseExact(greg, allFormats,
                    enCul.DateTimeFormat, DateTimeStyles.AllowWhiteSpaces);
                if (tempDate.Year >= startGreg && tempDate.Year <= endGreg)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                //cur.Trace.Warn("IsGreg Error :" + greg.ToString() + "\n" + ex.Message);
                return false;
            }
        }
    }
}
