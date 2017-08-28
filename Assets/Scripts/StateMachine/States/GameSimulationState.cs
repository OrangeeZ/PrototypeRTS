using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.World;
using Assets.Scripts.World.SocialModule;
using UnityEngine;

namespace Assets.Scripts.StateMachine.States
{
    public class GameSimulationState : State<GameState>
    {
        #region private properties

        private OnGuiController _guiController;
        private readonly TestWorldData _worldData;

        #endregion

        public GameSimulationState(IStateController<GameState> stateController,
            TestWorldData worldData) :
            base(stateController)
        {
            _worldData = worldData;
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

        private BaseWorld InitializeSimulationWorld()
        {
            var guiObject = new GameObject("GuiConroller");
            _guiController = guiObject.AddComponent<OnGuiController>();
            //initialize world
            var playerRelationshp = new RelationshipMap(1);
            playerRelationshp.SetRelationship(0, 10);
            playerRelationshp.SetRelationship(1, -1);
            var playerWorld = new PlayerWorld(playerRelationshp, _worldData.Fireplace.position);

            //init unit factory
            var unitFactory = _worldData.GetComponent<TestUnitFactory>();
            unitFactory.SetWorld(playerWorld);

            // create dummy stockpile
            var stockpile =
                unitFactory.CreateBuilding(unitFactory.BuildingInfos.First(info => info.Id == "StockpileBlock"));
            stockpile.SetPosition(Vector3.zero);
            playerWorld.Stockpile.AddStockpileBlock(stockpile as StockpileBlock);

            //create player
            var player = CreatePlayer(playerWorld);
            CreateWorldEvents(playerWorld, player, unitFactory);
            //init parent world
            var neutralRelationshipMap = new RelationshipMap(0);
            var world = new BaseWorld(neutralRelationshipMap, Vector3.zero);
            world.Children.Add(playerWorld);
            playerWorld.Parent = world;
            //initialize Temp OnGUI drawer
            InitializeOnGuiDrawer(playerWorld, player, unitFactory);
            return world;
        }

        private void CreateWorldEvents(BaseWorld world, Player player, TestUnitFactory unitFactory)
        {
            var popularityEventsBehaviour = new PopularityEvent(world, player, unitFactory, 4f);
            var debtEvent = new DebtEvent(world, player, 10f);
            //constructions
            var constructionModule = new ConstructionModule(world, unitFactory);
            var constructionOnGui = new ConstructionOnGui(unitFactory, constructionModule);
            _guiController.Add(constructionOnGui);
            //reggister events
            world.Events.Add(constructionModule);
            world.Events.Add(popularityEventsBehaviour);
            world.Events.Add(debtEvent);
        }

        private Player CreatePlayer(BaseWorld world)
        {
            var playerInfo = ScriptableObject.CreateInstance<PlayerInfo>();
            var player = new Player(playerInfo, world);
            return player;
        }

        private void InitializeOnGuiDrawer(BaseWorld world, Player player, TestUnitFactory unitFactory)
        {
            //initialize additional events
            var selectionManager = new SelectionManager(world);
            var uiController = new ImUiController(selectionManager, _worldData);
            var worldPanel = new WorldDataPanelOnGui(world);
            
            _guiController.Add(new ResourcesDrawer(world));
            _guiController.Add(new PlayerDrawer(player));
            _guiController.Add(worldPanel);
            _guiController.Add(new TestUnitOnGui(unitFactory));
            _guiController.Add(selectionManager, true);
            _guiController.Add(uiController, shown: true);
        }

        #endregion
    }
}