using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRates
{
    class Program
    {
        static void Main(string[] args)
        {
            string html = GetHtml();
            if (html == null)
            {
                Console.WriteLine("Невозможно получить данные.");
                Console.ReadKey();
            }
            else
            {
                CBCourseRecord cbRecord = new CBCourseRecord(html);
                cbRecord.ToDB();
            }
        }

        private static string GetHtml()
        {
            WebRequest request = WebRequest.Create("http://www.cbr.ru/currency_base/daily.aspx?date_req=" + DateTime.Now.ToString("dd.MM.yyyy"));
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
            }
            catch
            {
                Console.WriteLine("Похоже, нет доступа в интернет.");
                return null;
            }
            using (response)
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream))
                return reader.ReadToEnd();
        }
    }
}