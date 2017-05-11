using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.World;
using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    public class GameSimulationState : State<GameState> {


        #region private properties

        private readonly IFactory<TestWorldData> _city;
        private OnGuiController _guiController;

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

        public override void Stop()
        {
            base.Stop();
            Object.DestroyImmediate(_guiController);
        }
        
        #region private methods

        private IWorld InitializeSimulationWorld()
        {
            var testWorldData = _city.Create();
            var playerWorld = new BaseWorld(testWorldData.Stockpiles,
                testWorldData.Fireplace.position);
            var unitFactory = testWorldData.GetComponent<TestUnitFactory>();
            unitFactory.SetWorld(playerWorld);
            var player = CreatePlayer(playerWorld);
            var popularityEventsBehaviour = new PopularityEvent(playerWorld, player, unitFactory);
            playerWorld.Events.Add(popularityEventsBehaviour);
            var world = new BaseWorld(new List<Stockpile>(),Vector3.zero);
            world.Childs.Add(playerWorld);
            _guiController = InitializeOnGuiDrawer(playerWorld,player,unitFactory);
            return world;
        }

        private Player CreatePlayer(IWorld world)
        {
            var playerInfo = new PlayerInfo();
            var player = new Player(playerInfo, world);
            return player;
        }

        private OnGuiController InitializeOnGuiDrawer(IWorld world,Player player, TestUnitFactory unitFactory)
        {
            var guiObject = new GameObject("GuiConroller");
            var guiController = guiObject.AddComponent<OnGuiController>();
            guiController.Drawers.Add(new TestUnitOnGui(unitFactory));
            guiController.Drawers.Add(new PlayerDrawer(player));
            guiController.Drawers.Add(new ResourcesDrawer(world));
            return guiController;
        }

        #endregion
    }
}
