namespace TsdLib.Instrument
{
    public abstract class InstrumentWrapper<T>
        where T : ConnectionBase
    {
        private InstrumentBase<T>[] _instruments; 

        public InstrumentWrapper(params InstrumentBase<T>[] instruments)
        {
            _instruments = instruments;
        }
    }
}