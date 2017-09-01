using System.Collections;

namespace Assets.Scripts.StateMachine
{
    public interface IState
    {
        void OnStateEnter();
        
        /// <summary>
        /// state execution logic
        /// </summary>
        /// <returns></returns>
        IEnumerator Execute();

        /// <summary>
        /// stop state execution
        /// </summary>
        void OnStateExit();
    }
}