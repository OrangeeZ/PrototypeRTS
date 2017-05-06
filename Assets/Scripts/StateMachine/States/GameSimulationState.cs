using System.Collections;
using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    public class GameSimulationState : State<GameState> {

        public GameSimulationState(IStateController<GameState> stateController) : 
            base(stateController)
        {
        }

        public override IEnumerator Execute()
        {
            var world = InitializeSimulationWorld();
            while (true)
            {
                world.UpdateStep(Time.unscaledDeltaTime);
                yield return null;
            }
        }

        private TestWorld InitializeSimulationWorld()
        {
            //todo initialize simualtion gmae world state machine
            //use test game world from scene
            return Object.FindObjectOfType<TestWorld>();
        }
    }
}
