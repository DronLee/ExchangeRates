using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using ExchangeRates.Properties;

namespace ExchangeRates
{
    internal class CBCourseRecord
    {
        private static Regex usdRegex = new Regex(Resources.USDCourseRegex, RegexOptions.Compiled);
        private static Regex eurRegex = new Regex(Resources.EURCourseRegex, RegexOptions.Compiled);

        public readonly double usa;
        public readonly double ue;

        public CBCourseRecord(string html)
        {
            Match usdMatch = usdRegex.Match(html);
            usa = Convert.ToDouble(usdMatch.Value);
            Match ueMatch = eurRegex.Match(html);
            ue = Convert.ToDouble(ueMatch.Value);
        }

        private string ConnectionString
        {
            get
            {
                SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
                builder.DataSource = @"UX32L\SQLEXPRESS";
                builder.InitialCatalog = "ExchangeRates";
                builder.IntegratedSecurity = true;
                return builder.ConnectionString;
            }
        }

        private bool ExistsInDB(SqlConnection openConnecion, DateTime date)
        {
            using (SqlCommand select = new SqlCommand("SELECT COUNT(1) FROM dbo.CBCourse WHERE [Date] = @Date", openConnecion))
            {
                select.Parameters.AddWithValue("@Date", date);
                return Convert.ToInt32(select.ExecuteScalar()) > 0;
            }
        }

        internal void ToDB()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                DateTime nowDate = DateTime.Now.Date;
                if (!ExistsInDB(connection, nowDate))
                {
                    using (SqlTransaction transaction = connection.BeginTransaction())
                    using (SqlCommand insert = new SqlCommand("INSERT INTO CBCourse VALUES(@CurrencyId, @Date, @Course)",
                        connection, transaction))
                    {
                        insert.Parameters.Add("@CurrencyId", SqlDbType.SmallInt);
                        insert.Parameters.Add("@Date", SqlDbType.Date);
                        insert.Parameters.Add("@Course", SqlDbType.Money);

                        insert.Parameters["@CurrencyId"].Value = 1;
                        insert.Parameters["@Date"].Value = nowDate;
                        insert.Parameters["@Course"].Value = usa;
                        insert.ExecuteNonQuery();

                        insert.Parameters["@CurrencyId"].Value = 2;
                        insert.Parameters["@Date"].Value = nowDate;
                        insert.Parameters["@Course"].Value = ue;
                        insert.ExecuteNonQuery();

                        transaction.Commit();
                    }
                }
            }
        }
    }
}