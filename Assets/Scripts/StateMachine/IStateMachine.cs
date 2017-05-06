using System;

namespace Assets.Scripts.StateMachine
{
    public interface IStateMachine : IDisposable
    {
        /// <summary>
        /// Start new State
        /// </summary>
        /// <param name="state">target state</param>
        void StartState(IState state);
        /// <summary>
        /// Stop active gameState
        /// </summary>
        void Stop();
    }
}