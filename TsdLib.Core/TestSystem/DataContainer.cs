using System;

namespace TsdLib.TestSystem
{
    public class DataContainer : MarshalByRefObject
    {
        public object Data { get; private set; }

        public DataContainer(object data)
        {
            Data = data;
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
