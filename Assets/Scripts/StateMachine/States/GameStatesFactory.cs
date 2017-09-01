using System;
using Assets.Scripts.World;
using StateMachine.States;
using Object = UnityEngine.Object;

namespace Assets.Scripts.StateMachine
{
    public class GameStatesFactory : IStateFactory<GameState>
    {
        #region public methods

        public IState Create(IStateController<GameState> stateController,GameState gameState)
        {
            switch (gameState)
            {
                case GameState.Simulate:
                    return new GameSimulationState(stateController, Object.FindObjectOfType<WorldInfo>());
                case GameState.Initialize:
                    return new GameInitializeState(stateController);
                default:
                    throw new ArgumentOutOfRangeException("gameState", gameState, null);
            }
        }
        #endregion

    }
}
