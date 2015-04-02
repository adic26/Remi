namespace TsdLib.Configuration.Management
{
    public interface IConfigEditor
    {
        ITestSystemIdentityManager IdentityManager { get; }
        void Edit(IConfigManagerProvider configManagerProdiver);
    }
}