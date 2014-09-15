namespace TsdLib.TestResults
{
    /// <summary>
    /// Exception caused by attempting to access a MeasurementParameter that does not exist in the MeasurementParameterCollection.
    /// </summary>
    public class MeasurementParameterException : TsdLibException
    {
        /// <summary>
        /// Initialize a MeasurementParameterException for the specified parameter name.
        /// </summary>
        /// <param name="name">Name of the parameter that does not exisit.</param>
        public MeasurementParameterException(string name)
            : base("A MeasurementParameter named " + name + " does not exist.") { }
    }
}
