using System.Collections;
using Assets.Scripts.StateMachine;
using BehaviourStateMachine;

namespace StateMachine.States
{
    public abstract class GameStateBehaviour : StateBehaviour
    {
        protected readonly IStateController<GameState> _stateController;

        public GameStateBehaviour(IStateController<GameState> stateController)
        {
            _stateController = stateController;
        }
    }
}
