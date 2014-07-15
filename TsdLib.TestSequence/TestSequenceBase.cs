using System;
using System.ComponentModel;
using TsdLib.Instrument;

namespace TsdLib.TestSequence
{
    public abstract class TestSequenceBase //TODO: will make abstract
    {
        public MeasurementCollection Measurements;
        


        public abstract void Execute();

        //public MeasurementCollection RunTest()
        //{
            
        //    //TODO: take this to client
        //    Measurements = new MeasurementCollection();
        //    Measurements.ListChanged += measurements_ListChanged;


        //    return Measurements;
        //}

        void measurements_ListChanged(object sender, ListChangedEventArgs e)
        {
            IBindingList list = sender as IBindingList;
            if (list != null)
                Console.WriteLine("Adding measurement: " + list[e.NewIndex]);
        }
    }
}
