using System;

namespace BehaviourStateMachine
{
    public interface IStateController<TStateType> : IDisposable
    {
        TStateType CurrentState { get; }
        TStateType PreviousState { get; }
        UniRx.IObservable<TStateType> StateObservable { get; }

        void SetState(TStateType state);
    }
}