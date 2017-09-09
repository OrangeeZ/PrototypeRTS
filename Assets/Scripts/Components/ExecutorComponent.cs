using System.Collections;
using Assets.Scripts.StateMachine;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class ExecutorComponent : MonoBehaviour, IExecutor
    {

        #region inspector properties

        [SerializeField]
        private bool _isRunning;

        #endregion

        public void Execute(IEnumerator enumerator)
        {
            Stop();
            _isRunning = true;
            //GameLog.LogFormat("Start Executor Name : {0} {1}", Color.green, name, GetType().Name);
            StartCoroutine(enumerator);          
        }

        public void Stop()
        {
            //GameLog.LogFormat("Stop Executor {0}", Color.green, GetType().Name);
            StopAllCoroutines();
            _isRunning = false;
        }

        #region private methods
        
        private void OnEnable()
        {
            Debug.Log("ExecutorComponent OnEnable");
        }

        private void OnDisable()
        {
            Debug.Log("ExecutorComponent OnDisable");
        }

        #endregion
    }
}
