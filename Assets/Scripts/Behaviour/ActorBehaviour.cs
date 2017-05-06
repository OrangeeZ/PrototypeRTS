using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ActorBehaviour
{
    protected Actor Actor;

    private IEnumerator _routine;

    protected float DeltaTime;

    public void SetActor(Actor actor)
    {
        Actor = actor;
    }

    public bool Update(float deltaTime)
    {
        DeltaTime = deltaTime;

        _routine = _routine ?? UpdateRoutine();

        return _routine.MoveNext();
    }

    protected virtual IEnumerator UpdateRoutine()
    {
        yield return null;
    }
}
