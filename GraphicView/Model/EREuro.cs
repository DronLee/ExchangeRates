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
            get { return "GetLastYearCBEuro"; }
        }

        internal override ExchangeRates LastMonth
        {
            get { return new EREuro(rates.Where(r => r.date >= DateTime.Now.AddMonths(-1)).ToArray()); }
        }
    }
}
