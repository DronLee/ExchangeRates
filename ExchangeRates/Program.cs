using System;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using ExchangeRates.Properties;

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
                try
                {
                    var cbRecord = new CBCourseRecord(ConnectionString, html);
                    cbRecord.ToDB();
                }
                catch(Exception exc)
                {
                    Console.WriteLine(exc.Message);
                    Console.ReadKey();
                }
            }
        }

        private static string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = Settings.Default.DBServer;
                builder.InitialCatalog = Settings.Default.DBName;
                builder.IntegratedSecurity = true;
                return builder.ConnectionString;
            }
        }

        private static string GetHtml()
        {
            WebRequest request = WebRequest.Create("https://cbr.ru/currency_base/daily");
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