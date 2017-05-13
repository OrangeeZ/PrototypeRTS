using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using UnityEngine;

namespace Assets.Scripts.Workplace
{
    public enum ResourceType
    {
        Bread,
        Wood,
    }

    public class WorkplaceSelectionEventHandler : SelectionEventHandler
    {
        public override bool HandleDestinationClick(Vector3 destination)
        {
            return false;
        }

        public override bool HandleEntityClick(Entity entity)
        {
            return false;
        }
    }

    public class Workplace : Building
    {
        public bool HasResources { get; private set; }

        protected Actor Worker;

        public Workplace(BaseWorld world) : base(world)
        {
        }

        public void SetWorker(Actor actor)
        {
            Worker = actor;
            Worker.SetBehaviour(new WorkerBehaviour(this));
        }

        public float BeginProduction()
        {
            HasResources = true;

            return Info.ProductionDuration;
        }

        public void EndProduction()
        {
            HasResources = false;
        }

        public override void Update(float deltaTime)
        {
            if (Worker != null)
            {
                return;
            }
            var city = World;
            var freeCitizen = city.HireCitizen();
            if (freeCitizen != null)
            {
                SetWorker(freeCitizen);
            }
        }

    }
}