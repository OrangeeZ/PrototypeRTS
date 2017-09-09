using System.Collections;
using Assets.Scripts.StateMachine;

namespace StateMachine
{
    public abstract class State<TState> : IState
    {
        protected IStateController<TState> StateController;

        protected State(IStateController<TState> stateController)
        {
            StateController = stateController;
        }

        public virtual void OnStateEnter()
        {
        }

        public abstract IEnumerator Execute();

        public virtual void OnStateExit()
        {
        }
    }
}