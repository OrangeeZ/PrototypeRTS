using System.Collections;
using Assets.Scripts.Workplace;

namespace Assets.Scripts.Behaviour
{
    public class DeliverResourcesOrder : ActorBehaviour
    {
        public DeliverResourcesOrder(ResourceType resourceType, int amount)
        {

        }

        protected override IEnumerator UpdateRoutine()
        {
            //Wait until there's a place at stockpile available
            //Reserve stockpile place so that others can't grab it
            //Go to stockpile
            //Deliver the resource
            //Go back to workplace
            yield return null;
        }
    }
}
