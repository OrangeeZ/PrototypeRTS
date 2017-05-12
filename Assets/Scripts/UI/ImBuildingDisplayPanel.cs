using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Workplace;
using UnityEngine;

public class ImBuildingDisplayPanel : EntityDisplayPanel
{
    public override void DrawOnGUI()
    {
		var workplace = Entity as Workplace;

		GUILayout.Label(workplace.Info.Name);
    }
}
