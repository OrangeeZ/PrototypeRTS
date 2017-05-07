using Assets.Scripts.Behaviour;
using Assets.Scripts.World;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Actors
{
    public class Actor : Entity
    {
        public NavMeshAgent NavAgent { get { return ActorView.GetNavMeshAgent(); } }


        private ActorBehaviour _behaviour;

        public Actor(TestWorld world) : base(world)
        {

        }

        public void SetBehaviour(ActorBehaviour order)
        {
            _behaviour = order;
            _behaviour.SetActor(this);
        }

        public override void Update(float deltaTime)
        {
            if (NavAgent != null)
            {
                Position = NavAgent.transform.position;
            }

            if (_behaviour == null)
            {
                return;
            }

            if (!_behaviour.Update(deltaTime))
            {
            }
        }

        public override void SetPosition(Vector3 position)
        {
            base.SetPosition(position);

            NavAgent.transform.position = position;
        }

        public override void DealDamage(int amount)
        {
            base.DealDamage(amount);

            if (Health <= 0)
            {
                _behaviour = null;
            }
        }
    }
}
