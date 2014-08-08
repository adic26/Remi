namespace TsdLib
{
    public interface IRemiControl
    {
        void WriteConfigStringToRemi(string data, string applicationName, string applicationVersion, string fileName);
        string ReadConfigStringFromRemi(string applicationName, string applicationVersion, string configType);
    }

}