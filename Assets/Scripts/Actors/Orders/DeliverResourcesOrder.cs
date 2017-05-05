using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliverResourcesOrder : Order
{
    public DeliverResourcesOrder(ResourceType ResourceType, int amount)
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
