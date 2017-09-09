using System.Collections;
using Assets.Scripts.StateMachine;
using BehaviourStateMachine;

namespace StateMachine.States
{
    public class GameInitializeState : GameStateBehaviour
    {

        public GameInitializeState(IStateController<GameState> stateController) : 
            base(stateController)
        {
        }

        public override IEnumerator Execute()
        {
            //todo do somethink
            yield return null;
            _stateController.SetState(GameState.Simulate);
        }

    }
}
