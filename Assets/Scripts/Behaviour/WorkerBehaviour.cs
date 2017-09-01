using System.Collections;
using Buildings;

namespace Behaviour
{
    public class WorkerBehaviour : ActorBehaviour
    {
        private readonly Workplace _workplace;

        public WorkerBehaviour(Workplace workplace)
        {
            _workplace = workplace;
        }

        protected IEnumerable GoToWorkplace()
        {
            var navAgent = Actor.NavAgent;

            navAgent.SetDestination(_workplace.Position);

            while (navAgent.pathPending)
            {
                yield return null;
            }

            while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
            {
                yield return null;
            }
        }

        protected IEnumerable FetchResources()
        {
            var navAgent = Actor.NavAgent;

            if (!_workplace.HasResources && _workplace.ActiveProductionCycle.InputResourceQuantity > 0)
            {
                var closestStockpileBlock = default(StockpileBlock);

                do
                {
                    var delay = 1f;
                    while (delay > 0)
                    {
                        delay -= DeltaTime;
                        yield return null;
                    }

                    closestStockpileBlock = _workplace.World.Stockpile.GetClosestStockpileWithResource
                    (
                        _workplace.Position,
                        _workplace.ActiveProductionCycle.InputResource
                    );
                } while (closestStockpileBlock == null);

                navAgent.SetDestination(closestStockpileBlock.Position);
                while (navAgent.pathPending)
                {
                    yield return null;
                }

                while (navAgent.remainingDistance > 1f)
                {
                    yield return null;
                }
            }
        }

        protected IEnumerable RunProductionCycle()
        {
            var duration = _workplace.BeginProduction();
            while (duration > 0f)
            {
                duration -= DeltaTime;

                yield return null;
            }

            _workplace.EndProduction();
        }

        protected IEnumerable CarryProducedResourcesToStockpile()
        {
            var navAgent = Actor.NavAgent;

            var world = Actor.World;
            var closestStockpileBlock = default(StockpileBlock);

            while (true)
            {
                closestStockpileBlock =
                    world.Stockpile.GetClosestStockpileBlock(Actor.Position,
                        _workplace.ActiveProductionCycle.OutputResource);
                if (closestStockpileBlock != null)
                    break;

                yield return null;
            }

            navAgent.SetDestination(closestStockpileBlock.Position);
            while (!navAgent.hasPath || navAgent.remainingDistance > 1f)
            {
                yield return null;
            }

            _workplace.PutResourcesToStockpile(closestStockpileBlock);
        }

        protected override IEnumerator UpdateRoutine()
        {
            while (true)
            {
                var routine = GoToWorkplace().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                routine = FetchResources().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                routine = GoToWorkplace().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                routine = RunProductionCycle().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                routine = CarryProducedResourcesToStockpile().GetEnumerator();
                while (routine.MoveNext())
                {
                    yield return null;
                }

                yield return null;
            }
        }
    }
}