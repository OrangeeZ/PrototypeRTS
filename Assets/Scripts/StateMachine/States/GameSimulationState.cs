using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.StateMachine;
using Assets.Scripts.World;
using UnityEngine;
using World.SocialModule;

namespace StateMachine.States
{
    public class GameSimulationState : State<GameState>
    {
        private OnGuiController _guiController;
        private readonly WorldInfo _worldInfo;

        private BaseWorld _gameWorld;
        private RelationshipMap _relationshipMap;

        public GameSimulationState(IStateController<GameState> stateController, WorldInfo worldInfo) :
            base(stateController)
        {
            _worldInfo = worldInfo;
        }

        public override void OnStateEnter()
        {
            InitializeSimulationWorlds();
        }

        public override IEnumerator Execute()
        {
            while (true)
            {
                _gameWorld.Update(Time.unscaledDeltaTime);

                yield return null;
            }
        }

        public override void OnStateExit()
        {
            Object.DestroyImmediate(_guiController);
        }

        private void InitializeSimulationWorlds()
        {
            _guiController = new GameObject("GuiConroller").AddComponent<OnGuiController>();
            _relationshipMap = new RelationshipMap();

            CreatePlayerAndWorld();
        }

        private void CreatePlayerAndWorld()
        {
            var playerWorld = new PlayerWorld(_relationshipMap, _worldInfo.Fireplace.position);

            //init unit factory
            var unitFactory = _worldInfo.GetComponent<TestUnitFactory>();
            unitFactory.SetWorld(playerWorld, _worldInfo);

            // create dummy stockpile
            var stockpile =
                unitFactory.CreateBuilding(unitFactory.BuildingInfos.First(info => info.Id == "StockpileBlock"));
            stockpile.SetPosition(Vector3.zero);
            playerWorld.Stockpile.AddStockpileBlock(stockpile as StockpileBlock);
            playerWorld.PopulationLimit = 15;

            _worldInfo.PopulateWorld(playerWorld);

            var player = CreatePlayer(playerWorld);
            CreateWorldEvents(playerWorld, player, unitFactory);

            //initialize Temp OnGUI drawer
            InitializeOnGuiDrawer(playerWorld, player, unitFactory);
            _gameWorld = playerWorld;
        }

        private void CreateWorldEvents(BaseWorld world, Player player, TestUnitFactory unitFactory)
        {
            var popularityEventsBehaviour = new PopularityEvent(world, _worldInfo, player, unitFactory, 4f);
            var debtEvent = new DebtEvent(world, player, 10f);

            var constructionModule = new ConstructionModule(world, unitFactory);
            var constructionOnGui = new ConstructionOnGui(unitFactory, constructionModule);
            _guiController.Add(constructionOnGui);

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
            var uiController = new ImUiController(selectionManager, _worldInfo);
            var worldPanel = new WorldDataPanelOnGui(world);

            _guiController.Add(new ResourcesDrawer(world));
            _guiController.Add(new PlayerDrawer(player));
            _guiController.Add(worldPanel);
            _guiController.Add(new TestUnitOnGui(unitFactory));
            _guiController.Add(selectionManager, shown: true);
            _guiController.Add(uiController, shown: true);
        }
    }
}