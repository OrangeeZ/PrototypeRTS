using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.Workplace;
using Packages.EventSystem;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class TestWorldData : MonoBehaviour
    {
        public readonly EventSystem EventSystem = new EventSystem();

        [SerializeField]
        private List<Stockpile> _stockpiles = new List<Stockpile>();
        [SerializeField]
        private Transform _fireplace;
        
        public List<Stockpile> Stockpiles { get { return _stockpiles; } }

        public Transform Fireplace { get { return _fireplace; } }
    }
}
