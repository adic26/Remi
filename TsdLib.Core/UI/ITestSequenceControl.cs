using System;
using TsdLib.Configuration;

namespace TsdLib.UI
{
    public interface ITestSequenceControl<TStationConfig, TProductConfig, TTestConfig> : ITsdLibControl
        where TStationConfig : IStationConfig
        where TProductConfig : IProductConfig
        where TTestConfig : ITestConfig
    {
        /// <summary>
        /// Event fired when requesting to execute the Test Sequence.
        /// </summary>
        event EventHandler ExecuteTestSequence;

        /// <summary>
        /// Event fired when requesting to abort the Test Sequence current in progress.
        /// </summary>
        event EventHandler AbortTestSequence;

        TStationConfig[] SelectedStationConfig { get; set; }
        TProductConfig[] SelectedProductConfig { get; set; }
        TTestConfig[] SelectedTestConfig { get; set; }
        ISequenceConfig[] SelectedSequenceConfig { get; set; }
        bool PublishResults { get; }
    }
}