using Assets.Scripts.Workplace;
using UnityEngine;

public class ImBuildingDisplayPanel : EntityDisplayPanel
{
    public override void DrawOnGUI()
    {
		var workplace = Entity as Workplace;
        if (workplace == null)
            return;
		GUILayout.Label(workplace.Info.Name);
    }
}
