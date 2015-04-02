using System.Collections.Generic;

namespace TsdLib.Configuration
{
    public interface IStationConfig : IConfigItem
    {
        HashSet<string> MachineNames { get; set; }
    }
}