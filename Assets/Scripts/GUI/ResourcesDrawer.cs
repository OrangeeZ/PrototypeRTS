using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesDrawer : GuiDrawer
{
    private readonly IWorld _world;

    public ResourcesDrawer(IWorld world)
    {
        _world = world;
    }

    public override void Draw()
    {
        GUILayout.Space(10);
        var resourceContainer = _world.GetClosestStockpile(Vector3.down);
        var food = resourceContainer["Bread"];
        GUILayout.Label(string.Format("Breads Count : {0}", food));
    }
}
