using Assets.Scripts.StateMachine;
using BehaviourStateMachine;
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
        private StateManager<GameState> InitializeGameStateMachine()
        {
            var executor = new BehaviourExecutor(true,null,"GameStateExecutor");
            var stateMachine = new FiniteStateMachine(executor);
            var controller = new StateController<GameState>();
            var gameStateFactory = new GameStatesFactory(controller);
            var stateManager = new StateManager<GameState>(controller, stateMachine,gameStateFactory,null);
            return stateManager;
        }
    }
}
