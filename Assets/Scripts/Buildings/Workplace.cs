using Actors;
using Behaviour;
using UnityEngine;

namespace Buildings
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

            if (Info.Id == "Woodcutter")
            {
                Worker.SetBehaviour(new GetterBehaviour(this));
            }
            else
            {
                Worker.SetBehaviour(new WorkerBehaviour(this));
            }
        }

        public float BeginProduction()
        {
            HasResources = true;
            return ActiveProductionCycle.ProductionDuration;
        }

        public void EndProduction()
        {
            HasResources = false;
        }

        public void PutResourcesToStockpile(StockpileBlock stockpileBlock)
        {
            stockpileBlock.ChangeResource(ActiveProductionCycle.OutputResource.Id, ActiveProductionCycle.OutputResourceQuantity);
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