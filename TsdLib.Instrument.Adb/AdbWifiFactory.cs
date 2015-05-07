using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TsdLib.Instrument.Adb
{
    public class AdbWifiFactory : FactoryBase<AdbWifiConnection>
    {
        protected override IEnumerable<string> SearchForInstruments()
        {
            return Enumerable.Empty<string>();
        }

        protected override AdbWifiConnection CreateConnection(string address, params ConnectionSettingAttribute[] attributes)
        {
            throw new NotImplementedException();
        }

        protected override string GetInstrumentIdentifier(AdbWifiConnection connection, IdQueryAttribute idAttribute)
        {
            throw new NotImplementedException();
        }
    }
}
