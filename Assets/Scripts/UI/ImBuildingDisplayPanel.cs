using Assets.Scripts.Workplace;
using UnityEngine;

public class ImBuildingDisplayPanel : EntityDisplayPanel
{
    private const string STOP = "STOP";
    private const string ACTIVATE = "ACTIVATE";

    /// <summary>
    /// destroy building
    /// </summary>
    public void Remove()
    {
        
    }

    /// <summary>
    /// stop building func
    /// </summary>
    public void SwitchActiveState()
    {
        
    }

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
        if(GUILayout.Button(workplace.IsActive?STOP : ACTIVATE))
           workplace.SetState(!workplace.IsActive);
        GUILayout.EndVertical();
    }
}