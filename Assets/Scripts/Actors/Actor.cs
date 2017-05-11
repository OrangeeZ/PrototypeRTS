using Assets.Scripts.Behaviour;
using Assets.Scripts.World;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Actors
{
    public class Actor : Entity
    {
        public NavMeshAgent NavAgent { get { return ActorView.GetNavMeshAgent(); } }

        public UnitInfo Info { get; private set; }

        private ActorBehaviour _behaviour;

        public Actor(IWorld world) : base(world)
        {

        }

        public void SetInfo(UnitInfo info)
        {
            Info = info;
        }

        public void SetBehaviour(ActorBehaviour order)
        {
            _behaviour = order;
            _behaviour.SetActor(this);
            _behaviour.Initialize();
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

            if (Health <= 0 && _behaviour != null)
            {
                _behaviour.Dispose();
                _behaviour = null;
            }
        }
    }
}
