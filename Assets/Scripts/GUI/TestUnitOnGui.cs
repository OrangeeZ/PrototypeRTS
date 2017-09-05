using UnityEngine;

namespace Assets.Scripts.World
{
    public class TestUnitOnGui : GuiDrawer
    {
        private readonly TestUnitFactory _unitFactory;
        
        public TestUnitOnGui(TestUnitFactory unitFactory)
        {
            _unitFactory = unitFactory;
        }

        public override void Draw()
        {
            GUILayout.Space(10);

            var factionId = (int)_unitFactory.FactionId;
            var factionIdString = GUILayout.TextArea(_unitFactory.FactionId.ToString(), "Faction Id");
            
            if (int.TryParse(factionIdString, out factionId))
            {
                _unitFactory.FactionId = (byte)factionId;
            }
            
            foreach (var each in _unitFactory.UnitInfos)
            {
                if (GUILayout.Button("Create " + each.Name))
                {
                    _unitFactory.CreateUnit(each);
                }
            }
            
            GUILayout.Space(10);
            foreach (var each in _unitFactory.BuildingInfos)
            {
                if (GUILayout.Button("Create " + each.Name))
                {
                    _unitFactory.CreateBuilding(each);
                }
            }
        }
    }
}
