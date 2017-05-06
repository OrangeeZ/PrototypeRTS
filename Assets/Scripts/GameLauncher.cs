using Assets.Scripts.StateMachine;
using UnityEngine;

namespace Assets.Scripts
{
    public class GameLauncher : MonoBehaviour
    {
        private void Start()
        {
            //make main go immortal
            DontDestroyOnLoad(gameObject);
            var controller = InitializeGameStateMachine();
            controller.SetState(GameState.Initialize);
        }

        /// <summary>
        /// start game states execution
        /// </summary>
        private IStateController<GameState> InitializeGameStateMachine()
        {
            var gameStateFactory = new GameStatesFactory();
            var stateMachine = new StateMachine.StateMachine(this);
            return new StateController<GameState>(stateMachine, gameStateFactory);
        }
    }
}
