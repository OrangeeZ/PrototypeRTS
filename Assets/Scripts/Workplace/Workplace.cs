using System;
using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using Assets.Scripts.World;

namespace Assets.Scripts.Workplace
{
    public enum ResourceType
    {
        Bread,
        Wood,
    }

    public class Workplace : Entity
    {
        public bool HasResources { get; private set; }

        public BuildingInfo Info { get; private set; }

        protected Actor Worker;

        public Workplace(GameWorld world) : base(world)
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
            var city = World.ActivePlayer.City;
            var freeCitizen = city.GetFreeCitizen();
            if (freeCitizen != null)
            {
                SetWorker(freeCitizen);
            }
        }

        public void SetInfo(BuildingInfo info)
        {
            Info = info;
        }
    }
}