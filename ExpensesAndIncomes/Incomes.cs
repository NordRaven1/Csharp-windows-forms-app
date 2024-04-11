using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesAndIncomes
{
    public sealed class Income : Money, IMethods
    {
        private string month;
        public string Month
        {
            get
            {
                return this.month;
            }
            set
            {
                this.month = value;
            }
        }
        public Income(string title, string month) : base(title)
        {
            Month = month;
        }

        public int Difference(int anotheranotherIncome)
        {
            return Math.Abs(SumUpElements() - anotheranotherIncome);
        }

        public string Comparison(int anotherIncome)
        {
            if (SumUpElements() > anotherIncome)
            {
                return "Раньше начисления были больше";
            }
            else
            {
                return "Раньше начисления были меньше";
            }
        }
    }
}
