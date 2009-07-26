using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging.InProcess
{
    public class InProcessSagasRepository : ISagasRepository
    {
        public InProcessSagasRepository()
        {
            Store = new Dictionary<Guid, ISaga>();
        }

        protected readonly IDictionary<Guid, ISaga> Store;
        protected readonly object StoreLocker = new object();

        public ISaga WithId(Guid id)
        {
            lock (StoreLocker)
            {
                ISaga result = null;
                if (!Store.TryGetValue(id, out result))
                {
                    result = null;
                }
                return result;
            }
        }

        public void Save(ISaga saga)
        {
            lock (StoreLocker)
            {
                Store[saga.CorrelationId] = saga;
            }
        }

        public void Delete(Guid id)
        {
            lock (StoreLocker)
            {
                Store.Remove(id);
            }
        }
    }
}
