using System;
using System.Collections;
using Assets.Scripts.Common;
using Assets.Scripts.Extensions;
using Assets.Scripts.StateMachine;
using UniRx;
using UnityEngine;

namespace Assets.Scripts.Components
{
    public class UpdateExecutor : IExecutor, IDisposable
    {
        private IEnumerator _enumerator;
        private IDisposable _updateDisposable;

        #region inspector properties

        [SerializeField]
        private bool _isRunning;

        #endregion

        #region public methods

        public void Execute(IEnumerator enumerator)
        {
            Stop();
            _enumerator = enumerator;
            _isRunning = _enumerator != null;
            _updateDisposable = Observable.
                FromMicroCoroutine(x => _enumerator).
                Subscribe();
        }

        public void Stop()
        {
            _isRunning = false;
            _updateDisposable.Cancel();
        }

        public void Dispose()
        {
            Stop();
            _enumerator = null;
        }

        //public void Update(float delta)
        //{
        //    if (_isRunning)
        //    {
        //        _isRunning = _enumerator.MoveNext();
        //    }
        //}

        #endregion


    }
}
