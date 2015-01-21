using System;
using TsdLib.Configuration;

namespace TsdLib.UI
{
    public interface ITestSequenceControl : ITsdLibControl
    {
        /// <summary>
        /// Event fired when requesting to execute the Test Sequence.
        /// </summary>
        event EventHandler ExecuteTestSequence;

        /// <summary>
        /// Event fired when requesting to abort the Test Sequence current in progress.
        /// </summary>
        event EventHandler AbortTestSequence;

        IConfigItem[] SelectedStationConfig { get; set; }
        IConfigItem[] SelectedProductConfig { get; set; }
        IConfigItem[] SelectedTestConfig { get; set; }
        IConfigItem[] SelectedSequenceConfig { get; set; }
        bool PublishResults { get; }
    }
}