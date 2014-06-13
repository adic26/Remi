using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TsdLib.StateMachine
{
    public class StateMachine<TState> : IStateMachine
        where TState : State
    {
        public event EventHandler<StateChangedEventArgs> StateChanged;

        readonly LinkedList<TState> _states;

        LinkedListNode<TState> _currentStateNode;
        LinkedListNode<TState> currentStateNode
        {
            get { return _currentStateNode; }
            set
            {
                Debug.WriteLine("Switching to " + value.Value);

                if (StateChanged != null)
                {
                    EventHandler<StateChangedEventArgs> handler = StateChanged;
                    handler(this, new StateChangedEventArgs(_currentStateNode.Value, value.Value));
                }

                if (_currentStateNode != null)
                    _currentStateNode.Value.OnExit();
                _currentStateNode = value;
                _currentStateNode.Value.OnEntry();
            }
        }

        public TState CurrentState { get { return currentStateNode.Value; } }

        readonly LinkedListNode<TState> _loopStateNode;

        //indexers
        ///// <summary>
        ///// Gets a state object by index number from the state machine.
        ///// </summary>
        ///// <param name="index">Zero-based index number of the state to retrieve.</param>
        ///// <returns>The state object whose index matches the specified index.</returns>
        //public TState this[int index] { get { return States.ElementAt(index); } }
        ///// <summary>
        ///// Gets a state object by type name from the state machine.
        ///// </summary>
        ///// <param name="stateName">Type name of the state to retrieve.</param>
        ///// <returns>The first state whose type name matches the specified stateName.</returns>
        //public TState this[string stateName] { get { return States.First(s => s.GetType().Name == stateName); } }

        public StateMachine(IEnumerable<TState> states)
            : this(states, -1) { }

        public StateMachine(IEnumerable<TState> states, int loopStateIndex)
        {
            TState[] enumerable = states as TState[] ?? states.ToArray();
            if (!enumerable.Any())
                throw new ArgumentNullException("states", "Must specify at least one state");

            _states = new LinkedList<TState>(enumerable);
            foreach (TState state in _states)
                state.Machine = this;

            Debug.WriteLine("Initialized state machine");

            currentStateNode = _states.First;
            _loopStateNode = _states.EnumerateNodes().ElementAtOrDefault(loopStateIndex);
        }

        public void AddState(TState state)
        {

        }

        public void ToStart()
        {
            currentStateNode = _states.First;
        }

        public void ToNext()
        {
            currentStateNode = (currentStateNode.Next ?? _loopStateNode) ?? currentStateNode;
        }

        public void ToPrevious()
        {
            currentStateNode = currentStateNode.Previous ?? currentStateNode;
        }
    }


    public class StateChangedEventArgs : EventArgs
    {
        public State PreviousState { get; private set; }
        public State NewState { get; private set; }

        public StateChangedEventArgs(State previousState, State newState)
        {
            PreviousState = previousState ?? newState;
            NewState = newState;
        }
    }

    public static class LinkedListExtensions
    {
        public static IEnumerable<LinkedListNode<T>> EnumerateNodes<T>(this LinkedList<T> list)
        {
            LinkedListNode<T> node = list.First;
            while (node != null)
            {
                yield return node;
                node = node.Next;
            }
        }
    }
}
