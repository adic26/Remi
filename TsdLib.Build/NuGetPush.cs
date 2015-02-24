

using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace TsdLib.Build
{
    public class NuGetPush : Task
    {
        [Required]
        public string Package { get; set; }

        [Required]
        public string Source { get; set; }

        public override bool Execute()
        {
            try
            {


                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
