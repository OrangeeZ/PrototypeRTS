using System.Collections;
using System.Linq;
using Assets.Scripts.StateMachine;
using Assets.Scripts.World;
using BehaviourStateMachine;
using UnityEngine;
using World.SocialModule;

namespace StateMachine.States
{
    public class GameSimulationState : GameStateBehaviour
    {
        private OnGuiController _guiController;
        private readonly WorldData _worldData;

        private BaseWorld _gameWorld;
        private RelationshipMap _relationshipMap;

        public GameSimulationState(IStateController<GameState> stateController, 
            WorldData worldData) :
            base(stateController)
        {
            _worldData = worldData;
        }

        public override IEnumerator Execute()
        {
            InitializeSimulationWorlds();
            while (true)
            {
                _gameWorld.Update(Time.unscaledDeltaTime);

                yield return null;
            }
        }

        public override void Stop()
        {
            Object.DestroyImmediate(_guiController);
        }

        private void InitializeSimulationWorlds()
        {
            _guiController = new GameObject("GuiConroller").AddComponent<OnGuiController>();
            _relationshipMap = new RelationshipMap();
            _relationshipMap.SetRelationship(0, 1, RelationshipMap.RelationshipType.Hostile); // Players A and B
            _relationshipMap.SetRelationship(0, 2, RelationshipMap.RelationshipType.Neutral); // Other players and nature
            _relationshipMap.SetRelationship(1, 2, RelationshipMap.RelationshipType.Neutral);

            CreatePlayerAndWorld();
        }

        private void CreatePlayerAndWorld()
        {
            var world = new BaseWorld(_relationshipMap, _worldData.Fireplace.position);

            //init unit factory
            var unitFactory = _worldData.GetComponent<TestUnitFactory>();
            unitFactory.SetWorld(world, _worldData);

            // create dummy stockpile
            var stockpile =
                unitFactory.CreateBuilding(unitFactory.BuildingInfos.First(info => info.Id == "StockpileBlock"));
            stockpile.SetPosition(Vector3.zero);
            world.Stockpile.AddStockpileBlock(stockpile as StockpileBlock);
            world.PopulationLimit = 15;

            _worldData.PopulateWorld(world);

            var player = CreatePlayer(world);
            CreateWorldEvents(world, player, unitFactory);

            //initialize Temp OnGUI drawer
            InitializeOnGuiDrawer(world, player, unitFactory);
            _gameWorld = world;
        }

        private void CreateWorldEvents(BaseWorld world, Player player, TestUnitFactory unitFactory)
        {
            var popularityEventsBehaviour = new PopularityEvent(world, _worldData, player, unitFactory, 4f);
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
            var uiController = new ImUiController(selectionManager, _worldData);
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