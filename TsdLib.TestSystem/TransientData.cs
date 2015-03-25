using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TsdLib.TestSystem
{
    public class TransientData : MarshalByRefObject
    {
        public SynchronizationContext Context { get; private set; }

        public AppDomain Domain { get; private set; }

        public TransientData(SynchronizationContext context, AppDomain domain)
        {
            Context = context;
            Domain = domain;
        }
    }

    public class TransientData<T> : TransientData
    {
        public T Data { get; private set; }

        public TransientData(SynchronizationContext context, AppDomain domain, T data)
            : base(context, domain)
        {

            Data = data;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
