using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace ExpensesAndIncomes
{
    public abstract class Money
    {
        protected string title;
        public string Title
        {
            get
            {
                return this.title;
            }
            set
            {
                this.title = value;
            }
        }
        private List<Service> servicesList = new List<Service>();
        public List<Service> ServicesList
        {
            get
            {
                return this.servicesList;
            }
        }

        public Money(string title)
        {
            Title = title;
        }

        public string this[int index]
        {
            get
            {
                return $"{ServicesList[index].ServiceName} {ServicesList[index].Amount}";
            }
        }

        public int SumUpElements()
        {
            int sum = 0;
            foreach (Service service in ServicesList)
            {
                sum += service.Amount;
            }
            return sum;
        }

        public void SortServicesList()
        {
            bool flag = true;
            while (flag)
            {
                int count = 0;
                for (int i = 0; i < ServicesList.Count - 1; i++)
                {
                    if (ServicesList[i] < ServicesList[i + 1])
                    {
                        ServicesList.Reverse(i, 2);
                        count += 1;
                    }
                }
                if (count == 0) { flag = false; }
            }
        }
    }
}
