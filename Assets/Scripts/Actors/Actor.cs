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

        public ActorBehaviour Behaviour { get; private set; }

        public Actor(TestWorld world) : base(world)
        {

        }

        public void SetInfo(UnitInfo info)
        {
            Info = info;
        }

        public void SetBehaviour(ActorBehaviour order)
        {
            Behaviour = order;
            Behaviour.SetActor(this);
            Behaviour.Initialize();
        }

        public override void Update(float deltaTime)
        {
            if (NavAgent != null)
            {
                Position = NavAgent.transform.position;
            }

            if (Behaviour == null)
            {
                return;
            }

            if (!Behaviour.Update(deltaTime))
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

            if (Health <= 0 && Behaviour != null)
            {
                Behaviour.Dispose();
                Behaviour = null;
            }
        }
    }
}
