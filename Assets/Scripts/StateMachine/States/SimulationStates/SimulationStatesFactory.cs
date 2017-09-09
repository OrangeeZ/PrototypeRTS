using System;
using System.Collections.Generic;
using Assets.Scripts.StateMachine.States.SimulationStates;
using Assets.Scripts.World;
using BehaviourStateMachine;

namespace States.SimulationStates
{
    public class SimulationStatesFactory : IStateFactory<SimulationState>
    {
        private readonly TestUnitFactory _unitFactory;
        private DummyState _dummyState;
        private Dictionary<SimulationState, IStateBehaviour> _stateBehaviours;

        public SimulationStatesFactory(IStateController<SimulationState> controller,
            TestUnitFactory unitFactory,
            SimulationOnGui simulationOnGui,
            Player player,
            WorldData worldData,BaseWorld gameWorld)
        {
            _unitFactory = unitFactory;
            _dummyState = new DummyState();
            _stateBehaviours = new Dictionary<SimulationState, IStateBehaviour>()
            {
                {SimulationState.Idle, new SimulationIdleState(gameWorld)},
                {SimulationState.Initialize, new SimulationInitializeState(controller,unitFactory,player,
                    simulationOnGui,gameWorld,worldData)},
            };
        }

        public IStateBehaviour Create(SimulationState state)
        {
            if (_stateBehaviours.ContainsKey(state))
                return _stateBehaviours[state];
            return _dummyState;
        }
    }
}
