using System.Collections.Generic;
using UnityEngine;

public class OnGuiController : MonoBehaviour
{
    private List<IGuiDrawer> _drawers = new List<IGuiDrawer>();
    public List<IGuiDrawer> Drawers { get { return _drawers; } }

    private void OnGUI()
    {
        for (int i = 0; i < Drawers.Count; i++)
        {
            var drawer = Drawers[i];
            drawer.Draw();
        }
    }

}
