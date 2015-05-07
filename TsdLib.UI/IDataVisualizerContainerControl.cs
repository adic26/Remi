using System.Collections.Generic;

namespace TsdLib.UI
{
    /// <summary>
    ///     Defines members to display multiple data visualizers in a visual container.
    /// </summary>
    public interface IDataVisualizerContainerControl : ITsdLibControl
    {
        /// <summary>
        ///     Adds data to all data visualizers that are configured to accept the specified data type.
        /// </summary>
        /// <typeparam name="T">Type of data being added.</typeparam>
        /// <param name="data">Data to add.</param>
        void Add<T>(T data);
    }
}