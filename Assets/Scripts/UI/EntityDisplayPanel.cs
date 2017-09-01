using System.Collections;
using System.Collections.Generic;
using Actors;
<<<<<<< f8148aa54878b436da711513ce755c353fa6977b
using Assets.Scripts.Actors;
=======
>>>>>>> 51109a5ae2f0af8e4c1aa3bacf25fb4abc855286
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