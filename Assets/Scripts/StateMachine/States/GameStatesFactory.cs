using System;
using Assets.Scripts.StateMachine.States;

namespace Assets.Scripts.StateMachine
{
    public class GameStatesFactory : IStateFactory<GameState>
    {
        private WorldFactory _worldFactory = new WorldFactory();

        #region public methods

        public IState Create(IStateController<GameState> stateController,GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Simulate:
                    return new GameSimulationState(stateController, _worldFactory);
                case GameState.Initialize:
                    return new GameInitializeState(stateController);
                default:
                    throw new ArgumentOutOfRangeException("gameState", gameState, null);
            }
        }
        #endregion

    }
}
