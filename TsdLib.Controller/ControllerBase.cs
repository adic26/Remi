
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
