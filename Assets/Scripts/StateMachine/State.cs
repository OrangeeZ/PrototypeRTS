using System.Collections;

namespace Assets.Scripts.StateMachine
{
    public abstract class State<TState> : IState
    {
        protected IStateController<TState> _stateController;

        #region constructor

        /// <summary>
        /// single state of statemachine
        /// </summary>
        public State(IStateController<TState> stateController)
        {
            _stateController = stateController;
        }

        #endregion

        #region public methods

        public abstract IEnumerator Execute();

        public virtual void Stop(){}

        #endregion
    }
}
