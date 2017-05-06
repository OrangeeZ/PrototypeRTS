using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
