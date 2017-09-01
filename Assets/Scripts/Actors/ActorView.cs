using UnityEngine;
using UnityEngine.AI;

namespace Actors
{
    public class ActorView : MonoBehaviour
    {
        [SerializeField]
        private NavMeshAgent _navMeshAgent;

        [SerializeField]
        private Renderer[] _renderers;

        private MaterialPropertyBlock _materialPropertyBlock;

        void OnValidate()
        {
            _renderers = GetComponentsInChildren<Renderer>(includeInactive: true);
        }

        public NavMeshAgent GetNavMeshAgent()
        {
            return _navMeshAgent;
        }

        public void SetIsEnemy(bool isEnemy)
        {
            if (_materialPropertyBlock == null)
            {
                _materialPropertyBlock = new MaterialPropertyBlock();
            }

            _materialPropertyBlock.SetColor("_Color", isEnemy ? Color.red : Color.green);

            foreach(var each in _renderers)
            {
                each.SetPropertyBlock(_materialPropertyBlock);
            }
        }
    }
}
