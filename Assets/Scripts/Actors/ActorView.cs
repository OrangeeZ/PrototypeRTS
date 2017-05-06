using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Actors
{
    public class ActorView : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;

        public NavMeshAgent GetNavMeshAgent()
        {
            return _navMeshAgent;
        }
    }
}
