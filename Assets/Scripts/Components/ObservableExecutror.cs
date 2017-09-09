using System;
using System.Collections;
using Assets.Scripts.Extensions;
using Assets.Scripts.StateMachine;
using UniRx;

namespace Assets.Scripts.Components
{
    public class ObservableExecutror : IExecutor,IDisposable
    {
        private IDisposable _executorDisposable;

        #region public methods

        public void Execute(IEnumerator enumerator)
        {
            _executorDisposable = Observable.
                FromCoroutine(x => enumerator).Subscribe();
        }

        public void Stop()
        {
            _executorDisposable.Cancel();
        }

        public void Dispose()
        {
            Stop();
        }

        #endregion
    }
}
