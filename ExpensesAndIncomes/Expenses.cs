using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesAndIncomes
{
    public sealed class Expenses : Money, IMethods
    {
        private int expectedExpenses;
        public int ExpectedExpenses
        {
            get
            {
                return this.expectedExpenses;
            }
            set
            {
                this.expectedExpenses = value;
            }
        }

        public Expenses(string category, int expenses) : base(category)
        {
            ExpectedExpenses = expenses;
        }

        public int Difference(int anotheranotherAmount)
        {
            return Math.Abs(SumUpElements() - ExpectedExpenses);
        }

        public string Comparison(int anotherAmount)
        {
            if (SumUpElements() > anotherAmount)
            {
                return "Превысило планы";
            }
            else
            {
                return "Не превысило планы";
            }
        }
    }
}
