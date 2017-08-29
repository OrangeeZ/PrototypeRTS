using Assets.Scripts.Workplace;
using UnityEngine;

public class ImBuildingDisplayPanel : EntityDisplayPanel
{
    private const string Stop = "STOP";
    private const string Activate = "ACTIVATE";
    
    public override void DrawOnGUI()
    {
        var workplace = Entity as Workplace;
        if (workplace == null)
        {
            return;
        }
        GUILayout.Label(workplace.Info.Name);
        GUILayout.BeginVertical();
        if(GUILayout.Button("REMOVE"))
            workplace.Kill();
        if(GUILayout.Button(workplace.IsActive?Stop : Activate))
           workplace.SetState(!workplace.IsActive);
        GUILayout.EndVertical();
    }
}