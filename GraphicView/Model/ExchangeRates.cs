using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace GraphicView.Model
{
    internal abstract class ExchangeRates
    {
        protected readonly Cours[] rates;

        internal ExchangeRates()
        {
            List<Cours> courses = new List<Cours>();
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            using (SqlCommand select = new SqlCommand(SelectProcedureName, connection))
            {
                connection.Open();
                using (SqlDataReader reader = select.ExecuteReader())
                    while (reader.Read())
                        courses.Add(new Cours()
                        {
                            date = (DateTime)reader["Date"],
                            value = Convert.ToDouble(reader["Course"])
                        });
            }
            rates = courses.ToArray();
        }

        protected ExchangeRates(Cours[] rates)
        {
            this.rates = rates;
        }

        protected abstract string SelectProcedureName { get; }

        protected string ConnectionString
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

        internal string[] Days
        {
            get { return rates.Select(r => string.Format("{0} {1}", r.date.Day, r.date.DayOfWeek)).ToArray(); }
        }

        internal DateTime[] Dates
        {
            get { return rates.Select(r => r.date).ToArray(); }
        }

        internal double[] Values
        {
            get { return rates.Select(r => r.value).ToArray(); }
        }

        internal abstract ExchangeRates LastMonth { get; }
    }
}