using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romialyo.Messaging
{
    public interface ISagasRepository
    {
        ISaga WithId(Guid id);

        void Save(ISaga saga);

        void Delete(Guid id);

    }
}
