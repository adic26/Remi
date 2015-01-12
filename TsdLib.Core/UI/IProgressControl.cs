namespace TsdLib.UI
{
    public interface IProgressControl : ITsdLibControl
    {
        void UpdateProgress(int currentStep, int numberOfSteps);
    }
}