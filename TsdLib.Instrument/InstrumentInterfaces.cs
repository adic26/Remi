namespace TsdLib.Instrument
{
    public interface IPowerSupply
    {
        void SetVoltage(double voltage);
        double ReadCurrent();
    }

    public interface IBlackBerry
    {
        void Reboot();
    }
}
