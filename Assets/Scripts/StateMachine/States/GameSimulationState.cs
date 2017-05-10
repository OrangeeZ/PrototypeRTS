using System.Collections;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    public class GameSimulationState : State<GameState> {


        #region private properties

        private readonly IFactory<TestWorld> _testWorld;

        #endregion

        public GameSimulationState(IStateController<GameState> stateController,
            IFactory<TestWorld> testWorld) : 
            base(stateController)
        {
            _testWorld = testWorld;
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
            var world = _testWorld.Create();
            var unitFactory = world.GetComponent<TestUnitFactory>();
            unitFactory.SetWorld(world);
            var playerInfo = new PlayerInfo();
            var player = new Player(playerInfo);
            var popularityEventsBehaviour = new PopularityEventBehaviour(world, player,unitFactory);
            world.AddWorldBehaviour(popularityEventsBehaviour);
            return world;
        }
    }
}
