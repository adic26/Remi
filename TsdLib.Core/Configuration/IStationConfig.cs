using System.Collections.Generic;

namespace TsdLib.Configuration
{
    public interface IStationConfig : IConfigItem
    {
        List<string> MachineNames { get; set; }
    }
}