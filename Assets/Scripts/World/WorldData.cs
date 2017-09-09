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

        public UnitInfo TreeInfo => _treeInfo;

        #endregion

        void Start()
        {
            foreach (var storageInfo in StorageInfos)
            {
                storageInfo.Init(ResourceInfos);
            }
        }

    }
}