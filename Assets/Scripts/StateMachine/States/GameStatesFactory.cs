using System;
using Assets.Scripts.World;
using BehaviourStateMachine;
using StateMachine.States;
using Object = UnityEngine.Object;

namespace Assets.Scripts.StateMachine
{
    public class GameStatesFactory : IStateFactory<GameState>
    {
        private readonly IStateController<GameState> _stateController;

        public GameStatesFactory(IStateController<GameState> stateController)
        {
            _stateController = stateController;
        }

        #region public methods

        public IStateBehaviour Create(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Simulate:
                    return new GameSimulationState(_stateController, Object.FindObjectOfType<WorldData>());
                case GameState.Initialize:
                    return new GameInitializeState(_stateController);
                default:
                    throw new ArgumentOutOfRangeException("gameState", gameState, null);
            }
        }
        #endregion

    }
}
