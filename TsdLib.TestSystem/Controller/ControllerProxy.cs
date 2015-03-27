﻿using System;
using System.Diagnostics;
using System.Threading;
using TsdLib.Measurements;
using TsdLib.Observer;
using TsdLib.UI;

namespace TsdLib.TestSystem.Controller
{
    /// <summary>
    /// Processes data generated by the test sequence and passes it to the view as required.
    /// </summary>
    public class ControllerProxy : MarshalByRefObject, IObserver<DataContainer>, IObserver<IMeasurement>, IObserver<ITestInfo>, IObserver<Tuple<int, int>>
    {
        public void OnCompleted()
        {

        }

        public void OnError(Exception error)
        {

        }

        /// <summary>
        /// Gets a reference to the View object, representing the user interface.
        /// </summary>
        private readonly IView _viewProxy;

        /// <summary>
        /// Gets a reference to the object used to cancel/abort the test sequence.
        /// </summary>
        private ICancellationManager _testSequenceCancellationManager;

        private readonly SynchronizationContext _uiContext;

        /// <summary>
        /// Initialize a new ControllerProxy.
        /// </summary>
        /// <param name="view">An instance of <see cref="IView"/> that will be used to handle UI events.</param>
        /// <param name="testSequenceCancellationManager">Reference to the test sequence cancellation manager.</param>
        public ControllerProxy(IView view, ICancellationManager testSequenceCancellationManager)
        {
            _viewProxy = view;
            _testSequenceCancellationManager = testSequenceCancellationManager;
            _uiContext = SynchronizationContext.Current;
        }

        public virtual void OnNext(DataContainer data)
        {
            try
            {
                _uiContext.Post(s => _viewProxy.AddData(data), null);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Failed to update data on the UI{0}Data type: {1}{0}Error: {2}", Environment.NewLine, data.GetType().Name, ex));
            }

        }

        /// <summary>
        /// Provides the observer with new data.
        /// </summary>
        /// <param name="measurement">The measurement information.</param>
        public virtual void OnNext(IMeasurement measurement)
        {
            try
            {
                if (_viewProxy.MeasurementDisplayControl != null)
                    _uiContext.Post(s => _viewProxy.MeasurementDisplayControl.AddMeasurement(measurement), null);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Failed to update measurement on the UI{0}Test Info: {1}{0}Error: {2}", Environment.NewLine, measurement, ex));
            }
        }

        public virtual void OnNext(ITestInfo testInfo)
        {
            try
            {
                if (_viewProxy.TestInfoDisplayControl != null)
                    _uiContext.Post(s => _viewProxy.TestInfoDisplayControl.AddTestInfo(testInfo), null);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Failed to update test info on the UI{0}Test Info: {1}{0}Error: {2}", Environment.NewLine, testInfo, ex));
            }
        }

        public virtual void OnNext(Tuple<int, int> progress)
        {
            try
            {
                if (_viewProxy.ProgressControl != null)
                    _uiContext.Post(s => _viewProxy.ProgressControl.UpdateProgress(progress.Item1, progress.Item2), null);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Failed to update test info on the UI{0}Progress: {1}/{2}{0}Error: {3}", Environment.NewLine, progress.Item1, progress.Item2, ex));
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
