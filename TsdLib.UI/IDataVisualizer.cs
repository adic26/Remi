using System.ComponentModel;

namespace TsdLib.UI
{
    public interface IDataVisualizer : IComponent, ITsdLibControl
    {
        string Title { get; }
    }

    public interface IDataVisualizer<in T> : IDataVisualizer
    {
        void AddData(T data);
    }
}