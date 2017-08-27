using Assets.Scripts.Behaviour;
using Assets.Scripts.World;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Actors
{
    public class ActorSelectionEventHandler : SelectionEventHandler
    {
        private readonly Actor _actor;

        public ActorSelectionEventHandler(Actor actor)
        {
            _actor = actor;
        }

        public override bool HandleDestinationClick(Vector3 destination)
        {
            if (_actor.Behaviour is SoldierBehaviour)
            {
                (_actor.Behaviour as SoldierBehaviour).SetDestination(destination);

                return true;
            }

            return false;
        }

        public override bool HandleEntityClick(Entity entity)
        {
            if (!entity.IsEnemy)
            {
                return false;
            }

            if (_actor.Behaviour is SoldierBehaviour)
            {
                (_actor.Behaviour as SoldierBehaviour).SetAttackTarget(entity);

                return true;
            }

            return false;
        }
    }

    public class Actor : Entity
    {
        public NavMeshAgent NavAgent => ActorView.GetNavMeshAgent();

        public UnitInfo Info { get; private set; }

        public ActorBehaviour Behaviour { get; private set; }

        private SelectionEventHandler _selectionEventHandler;

        public Actor(BaseWorld world) : base(world)
        {
            _selectionEventHandler = new ActorSelectionEventHandler(this);
        }

        public void SetInfo(UnitInfo info)
        {
            Info = info;
        }

        public void SetBehaviour(ActorBehaviour behaviour)
        {
            Behaviour?.Dispose();
            
            Behaviour = behaviour;
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

        public override SelectionEventHandler GetSelectionEventHandler()
        {
            return _selectionEventHandler;
        }

        public override EntityDisplayPanel GetDisplayPanelPrefab()
        {
            return Info.DisplayPanelPrefab;
        }
    }
}
