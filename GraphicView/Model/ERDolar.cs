using System;
using System.Linq;

namespace GraphicView.Model
{
    internal class ERDolar:ExchangeRates
    {
        internal ERDolar() : base() { }
        private ERDolar(Cours[] rates) : base(rates) { }

        protected override string SelectProcedureName
        {
            get { return "GetLast3YearsCBDollars"; }
        }

        internal override ExchangeRates LastYear
        {
            get { return new ERDolar(rates.Where(r => r.date >= DateTime.Now.AddYears(-1)).ToArray()); }
        }

        internal override ExchangeRates LastMonth
        {
            get { return new ERDolar(rates.Where(r => r.date >= DateTime.Now.AddMonths(-1)).ToArray()); }
        }
    }
}
