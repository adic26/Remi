//TODO: move to 2nd-order instrument library
namespace TsdLib.Instrument
{
    public interface IPowerSupply
    {
        void SetVoltage(double voltage);
        double ReadCurrent();
    }

    public interface IBlackBerryRadio
    {
        void Reboot();
        void EnableRadio();
        void DisableRadio();
        bool IsRadioEnabled();
        void PlacePhoneCall(int phoneNumber);
    }
}
