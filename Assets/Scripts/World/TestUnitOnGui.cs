using UnityEngine;

namespace Assets.Scripts.World
{
    public class TestUnitOnGui : MonoBehaviour
    {
        private TestUnitFactory _testUnitFactory;

        private void Awake()
        {
            _testUnitFactory = GetComponent<TestUnitFactory>();
        }

        public void DrawMenu()
        {
            GUILayout.Space(10);

            _testUnitFactory.IsEnemy = 
                GUILayout.Toggle(_testUnitFactory.IsEnemy, "Is Enemy");

            foreach (var each in _testUnitFactory.UnitInfos)
            {
                if (GUILayout.Button("Create " + each.Name))
                {
                    _testUnitFactory.CreateUnit(each);
                }
            }

            GUILayout.Space(10);

            foreach (var each in _testUnitFactory.BuildingInfos)
            {
                if (GUILayout.Button("Create " + each.Name))
                {
                    _testUnitFactory.CreateBuilding(each);
                }
            }
        }
        
        private void OnGUI()
        {
            DrawMenu();
        }

    }
}
