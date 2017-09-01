using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using UnityEngine;

namespace Assets.Scripts.Workplace
{
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
            IsActive = true;
        }

        public void SetWorker(Actor actor)
        {
            Worker = actor;
            Worker.SetBehaviour(new WorkerBehaviour(this));
        }

        public float BeginProduction()
        {
            HasResources = true;
            return ProductionCycle.ProductionDuration;
        }

        public void EndProduction()
        {
            HasResources = false;
        }

        public void PutResourcesToStockpile(StockpileBlock stockpileBlock)
        {
            stockpileBlock.ChangeResource(ProductionCycle.OutputResource.Id, ProductionCycle.OutputResourceQuantity);
        }

        public override void Update(float deltaTime)
        {
            if (Worker != null || !IsActive)
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

        #region private methods

        private void DismissWorker()
        {
            if (Worker == null) return;
            Worker.SetBehaviour(new CitizenBehaviour());
            Worker = null;
        }

        protected override void Deactivate()
        {
            DismissWorker();
        }

        #endregion

    }
}