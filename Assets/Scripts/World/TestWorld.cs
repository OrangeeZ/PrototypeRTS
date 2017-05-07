using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Actors;
using Assets.Scripts.Behaviour;
using Assets.Scripts.Workplace;
using UnityEngine;

namespace Assets.Scripts.World
{
    public class TestWorld : MonoBehaviour
    {
        private List<Entity> _entities = new List<Entity>();
		private List<Entity> _entitiesToRemove = new List<Entity>();

        private Queue<Actor> _freeCitizens = new Queue<Actor>();

        [SerializeField]
        private List<Stockpile> _stockpiles = new List<Stockpile>();

        [SerializeField]
        private Transform _fireplace;

        [SerializeField]
        private ActorView _testActorView;

        [SerializeField]
        private ActorView _testFriendlySoldierView;

        [SerializeField]
        private ActorView _testEnemySoldierView;

        [SerializeField]
        private ActorView _testBuildingView;

        public void AddCitizen()
        {
            var citizen = new Actor(this);

            citizen.SetView(Instantiate(_testActorView));
            citizen.SetBehaviour(new CitizenBehaviour());
            citizen.SetPosition(_fireplace.position);

			citizen.SetHealth(2);

            _entities.Add(citizen);
            _freeCitizens.Enqueue(citizen);
        }

        public void AddBuilding()
        {
            var building = new Workplace.Workplace(this);

            building.SetView(Instantiate(_testBuildingView));

            var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
            randomPosition.y = 0;

            building.SetPosition(_fireplace.position + randomPosition);
			building.SetHealth(10);

            _entities.Add(building);
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

		public void RemoveEntity(Entity entity)
		{
			_entitiesToRemove.Add(entity);
		}

        void OnGUI()
        {
            if (GUILayout.Button("Add citizen"))
            {
                AddCitizen();
            }

            if (GUILayout.Button("Add building"))
            {
                AddBuilding();
            }

            if (GUILayout.Button("Add friendly soldier"))
            {
                AddSoldier(false);
            }

            if (GUILayout.Button("Add enemy soldier"))
            {
                AddSoldier(true);
            }
        }

        private void AddSoldier(bool isEnemy)
        {
            var soldier = new Actor(this);

            soldier.SetView(Instantiate(isEnemy ? _testEnemySoldierView : _testFriendlySoldierView));
            soldier.SetBehaviour(new SoldierBehaviour());
            soldier.SetIsEnemy(isEnemy);
            soldier.SetHealth(4);

            var randomPosition = UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(5, 10f);
            randomPosition.y = 0;

            soldier.SetPosition(_fireplace.position + randomPosition);

            _entities.Add(soldier);
        }
    }
}
