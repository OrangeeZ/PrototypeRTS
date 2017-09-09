using System.Linq;
using Packages.EventSystem;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class WorldInfo : MonoBehaviour
    {
        public readonly EventSystem EventSystem = new EventSystem();

        public EntityDisplayPanel DefaultDisplayPanel;

        #region inspector properties

        [SerializeField]
        private ResourceInfo[] _resourceInfos;

        [SerializeField]
        private StorageInfo[] _storageInfos;

        [SerializeField]
        private UnitInfo[] _unitInfos;

        [SerializeField]
        private BuildingInfo[] _buildingInfos;

        [SerializeField]
        private Transform _fireplace;

        [Space]
        [SerializeField]
        private UnitInfo _treeInfo;

        #endregion

        #region public properties

        public UnitInfo[] UnitInfos => _unitInfos;

        public BuildingInfo[] BuildingInfos => _buildingInfos;

        public ResourceInfo[] ResourceInfos
        {
            get { return _resourceInfos; }
        }

        public StorageInfo[] StorageInfos => _storageInfos;

        public Transform Fireplace
        {
            get { return _fireplace; }
        }

        #endregion

        void Start()
        {
            foreach (var storageInfo in _storageInfos)
            {
                storageInfo.Init(_resourceInfos);
            }
        }

        [ContextMenu("Hook data")]
        private void HookData()
        {
            _unitInfos = UnityEditor.AssetDatabase
                .FindAssets("t:unitinfo")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<UnitInfo>)
                .ToArray();

            _buildingInfos = UnityEditor.AssetDatabase
                .FindAssets("t:buildinginfo")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<BuildingInfo>)
                .ToArray();

            _resourceInfos = UnityEditor.AssetDatabase
                .FindAssets("t:resourceinfo")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<ResourceInfo>)
                .ToArray();

            _storageInfos = UnityEditor.AssetDatabase
                .FindAssets("t:storageinfo")
                .Select(UnityEditor.AssetDatabase.GUIDToAssetPath)
                .Select(UnityEditor.AssetDatabase.LoadAssetAtPath<StorageInfo>)
                .ToArray();
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