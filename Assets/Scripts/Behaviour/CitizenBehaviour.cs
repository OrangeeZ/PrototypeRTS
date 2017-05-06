using System.Collections;

namespace Assets.Scripts.Behaviour
{
    public class CitizenBehaviour : ActorBehaviour
    {
        protected override IEnumerator UpdateRoutine()
        {
            while (true)
            {
                yield return null;
            }
        }
    }
}
