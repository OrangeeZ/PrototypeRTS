using System.Collections.Generic;
using UnityEngine;

public class OnGuiController : MonoBehaviour
{
    private List<GuiDrawer> _drawers = new List<GuiDrawer>();
    public List<GuiDrawer> Drawers { get { return _drawers; } }

    private void OnGUI()
    {
        for (int i = 0; i < Drawers.Count; i++)
        {
            var drawer = Drawers[i];
            drawer.Draw();
        }
    }

}
