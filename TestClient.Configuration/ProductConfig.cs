using System;
using System.Collections.Generic;
using System.ComponentModel;
using TsdLib.Configuration;

namespace TestClient.Configuration
{
    [Serializable]
    public class ProductConfig : ProductConfigCommon
    {
        //TODO: Create a product configuration structure using public properties with get and set accessors.
        //The values for these properties will be configured by the application at run-time (in Development mode only) or in Remi
        //The property values will be accessed by the TestSequence.Execute() method

        [Category("RF")]
        public List<int> WcdmaBands { get; set; }

        //Use public parameterless constructor to set default values
        public ProductConfig()
        {
            if (WcdmaBands == default(List<int>))
                WcdmaBands = new List<int>();
        }
    }
}
