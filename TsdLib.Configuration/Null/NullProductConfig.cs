using System;
using TsdLib.Configuration.Common;

namespace TsdLib.Configuration.Null
{
    [Serializable]
    public class NullProductConfig : ProductConfigCommon
    {
        public NullProductConfig()
        {
            Name = "NullProductConfig";
            StoreInDatabase = false;
            IsDefault = true;
        }

        public override void InitializeDefaultValues()
        {

        }
    }
}
