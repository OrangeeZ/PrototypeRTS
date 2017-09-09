using System.Linq;
using Packages.EventSystem;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class WorldData : MonoBehaviour
    {
        public readonly EventSystem EventSystem = new EventSystem();

        public EntityDisplayPanel DefaultDisplayPanel;

        #region inspector properties

        [SerializeField]
        private WorldInfo _worldInfo;

        [SerializeField]
        private Transform _fireplace;

        [Space]
        [SerializeField]
        private UnitInfo _treeInfo;

        #endregion

        #region public properties

        public UnitInfo[] UnitInfos => _worldInfo.UnitInfos;

        public BuildingInfo[] BuildingInfos => _worldInfo.BuildingInfos;

        public ResourceInfo[] ResourceInfos => _worldInfo.ResourceInfos;

        public StorageInfo[] StorageInfos => _worldInfo.StorageInfos;

        public Transform Fireplace => _fireplace;

        public WorldInfo WorldInfo => _worldInfo;

        #endregion

        void Start()
        {
            foreach (var storageInfo in StorageInfos)
            {
                storageInfo.Init(ResourceInfos);
            }
        }



        public void PopulateWorld(BaseWorld world)
        {
            var factory = GetComponent<TestUnitFactory>();

            for (var i = 0; i < 5; ++i)
            {
                var unit = factory.CreateUnit(_treeInfo);
                var randomDirection = Random.onUnitSphere.Set(y: 0).normalized;
                var randomPosition = randomDirection * 10 + randomDirection * Random.Range(5, 10);
                unit.SetPosition(randomPosition);
                unit.SetFactionId(2); // Neutral, otherwise trees will be hostile to enemies
            }
        }
    }
}