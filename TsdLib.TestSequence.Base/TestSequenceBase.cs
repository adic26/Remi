using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TsdLib.TestSequence
{
    public abstract class TestSequenceBase
    {
        public MeasurementCollection Measurements { get; protected set; }

        protected TestSequenceBase()
        {
            Measurements = new MeasurementCollection();
        }

        //This is the method that station-specific test sequences will override to define their test steps
        protected abstract void Execute(CancellationToken token);

        public async Task ExecuteAsync(CancellationToken token)
        {
            await Task.Run(() => Execute(token), token);
        }
    }
}
