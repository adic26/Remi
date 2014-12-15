using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsdLib.Instrument.Ssh
{
    public abstract class SshConnection : ConnectionBase
    {
        internal SshConnection(string address)
            : base(address)
        {
        }
    }
}
