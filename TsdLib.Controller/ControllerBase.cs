using TsdLib.TestSequence;

namespace TsdLib.Controller
{
    public class ControllerBase //TODO: make abstract
    {
        private readonly ISettings _settings;
        private readonly TestSequenceBase _testSequence;

        public ControllerBase(ISettings settings, TestSequenceBase testSequence)
        {
            _settings = settings;
            _testSequence = testSequence;
            _testSequence.Measurements.ListChanged += Measurements_ListChanged;
        }

        void Measurements_ListChanged(object sender, System.ComponentModel.ListChangedEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        public void Configure()
        {
            _settings.Edit();
            
        }

        public void Run()
        {
            _testSequence.Execute();
        }
    }
}
