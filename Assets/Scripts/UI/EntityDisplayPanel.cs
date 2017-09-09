using System.Collections;
using System.Collections.Generic;
using Actors;
using UnityEngine;

public class EntityDisplayPanel : MonoBehaviour
{
    protected SelectionManager SelectionManager { get; private set; }
    protected Entity Entity { get; private set; }

    public void Initialize(SelectionManager selectionManager, Entity target)
    {
        Entity = target;
        SelectionManager = selectionManager;
    }

    public virtual void Show()
    {
    }

    public virtual void Hide()
    {
    }

    public virtual void DrawOnGUI()
    {
    }
}