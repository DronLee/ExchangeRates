using System;
using System.Linq;

namespace GraphicView.Model
{
    internal class EREuro:ExchangeRates
    {
        internal EREuro() : base() { }
        private EREuro(Cours[] rates) : base(rates) { }

        protected override string SelectProcedureName
        {
            get { return "GetLast3YearsCBEuro"; }
        }

        internal override ExchangeRates LastYear
        {
            get { return new EREuro(rates.Where(r => r.date >= DateTime.Now.AddYears(-1)).ToArray()); }
        }

        internal override ExchangeRates LastMonth
        {
            get { return new EREuro(rates.Where(r => r.date >= DateTime.Now.AddMonths(-1)).ToArray()); }
        }
    }
}
