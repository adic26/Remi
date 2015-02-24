using System;
using TsdLib.Configuration.Common;

namespace TsdLib.Configuration.Null
{
    [Serializable]
    public class NullTestConfig : TestConfigCommon
    {
        public NullTestConfig()
        {
            Name = "NullTestConfig";
            StoreInDatabase = false;
            IsDefault = true;
        }

        public override void InitializeDefaultValues()
        {

        }
    }
}
