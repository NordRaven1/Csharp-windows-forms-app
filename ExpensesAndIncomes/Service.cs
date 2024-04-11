using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpensesAndIncomes
{
    public sealed class Service
    {
        private string serviceName;
        public string ServiceName
        {
            get
            {
                return this.serviceName;
            }

            set
            {
                this.serviceName = value;
            }
        }
        private int amount;
        public int Amount
        {
            get
            {
                return this.amount;
            }

            set
            {
                this.amount = value;
            }
        }

        public Service(string serviceName, int amount)
        {
            ServiceName = serviceName;
            Amount = amount;
        }

        public static bool operator >(Service cost1, Service cost2)
        {
            if (cost1.Amount.CompareTo(cost2.Amount) > 0) { return true; }
            else { return false; }
        }

        public static bool operator <(Service cost1, Service cost2)
        {
            if (cost1.Amount.CompareTo(cost2.Amount) < 0) { return true; }
            else { return false; }
        }
    }
}
