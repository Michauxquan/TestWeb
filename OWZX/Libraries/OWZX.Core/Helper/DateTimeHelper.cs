using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
    public static class DateTimeHelper
    {
        public static DateTime BaseTime = new DateTime(1970, 1, 1);//Unix起始时间

        /// <summary>
        /// 转换微信DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(long dateTimeFromXml)
        {
            return BaseTime.AddTicks((dateTimeFromXml + 8 * 60 * 60) * 10000000);
        }
        /// <summary>
        /// 转换微信DateTime时间到C#时间
        /// </summary>
        /// <param name="dateTimeFromXml">微信DateTime</param>
        /// <returns></returns>
        public static DateTime GetDateTimeFromXml(string dateTimeFromXml)
        {
            return GetDateTimeFromXml(long.Parse(dateTimeFromXml));
        }

        /// <summary>
        /// 获取微信DateTime（UNIX时间戳）
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns></returns>
        public static long GetWeixinDateTime(DateTime dateTime)
        {
            return (dateTime.Ticks - BaseTime.Ticks) / 10000000 - 8 * 60 * 60;
        }
        /// <summary>
        /// UnixToDateTime
        /// </summary>
        /// <param name="unixTime"></param>
        /// <returns></returns>
        public static DateTime FromUnixTime(long unixTime)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return epoch.AddMilliseconds(unixTime);
        }
        /// <summary>
        /// DateTimeToUnix
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static long ToUnixTime(DateTime date)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            return Convert.ToInt64((date.ToUniversalTime() - epoch).TotalMilliseconds);
        }

        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }
        /**/
        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /**/
        /// <summary>
        /// 取得上个月第一天
        /// </summary>
        /// <param name="datetime">要取得上个月第一天的当前时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfPreviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }

        /**/
        /// <summary>
        /// 取得上个月的最后一天
        /// </summary>
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfPrdviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }

        /// <summary>
        /// 本周起止时间
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>开始时间-结束时间</returns>
        public static string WeekRange(System.DateTime dt)
        {
            int weeknow = Convert.ToInt32(dt.DayOfWeek);
            int daydiff = (-1) * weeknow;
            int dayadd = 6 - weeknow;
            string datebegin = System.DateTime.Now.AddDays(daydiff).ToString("yyyy-MM-dd");
            string dateend = System.DateTime.Now.AddDays(dayadd).ToString("yyyy-MM-dd");
            return datebegin + "$" + dateend;
        }
        /// <summary>
        /// 将秒转换为时 分 秒
        /// </summary>
        /// <param name="second">秒</param>
        /// <param name="iscomp">是否完整显示 时 分 秒</param>
        /// <returns></returns>
        public static string SecondToTime(string second, bool iscomp)
        {
            TimeSpan ts = new TimeSpan(0, 0, Convert.ToInt32(second));
            StringBuilder str = new StringBuilder();
            if (iscomp)
            {
                str.Append(ts.Hours.ToString() + "时" + ts.Minutes.ToString() + "分" + ts.Seconds + "秒");
            }
            else
            {
                if (ts.Hours > 0)
                    str.Append(ts.Hours.ToString() + "时");
                if (ts.Minutes > 0)
                    str.Append(ts.Minutes.ToString() + "分");
                if (ts.Seconds > 0)
                    str.Append(ts.Seconds + "秒");
            }

            return str.ToString();
        }
    }
}
