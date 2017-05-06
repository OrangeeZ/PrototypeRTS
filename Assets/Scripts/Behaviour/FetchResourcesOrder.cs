using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FetchResourcesOrder : ActorBehaviour
{
    public FetchResourcesOrder(ResourceType ResourceType, int amount)
    {

    }

    protected override IEnumerator UpdateRoutine()
    {
        //Wait until resource available
        //Reserve resource so that others can't grab it
        //Go to stockpile
        //Take the resource
        //Go back to workplace
        yield return null;
    }
}
