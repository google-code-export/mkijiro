using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace Example
{
    public static class Ext
    {
        public static string GetResponseFrom(this string url)
        {
            WebRequest req = WebRequest.Create(url);

            using (WebResponse res = req.GetResponse())
            using (Stream stm = res.GetResponseStream())
            using (StreamReader sr = new StreamReader(stm, Encoding.GetEncoding("utf-8")))
            {
                return sr.ReadToEnd();
            }
        }

        public static string GetForConvWithGoogleTransliterate(this string hiragana)
        {
            StringBuilder url = new StringBuilder("http://www.google.com/transliterate?langpair=ja-Hira|ja&text=");
            url.Append(hiragana);

            return url.ToString().GetResponseFrom();
        }

        public static T SelectOneOf<T>(this T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                Console.WriteLine("{0} {1}", i + 1, array[i]);
            }

            int n=1;
            //do
            //{
            //    Console.Write("Select Number> ");
            //    Int32.TryParse(Console.ReadLine(), out n);
            //}
            //while (n < 1 || n > array.Length);

            return array[n - 1];
        }

        public static string ToKanji(this string hiragana)
        {
            StringBuilder kanji = new StringBuilder();
            if(hiragana != ""){

            JArray jar = JArray.Parse(hiragana.GetForConvWithGoogleTransliterate());

            foreach (JToken jt in jar)
            {
                var convArray = jt[1].ToArray();
                Console.WriteLine("{0}: {1} for conversion", jt[0], convArray.Length);
                kanji.Append((string)convArray.SelectOneOf());
            }
            }

            return kanji.ToString();
        }
    }
}