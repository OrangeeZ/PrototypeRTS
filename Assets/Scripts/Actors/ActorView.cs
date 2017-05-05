using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ActorView : MonoBehaviour
{
    [SerializeField]
    private NavMeshAgent _navMeshAgent;

    public NavMeshAgent GetNavMeshAgent()
    {
        return _navMeshAgent;
    }
}
