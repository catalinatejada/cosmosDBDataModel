using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CosmosDBDataModel.Config
{
    public class CosmosTestRun<T>
    {
        public List<T> _items { get; }
        public double _requestsConsumed { get; }

        private CosmosTestRun(List<T> items, double requestConsumed)
        {
            _items = items;
            _requestsConsumed = requestConsumed;
        }

        public static CosmosTestRun<T> SaveInTestRun(List<T> items, double requestsConsumed)
        {
            return new CosmosTestRun<T>(items, requestsConsumed);
        }

    }
}
