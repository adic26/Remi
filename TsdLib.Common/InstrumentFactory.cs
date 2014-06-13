using System;
using System.Collections.Generic;
using System.Linq;

namespace TsdLib
{
    [Obsolete]
    public static class InstrumentFactory<T>
        where T : class, IConnect
    {
        static readonly List<T> objects = new List<T>();

        public static T GetInstance(string address, bool connect = true)
        {
            T obj = objects.FirstOrDefault(t => t.Address == address);
            if (obj == null)
            {
                obj = (T)Activator.CreateInstance(typeof(T), address);
                objects.Add(obj);
            }
            if (connect && !obj.IsConnected)
                obj.Connect();
            return obj;
        }

        public static void Remove(T obj)
        {
            if (objects.Contains(obj))
                objects.Remove(obj);
        }
    }
}
