using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Workplace;
using Packages.EventSystem;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class TestWorld : MonoBehaviour
    {
		public readonly EventSystem EventSystem = new EventSystem();

        private List<Entity> _entities = new List<Entity>();
        private List<Entity> _entitiesToRemove = new List<Entity>();

        private Queue<Actor> _freeCitizens = new Queue<Actor>();

        [SerializeField]
        private List<Stockpile> _stockpiles = new List<Stockpile>();

        [SerializeField]
        private Transform _fireplace;

        void Awake()
        {
            GetComponent<TestUnitFactory>().SetWorld(this);
        }

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
            if (_freeCitizens.Any())
            {
                return _freeCitizens.Dequeue();
            }

            return null;
        }

        public Transform GetFireplace()
        {
            return _fireplace;
        }

        public void UpdateStep(float deltaTime)
        {
            foreach (var each in _entities)
            {
                each.Update(deltaTime);
            }

            foreach (var each in _entitiesToRemove)
            {
                _entities.Remove(each);
            }

            _entitiesToRemove.Clear();
        }

        public IList<Entity> GetEntities()
        {
            return _entities;//.OfType<Actor>();
        }

        public void AddEntity(Entity entity)
        {
            _entities.Add(entity);
        }

        public void RemoveEntity(Entity entity)
        {
            _entitiesToRemove.Add(entity);
        }

        void OnGUI()
        {
            GetComponent<TestUnitFactory>().DrawMenu();
        }
    }
}
