using System.Collections;
using Assets.Scripts.StateMachine;
using StateMachine;

namespace StateMachine.States
{
    public class GameInitializeState : State<GameState>
    {
        public GameInitializeState(IStateController<GameState> stateController)
            :base(stateController)
        {
            
        }

        public override IEnumerator Execute()
        {
            //todo do somethink
            yield return null;
            StateController.SetState(GameState.Simulate);
        }

    }
}
