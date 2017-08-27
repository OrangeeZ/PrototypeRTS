using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ResourcesDrawer : GuiDrawer
{
    private readonly BaseWorld _world;
    private bool _foldout;
    private Dictionary<string, string> _resources;

    public ResourcesDrawer(BaseWorld world)
    {
        _resources  =new Dictionary<string, string>();
        _world = world;
    }

    public override void Draw()
    {
        _foldout = EditorGUILayout.Foldout(_foldout,"Resources");
        if (!_foldout) return;
        GUILayout.BeginVertical();
        var blocks = _world.Stockpile.GetBlocks();
        for (int i = 0; i < blocks.Count; i++)
        {
            var block = blocks[i];
            var resources = block.ResourceIds;
            for (int j = 0; j < resources.Length; j++)
            {
                GUILayout.BeginHorizontal();
                var amount = block[resources[j]];
                var resource = resources[j];
                GUILayout.Label(string.Format("{0} Count : {1}", resource, amount));
                if (!_resources.ContainsKey(resource))
                    _resources[resource] = string.Empty;
                _resources[resource] = 
                    GUILayout.TextField(_resources[resource], 20);
                if (GUILayout.Button(" + "))
                {
                    int value;
                    if (int.TryParse(_resources[resource], out value))
                    {
                        block.ChangeResource(resource,value);
                    }
                }
                GUILayout.EndHorizontal();
            }
        }
        GUILayout.EndVertical();
    }
}
