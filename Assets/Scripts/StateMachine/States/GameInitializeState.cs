using System.Collections;
<<<<<<< f8148aa54878b436da711513ce755c353fa6977b
using Assets.Scripts.StateMachine;
=======
using StateMachine;
>>>>>>> 51109a5ae2f0af8e4c1aa3bacf25fb4abc855286

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
