using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using UnityEngine;

public class EntityDisplayPanel : MonoBehaviour
{
    protected Entity Entity { get; private set; }

    public void SetEntity(Entity target)
    {
        Entity = target;
    }

    public virtual void DrawOnGUI()
    {

    }
}
