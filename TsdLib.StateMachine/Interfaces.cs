using System;

namespace TsdLib.StateMachine
{
    public abstract class State
    {
        public IStateMachine Machine { get; internal set; }
        protected Func<bool> AllowProgression;
        protected Action OnEntryAction;

        protected State(Func<bool> allowProgression = null, Action onEntryAction = null)
        {
            AllowProgression = allowProgression ?? (() => false);
            OnEntryAction = onEntryAction ?? (() => { });
        }

        public virtual void OnEntry()
        {
            if (AllowProgression())
                Machine.ToNext();
            else
            {
                OnEntryAction();
                if (AllowProgression())
                    Machine.ToNext();
            }
        }

        public virtual void OnExit() { }
    }

    public interface IStateMachine
    {
        event EventHandler<StateChangedEventArgs> StateChanged;

        void ToStart();
        void ToNext();
        void ToPrevious();
    }
}