using System;

namespace BehaviourStateMachine
{
    public interface IStateMachine : IDisposable
    {
        void Execute(IStateBehaviour state);
        void Stop();
    }
}