using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.Workplace;
using Packages.EventSystem;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class TestWorld : MonoBehaviour, ICity
    {
        public readonly EventSystem EventSystem = new EventSystem();
        private Queue<Actor> _freeCitizens = new Queue<Actor>();

        [SerializeField]
        private List<Stockpile> _stockpiles = new List<Stockpile>();
        [SerializeField]
        private Transform _fireplace;

        #region public methods

        public int FreeCitizensCount { get { return _freeCitizens.Count; } }

        public void RegisterFreeCitizen(Actor actor)
        {
            _freeCitizens.Enqueue(actor);
        }

        public void GetClosestStockpileWithResource(ResourceType resourceType)
        {

        }

        public Stockpile GetClosestStockpile(Vector3 position)
        {
            return _stockpiles.First();
        }

        public Actor GetFreeCitizen()
        {
            return _freeCitizens.Any() ? _freeCitizens.Dequeue() : null;
        }

        public Transform GetFireplace()
        {
            return _fireplace;
        }

        #endregion


        
    }
}
