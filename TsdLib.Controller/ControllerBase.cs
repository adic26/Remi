using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsdLib.Controller
{
    class ControllerBase
    {
        private readonly ISettings _settings;

        public ControllerBase(ISettings settings)
        {
            _settings = settings;
        }

        public void Configure()
        {
            _settings.Edit();
            
        }
    }
}
