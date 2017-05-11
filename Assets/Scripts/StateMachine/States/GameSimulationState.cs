using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    public class GameSimulationState : State<GameState> {


        #region private properties

        private readonly IFactory<TestWorldData> _city;

        #endregion

        public GameSimulationState(IStateController<GameState> stateController,
            IFactory<TestWorldData> city) : 
            base(stateController)
        {
            _city = city;
        }

        public override IEnumerator Execute()
        {
            var world = InitializeSimulationWorld();
            while (true)
            {
                world.Update(Time.unscaledDeltaTime);
                yield return null;
            }
        }

        private IWorld InitializeSimulationWorld()
        {
            var testWorldData = _city.Create();
            var playerInfo = new PlayerInfo();
            var player = new Player(playerInfo);
            var playerWorld = new BaseWorld(testWorldData.Stockpiles,
                testWorldData.Fireplace.position);
            playerWorld.Player = player;
            var unitFactory = testWorldData.GetComponent<TestUnitFactory>();
            unitFactory.SetWorld(playerWorld);
            var popularityEventsBehaviour = new PopularityEventBehaviour(playerWorld, player, unitFactory);
            playerWorld.WorldEventsController.AddItem(popularityEventsBehaviour);

            var world = new BaseWorld(new List<Stockpile>(),Vector3.zero);
            world.Childs.Add(playerWorld);
            return world;
        }
    }
}
