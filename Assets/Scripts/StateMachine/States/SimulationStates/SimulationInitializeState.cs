using System.Collections;
using System.Linq;
using Assets.Scripts.World;
using Assets.Scripts.World.Events;
using BehaviourStateMachine;
using States.SimulationStates;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Assets.Scripts.StateMachine.States.SimulationStates
{
    public class SimulationInitializeState : IStateBehaviour
    {
        private readonly IStateController<SimulationState> _controller;
        private readonly TestUnitFactory _unitFactory;
        private readonly Player _player;
        private readonly SimulationOnGui _onGui;
        private readonly BaseWorld _world;
        private readonly WorldData _worldData;

        #region constructor

        public SimulationInitializeState(IStateController<SimulationState> controller,
            TestUnitFactory unitFactory,
            Player player,
            SimulationOnGui onGui,
            BaseWorld world,
            WorldData worldData)
        {
            _controller = controller;
            _unitFactory = unitFactory;
            _player = player;
            _onGui = onGui;
            _world = world;
            _worldData = worldData;
        }

        #endregion

        public IEnumerator Execute()
        {
            CreateWorldEvents(_world, _player, _unitFactory);
            CreateWorldData(_world);
            InitializeOnGuiDrawer(_world, _player, _unitFactory);
            PopulateWorld(_world,5);
            //todo
            _controller.SetState(SimulationState.Idle);
            yield break;
        }

        public void Stop()
        {

        }

        #region private methods

        private void CreateWorldEvents(BaseWorld world, Player player, TestUnitFactory unitFactory)
        {
            var socialUpdatePeriod = 4;
            var hireCitizen = new HireCitizenEvent(world, _worldData, player, unitFactory,socialUpdatePeriod);
            var foodEvent = new CityFoodConsumptionEvent(world,_worldData,socialUpdatePeriod);
            var constructionModule = new ConstructionModule(world, unitFactory);
            var constructionOnGui = new ConstructionOnGui(unitFactory, constructionModule);

            _onGui.Add(constructionOnGui);
            
            world.Events.Add(constructionModule);
            world.Events.Add(foodEvent);
            world.Events.Add(hireCitizen);
        }


        private void CreateWorldData(BaseWorld world)
        {
            // create dummy stockpile
            var stockpile = _unitFactory.CreateBuilding(_unitFactory.
                BuildingInfos.First(info => info.Id == "StockpileBlock"));
            stockpile.SetPosition(Vector3.zero);
            world.Stockpile.AddStockpileBlock(stockpile as StockpileBlock);
        }

        private void InitializeOnGuiDrawer(BaseWorld world, Player player, TestUnitFactory unitFactory)
        {
            //initialize additional events
            var selectionManager = new SelectionManager(world);
            var uiController = new ImUiController(selectionManager, _worldData);
            var worldPanel = new WorldDataPanelOnGui(world);
            _onGui.Add(new ResourcesDrawer(world));
            _onGui.Add(new PlayerDrawer(player));
            _onGui.Add(worldPanel);
            _onGui.Add(new TestUnitOnGui(unitFactory));
            _onGui.Add(selectionManager, true);
            _onGui.Add(uiController, true);
        }


        public void PopulateWorld(BaseWorld world,int count)
        {
            for (var i = 0; i < count; ++i)
            {
                var unit = _unitFactory.CreateUnit(_worldData.TreeInfo,true);
                var randomDirection = Random.onUnitSphere.Set(y: 0).normalized;
                var randomPosition = randomDirection * 10 + randomDirection * Random.Range(5, 10);
                unit.SetPosition(randomPosition);
                unit.SetFactionId(2); // Neutral, otherwise trees will be hostile to enemies
            }
        }

        #endregion
    }
}
