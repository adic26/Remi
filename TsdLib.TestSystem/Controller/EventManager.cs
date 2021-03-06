﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TsdLib.Measurements;
using TsdLib.TestSystem.TestSequence;
using TsdLib.UI;

namespace TsdLib.TestSystem.Controller
{
    /// <summary>
    /// Processes data generated by the test sequence and passes it to the view as required.
    /// </summary>
    public class EventManager : MarshalByRefObject
    {
        /// <summary>
        /// Gets a reference to the View object, representing the user interface.
        /// </summary>
        protected IView ViewProxy { get; private  set; }

        private readonly TaskScheduler _uiTaskScheduler;

        /// <summary>
        /// Initialize a new <see cref="EventManager"/>.
        /// </summary>
        /// <param name="view">An instance of <see cref="IView"/> that will be used to handle UI events.</param>
        public EventManager(IView view)
        {
            ViewProxy = view;
            _uiTaskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        }

        public virtual async void AddData(object sender, DataContainer data)
        {
            try
            {
                await PerformThreadSafeAction(() => ViewProxy.AddArbitraryData(data));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(string.Format("Failed to update data on the UI{0}Data type: {1}{0}Error: {2}", Environment.NewLine, data.Data.GetType().Name, ex));
            }
        }

        /// <summary>
        /// Adds a new measurement to the UI.
        /// </summary>
        /// <param name="sender">The test sequence that raised the event.</param>
        /// <param name="measurement">The measurement information.</param>
        public virtual async void AddMeasurement(object sender, IMeasurement measurement)
        {
            if (ViewProxy.MeasurementDisplayControl != null)
                await PerformThreadSafeAction(() => ViewProxy.MeasurementDisplayControl.AddMeasurement(measurement));
        }

        /// <summary>
        /// Adds new test information to the UI.
        /// </summary>
        /// <param name="sender">The test sequence that raised the event.</param>
        /// <param name="testInfo">The test information.</param>
        public virtual async void AddTestInfo(object sender, ITestInfo testInfo)
        {
            if (ViewProxy.TestInfoDisplayControl != null)
                await PerformThreadSafeAction(() => ViewProxy.TestInfoDisplayControl.AddTestInfo(testInfo));
        }

        /// <summary>
        /// Update the progress indicator on the UI.
        /// </summary>
        /// <param name="sender">The test sequence that raised the event.</param>
        /// <param name="progress">A Tuple containing the current step and number of steps in the test seuence</param>
        public virtual async void UpdateProgress(object sender, Tuple<int, int> progress)
        {
            if (ViewProxy.ProgressControl != null)
                await PerformThreadSafeAction(() => ViewProxy.ProgressControl.UpdateProgress(progress.Item1, progress.Item2));
        }

        public void TraceOutput(object sender, string message)
        {
            Trace.Write(message);
        }

        /// <summary>
        /// Perform an action on the UI thread.
        /// </summary>
        /// <param name="action">Action to perform.</param>
        /// <returns>A Task object that can be awaited.</returns>
        protected async Task PerformThreadSafeAction(Action action)
        {
            //try
            //{
                await Task.Factory.StartNew(
                    action,
                    CancellationToken.None,
                    TaskCreationOptions.None,
                    _uiTaskScheduler);
            //}
            //catch (Exception ex)
            //{
            //    Trace.WriteLine(string.Format("Failed to update {1} on the UI{0}Error: {2}", Environment.NewLine, state.GetType().Name, ex));
            //}
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
