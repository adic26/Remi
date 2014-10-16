namespace TsdLib.InstrumentLibrary.Helpers
{
    public interface IBlackBerryWlan
    {
        void EnablePlt();
        void DisableRx();
        void DisableMpc();
        void Enable();
        void SetBand(string band);
        void SetChannel(int channel);
        void SetRate(int rate);
        void StartRx();
        void ResetCounters();
        void StartCounters();

        int IsEnabled();
    }
}