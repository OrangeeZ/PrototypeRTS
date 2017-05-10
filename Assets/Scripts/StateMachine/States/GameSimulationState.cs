using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    public class GameSimulationState : State<GameState> {


        #region private properties

        private readonly IFactory<TestWorld> _city;

        #endregion

        public GameSimulationState(IStateController<GameState> stateController,
            IFactory<TestWorld> city) : 
            base(stateController)
        {
            _city = city;
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

        private GameWorld InitializeSimulationWorld()
        {
            var city = _city.Create();
            var playerInfo = new PlayerInfo();
            var player = new Player(playerInfo, city);
            var world = new GameWorld(new EntitiesBehaviour(),new List<Player>(){player},player);
            var unitFactory = city.GetComponent<TestUnitFactory>();
            unitFactory.SetWorld(world);
            var popularityEventsBehaviour = new PopularityEventBehaviour(world, player,unitFactory);
            world.AddWorldBehaviour(popularityEventsBehaviour);
            return world;
        }
    }
}
