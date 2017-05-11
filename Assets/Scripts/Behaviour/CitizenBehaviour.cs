using System.Collections;

namespace Assets.Scripts.Behaviour
{
    public class CitizenBehaviour : ActorBehaviour
    {
		public override void Initialize()
		{
			Actor.World.RegisterFreeCitizen(Actor);
		}

		public override void Dispose()
		{

		}

        protected override IEnumerator UpdateRoutine()
        {
            while (true)
            {
                yield return null;
            }
        }
    }
}
