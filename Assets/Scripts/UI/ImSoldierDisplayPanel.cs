using System.Collections;
using System.Collections.Generic;
using Actors;
<<<<<<< f8148aa54878b436da711513ce755c353fa6977b
using Assets.Scripts.Actors;
=======
>>>>>>> 51109a5ae2f0af8e4c1aa3bacf25fb4abc855286
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