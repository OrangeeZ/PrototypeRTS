using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor
{
    private ActorView _actorView;

    public Actor(ActorView actorView)
    {
        _actorView = actorView;
    }

    protected void SetDestination(Vector3 destination)
    {
        _actorView.GetNavMeshAgent().SetDestination(destination);
    }

    protected void ClearDestination()
    {
        _actorView.GetNavMeshAgent().Stop();
    }
}
