using System.Collections;
using Assets.Scripts.StateMachine;
using Assets.Scripts.World;
using BehaviourStateMachine;
using UnityEngine;
using World.SocialModule;
using States.SimulationStates;

namespace StateMachine.States
{
    public class GameSimulationState : GameStateBehaviour
    {
        private readonly WorldData _worldData;
        private Player _player;
        private SimulationOnGui _gui;
        private BaseWorld _gameWorld;
        private TestUnitFactory _unitFactory;

        public GameSimulationState(IStateController<GameState> stateController,
            WorldData worldData) :
            base(stateController)
        {
            _worldData = worldData;
        }

        public override IEnumerator Execute()
        {
            _gui = new GameObject("GuiConroller").AddComponent<SimulationOnGui>();
            _unitFactory = _worldData.GetComponent<TestUnitFactory>();
            var relationshipMap = CreateRelationshipMap();
            _gameWorld = CreateWorld(relationshipMap);
            _player = CreatePlayer(_gameWorld);
            

            //start state
            var stateManager = CreateStateManager();
            stateManager.SetState(SimulationState.Initialize);

            while (stateManager.CurrentState!=SimulationState.Leave)
            {
                yield return null;
            }
        }

        /// <summary>
        /// deinit simulation data
        /// </summary>
        public override void Stop()
        {
            Object.DestroyImmediate(_gui);
        }

        private IStateManager<SimulationState> CreateStateManager()
        {
            //init level state machine
            var stateController = new StateController<SimulationState>();
            var stateFactory = new SimulationStatesFactory(stateController, _unitFactory,
                _gui, _player,
                _worldData, _gameWorld);
            var executor = new BehaviourExecutor(false, null, "SimulationExecutor");
            var stateMachine = new FiniteStateMachine(executor);
            var stateManager = new StateManager<SimulationState>(stateController, stateMachine, stateFactory);
            return stateManager;
        }

        private RelationshipMap CreateRelationshipMap()
        {
            var relationshipMap = new RelationshipMap();
            relationshipMap.SetRelationship(0, 1, RelationshipMap.RelationshipType.Hostile); // Players A and B
            relationshipMap.SetRelationship(0, 2, RelationshipMap.RelationshipType.Neutral); // Other players and nature
            relationshipMap.SetRelationship(1, 2, RelationshipMap.RelationshipType.Neutral);
            return relationshipMap;
        }

        private BaseWorld CreateWorld(RelationshipMap relationshipMap)
        {
            var world = new BaseWorld(_worldData.WorldInfo, relationshipMap,
                _worldData.Fireplace.position);
            //init unit factory
            _unitFactory.Initialize(world, _worldData);
            return world;
        }
        
        private Player CreatePlayer(BaseWorld world)
        {
            var player = new Player(world);
            return player;
        }

    }
}