using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OWZX.Core
{
   public class SubStrHelper
    {
        /// <summary>
        ///截取字符串（根据实际字符长度截取）,以‘...’结尾
        /// </summary>
        /// <param name="RawString"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string getString(string RawString, Int32 Length)
        {
            if (RawString.Length <= Length)
            {
                return RawString;
            }
            else
            {
                for (Int32 i = RawString.Length - 1; i >= 0; i--)
                {
                    if (System.Text.Encoding.GetEncoding("GB2312").GetByteCount(RawString.Substring(0, i)) < Length)
                    {
                        return RawString.Substring(0, i) + "...";
                    }
                }
                return "...";
            }
        }
        /// <summary>
        ///截取字符串（根据实际字符长度截取），只取指定长度
        /// </summary>
        /// <param name="RawString"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string getLimitString(string RawString, Int32 Length)
        {
            if (RawString.Length <= Length)
            {
                return RawString;
            }
            else
            {
                for (Int32 i = RawString.Length - 1; i >= 0; i--)
                {
                    if (System.Text.Encoding.GetEncoding("GB2312").GetByteCount(RawString.Substring(0, i)) < Length)
                    {
                        return RawString.Substring(0, i) ;
                    }
                }
                return "...";
            }
        }

        /// <summary> 
        /// 截取字符串，不限制字符串长度 
        /// </summary> 
        /// <param name="str">待截取的字符串</param> 
        /// <param name="len">每行的长度，多于这个长度自动换行</param> 
        /// <returns></returns> 
        public static string CutStr(string str, int len)
        {
            string s = "";

            for (int i = 0; i < str.Length; i++)
            {
                int r = i % len;
                int last = (str.Length / len) * len;
                if (i != 0 && i <= last)
                {

                    if (r == 0)
                    {
                        s += str.Substring(i - len, len) + "<br>";
                    }

                }
                else if (i > last)
                {
                    s += str.Substring(i - 1);
                    break;
                }

            }

            return s;

        }


        /// <summary> 
        /// 截取字符串并限制字符串长度，多于给定的长度＋。。。 
        /// </summary> 
        /// <param name="str">待截取的字符串</param> 
        /// <param name="len">每行的长度，多于这个长度自动换行</param> 
        /// <param name="max">输出字符串最大的长度</param> 
        /// <returns></returns> 
        public static string CutStr(string str, int len, int max)
        {
            string s = "";
            string sheng = "";
            if (str.Length > max)
            {
                str = str.Substring(0, max);
                sheng = "";
            }
            for (int i = 0; i < str.Length; i++)
            {
                int r = i % len;
                int last = (str.Length / len) * len;
                if (i != 0 && i <= last)
                {

                    if (r == 0)
                    {
                        s += str.Substring(i - len, len) + "<br>";
                    }

                }
                else if (i > last)
                {
                    s += str.Substring(i - 1);
                    break;
                }

            }

            return s + sheng;

        }
    }
}
