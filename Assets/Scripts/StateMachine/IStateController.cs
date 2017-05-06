using System;

namespace Assets.Scripts.StateMachine
{
    /// <summary>
    /// proxy interaction with gameState machine
    /// </summary>
    public interface IStateController<TStateType> : IDisposable
    {
        /// <summary>
        /// active gameState of machine
        /// </summary>
        TStateType CurrentState { get; }
        /// <summary>
        /// launch new gameState
        /// </summary>
        void SetState(TStateType state);
    }
}