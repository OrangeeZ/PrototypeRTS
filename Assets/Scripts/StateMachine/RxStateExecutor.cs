using System;
using System.Collections;
using Assets.Scripts.Extensions;
using Assets.Scripts.StateMachine;
using UniRx;

public class RxStateExecutor : IExecutor
{
    private IDisposable _exucutionDisposable;

    public void Dispose()
    {
        _exucutionDisposable.Cancel();
    }

    public void Execute(IEnumerator enumerator)
    {
        _exucutionDisposable = Observable.FromCoroutine(x => enumerator).Subscribe();
    }

    public void Stop()
    {
        _exucutionDisposable.Cancel();
        _exucutionDisposable = null;
    }
}
