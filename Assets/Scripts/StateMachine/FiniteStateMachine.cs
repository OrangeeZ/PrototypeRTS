using Assets.Scripts.StateMachine;
using UnityEngine;

namespace BehaviourStateMachine
{
    public class FiniteStateMachine : IStateMachine
    {
        private readonly IExecutor _stateExecutor;
         
        #region constructor

        public FiniteStateMachine(IExecutor stateExecutor)
        {
            _stateExecutor = stateExecutor;
        }

        #endregion

        #region public properties

        public IStateBehaviour CurrentState { get; protected set; }

        #endregion

        #region public methods

        public void Execute(IStateBehaviour state)
        {
            Stop();
            ExecuteState(state);
        }

        public virtual void Stop()
        {
            if (CurrentState != null)
                CurrentState.Stop();
            Debug.LogFormat("STOP State: {0}", CurrentState);
            CurrentState = null;
            _stateExecutor.Stop();
        }

        public virtual void Dispose()
        {
            Stop();
            _stateExecutor.Stop();
        }

        #endregion

        #region private methods

        protected virtual void ExecuteState(IStateBehaviour stateType)
        {
            Debug.LogFormat("START State {0}", stateType);
            CurrentState = stateType;
            _stateExecutor.Execute(CurrentState.Execute());
        }

        #endregion

    }
}
