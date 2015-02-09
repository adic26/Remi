using System;
using TsdLib.Configuration.Common;

namespace TsdLib.Configuration.Null
{
    [Serializable]
    public class NullStationConfig : StationConfigCommon
    {
        public NullStationConfig()
        {
            Name = "NullStationConfig";
            StoreInDatabase = false;
            IsDefault = true;
        }

        public override void InitializeDefaultValues()
        {

        }
    }
}
