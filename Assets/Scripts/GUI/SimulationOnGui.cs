using System.Collections.Generic;
using UnityEngine;

public class SimulationOnGui : MonoBehaviour
{
    private List<IGuiDrawer> _drawers = new List<IGuiDrawer>();
    private List<bool> _visibility = new List<bool>();

    public List<IGuiDrawer> Drawers { get { return _drawers; } }

    #region public methods

    public void Add(IGuiDrawer drawer, bool shown = false)
    {
        _drawers.Add(drawer);
        _visibility.Add(shown);
    }

    #endregion

    private void OnGUI()
    {
        if (_visibility.Count != _drawers.Count)
        {
            _visibility.Clear();
            _drawers.ForEach(x => _visibility.Add(true));
        }
        for (int i = 0; i < _drawers.Count; i++)
        {
            var drawer = _drawers[i];
            _visibility[i] = GUILayout.Toggle(_visibility[i],
                string.Format("{0} : Visibility",drawer.GetType().Name));
            if(!_visibility[i])continue;
            drawer.Draw();
        }
    }

}
