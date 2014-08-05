namespace TsdLib
{
    public interface IRemiControl
    {
        void WriteConfigStringToRemi(string data, string applicationName, string applicationVersion, string configType, string fileName);
        string ReadConfigStringFromRemi(string applicationName, string applicationVersion, string configType);
    }

}