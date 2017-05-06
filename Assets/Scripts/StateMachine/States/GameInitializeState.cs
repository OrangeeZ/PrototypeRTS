using System.Collections;

namespace Assets.Scripts.StateMachine.States
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
            _stateController.SetState(GameState.Simulate);
        }

    }
}
