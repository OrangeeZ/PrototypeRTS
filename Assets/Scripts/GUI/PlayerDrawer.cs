using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDrawer : GuiDrawer {

    private readonly Player _player;

    public PlayerDrawer(Player player)
    {
        _player = player;
    }

    public override void Draw()
    {
        GUILayout.Space(10);
        var rect = GUILayoutUtility.GetRect(40, 20);
        GUI.Label(rect,string.Format("Player Popularity : {0}",_player.Popularity));
    }
}
