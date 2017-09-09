using System.Collections;
using BehaviourStateMachine;
using UnityEngine;

namespace States.SimulationStates
{
    public class SimulationIdleState : IStateBehaviour
    {
        private readonly BaseWorld _gameWorld;

        #region constructor

        public SimulationIdleState(BaseWorld gameWorld)
        {
            _gameWorld = gameWorld;
        }

        #endregion


        public IEnumerator Execute()
        {
            while (true)
            {
                _gameWorld.Update(Time.unscaledDeltaTime);
                //todo check win/lose
                yield return null;
            }
        }

        public void Stop()
        {
            
        }
    }
}
