using System;
using System.Collections;
using Assets.Scripts.Components;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Assets.Scripts.StateMachine
{
    public class BehaviourExecutor : IExecutor, IDisposable
    {
        private ExecutorComponent _executorObject;
        private bool _immortal;
        private readonly Transform _parent;
        private string _name;

        #region constructor

        public BehaviourExecutor(bool immortal = false, Transform parent = null, string name = null)
        {
            _immortal = immortal;
            _parent = parent;
            _name = name;
        }

        #endregion

        public bool IsRunning { get; protected set; }

        public ExecutorComponent Executor
        {
            get
            {
                if (!_executorObject)
                {
                    _executorObject = CreateExecutorObject();
                }
                return _executorObject;
            }
        }

        #region public methods

        public void Execute(IEnumerator enumerator)
        {
            Executor.Execute(enumerator);
        }

        public virtual void Stop()
        {
            Executor.Stop();
        }
        
        public void Dispose()
        {
            Stop();
        }

        #endregion

        #region private methods

        private ExecutorComponent CreateExecutorObject()
        {
            var executor = new GameObject().
                AddComponent<ExecutorComponent>();
            executor.transform.parent = _parent;
            if (!string.IsNullOrEmpty(_name))
                executor.name = _name;
            if (_immortal)
                Object.DontDestroyOnLoad(executor.gameObject);
            return executor;
        }

        #endregion

    }
}
