using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using ExchangeRates.Properties;

namespace ExchangeRates
{
    internal class CBCourseRecord
    {
        private readonly static Regex usdRegex = new Regex(Resources.USDCourseRegex, RegexOptions.Compiled);
        private readonly static Regex eurRegex = new Regex(Resources.EURCourseRegex, RegexOptions.Compiled);

        public readonly double usa;
        public readonly double ue;
        private readonly string _dbConnectionString;

        public CBCourseRecord(string dbConnectionString, string html)
        {
            var usdMatch = usdRegex.Match(html);
            if (!usdMatch.Success)
                throw new Exception("Запись о доларах на страннице не обнаружена.");
            usa = Convert.ToDouble(usdMatch.Value);

            var ueMatch = eurRegex.Match(html);
            if (!ueMatch.Success)
                throw new Exception("Запись о евро на страннице не обнаружена.");
            ue = Convert.ToDouble(ueMatch.Value);

            _dbConnectionString = dbConnectionString;
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
            using (SqlConnection connection = new SqlConnection(_dbConnectionString))
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