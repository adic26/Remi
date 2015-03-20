using System;

namespace TsdLib
{
    public class DataContainer<T> : MarshalByRefObject
    {
        public T DataObject { get; private set; }

        public DataContainer(T dataObject)
        {
            DataObject = dataObject;
        }
    }
}
