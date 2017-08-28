using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Actors;
using UnityEngine;

public class ImSoldierDisplayPanel : EntityDisplayPanel
{
    public override void DrawOnGUI()
    {
        var actor = Entity as Actor;
        if (actor == null)
        {
            return;
        }
        
        GUILayout.Label(actor.Info.Name);
        GUILayout.Label($"HP: {actor.Health}");
    }
}