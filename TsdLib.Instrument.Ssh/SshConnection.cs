namespace TsdLib.Instrument.Ssh
{
    public abstract class SshConnection : ConnectionBase
    {
        internal SshConnection(string address)
            : base(address)
        {
        }
    }
}
