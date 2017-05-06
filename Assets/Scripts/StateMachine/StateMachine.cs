using System.Collections;
using UnityEngine;

namespace Assets.Scripts.StateMachine
{
    public class StateMachine : IStateMachine
    {
        protected MonoBehaviour _behaviour;
        protected IState _activeState;

        #region cunstructor

        public StateMachine(MonoBehaviour behaviour)
        {
            _behaviour = behaviour;
        }

        #endregion
        
        #region public methods

        public void StartState(IState state)
        {
            Stop();
            _behaviour.StartCoroutine(ExecuteState(state));
        }

        public virtual void Stop()
        {
            if(_activeState != null)
                _activeState.Stop();
            _activeState = null;
            _behaviour.StopAllCoroutines();
        }
        
        public virtual void Dispose()
        {
            Stop();
        }

        #endregion

        #region private methods

        protected virtual IEnumerator ExecuteState(IState state)
        {
            _activeState = state;
            yield return state.Execute();
        }

        protected virtual void OnDestroy()
        {
            Dispose();
        }

        #endregion

    }
}
