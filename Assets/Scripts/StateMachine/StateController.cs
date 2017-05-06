using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Assets.Scripts.StateMachine
{
    public class StateController<TStateType>:
        IStateController<TStateType>
    {
        protected IStateMachine _stateMachine;
        private readonly IStateFactory<TStateType> _stateFactory;
        private Stopwatch _stopwatch;

        #region public properties

        public TStateType CurrentState { get; protected set; }

        #endregion

        #region constructor

        public StateController(IStateMachine stateMachine, 
            IStateFactory<TStateType> stateFactory)
        {
            _stopwatch = new Stopwatch();
            _stateMachine = stateMachine;
            _stateFactory = stateFactory;
        }

        #endregion

        #region public methods

        public virtual void SetState(TStateType state)
        {
            if (!ValidateTransition(state))
            {
                Debug.LogError(string.Format("State transition validation failed, to state {0}",state));
                return;
            }
            OnStateChanged(CurrentState,state);
            CurrentState = state;
            var stateBehavior = _stateFactory.Create(this, state);
            if (stateBehavior == null)
            {
                Debug.LogError(string.Format("ERROR: StateBehaviour of type {0} is NULL", state));
                return;
            }
            Debug.Log(string.Format("<color=blue>Start new State {0}</color>",state));
            _stateMachine.StartState(stateBehavior);
        }

        public virtual void Dispose()
        {
            _stateMachine.Dispose();
        }

        #endregion

        #region private methods

        protected virtual bool ValidateTransition(TStateType nextState)
        {
            return true;
        }

        private void OnStateChanged(TStateType fromGameState, TStateType toGameState)
        {
            _stopwatch.Stop();
            Debug.Log(string.Format("<color=green>From State {0} To State {1} execution finished. RESULT TIME {2}</color>",
                fromGameState, toGameState, _stopwatch.ElapsedMilliseconds));
            _stopwatch.Reset();
            _stopwatch.Start();
        }

        #endregion

    }
}
