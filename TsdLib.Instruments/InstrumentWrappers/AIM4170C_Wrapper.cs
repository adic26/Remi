using TsdLib.Instrument.Visa;

namespace TsdLib.Instrument
{
    public class AIM4170C_Wrapper : InstrumentWrapper<VisaConnection>
    {
        private AIM4170C _aim4170C;

        public AIM4170C_Wrapper(AIM4170C aim4170C)
        {
            _aim4170C = aim4170C;
        }
    }
}