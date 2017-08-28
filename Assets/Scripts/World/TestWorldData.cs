using System.Collections.Generic;
using Packages.EventSystem;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class TestWorldData : MonoBehaviour
    {
        public readonly EventSystem EventSystem = new EventSystem();

        public EntityDisplayPanel DefaultDisplayPanel;
        
        [SerializeField]
        private ResourceInfo[] _resourceInfos;

        [SerializeField]
        private Transform _fireplace;

        public Transform Fireplace { get { return _fireplace; } }
        public ResourceInfo[] ResourceInfos { get { return _resourceInfos; }}
    }
}
