using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using Assets.Scripts.World;

namespace Assets.Scripts.Workplace
{
    public enum WorkplaceState
    {
        WaitingForNewWorker,
        FetchingResources,
        Producing,
        DeliveringResources
    }

    public enum ResourceType
    {
        Bread,
        Wood,
    }

    public class Workplace : Entity
    {
        public bool HasResources { get; private set; }

        public ResourceType ResourceType { get; private set; }

        public int ProductionRate { get; private set; }

        protected Actor Worker;

        private WorkplaceState _currentWorkplaceState = WorkplaceState.WaitingForNewWorker;

        public Workplace(TestWorld world) : base(world)
        {
        }

        public void SetWorker(Actor actor)
        {
            Worker = actor;
            Worker.SetBehaviour(new WorkerBehaviour(this));
            ProductionRate = 1;
        }

        public void SetResourceType(ResourceType resourceType)
        {
            ResourceType = resourceType;
        }

        public float BeginProduction()
        {
            HasResources = true;
        
            return 2f;
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

            var freeCitizen = World.GetFreeCitizen();
            if (freeCitizen != null)
            {
                SetWorker(freeCitizen);
            }
        }
    }
}